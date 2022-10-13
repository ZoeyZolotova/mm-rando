using MMR.Common.Extensions;
using MMR.Randomizer.Asm;
using MMR.Randomizer.Attributes;
using MMR.Randomizer.Constants;
using MMR.Randomizer.Extensions;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.Models;
using MMR.Randomizer.Models.Rom;
using MMR.Randomizer.Models.Settings;
using System.Collections.Generic;
using System.Linq;

namespace MMR.Randomizer.Utils
{
    public static class ItemSwapUtils
    {
        const int BOTTLE_CATCH_TABLE = 0xCD7C08;
        static int GET_ITEM_TABLE = 0;
        public static ushort COLLECTABLE_TABLE_FILE_INDEX { get; private set; } = 0;

        /// <summary>
        /// Get the file index of <c>gi-table</c>.
        /// </summary>
        static int GiTableIndex => RomData.Files.ResolveExactIndex((uint)GET_ITEM_TABLE);

        public static void ReplaceGetItemTable()
        {
            var code = RomData.Files.GetCached(FileIndex.code.ToInt());

            // Handle gi-table.
            ResourceUtils.ApplyHack(Resources.mods.replace_gi_table);
            var giTable = RomData.Files.Append(Resources.mods.gi_table);
            GET_ITEM_TABLE = (int)giTable.AddressRange.Start;
            ReadWriteUtils.WriteS32(code.ToSpan(0xBDAEAC), giTable.Index);

            // Handle chest-table.
            ResourceUtils.ApplyHack(Resources.mods.update_chests);
            var chestTable = RomData.Files.Append(Resources.mods.chest_table);
            ReadWriteUtils.WriteS32(code.ToSpan(0xBDAEA8), chestTable.Index);

            // Handle collectable-table.
            var collectableTable = RomData.Files.Append(Resources.mods.collectable_table);
            COLLECTABLE_TABLE_FILE_INDEX = (ushort)collectableTable.Index;

            ResourceUtils.ApplyHack(Resources.mods.standing_hearts);
            ResourceUtils.ApplyHack(Resources.mods.fix_item_checks);
            SceneUtils.ResetSceneFlagMask();
            SceneUtils.UpdateSceneFlagMask(0x5B); // red potion
            SceneUtils.UpdateSceneFlagMask(0x91); // chateau romani
            SceneUtils.UpdateSceneFlagMask(0x92); // milk
            SceneUtils.UpdateSceneFlagMask(0x93); // gold dust
        }

        private static void InitGetBottleList()
        {
            RomData.BottleList = new Dictionary<int, BottleCatchEntry>();
            var fileData = RomData.Files.GetReadOnlySpanAt(BOTTLE_CATCH_TABLE);
            foreach (var getBottleItemIndex in ItemUtils.AllGetBottleItemIndices())
            {
                int offset = getBottleItemIndex * 6;
                RomData.BottleList[getBottleItemIndex] = new BottleCatchEntry
                {
                    ItemGained = fileData[offset + 3],
                    Index = fileData[offset + 4],
                    Message = fileData[offset + 5]
                };
            }
        }

        private static void InitGetItemList()
        {
            RomData.GetItemList = new Dictionary<int, GetItemEntry>();
            var fileData = RomData.Files.GetReadOnlySpan(GiTableIndex);
            for (var i = 0; i < fileData.Length; i += 8)
            {
                var getItemIndex = (i / 8) + 1;
                RomData.GetItemList[getItemIndex] = new GetItemEntry
                {
                    ItemGained = fileData[i],
                    Flag = fileData[i + 1],
                    Index = fileData[i + 2],
                    Type = fileData[i + 3],
                    Message = (short)((fileData[i + 4] << 8) | fileData[i + 5]),
                    Object = (short)((fileData[i + 6] << 8) | fileData[i + 7])
                };
            }
        }

        public static void InitItems()
        {
            InitGetItemList();
            InitGetBottleList();
        }

