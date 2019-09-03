using MMRando.Models;
using MMRando.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;

namespace MMRando.Forms
{
    public partial class ItemEditForm : Form
    {
        string[] ITEM_NAMES = new string[] { "Deku Mask", "Hero's Bow", "Fire Arrow", "Ice Arrow", "Light Arrow", "Bomb Bag", "Magic Bean", 
        "Powder Keg", "Pictobox", "Lens of Truth", "Hookshot", "Great Fairy Magic Meter", "Great Fairy Spin Attack", "Great Fairy Extended Magic",
        "Great Fairy Double Defense", "Great Fairy's Sword", "Witch Bottle", "Aliens Bottle", "Gold Dust", 
        "Beaver Race Bottle", "Dampe Bottle", "Chateau Bottle", "Bombers' Notebook", "Razor Sword", "Gilded Sword", "Mirror Shield",
        "Town Archery Quiver", "Swamp Archery Quiver", "Town Bomb Bag", "Mountain Bomb Bag", "Town Wallet", "Ocean Wallet", "Moon's Tear", 
        "Land Title Deed", "Swamp Title Deed", "Mountain Title Deed", "Ocean Title Deed", "Room Key", "Letter to Kafei", "Pendant of Memories",
        "Letter to Mama", "Mayor Dotour HP", "Postman HP", "Rosa Sisters HP", "??? HP", "Grandma Short Story HP", "Grandma Long Story HP",
        "Keaton Quiz HP", "Deku Playground HP", "Town Archery HP", "Honey and Darling HP", "Swordsman's School HP", "Postbox HP",
        "Termina Field Gossips HP", "Termina Field Business Scrub HP", "Swamp Archery HP", "Pictograph Contest HP", "Boat Archery HP",
        "Frog Choir HP", "Beaver Race HP", "Seahorse HP", "Fisherman Game HP", "Evan HP", "Dog Race HP", "Poe Hut HP", 
        "Treasure Chest Game HP", "Peahat Grotto HP", "Dodongo Grotto HP", "Woodfall Chest HP", "Twin Islands Chest HP",
        "Ocean Spider House HP", "Graveyard Iron Knuckle HP", "Postman's Hat", "All Night Mask", "Blast Mask", "Stone Mask", "Great Fairy's Mask",
        "Keaton Mask", "Bremen Mask", "Bunny Hood", "Don Gero's Mask", "Mask of Scents", "Romani Mask", "Circus Leader's Mask", "Kafei's Mask",
        "Couple's Mask", "Mask of Truth", "Kamaro's Mask", "Gibdo Mask", "Garo Mask", "Captain's Hat", "Giant's Mask", "Goron Mask", "Zora Mask",
        "Song of Healing", "Song of Soaring", "Epona's Song", "Song of Storms", "Sonata of Awakening", "Goron Lullaby", "New Wave Bossa Nova",
        "Elegy of Emptiness", "Oath to Order",
        "Woodfall Map", "Woodfall Compass", "Woodfall Boss Key", "Woodfall Key 1", "Snowhead Map", "Snowhead Compass", "Snowhead Boss Key",
        "Snowhead Key 1", "Snowhead Key 2", "Snowhead Key 3", "Great Bay Map", "Great Bay Compass", "Great Bay Boss Key", "Great Bay Key 1",
        "Stone Tower Map", "Stone Tower Compass", "Stone Tower Boss Key", "Stone Tower Key 1", "Stone Tower Key 2", "Stone Tower Key 3",
        "Stone Tower Key 4", "Trading Post Red Potion", "Trading Post Green Potion", "Trading Post Shield", "Trading Post Fairy",
        "Trading Post Stick", "Trading Post Arrow 30", "Trading Post Nut 10", "Trading Post Arrow 50", "Witch Shop Blue Potion",
        "Witch Shop Red Potion", "Witch Shop Green Potion", "Bomb Shop Bomb 10", "Bomb Shop Chu 10", "Goron Shop Bomb 10", "Goron Shop Arrow 10",
        "Goron Shop Red Potion", "Zora Shop Shield", "Zora Shop Arrow 10", "Zora Shop Red Potion", "Bottle: Fairy", "Bottle: Deku Princess",
        "Bottle: Fish", "Bottle: Bug", "Bottle: Poe", "Bottle: Big Poe", "Bottle: Spring Water", "Bottle: Hot Spring Water", "Bottle: Zora Egg",
        "Bottle: Mushroom", "Lens Cave 20r", "Lens Cave 50r", "Bean Grotto 20r", "HSW Grotto 20r", "Graveyard Bad Bats", "Ikana Grotto",
        "PF 20r Lower", "PF 20r Upper", "PF Tank 20r", "PF Guard Room 100r", "PF HP Room 20r", "PF HP Room 5r", "PF Maze 20r", "PR 20r (1)", "PR 20r (2)",
        "Bombers' Hideout 100r", "Termina Bombchu Grotto", "Termina 20r Grotto", "Termina Underwater 20r", "Termina Grass 20r", "Termina Stump 20r",
        "Great Bay Coast Grotto", "Great Bay Cape Ledge (1)", "Great Bay Cape Ledge (2)", "Great Bay Cape Grotto", "Great Bay Cape Underwater", 
        "PF Exterior 20r (1)", "PF Exterior 20r (2)", "PF Exterior 20r (3)", "Path to Swamp Grotto", "Doggy Racetrack 50r", "Graveyard Grotto",
        "Swamp Grotto", "Woodfall 5r", "Woodfall 20r", "Well Right Path 50r", "Well Left Path 50r", "Mountain Village Chest (Spring)",
        "Mountain Village Grotto Bottle (Spring)", "Path to Ikana 20r", "Path to Ikana Grotto", "Stone Tower 100r", "Stone Tower Bombchu 10",
        "Stone Tower Magic Bean", "Path to Snowhead Grotto", "Twin Islands 20r", "Secret Shrine HP", "Secret Shrine Dinolfos",
        "Secret Shrine Wizzrobe", "Secret Shrine Wart", "Secret Shrine Garo Master", "Inn Staff Room", "Inn Guest Room", "Mystery Woods Grotto",
        "East Clock Town 100r", "South Clock Town 20r", "South Clock Town 50r", "Bank HP", "South Clock Town HP", "North Clock Town HP",
        "Path to Swamp HP", "Swamp Scrub HP", "Deku Palace HP", "Goron Village Scrub HP", "Bio Baba Grotto HP", "Lab Fish HP", "Great Bay Like-Like HP",
        "Pirates' Fortress HP", "Zora Hall Scrub HP", "Path to Snowhead HP", "Great Bay Coast HP", "Ikana Scrub HP", "Ikana Castle HP", 
        "Odolwa Heart Container", "Goht Heart Container", "Gyorg Heart Container", "Twinmold Heart Container", "Map: Clock Town", "Map: Woodfall",
        "Map: Snowhead", "Map: Romani Ranch", "Map: Great Bay", "Map: Stone Tower", "Goron Racetrack Grotto", "Ikana Scrub 200r", "Deku Trial HP",
        "Goron Trial HP", "Zora Trial HP", "Link Trial HP", "Fierce Deity's Mask", "Link Trial 30 Arrows", "Link Trial 10 Bombchu", "Pre-Clocktown 10 Deku Nuts",
        "Starting Sword", "Starting Shield", "Starting Heart 1", "Starting Heart 2" };

