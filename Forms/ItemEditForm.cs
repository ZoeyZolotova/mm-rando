using MMRando.Models;
using MMRando.Models.Settings;
using MMRando.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;


namespace MMRando.Forms
{
    public partial class ItemEditForm : Form
    {
        /*Starting Items*/
        readonly string[,] STARTING_ITEMS = new string[,] { { "Starting Sword", "Starting Shield", "Starting Heart 1", "Starting Heart 2" }, 
            { "247", "248", "249", "250" } };
        /* Collectable Items */
        readonly string[,] ITEM_LIST = new string[,]  {  { "Hero's Bow", "Fire Arrow", "Ice Arrow", "Light Arrow", "Bomb Bag", "Magic Bean", "Powder Keg", "Pictobox", "Lens of Truth",
                "Hookshot", "Witch Bottle", "Aliens Bottle", "Gold Dust", "Beaver Race Bottle", "Dampe Bottle", "Chateau Bottle", "Bombers' Notebook", "Razor Sword", "Gilded Sword",
                "Mirror Shield", "Town Archery Quiver", "Swamp Archery Quiver", "Town Bomb Bag", "Mountain Bomb Bag", "Town Wallet", "Ocean Wallet", "Moon's Tear", "Land Title Deed",
                "Swamp Title Deed", "Mountain Title Deed", "Ocean Title Deed", "Room Key", "Letter to Kafei", "Pendant of Memories", "Letter to Mama" },
            { "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31",
                "32", "33", "34", "35", "36", "37", "38", "39", "40", "41" }  };
        /* Masks */
        readonly string[,] MASK_LIST = new string[,] { { "Deku Mask", "Postman's Hat", "All Night Mask", "Blast Mask", "Stone Mask", "Great Fairy's Mask", "Keaton Mask",
                "Bremen Mask", "Bunny Hood", "Don Gero's Mask", "Mask of Scents", "Romani Mask", "Circus Leader's Mask", "Kafei's Mask", "Couple's Mask", "Mask of Truth",
                "Kamaro's Mask", "Gibdo Mask", "Garo Mask", "Captain's Hat", "Giant's Mask", "Goron Mask", "Zora Mask", "Fierce Deity's Mask" },
            { "1", "73", "74", "75", "76", "77", "78", "79", "80", "81", "82", "83", "84", "85", "86", "87", "88", "89", "90", "91", "92", "93", "94", "243" } };
        /* Heart Pieces & Containers */
        readonly string[,] HEART_LOCATIONS = new string[,]  {  { "Mayor Dotour HP", "Postman HP", "Rosa Sisters HP", "??? HP", "Grandma Short Story HP", "Grandma Long Story HP",
                "Keaton Quiz HP", "Deku Playground HP", "Town Archery HP", "Honey and Darling HP", "Swordsman's School HP", "Postbox HP", "Termina Field Gossips HP",
                "Termina Field Business Scrub HP", "Swamp Archery HP", "Pictograph Contest HP", "Boat Archery HP", "Frog Choir HP", "Beaver Race HP", "Seahorse HP",
                "Fisherman Game HP", "Evan HP", "Dog Race HP", "Poe Hut HP", "Treasure Chest Game HP", "Peahat Grotto HP", "Dodongo Grotto HP", "Woodfall Chest HP",
                "Twin Islands Chest HP", "Ocean Spider House HP", "Graveyard Iron Knuckle HP", "PF HP Room 20r", "PF HP Room 5r", "Secret Shrine HP", "Bank HP",
                "South Clock Town HP", "North Clock Town HP", "Path to Swamp HP", "Swamp Scrub HP", "Deku Palace HP", "Goron Village Scrub HP", "Bio Baba Grotto HP",
                "Lab Fish HP", "Great Bay Like-Like HP", "Pirates' Fortress HP", "Zora Hall Scrub HP", "Path to Snowhead HP", "Great Bay Coast HP", "Ikana Scrub HP",
                "Ikana Castle HP", "Odolwa Heart Container", "Goht Heart Container", "Gyorg Heart Container", "Twinmold Heart Container", "Deku Trial HP", "Goron Trial HP",
                "Zora Trial HP", "Link Trial HP" },
            { "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59", "60", "61", "62", "63", "64", "65", "66", "67", "68",
                "69", "70", "71", "72", "164", "165", "200", "211", "212", "213", "214", "215", "216", "217", "218", "219", "220", "221", "222", "223", "224", "225", "226", "227",
                "228", "229", "230", "239", "240", "241", "242" } };
        /* Rupies */
        readonly string[,] RUPIE_LOCATIONS = new string[,]  { { "Lens Cave 20r", "Lens Cave 50r", "Bean Grotto 20r", "HSW Grotto 20r", "PF 20r Lower", "PF 20r Upper", "PF Tank 20r",
                "PF Guard Room 100r", "PF HP Room 20r", "PF HP Room 5r", "PF Maze 20r", "PR 20r (1)", "PR 20r (2)", "Bombers' Hideout 100r", "Termina 20r Grotto",
                "Termina Underwater 20r", "Termina Grass 20r", "Termina Stump 20r", "PF Exterior 20r (1)", "PF Exterior 20r (2)", "PF Exterior 20r (3)", "Doggy Racetrack 50r",
                "Woodfall 5r", "Woodfall 20r", "Well Right Path 50r", "Well Left Path 50r", "Path to Ikana 20r", "Stone Tower 100r", "Twin Islands 20r", "East Clock Town 100r",
                "South Clock Town 20r", "South Clock Town 50r", "Ikana Scrub 200r" },
            { "154", "155", "156", "157", "160", "161", "162", "163", "164", "165", "166", "167", "168", "169", "171", "172", "173", "174", "180", "181", "182", "184",
                "187", "188", "189", "190", "193", "195", "199", "208", "209", "210", "238" } };
        /* Tingle Maps */
        readonly string[,] TINGLE_MAPS = new string[,] { { "Clock Town", "Woodfall", "Snowhead", "Romani Ranch", "Great Bay", "Stone Tower" },
            { "231", "232", "233", "234", "235", "236" } };
        /* Songs */
        readonly string[,] SONG_LIST = new string[,]  { { "Song of Healing", "Song of Soaring", "Epona's Song", "Song of Storms",
                "Sonata of Awakening", "Goron Lullaby", "New Wave Bossa Nova", "Elegy of Emptiness", "Oath to Order" }, 
            { "95", "96", "97", "98", "99", "100", "101", "102", "103" } };
        /* Dungion Map, Compass & Small Keys */
        readonly string[,] DUNGEON_ITEMS = new string[,]  { { "Woodfall Map", "Woodfall Compass", "Woodfall Boss Key", "Woodfall Key 1", "Snowhead Map", "Snowhead Compass",
                "Snowhead Boss Key", "Snowhead Key 1", "Snowhead Key 2", "Snowhead Key 3", "Great Bay Map", "Great Bay Compass", "Great Bay Boss Key", "Great Bay Key 1", "Stone Tower Map",
                "Stone Tower Compass", "Stone Tower Boss Key", "Stone Tower Key 1", "Stone Tower Key 2", "Stone Tower Key 3", "Stone Tower Key 4" },
            { "104", "105", "106", "107", "108", "109", "110", "111", "112", "113", "114", "115", "116", "117", "118", "119", "120", "121", "122", "123", "124" } };
        /* Bottled Items */
        readonly string[,] BOTTLED_ITEMS = new string[,]  { { "Fairy", "Deku Princess", "Fish", "Bug", "Poe", "Big Poe", "Spring Water", "Hot Spring Water", "Zora Egg", "Mushroom" },
            { "144", "145", "146", "147", "148", "149", "150", "151", "152", "153" } };
        /* Shop Items */
        readonly string[,] SHOP_ITEMS = new string[,]  { { "Trading Post Red Potion", "Trading Post Green Potion", "Trading Post Shield", "Trading Post Fairy", "Trading Post Stick",
                "Trading Post Arrow 30", "Trading Post Nut 10", "Trading Post Arrow 50", "Witch Shop Blue Potion", "Witch Shop Red Potion", "Witch Shop Green Potion", "Bomb Shop Bomb 10",
                "Bomb Shop Chu 10", "Goron Shop Bomb 10", "Goron Shop Arrow 10", "Goron Shop Red Potion", "Zora Shop Shield", "Zora Shop Arrow 10", "Zora Shop Red Potion" },
            { "125", "126", "127", "128", "129", "130", "131", "132", "133", "134", "135", "136", "137", "138", "139", "140", "141", "142", "143" } };
        /* Bottled Items */
        readonly string[,] FAIRY_REWARDS = new string[,]  { { "Great Fairy Magic Meter", "Great Fairy Spin Attack", "Great Fairy Extended Magic", "Great Fairy Double Defense", "Great Fairy's Sword" }, 
            { "12", "13", "14", "15", "16" } };
        /* Anything that didn't fit in the above categories */
        readonly string[,] OTHER_ITEMS = new string[,]  { { "Graveyard Bad Bats", "Ikana Grotto", "Termina Bombchu Grotto", "Great Bay Coast Grotto", "Great Bay Cape Ledge (1)",
                "Great Bay Cape Ledge (2)", "Great Bay Cape Grotto", "Great Bay Cape Underwater", "Path to Swamp Grotto", "Graveyard Grotto", "Swamp Grotto", "Mountain Village Chest (Spring)",
                "Mountain Village Grotto Bottle (Spring)", "Path to Ikana Grotto", "Stone Tower Bombchu 10", "Stone Tower Magic Bean", "Path to Snowhead Grotto", "Secret Shrine Dinolfos",
                "Secret Shrine Wizzrobe", "Secret Shrine Wart", "Secret Shrine Garo Master", "Inn Staff Room", "Inn Guest Room", "Mystery Woods Grotto", "Goron Racetrack Grotto",
                "Link Trial 30 Arrows", "Link Trial 10 Bombchu", "Pre-Clocktown 10 Deku Nuts" },
            { "158", "159", "170", "175", "176", "177", "178", "179", "183", "185", "186", "191", "192", "194", "196", "197", "198", "201", "202",
                "203", "204", "205", "206", "207", "237", "244", "245", "246" } };

