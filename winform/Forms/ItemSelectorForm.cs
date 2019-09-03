using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MMRando
{
    public partial class ItemSelectorForm : Form
    {
        private readonly static string[] DEFAULT_ITEM_NAMES = new string[] 
        {
            "Deku Mask",
            "Hero's Bow",
            "Fire Arrow",
            "Ice Arrow",
            "Light Arrow",
            "Bomb Bag (20)",
            "Magic Bean",
            "Powder Keg",
            "Pictobox",
            "Lens of Truth",
            "Hookshot",
            "Great Fairy Magic Meter",
            "Great Fairy Spin Attack",
            "Great Fairy Extended Magic",
            "Great Fairy Double Defense",
            "Great Fairy's Sword",
            "Witch Bottle",
            "Aliens Bottle",
            "Gold Dust",
            "Beaver Race Bottle",
            "Dampe Bottle",
            "Chateau Bottle",
            "Bombers' Notebook",
            "Razor Sword",
            "Gilded Sword",
            "Mirror Shield",
            "Town Archery Quiver (40)",
            "Swamp Archery Quiver (50)",
            "Town Bomb Bag (30)",
            "Mountain Bomb Bag (40)",
            "Town Wallet (200)",
            "Ocean Wallet (500)",
            "Moon's Tear",
            "Land Title Deed",
            "Swamp Title Deed",
            "Mountain Title Deed",
            "Ocean Title Deed",
            "Room Key",
            "Letter to Kafei",
            "Pendant of Memories",
            "Letter to Mama",
            "Mayor Dotour HP",
            "Postman HP",
            "Rosa Sisters HP",
            "??? HP",
            "Grandma Short Story HP",
            "Grandma Long Story HP",
            "Keaton Quiz HP",
            "Deku Playground HP",
            "Town Archery HP",
            "Honey and Darling HP",
            "Swordsman's School HP",
            "Postbox HP",
            "Termina Field Gossips HP",
            "Termina Field Business Scrub HP",
            "Swamp Archery HP",
            "Pictograph Contest HP",
            "Boat Archery HP",
            "Frog Choir HP",
            "Beaver Race HP",
            "Seahorse HP",
            "Fisherman Game HP",
            "Evan HP",
            "Dog Race HP",
            "Poe Hut HP",
            "Treasure Chest Game HP",
            "Peahat Grotto HP",
            "Dodongo Grotto HP",
            "Woodfall Chest HP",
            "Twin Islands Chest HP",
            "Ocean Spider House HP",
            "Graveyard Iron Knuckle HP",
            "Postman's Hat",
            "All Night Mask",
            "Blast Mask",
            "Stone Mask",
            "Great Fairy's Mask",
            "Keaton Mask",
            "Bremen Mask",
            "Bunny Hood",
            "Don Gero's Mask",
            "Mask of Scents",
            "Romani Mask",
            "Circus Leader's Mask",
            "Kafei's Mask",
            "Couple's Mask",
            "Mask of Truth",
            "Kamaro's Mask",
            "Gibdo Mask",
            "Garo Mask",
            "Captain's Hat",
            "Giant's Mask",
            "Goron Mask",
            "Zora Mask",
            "Song of Healing",
            "Song of Soaring",
            "Epona's Song",
            "Song of Storms",
            "Sonata of Awakening",
            "Goron Lullaby",
            "New Wave Bossa Nova",
            "Elegy of Emptiness",
            "Oath to Order",
            "Poison swamp access",
            "Woodfall Temple access",
            "Woodfall clear",
            "North access",
            "Snowhead Temple access",
            "Snowhead clear",
            "Epona access",
            "West access",
            "Pirates' Fortress access",
            "Great Bay Temple access",
            "Great Bay clear",
            "East access",
            "Ikana Canyon access",
            "Stone Tower Temple access",
            "Inverted Stone Tower Temple access",
            "Ikana clear",
            "Explosives",
            "Arrows",
            "(Unused)",
            "(Unused)",
            "(Unused)",
            "(Unused)",
            "(Unused)",
            "Woodfall Map",
            "Woodfall Compass",
            "Woodfall Boss Key",
            "Woodfall Key 1",
            "Snowhead Map",
            "Snowhead Compass",
            "Snowhead Boss Key",
            "Snowhead Key 1 - block room",
            "Snowhead Key 2 - icicle room",
            "Snowhead Key 3 - bridge room",
            "Great Bay Map",
            "Great Bay Compass",
            "Great Bay Boss Key",
            "Great Bay Key 1",
            "Stone Tower Map",
            "Stone Tower Compass",
            "Stone Tower Boss Key",
            "Stone Tower Key 1 - armos room",
            "Stone Tower Key 2 - eyegore room",
            "Stone Tower Key 3 - updraft room",
            "Stone Tower Key 4 - death armos maze",
            "Trading Post Red Potion",
            "Trading Post Green Potion",
            "Trading Post Shield",
            "Trading Post Fairy",
            "Trading Post Stick",
            "Trading Post Arrow 30",
            "Trading Post Nut 10",
            "Trading Post Arrow 50",
            "Witch Shop Blue Potion",
            "Witch Shop Red Potion",
            "Witch Shop Green Potion",
            "Bomb Shop Bomb 10",
            "Bomb Shop Chu 10",
            "Goron Shop Bomb 10",
            "Goron Shop Arrow 10",
            "Goron Shop Red Potion",
            "Zora Shop Shield",
            "Zora Shop Arrow 10",
            "Zora Shop Red Potion",
            "Bottle: Fairy",
            "Bottle: Deku Princess",
            "Bottle: Fish",
            "Bottle: Bug",
            "Bottle: Poe",
            "Bottle: Big Poe",
            "Bottle: Spring Water",
            "Bottle: Hot Spring Water",
            "Bottle: Zora Egg",
            "Bottle: Mushroom",
            "Lens Cave 20r",
            "Lens Cave 50r",
            "Bean Grotto 20r",
            "HSW Grotto 20r",
            "Graveyard Bad Bats",
            "Ikana Grotto",
            "PF 20r Lower",
            "PF 20r Upper",
            "PF Tank 20r",
            "PF Guard Room 100r",
            "PF HP Room 20r",
            "PF HP Room 5r",
            "PF Maze 20r",
            "PR 20r (1)",
            "PR 20r (2)",
            "Bombers' Hideout 100r",
            "Termina Bombchu Grotto",
            "Termina 20r Grotto",
            "Termina Underwater 20r",
            "Termina Grass 20r",
            "Termina Stump 20r",
            "Great Bay Coast Grotto",
            "Great Bay Cape Ledge (1)",
            "Great Bay Cape Ledge (2)",
            "Great Bay Cape Grotto",
            "Great Bay Cape Underwater",
            "PF Exterior 20r (1)",
            "PF Exterior 20r (2)",
            "PF Exterior 20r (3)",
            "Path to Swamp Grotto",
            "Doggy Racetrack 50r",
            "Graveyard Grotto",
            "Swamp Grotto",
            "Woodfall 5r",
            "Woodfall 20r",
            "Well Right Path 50r",
            "Well Left Path 50r",
            "Mountain Village Chest (Spring)",
            "Mountain Village Grotto Bottle (Spring)",
            "Path to Ikana 20r",
            "Path to Ikana Grotto",
            "Stone Tower 100r",
            "Stone Tower Bombchu 10",
            "Stone Tower Magic Bean",
            "Path to Snowhead Grotto",
            "Twin Islands 20r",
            "Secret Shrine HP",
            "Secret Shrine Dinolfos",
            "Secret Shrine Wizzrobe",
            "Secret Shrine Wart",
            "Secret Shrine Garo Master",
            "Inn Staff Room",
            "Inn Guest Room",
            "Mystery Woods Grotto",
            "East Clock Town 100r",
            "South Clock Town 20r",
            "South Clock Town 50r",
            "Bank HP",
            "South Clock Town HP",
            "North Clock Town HP",
            "Path to Swamp HP",
            "Swamp Scrub HP",
            "Deku Palace HP",
            "Goron Village Scrub HP",
            "Bio Baba Grotto HP",
            "Lab Fish HP",
            "Great Bay Like-Like HP",
            "Pirates' Fortress HP",
            "Zora Hall Scrub HP",
            "Path to Snowhead HP",
            "Great Bay Coast HP",
            "Ikana Scrub HP",
            "Ikana Castle HP",
            "Odolwa Heart Container",
            "Goht Heart Container",
            "Gyorg Heart Container",
            "Twinmold Heart Container",
            "Map: Clock Town",
            "Map: Woodfall",
            "Map: Snowhead",
            "Map: Romani Ranch",
            "Map: Great Bay",
            "Map: Stone Tower",
            "Goron Racetrack Grotto",
            "Ikana Scrub 200r",
            "One Mask",
            "Two Masks",
            "Three Masks",
            "Four Masks",
            "Moon Access",
            "Deku Trial HP",
            "Goron Trial HP",
            "Zora Trial HP",
            "Link Trial HP",
            "Fierce Deity's Mask",
        };

        private static string[] ITEM_NAMES = DEFAULT_ITEM_NAMES.ToArray();

        public static void AddItem(string itemName)
        {
            var newList = ITEM_NAMES.ToList();
            newList.Add(itemName);
            ITEM_NAMES = newList.ToArray();
        }

        public static void ResetItems()
        {
            ITEM_NAMES = DEFAULT_ITEM_NAMES.ToArray();
        }

        public static List<int> ReturnItems;

        public ItemSelectorForm(List<int> selectedItems = null, bool checkboxes = true, List<int> highlightedItems = null)
        {
            InitializeComponent();
            for (int i = 0; i < ITEM_NAMES.Length; i++)
            {
                var item = new ListViewItem(ITEM_NAMES[i]);
                item.Checked = selectedItems?.Contains(i) ?? false;
                lItems.Items.Add(item);
                if (highlightedItems != null)
                {
                    item.ForeColor = highlightedItems.Contains(i)
                        ? Color.Black
                        : Color.LightGray;
                }
            };
            this.ActiveControl = textBoxFilter;
            lItems.CheckBoxes = checkboxes;
        }

        private void bDone_Click(object sender, EventArgs e)
        {
            ReturnItems = new List<int>();
            foreach (ListViewItem l in lItems.Items)
            {
                if (l.Checked)
                {
                    ReturnItems.Add(l.Index);
                };
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void textBoxFilter_TextChanged(object sender, EventArgs e)
        {
            var filter = textBoxFilter.Text.ToLower();
            foreach (var item in lItems.Items.Cast<ListViewItem>())
            {
                item.ForeColor = item.Text.ToLower().Contains(filter)
                    ? Color.Black
                    : Color.LightGray;
            }
        }

        private void lItems_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lItems.CheckBoxes)
            {
                return;
            }
            ReturnItems = lItems.SelectedIndices.Cast<int>().ToList();
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
