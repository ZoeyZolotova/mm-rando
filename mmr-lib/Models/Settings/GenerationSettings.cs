using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace MMRando.Models
{
    /// <summary>
    /// Stores all settings that should affect seed generation
    /// </summary>
    public class GenerationSettings : INotifyPropertyChanged
    {

        /// <summary>
        /// The randomizer seed
        /// </summary>
        public string Seed
        {
            get => seed;
            set => SetField(ref seed, value);
        }
        private string seed = "";

        /// <summary>
        /// Selected mode of logic (affects randomization rules)
        /// </summary>
        public LogicMode LogicMode
        {
            get => logicMode;
            set => SetField(ref logicMode, value);
        }
        private LogicMode logicMode = LogicMode.Casual;

        /// <summary>
        /// Use Custom Item list for the logic.
        /// </summary>
        public bool UseCustomItemList
        {
            get => useCustomItemList;
            set
            {
                SetField(ref useCustomItemList, value);

                //Todo: enable/disable ui elements instead, then have a "validate" stage for generating settings
                //this.AddDungeonItems = false; // cDChests.Checked
                //this.AddShopItems = false; // cShop.Checked
                //this.RandomizeBottleCatchContents = false; //cBottled.Checked
                //this.ExcludeSongOfSoaring = false; // cSoS.Checked
                //this.AddOtherItems = false; //cAdditional.Checked
                //this.AddMoonItems = false; //cMoonItems.Checked
                //this.AddNutChest = false; //cNutChest.Checked = false;
                //this.NoStartingItems = false; //cStartingItems.Checked = false;
            }
        }
        private bool useCustomItemList;

        /// <summary>
        /// Add songs to the randomization pool
        /// </summary>
        public bool AddSongs
        {
            get => addSongs;
            set => SetField(ref addSongs, value);
        }
        private bool addSongs;

        /// <summary>
        /// (KeySanity) Add dungeon items (maps, compasses, keys) to the randomization pool
        /// </summary>
        public bool AddDungeonItems
        {
            get => (UseCustomItemList) ? false : addDungeonItems;
            set => SetField(ref addDungeonItems, value);
        }
        private bool addDungeonItems;

        /// <summary>
        /// Add shop items to the randomization pool
        /// </summary>
        public bool AddShopItems
        {
            get => (UseCustomItemList) ? false : addShopItems;
            set => SetField(ref addShopItems, value);
        }
        private bool addShopItems;

        /// <summary>
        /// Add moon items to the randomization pool
        /// </summary>
        public bool AddMoonItems
        {
            get => (UseCustomItemList) ? false : addMoonItems;
            set => SetField(ref addMoonItems, value);
        }
        private bool addMoonItems;

        /// <summary>
        /// Add great fairy rewards to the randomization pool
        /// </summary>
        public bool AddFairyRewards
        {
            get => (UseCustomItemList) ? false : addFairyRewards;
            set => SetField(ref addFairyRewards, value);
        }
        private bool addFairyRewards;

        /// <summary>
        /// Add everything else to the randomization pool
        /// </summary>
        public bool AddOtherItems
        {
            get => (UseCustomItemList) ? false : addOtherItems;
            set => SetField(ref addOtherItems, value);
        }
        private bool addOtherItems = true;

        /// <summary>
        /// Add pre-clocktown nut chest to the randomization pool
        /// </summary>
        public bool AddNutChest
        {
            get => (UseCustomItemList) ? false : addNutChest;
            set => SetField(ref addNutChest, value);
        }
        private bool addNutChest;

        /// <summary>
        /// Add starting sword/shield/heart containers to the randomization pool
        /// </summary>
        public bool CrazyStartingItems
        {
            get => (UseCustomItemList) ? false : crazyStartingItems;
            set => SetField(ref crazyStartingItems, value);
        }
        private bool crazyStartingItems;


        /// <summary>
        /// Randomize the content of a bottle when catching (e.g. catching a fairy puts poe in bottle)
        /// </summary>
        public bool RandomizeBottleCatchContents
        {
            get => (UseCustomItemList) ? false : randomizeBottleCatchContents;
            set => SetField(ref randomizeBottleCatchContents, value);
        }
        private bool randomizeBottleCatchContents;

        /// <summary>
        /// Exclude song of soaring from randomization (it will be found in vanilla location)
        /// </summary>
        public bool ExcludeSongOfSoaring
        {
            get => (UseCustomItemList) ? false : excludeSongOfSoaring;
            set => SetField(ref excludeSongOfSoaring, value);
        }
        private bool excludeSongOfSoaring = true;

        /// <summary>
        /// Gossip stones give hints on where to find items, and sometimes junk
        /// </summary>
        public GossipHintStyle GossipHintStyle
        {
            get => gossipHintStyle;
            set => SetField(ref gossipHintStyle, value);
        }
        private GossipHintStyle gossipHintStyle = GossipHintStyle.Default;

        /// <summary>
        /// FrEe HiNtS FoR WeNiEs
        /// </summary>
        public bool FreeHints
        {
            get => freeHints;
            set => SetField(ref freeHints, value);
        }
        private bool freeHints;

        /// <summary>
        /// Clear hints
        /// </summary>
        public bool ClearHints
        {
            get => clearHints;
            set => SetField(ref clearHints, value);
        }
        private bool clearHints;

        /// <summary>
        /// Randomize which dungeon you appear in when entering one
        /// </summary>
        public bool RandomizeDungeonEntrances
        {
            get => randomizeDungeonEntrances;
            set => SetField(ref randomizeDungeonEntrances, value);
        }
        private bool randomizeDungeonEntrances;

        /// <summary>
        /// (Beta) Randomize enemies
        /// </summary>
        public bool RandomizeEnemies
        {
            get => randomizeEnemies;
            set => SetField(ref randomizeEnemies, value);
        }
        private bool randomizeEnemies;


        /// <summary>
        /// Prevents player starting with any items that are randomized.
        /// </summary>
        public bool NoStartingItems
        {
            get => (UseCustomItemList || AddOtherItems) ? noStartingItems : false;
            set => SetField(ref noStartingItems, value);
        }
        private bool noStartingItems = true;


        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            Debug.WriteLine($"Update {propertyName}: {field} -> {value}");
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                OnPropertyChanged(propertyName);
            }

        }
        #endregion
    }
}