        const int NUM_ITEMS = Items.TotalNumberOfItems - Items.NumberOfAreasAndOther;
        public HashSet<int> CustomItemList = new HashSet<int>();

        public ItemEditForm(Settings settings)
        {
            InitializeComponent();
            for (int i = 0; i < ITEM_NAMES.Length; i++)
            {
                lItems.Items.Add(ITEM_NAMES[i]);
            }
            CustomItemList.UnionWith(settings.CustomItemList);
            PruneCustomItemList(CustomItemList);
        }

        private void InitializeSettings()
        {
            foreach (ListViewItem l in lItems.Items)
            {
                l.Checked = CustomItemList.Contains(l.Index);
            }
            UpdateString();
        }

        private void UpdateString()
        {
            tSetting.Text = GetCustomItemListString(CustomItemList);
            tSetting.BackColor = Color.White;
        }

        public static string GetCustomItemListString(IEnumerable<int> itemList)
        {
            int[] n = new int[8];
            string[] ns = new string[8];
            foreach (int item in itemList)
            {
                int j = item / 32;
                int k = item % 32;
                n[j] |= (int)(1 << k);
            }
            for (int i = 0; i < ns.Length; i++)
            {
                if (n[i] != 0)
                    ns[i] = Convert.ToString(n[i], 16);
            }
            return string.Join("-", ns.Reverse());
        }

        public bool TrySetCustomItemList(string itemListString)
        {
            tSetting.Text = itemListString;
            if (!TryParseCustomItemListString(itemListString, out var newItemList))
            {
                return false;
            }

            //update

            tSetting.BackColor = Color.White;
            CustomItemList = newItemList;

            foreach (ListViewItem l in lItems.Items)
            {
                l.Checked = CustomItemList.Contains(l.Index);
            }
            return true;
        }

        public static bool TryParseCustomItemListString(string c, out HashSet<int> newItemList)
        {
            newItemList = new HashSet<int>();
            try
            {
                string[] v = c.Split('-');
                int[] vi = new int[8];
                if (v.Length != vi.Length)
                {
                    return false;
                }
                for (int i = 0; i < 8; i++)
                {
                    if (v[7 - i] != "")
                    {
                        vi[i] = Convert.ToInt32(v[7 - i], 16);
                    }
                }
                for (int i = 0; i < NUM_ITEMS; i++)
                {
                    int j = i / 32;
                    int k = i % 32;
                    if (((vi[j] >> k) & 1) > 0)
                    {
                        if (i >= ItemUtils.AllLocations().Count())
                        {
                            throw new IndexOutOfRangeException();
                        }
                        newItemList.Add(i);
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        private void tSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (TrySetCustomItemList(tSetting.Text))
                {
                    tSetting.BackColor = Color.White;
                }
                else
                {
                    tSetting.BackColor = Color.Pink;
                }
            }
        }
        private void lItems_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                CustomItemList.Add(e.Index);
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                CustomItemList.Remove(e.Index);
            }
            UpdateString();
        }

        private void ItemEditForm_Load(object sender, EventArgs e)
        {
            InitializeSettings();
        }

        public static void PruneCustomItemList(ICollection<int> itemList)
        {
            foreach (var item in itemList)
            {
                if (item >= NUM_ITEMS)
                {
                    itemList.Remove(item);
                }
            }
        }


    }
}