        public static void WriteNewBottle(Item location, Item item)
        {
            System.Diagnostics.Debug.WriteLine($"Writing {item.Name()} --> {location.Location()}");

            var span = RomData.Files.GetSpanAt(BOTTLE_CATCH_TABLE);
            foreach (var index in location.GetBottleItemIndices())
            {
                var offset = index * 6;
                var newBottle = RomData.BottleList[item.GetBottleItemIndices()[0]];
                var data = new byte[]
                {
                    newBottle.ItemGained,
                    newBottle.Index,
                    newBottle.Message,
                };
                var dest = span.Slice(offset + 3, data.Length);
                ReadWriteUtils.WriteExact(dest, data);
            }
        }

        public static void WriteNewItem(ItemObject itemObject, List<MessageEntry> newMessages, GameplaySettings settings, ChestTypeAttribute.ChestType? overrideChestType, MessageTable messageTable, ExtendedObjects extendedObjects)
        {
            var item = itemObject.Item;
            var location = itemObject.NewLocation.Value;
            System.Diagnostics.Debug.WriteLine($"Writing {item.Name()} --> {location.Location()}");

            var span = RomData.Files.GetSpan(COLLECTABLE_TABLE_FILE_INDEX);
            if (!itemObject.IsRandomized)
            {
                var indices = location.GetCollectableIndices();
                if (indices.Any())
                {
                    foreach (var collectableIndex in location.GetCollectableIndices())
                    {
                        ReadWriteUtils.Arr_WriteU16(span, collectableIndex * 2, 0);
                    }
                    return;
                }
            }

            var fileData = RomData.Files.GetSpan(GiTableIndex);
            var getItemIndex = location.GetItemIndex().Value;
            int offset = (getItemIndex - 1) * 8;

            GetItemEntry newItem;
            if (!itemObject.IsRandomized && location.IsNullableItem())
            {
                newItem = new GetItemEntry();
            }
            else if (item.IsExclusiveItem())
            {
                newItem = item.ExclusiveItemEntry();
            }
            else
            {
                newItem = RomData.GetItemList[item.GetItemIndex().Value];
            }

            // Attempt to resolve extended object Id, which should affect "Exclusive Items" as well.
            var graphics = extendedObjects.ResolveGraphics(newItem);
            if (graphics.HasValue)
            {
                newItem.Object = graphics.Value.objectId;
                newItem.Index = graphics.Value.graphicId;
            }

            var data = new byte[]
            {
                newItem.ItemGained,
                newItem.Flag,
                newItem.Index,
                newItem.Type,
                (byte)(newItem.Message >> 8),
                (byte)(newItem.Message & 0xFF),
                (byte)(newItem.Object >> 8),
                (byte)(newItem.Object & 0xFF),
            };
            ReadWriteUtils.Write(fileData.Slice(offset), data);

            int? refillGetItemIndex = item switch
            {
                Item.ItemBottleMadameAroma => 0x91,
                Item.ItemBottleAliens => 0x92,
                _ => null,
            };

            if (refillGetItemIndex.HasValue)
            {
                var refillItem = RomData.GetItemList[refillGetItemIndex.Value];
                var refillGraphics = extendedObjects.ResolveGraphics(refillItem);
                if (refillGraphics.HasValue)
                {
                    refillItem.Object = refillGraphics.Value.objectId;
                    refillItem.Index = refillGraphics.Value.graphicId;
                }
                var refillData = new byte[]
                {
                    refillItem.ItemGained,
                    refillItem.Flag,
                    refillItem.Index,
                    refillItem.Type,
                    (byte)(refillItem.Message >> 8),
                    (byte)(refillItem.Message & 0xFF),
                    (byte)(refillItem.Object >> 8),
                    (byte)(refillItem.Object & 0xFF),
                };
                var refillOffset = (refillGetItemIndex.Value - 1) * 8;
                ReadWriteUtils.Write(fileData.Slice(refillOffset), refillData);
            }

            if (location.IsRupeeRepeatable())
            {
                settings.AsmOptions.MMRConfig.RupeeRepeatableLocations.Add(getItemIndex);
            }

            var isRepeatable = item.IsRepeatable(settings) || (!settings.PreventDowngrades && item.IsDowngradable());
            if (settings.ProgressiveUpgrades && item.HasAttribute<ProgressiveAttribute>())
            {
                isRepeatable = false;
            }
            if (item.IsReturnable(settings))
            {
                isRepeatable = false;
                settings.AsmOptions.MMRConfig.ItemsToReturnIds.Add(getItemIndex);
            }
            if (!isRepeatable)
            {
                SceneUtils.UpdateSceneFlagMask(getItemIndex);
            }

            if (settings.UpdateChests)
            {
                UpdateChest(location, item, overrideChestType);
            }

            if (settings.UpdateShopAppearance)
            {
                UpdateShop(itemObject, newMessages, messageTable);
            }

            if (itemObject.IsRandomized)
            {
                var hackContentAttributes = location.GetAttributes<HackContentAttribute>();
                if (location == item)
                {
                    hackContentAttributes = hackContentAttributes.Where(h => !h.ApplyOnlyIfItemIsDifferent);
                }
                foreach (var hackContent in hackContentAttributes.Select(h => h.HackContent))
                {
                    ResourceUtils.ApplyHack(hackContent);
                }
            }
        }