        bool updating = false;
        private readonly SettingsObject _settings;

        public ItemEditForm(SettingsObject settings)
        {
            InitializeComponent();
            _settings = settings;
            //Add each group to proper parent node

            for (int i = 0; i < STARTING_ITEMS.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[0].Nodes.Add(STARTING_ITEMS[0, i]);
                ItemListEditorTree.Nodes[0].Nodes[i].Tag = STARTING_ITEMS[1, i];
            }
            for (int i = 0; i < ITEM_LIST.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[1].Nodes.Add(ITEM_LIST[0, i]);
                ItemListEditorTree.Nodes[1].Nodes[i].Tag = ITEM_LIST[1, i];
            }
            for (int i = 0; i < MASK_LIST.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[2].Nodes.Add(MASK_LIST[0, i]);
                ItemListEditorTree.Nodes[2].Nodes[i].Tag = MASK_LIST[1, i];
            }
            for (int i = 0; i < HEART_LOCATIONS.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[3].Nodes.Add(HEART_LOCATIONS[0, i]);
                ItemListEditorTree.Nodes[3].Nodes[i].Tag = HEART_LOCATIONS[1, i];
            }
            for (int i = 0; i < RUPIE_LOCATIONS.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[4].Nodes.Add(RUPIE_LOCATIONS[0, i]);
                ItemListEditorTree.Nodes[4].Nodes[i].Tag = RUPIE_LOCATIONS[1, i];
            }
            for (int i = 0; i < TINGLE_MAPS.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[5].Nodes.Add(TINGLE_MAPS[0, i]);
                ItemListEditorTree.Nodes[5].Nodes[i].Tag = TINGLE_MAPS[1, i];
            }
            for (int i = 0; i < SONG_LIST.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[6].Nodes.Add(SONG_LIST[0, i]);
                ItemListEditorTree.Nodes[6].Nodes[i].Tag = SONG_LIST[1, i];
            }
            for (int i = 0; i < DUNGEON_ITEMS.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[7].Nodes.Add(DUNGEON_ITEMS[0, i]);
                ItemListEditorTree.Nodes[7].Nodes[i].Tag = DUNGEON_ITEMS[1, i];
            }
            for (int i = 0; i < BOTTLED_ITEMS.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[8].Nodes.Add(BOTTLED_ITEMS[0, i]);
                ItemListEditorTree.Nodes[8].Nodes[i].Tag = BOTTLED_ITEMS[1, i];
            }
            for (int i = 0; i < SHOP_ITEMS.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[9].Nodes.Add(SHOP_ITEMS[0, i]);
                ItemListEditorTree.Nodes[9].Nodes[i].Tag = SHOP_ITEMS[1, i];
            }
            for (int i = 0; i < FAIRY_REWARDS.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[10].Nodes.Add(FAIRY_REWARDS[0, i]);
                ItemListEditorTree.Nodes[10].Nodes[i].Tag = FAIRY_REWARDS[1, i];
            }
            for (int i = 0; i < OTHER_ITEMS.GetLength(1); i++)
            {
                ItemListEditorTree.Nodes[11].Nodes.Add(OTHER_ITEMS[0, i]);
                ItemListEditorTree.Nodes[11].Nodes[i].Tag = OTHER_ITEMS[1, i];
            }
            ItemListEditorTree.Nodes[0].Text = "Starting Items (" + (STARTING_ITEMS.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[1].Text = "Items (" + (ITEM_LIST.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[2].Text = "Masks (" + (MASK_LIST.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[3].Text = "Hearts (" + (HEART_LOCATIONS.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[4].Text = "Rupies (" + (RUPIE_LOCATIONS.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[5].Text = "Tingle Maps (" + (TINGLE_MAPS.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[6].Text = "Songs (" + (SONG_LIST.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[7].Text = "Dungeon Items (" + (DUNGEON_ITEMS.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[8].Text = "Bottled Items (" + (BOTTLED_ITEMS.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[9].Text = "Shop Items (" + (SHOP_ITEMS.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[10].Text = "Great Fairy Rewards (" + (FAIRY_REWARDS.GetLength(1) + 1) + ")";
            ItemListEditorTree.Nodes[11].Text = "Other Items (" + (OTHER_ITEMS.GetLength(1) + 1) + ")";

            if (_settings.CustomItemList != null) { UpdateString(_settings.CustomItemList); }
            else { tSetting.Text = "-------"; }
        }

        private void fItemEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void UpdateString(List<int> selections)
        {
            int[] n = new int[8];
            string[] ns = new string[8];
            for (int i = 0; i < selections.Count; i++)
            {
                int j = selections[i] / 32;
                int k = selections[i] % 32;
                n[j] |= (int)(1 << k);
                ns[j] = Convert.ToString(n[j], 16);
            }
            tSetting.Text = ns[7] + "-" + ns[6] + "-" + ns[5] + "-" + ns[4] + "-"
                + ns[3] + "-" + ns[2] + "-" + ns[1] + "-" + ns[0];
            _settings.CustomItemListString = tSetting.Text;
        }

        public bool UpdateChecks(string c)
        {
            _settings.CustomItemListString = c;
            _settings.CustomItemList.Clear();
            string[] v = c.Split('-');
            int[] vi = new int[8];
            if (v.Length < 7)
            {
                stringErr("Incorrect number of deviders (-)");
                return false;
            }
            for (int i = 0; i < 8; i++)
            {
                if (v[7 - i].Length > 8)
                {
                    stringErr("Too many values in section " + i);
                    return false;
                }
                else
                {
                    if (v[7 - i] != "")
                    {
                        try
                        {
                            vi[i] = Convert.ToInt32(v[7 - i], 16);
                        }
                        catch
                        {
                            stringErr("Invalid number");
                            return false;
                        }
                    }
                }
            }
            for (int i = 0; i < 256; i++)
            {
                int j = i / 32;
                int k = i % 32;
                if (((vi[j] >> k) & 1) > 0)
                {
                    if (i >= ItemUtils.AllLocations().Count())
                    {
                        stringErr("Value too high");
                        return false;
                    }
                    _settings.CustomItemList.Add(i);
                }
            }
            for (int s = 0; s < ItemListEditorTree.Nodes.Count; s++)
            {
                foreach (TreeNode n in ItemListEditorTree.Nodes[s].Nodes)
                {
                    int index = System.Convert.ToInt32(n.Tag);
                    if (_settings.CustomItemList.Contains(index)) { n.Checked = true; }
                    else { n.Checked = false; }
                    CheckParents(n.Parent);
                }
            }
            return true;
        }

        private void tSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter) { UpdateChecks(tSetting.Text); }
        }

        private void lItems_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (updating) { return; }
            updating = true;
            if (e.Item.Checked) { _settings.CustomItemList.Add(e.Item.Index); }
            else { _settings.CustomItemList.Remove(e.Item.Index); }
            UpdateString(_settings.CustomItemList);
            updating = false;
        }

        private void ItemListEditorTree_AfterCheck(object sender, TreeViewEventArgs e)
        {
            int index = System.Convert.ToInt32(e.Node.Tag);
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Parent != null)
                {
                    CheckParents(e.Node.Parent);
                    if (updating) { return; };
                    if (e.Node.Checked) { _settings.CustomItemList.Add(index); }
                    else { _settings.CustomItemList.Remove(index); }
                }
                else { RecursiveChildCheck(e.Node, e.Node.Checked); }
                updating = false;
            }
            UpdateString(_settings.CustomItemList);
        }

        private void RecursiveChildCheck(TreeNode treeNode, bool parentChecked)
        {
            foreach (TreeNode childNode in treeNode.Nodes)
            {
                int index = System.Convert.ToInt32(childNode.Tag);
                if (parentChecked && !childNode.Checked)
                {
                    childNode.Checked = true;
                    _settings.CustomItemList.Add(index);
                }
                else if (!parentChecked && childNode.Checked)
                {
                    childNode.Checked = false;
                    _settings.CustomItemList.Remove(index);
                }
                if (childNode.Nodes.Count > 0)
                { RecursiveChildCheck(childNode, parentChecked); }
            }
        }

        private void CheckParents(TreeNode eNode)
        {
            bool check = true;
            for (int sn = 0; sn < eNode.Nodes.Count; sn++)
            {
                if (!eNode.Nodes[sn].Checked) { check = false; }
            }
            eNode.Checked = check;
        }

        private void ExpandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int n = 0; n < ItemListEditorTree.Nodes.Count; n++)
            { ItemListEditorTree.Nodes[n].Expand(); }
        }

        private void CollapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int n = 0; n < ItemListEditorTree.Nodes.Count; n++)
            { ItemListEditorTree.Nodes[n].Collapse(); }
        }

        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tSetting.Text = "3ffffff-ffffffff-ffffffff-ffffffff-ffffffff-ffffffff-ffffffff-ffffffff";
            UpdateChecks(tSetting.Text);
        }

        private void SelectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tSetting.Text = "-------";
            UpdateChecks(tSetting.Text);
        }

        public void stringErr(string r)
        {
            MessageBox.Show("Invalid custom item string: \n" + r);
            tSetting.Text = "-------";
            _settings.CustomItemListString = "-------";
            _settings.CustomItemList.Clear();
        }
    }
}