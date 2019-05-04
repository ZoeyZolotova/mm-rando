using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace MMRando
{

    public partial class mmrMain : Form
    {
        bool Output_ROM = true;
        bool Updating = false;
        bool Output_VC = false;
        string SettingOld = "";
        int SeedOld = 0;

        fAbout About = new fAbout();
        fManual Manual = new fManual();
        fLogicEdit LogicEditor = new fLogicEdit();
        fItemEdit ItemEditor = new fItemEdit();

        public static string MainDir = Application.StartupPath;
        public static string MusicDir = Application.StartupPath + "\\music\\";
        public static string ModsDir = Application.StartupPath + "\\mods\\";
        public static string AddrsDir = Application.StartupPath + "\\addresses\\";
        public static string ObjsDir = Application.StartupPath + "\\obj\\";
        public static string VCDir = Application.StartupPath + "\\vc\\";

        //read/write settings

        private void SeedRNG()
        {
            RNG = new Random(Convert.ToInt32(tSeed.Text));
        }

        private int[] GetOptions()
        {
            int[] O = new int[3];
            if (cUserItems.Checked) { O[0] += 8192; };
            if (cAdditional.Checked) { O[0] += 4096; };
            if (cGossip.Checked) { O[0] += 2048; };
            if (cSoS.Checked) { O[0] += 1024; };
            if (cSpoiler.Checked) { O[0] += 512; };
            if (cMixSongs.Checked) { O[0] += 256; };
            if (cBottled.Checked) { O[0] += 128; };
            if (cDChests.Checked) { O[0] += 64; };
            if (cShop.Checked) { O[0] += 32; };
            if (cDEnt.Checked) { O[0] += 16; };
            if (cBGM.Checked) { O[0] += 8; };
            if (cEnemy.Checked) { O[0] += 4; };
            if (cCutsc.Checked) { O[0] += 2; };
            if (cQText.Checked) { O[0] += 1; };
            O[1] = ((byte)cMode.SelectedIndex << 16) | ((byte)cLink.SelectedIndex << 8) | ((byte)cTatl.SelectedIndex) |
                ((byte)cDType.SelectedIndex << 24) | ((byte)cDMult.SelectedIndex << 28);
            O[2] = (bTunic.BackColor.R << 16) | (bTunic.BackColor.G << 8) | (bTunic.BackColor.B) |
                ((byte)cFloors.SelectedIndex << 24) | ((byte)cGravity.SelectedIndex << 28);
            return O;
        }

        private string UpdateSettingsString()
        {
            string Settings;
            Settings = Base36.Encode(Convert.ToInt32(tSeed.Text));
            Settings += "-";
            int[] Options = GetOptions();
            Settings += Base36.Encode(Options[0]);
            Settings += "-";
            Settings += Base36.Encode(Options[1]);
            Settings += "-";
            Settings += Base36.Encode(Options[2]);
            tSString.Text = Settings;
            saveROM.FileName = "MMR-" + Settings + ".z64";
            saveWad.FileName = "MMR-" + Settings + ".wad";
            return Settings;
        }

        private void SetOptions(string[] O)
        {
            tSeed.Text = Base36.Decode(O[0]).ToString();
            int Checks = (int)Base36.Decode(O[1]);
            int Combos = (int)Base36.Decode(O[2]);
            int Colour = (int)Base36.Decode(O[3]);
            cUserItems.Checked = (Checks & 8192) > 0;
            cAdditional.Checked = (Checks & 4096) > 0;
            cGossip.Checked = (Checks & 2048) > 0;
            cSoS.Checked = (Checks & 1024) > 0;
            cSpoiler.Checked = (Checks & 512) > 0;
            cMixSongs.Checked = (Checks & 256) > 0;
            cBottled.Checked = (Checks & 128) > 0;
            cDChests.Checked = (Checks & 64) > 0;
            cShop.Checked = (Checks & 32) > 0;
            cDEnt.Checked = (Checks & 16) > 0;
            cBGM.Checked = (Checks & 8) > 0;
            cEnemy.Checked = (Checks & 4) > 0;
            cCutsc.Checked = (Checks & 2) > 0;
            cQText.Checked = (Checks & 1) > 0;
            cDMult.SelectedIndex = (int)((Combos & 0xF0000000) >> 28);
            cDType.SelectedIndex = (Combos & 0xF000000) >> 24;
            cMode.SelectedIndex = (Combos & 0xFF0000) >> 16;
            cLink.SelectedIndex = (Combos & 0xFF00) >> 8;
            cTatl.SelectedIndex = Combos & 0xFF;
            cGravity.SelectedIndex = (int)((Colour & 0xF0000000) >> 28);
            cFloors.SelectedIndex = (Colour & 0xF000000) >> 24;
            bTunic.BackColor = Color.FromArgb((Colour & 0xFF0000) >> 16, (Colour & 0xFF00) >> 8, Colour & 0xFF);
        }

        private void DecodeSettings(string Settings)
        {
            SetOptions(Settings.Split('-'));
            saveROM.FileName = "MMR-" + Settings + ".z64";
            saveWad.FileName = "MMR-" + Settings + ".wad";
        }

        //form functions

        public mmrMain()
        {
            InitializeComponent();
        }

        private void EnableBoxes()
        {
            if (cMode.SelectedIndex == 2)
            {
                cMixSongs.Enabled = false;
                cSoS.Enabled = false;
                cDChests.Enabled = false;
                cDEnt.Enabled = false;
                cBottled.Enabled = false;
                cShop.Enabled = false;
                cSpoiler.Enabled = false;
                cGossip.Enabled = false;
                cAdditional.Enabled = false;
                cUserItems.Enabled = false;
            }
            else
            {
                cMixSongs.Enabled = true;
                cSoS.Enabled = true;
                cDChests.Enabled = true;
                cDEnt.Enabled = true;
                cBottled.Enabled = true;
                cShop.Enabled = true;
                cSpoiler.Enabled = true;
                cGossip.Enabled = true;
                cAdditional.Enabled = true;
                cUserItems.Enabled = true;
            };
            if (cUserItems.Checked)
            {
                cSoS.Enabled = false;
                cDChests.Enabled = false;
                cBottled.Enabled = false;
                cShop.Enabled = false;
                cAdditional.Enabled = false;
            }
            else
            {
                if (cMode.SelectedIndex != 2)
                {
                    cSoS.Enabled = true;
                    cDChests.Enabled = true;
                    cBottled.Enabled = true;
                    cShop.Enabled = true;
                    cAdditional.Enabled = true;
                };
            };
        }

        private void mmrMain_Load(object sender, EventArgs e)
        {
            // initialise some stuff
            Updating = true;
            cDMult.SelectedIndex = 0;
            cDType.SelectedIndex = 0;
            cGravity.SelectedIndex = 0;
            cFloors.SelectedIndex = 0;
            cMode.SelectedIndex = 0;
            cLink.SelectedIndex = 0;
            cTatl.SelectedIndex = 0;
            cSpoiler.Checked = true;
            cSoS.Checked = true;
            cGossip.Checked = true;
            //cQText.Checked = true;
            //cCutsc.Checked = true;
            bTunic.BackColor = Color.FromArgb(0x1E, 0x69, 0x1B);
            tSeed.Text = Math.Abs(Environment.TickCount).ToString();
            SettingOld = UpdateSettingsString();
            Updating = false;
        }

        private void bTunic_Click(object sender, EventArgs e)
        {
            Updating = true;
            cTunic.ShowDialog();
            bTunic.BackColor = cTunic.Color;
            UpdateSettingsString();
            Updating = false;
        }

        private void bopen_Click(object sender, EventArgs e)
        {
            openROM.ShowDialog();
            tROMName.Text = openROM.FileName;
        }

        private void bRandomise_Click(object sender, EventArgs e)
        {
            try
            {
                SeedRNG();
                if (cMode.SelectedIndex != 2)
                {
                    ReadRulesetItemData();
                    //check if entrance shuffle is on
                    if (cDEnt.Checked) { EntranceShuffle(); };
                    int[] OathReq = new int[] { 100, 103, 108, 113 };
                    ItemList[97].Dependence = new List<int>();
                    ItemList[97].Dependence.Add(OathReq[RNG.Next(4)]);
                    ItemShuffle();
                    //gossip
                    SeedRNG();
                    MakeGossipQuotes();
                };
                //tatl colour
                SeedRNG();
                SetTatlColour();
                //bgm sort
                SeedRNG();
                SortBGM();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error randomizing logic: {ex.Message}\r\n\r\nPlease try a different seed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Output_ROM && !File.Exists(tROMName.Text))
            {
                MessageBox.Show("Input ROM not selected or doesn't exist, cannot generate output.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Output_ROM && saveROM.ShowDialog() != DialogResult.OK)
            {
                MessageBox.Show("No output selected; ROM will not be saved.",
                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (Output_ROM && saveROM.FileName == "")
            {
                MessageBox.Show("Output file not selected.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Output_VC)
            {
                if (saveWad.ShowDialog() != DialogResult.OK)
                {
                    MessageBox.Show("Output file not selected.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                };
            };
            if (Output_ROM && !ValidateROM(tROMName.Text))
            {
                MessageBox.Show("Cannot verify input ROM is Majora's Mask (U).",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Output_ROM && !Output_VC)
            {
                if (cSpoiler.Checked)
                {
                    WriteSpoilerLog();

                    MessageBox.Show("Successfully built output spoiler log!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                else
                {
                    MessageBox.Show("No output specified!",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.None);
                }
                return;
            }

            MakeROM(tROMName.Text, saveROM.FileName);
            MessageBox.Show("Successfully built output ROM!",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        private void tSString_Enter(object sender, EventArgs e)
        {
            SettingOld = tSString.Text;
            Updating = true;
        }

        private void tSString_Leave(object sender, EventArgs e)
        {
            try
            {
                DecodeSettings(tSString.Text);
            }
            catch
            {
                tSString.Text = SettingOld;
                DecodeSettings(tSString.Text);
                MessageBox.Show("Settings string is invalid; reverted to previous settings.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            Updating = false;
        }

        private void tSString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                cDummy.Select();
            };
        }

        private void tSeed_Enter(object sender, EventArgs e)
        {
            SeedOld = Convert.ToInt32(tSeed.Text);
            Updating = true;
        }

        private void tSeed_Leave(object sender, EventArgs e)
        {
            try
            {
                int s = Convert.ToInt32(tSeed.Text);
                if (s < 0)
                {
                    s = Math.Abs(s);
                    tSeed.Text = s.ToString();
                    MessageBox.Show("Seed must be positive",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                };
            }
            catch
            {
                tSeed.Text = SeedOld.ToString();
                MessageBox.Show("Invalid seed: must be a positive integer.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            UpdateSettingsString();
            Updating = false;
        }

        private void tSeed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                cDummy.Select();
            };
        }

        private void cSpoiler_CheckedChanged(object sender, EventArgs e)
        {
            EnableBoxes();
            if (!Updating)
            {
                Updating = true;
                UpdateSettingsString();
                Updating = false;
            };
        }

        private void cMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableBoxes();
            if (!Updating)
            {
                if (cMode.SelectedIndex == 3)
                {
                    if (openLogic.ShowDialog() == DialogResult.OK)
                    {
                        //do nothing?
                    }
                    else
                    {
                        cMode.SelectedIndex = 0;
                    };
                };
                Updating = true;
                UpdateSettingsString();
                Updating = false;
            };
        }

        private void cVC_CheckedChanged(object sender, EventArgs e)
        {
            cN64.Enabled = !cVC.Checked;
            cN64.Checked = cVC.Checked || cN64.Checked;

            Output_ROM = cVC.Checked;
            Output_VC = cVC.Checked;
        }

        private void mExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void mAbout_Click(object sender, EventArgs e)
        {
            About.ShowDialog();
        }

        private void mManual_Click(object sender, EventArgs e)
        {
            Manual.Show();
        }

        private void mByteswap_Click(object sender, EventArgs e)
        {
            if (openBROM.ShowDialog() == DialogResult.OK)
            {
                int r = ROMFuncs.ByteswapROM(openBROM.FileName);
                switch (r)
                {
                    case 0:
                        MessageBox.Show("Successfully byteswapped ROM.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                        break;
                    case 1:
                        MessageBox.Show("ROM appears to be big endian.",
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.None);
                        break;
                    default:
                        MessageBox.Show("Could not byteswap ROM.",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                };
            };
        }

        private void mLogicEdit_Click(object sender, EventArgs e)
        {
            LogicEditor.Show();
        }

        private void mItemIncl_Click(object sender, EventArgs e)
        {
            ItemEditor.Show();
        }

        private void cN64_CheckedChanged(object sender, EventArgs e)
        {
            Output_ROM = cN64.Checked;
            cVC.Enabled = cN64.Checked;

            bopen.Enabled = Output_ROM;
        }
    }

}
