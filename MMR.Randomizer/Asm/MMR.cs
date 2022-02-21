using Be.IO;
using MMR.Common.Extensions;
using MMR.Randomizer.GameObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace MMR.Randomizer.Asm
{
    /// <summary>
    /// MMR configuration structure.
    /// </summary>
    public struct MMRConfigStruct : IAsmConfigStruct
    {
        public uint Version;
        public ushort[] RupeeRepeatableLocations;
        public ushort RupeeRepeatableLocationsLength;

        public ushort LocationBottleRedPotion;
        public ushort LocationBottleGoldDust;
        public ushort LocationBottleMilk;
        public ushort LocationBottleChateau;

        public ushort LocationSwordKokiri;
        public ushort LocationSwordRazor;
        public ushort LocationSwordGilded;

        public ushort LocationMagicSmall;
        public ushort LocationMagicLarge;

        public ushort LocationWalletAdult;
        public ushort LocationWalletGiant;
        public ushort LocationWalletRoyal;

        public ushort LocationBombBagSmall;
        public ushort LocationBombBagBig;
        public ushort LocationBombBagBiggest;

        public ushort LocationQuiverSmall;
        public ushort LocationQuiverLarge;
        public ushort LocationQuiverLargest;

        public byte ExtraStartingMaps;
        public byte[] ExtraStartingItemIds;
        public ushort ExtraStartingItemIdsLength;

        public ushort[] ItemsToReturnIds;
        public ushort ItemsToReturnIdsLength;

        /// <summary>
        /// Convert to bytes.
        /// </summary>
        /// <returns>Bytes</returns>
        public byte[] ToBytes()
        {
            using (var memStream = new MemoryStream())
            using (var writer = new BeBinaryWriter(memStream))
            {
                writer.WriteUInt32(this.Version);

                foreach (var val in this.RupeeRepeatableLocations)
                {
                    writer.WriteUInt16(val);
                }

                writer.WriteUInt16(RupeeRepeatableLocationsLength);

                writer.WriteUInt16(this.LocationBottleRedPotion);
                writer.WriteUInt16(this.LocationBottleGoldDust);
                writer.WriteUInt16(this.LocationBottleMilk);
                writer.WriteUInt16(this.LocationBottleChateau);

                writer.WriteUInt16(this.LocationSwordKokiri);
                writer.WriteUInt16(this.LocationSwordRazor);
                writer.WriteUInt16(this.LocationSwordGilded);

                writer.WriteUInt16(this.LocationMagicSmall);
                writer.WriteUInt16(this.LocationMagicLarge);

                writer.WriteUInt16(this.LocationWalletAdult);
                writer.WriteUInt16(this.LocationWalletGiant);
                writer.WriteUInt16(this.LocationWalletRoyal);

                writer.WriteUInt16(this.LocationBombBagSmall);
                writer.WriteUInt16(this.LocationBombBagBig);
                writer.WriteUInt16(this.LocationBombBagBiggest);

                writer.WriteUInt16(this.LocationQuiverSmall);
                writer.WriteUInt16(this.LocationQuiverLarge);
                writer.WriteUInt16(this.LocationQuiverLargest);

                writer.WriteByte(this.ExtraStartingMaps);
                writer.WriteByte(0); // padding
                writer.WriteBytes(this.ExtraStartingItemIds);
                writer.WriteUInt16(this.ExtraStartingItemIdsLength);
                foreach (var val in this.ItemsToReturnIds)
                {
                    writer.WriteUInt16(val);
                }
                writer.WriteUInt16(this.ItemsToReturnIdsLength);

                return memStream.ToArray();
            }
        }
    }

    /// <summary>
    /// MMR configuration.
    /// </summary>
    public class MMRConfig : AsmConfig
    {
        public List<ushort> RupeeRepeatableLocations { get; }

        public ushort LocationBottleRedPotion { get; set; }
        public ushort LocationBottleGoldDust { get; set; }
        public ushort LocationBottleMilk { get; set; }
        public ushort LocationBottleChateau { get; set; }

        public ushort LocationSwordKokiri { get; set; }
        public ushort LocationSwordRazor { get; set; }
        public ushort LocationSwordGilded { get; set; }

        public ushort LocationMagicSmall { get; set; }
        public ushort LocationMagicLarge { get; set; }

        public ushort LocationWalletAdult { get; set; }
        public ushort LocationWalletGiant { get; set; }
        public ushort LocationWalletRoyal { get; set; }

        public ushort LocationBombBagSmall { get; set; }
        public ushort LocationBombBagBig { get; set; }
        public ushort LocationBombBagBiggest { get; set; }

        public ushort LocationQuiverSmall { get; set; }
        public ushort LocationQuiverLarge { get; set; }
        public ushort LocationQuiverLargest { get; set; }

        public TingleMap ExtraStartingMaps { get; set; }

        public List<byte> ExtraStartingItemIds { get; set; }

        public List<ushort> ItemsToReturnIds { get; set; }

        public MMRConfig()
        {
            ExtraStartingItemIds = new List<byte>();
            RupeeRepeatableLocations = new List<ushort>();
            ItemsToReturnIds = new List<ushort>();
        }

        /// <summary>
        /// Get a <see cref="MiscConfigStruct"/> representation.
        /// </summary>
        /// <param name="version">Structure version</param>
        /// <returns>Configuration structure</returns>
        public override IAsmConfigStruct ToStruct(uint version)
        {
            if (this.ExtraStartingItemIds.Count > 0x10)
            {
                throw new ArgumentException($"{nameof(ExtraStartingItemIds)} is too large.");
            }

            var endBuffer = 0x80 - this.RupeeRepeatableLocations.Count;
            if (endBuffer < 0)
            {
                throw new ArgumentException($"{nameof(RupeeRepeatableLocations)} is too large.");
            }

            var rupeeRepeatableLocations = this.RupeeRepeatableLocations.Concat(Enumerable.Repeat((ushort)0, endBuffer)).ToArray();
            var extraStartingItemIds = this.ExtraStartingItemIds.ToArray();
            Array.Resize(ref extraStartingItemIds, 0x10);

            var itemsToReturnIds = this.ItemsToReturnIds.ToArray();
            Array.Resize(ref itemsToReturnIds, 0x10);
            return new MMRConfigStruct
            {
                Version = version,
                RupeeRepeatableLocations = rupeeRepeatableLocations,
                RupeeRepeatableLocationsLength = (ushort)rupeeRepeatableLocations.Length,

                LocationBottleRedPotion = this.LocationBottleRedPotion,
                LocationBottleGoldDust = this.LocationBottleGoldDust,
                LocationBottleMilk = this.LocationBottleMilk,
                LocationBottleChateau = this.LocationBottleChateau,

                LocationSwordKokiri = this.LocationSwordKokiri,
                LocationSwordRazor = this.LocationSwordRazor,
                LocationSwordGilded = this.LocationSwordGilded,

                LocationMagicSmall = this.LocationMagicSmall,
                LocationMagicLarge = this.LocationMagicLarge,

                LocationWalletAdult = this.LocationWalletAdult,
                LocationWalletGiant = this.LocationWalletGiant,
                LocationWalletRoyal = this.LocationWalletRoyal,

                LocationBombBagSmall = this.LocationBombBagSmall,
                LocationBombBagBig = this.LocationBombBagBig,
                LocationBombBagBiggest = this.LocationBombBagBiggest,

                LocationQuiverSmall = this.LocationQuiverSmall,
                LocationQuiverLarge = this.LocationQuiverLarge,
                LocationQuiverLargest = this.LocationQuiverLargest,

                ExtraStartingMaps = (byte)this.ExtraStartingMaps,
                ExtraStartingItemIds = extraStartingItemIds,
                ExtraStartingItemIdsLength = (ushort)this.ExtraStartingItemIds.Count,

                ItemsToReturnIds = itemsToReturnIds,
                ItemsToReturnIdsLength = (ushort)this.ItemsToReturnIds.Count,
            };
        }
    }
}