        private static void UpdateShop(ItemObject itemObject, List<MessageEntry> newMessages, MessageTable messageTable)
        {
            var location = itemObject.NewLocation.Value;

            var shopInventories = location.GetAttributes<ShopInventoryAttribute>();
            foreach (var shopInventory in shopInventories)
            {
                var messageId = ReadWriteUtils.ReadU16(shopInventory.ShopItemAddress + 0x0A);
                var oldMessage = messageTable.GetMessage((ushort)(messageId + 1));
                var cost = ReadWriteUtils.Arr_ReadU16(oldMessage.Header, 5);
                newMessages.Add(new MessageEntryBuilder()
                    .Id(messageId)
                    .Message(it =>
                    {
                        it.Red(() =>
                        {
                            it.RuntimeItemName(itemObject.DisplayName(), location).Text(": ").Text(cost.ToString()).Text(" Rupees").NewLine();
                        })
                        .RuntimeWrap(() =>
                        {
                            it.RuntimeItemDescription(itemObject.DisplayItem, shopInventory.Keeper, location);
                        })
                        .DisableTextBoxClose()
                        .EndFinalTextBox();
                    })
                    .Build()
                );

                newMessages.Add(new MessageEntryBuilder()
                    .Id((ushort)(messageId + 1))
                    .Message(it =>
                    {
                        it.RuntimeItemName(itemObject.DisplayName(), location).Text(": ").Text(cost.ToString()).Text(" Rupees").NewLine()
                        .Text(" ").NewLine()
                        .StartGreenText()
                        .TwoChoices()
                        .Text("I'll buy ").RuntimePronoun(itemObject.DisplayItem, location).NewLine()
                        .Text("No thanks")
                        .EndFinalTextBox();
                    })
                    .Build()
                );
            }
        }

        private static void UpdateChest(Item location, Item item, ChestTypeAttribute.ChestType? overrideChestType)
        {
            var chestType = item.GetAttribute<ChestTypeAttribute>().Type;
            if (overrideChestType.HasValue)
            {
                chestType = overrideChestType.Value;
            }
            var chestAttribute = location.GetAttribute<ChestAttribute>();
            if (chestAttribute != null)
            {
                foreach (var address in chestAttribute.Addresses)
                {
                    var span = RomData.Files.GetSpanAt((uint)address, 1);
                    var chestVariable = ReadWriteUtils.ReadU8(span);
                    chestVariable &= 0x0F; // remove existing chest type
                    var newChestType = ChestAttribute.GetType(chestType, chestAttribute.Type);
                    newChestType <<= 4;
                    chestVariable |= newChestType;
                    ReadWriteUtils.WriteU8(span, chestVariable);
                }
            }

            var grottoChestAttribute = location.GetAttribute<GrottoChestAttribute>();
            if (grottoChestAttribute != null)
            {
                foreach (var address in grottoChestAttribute.Addresses)
                {
                    var span = RomData.Files.GetSpanAt((uint)address, 1);
                    var grottoVariable = ReadWriteUtils.ReadU8(span);
                    grottoVariable &= 0x1F; // remove existing chest type
                    var newChestType = (byte)chestType;
                    newChestType <<= 5;
                    grottoVariable |= newChestType; // add new chest type
                    ReadWriteUtils.WriteU8(span, grottoVariable);
                }
            }
        }

    }

}
