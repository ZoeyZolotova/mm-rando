using System;
using System.Collections.Generic;
using System.Linq;
using MMR.Common.Extensions;
using MMR.Common.Utils;

namespace MMR.Randomizer.LogicMigrator
{
    public static partial class Migrator
    {
        public const int CurrentVersion = 11;

        public static string ApplyMigrations(string logic)
        {
            JsonFormatLogic logicObject;
            try
            {
                logicObject = JsonSerializer.Deserialize<JsonFormatLogic>(logic);
            }
            catch (System.Text.Json.JsonException)
            {
                var lines = logic.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).ToList();

                if (GetVersion(lines) < 0)
                {
                    AddVersionNumber(lines);
                }

                if (GetVersion(lines) < 1)
                {
                    AddItemNames(lines);
                }

                if (GetVersion(lines) < 2)
                {
                    AddMoonItems(lines);
                }

                if (GetVersion(lines) < 3)
                {
                    AddRequirementsForSongOath(lines);
                }

                if (GetVersion(lines) < 4)
                {
                    AddSongOfHealing(lines);
                }

                if (GetVersion(lines) < 5)
                {
                    AddIkanaScrubGoldRupee(lines);
                }

                if (GetVersion(lines) < 6)
                {
                    AddPreClocktownChestLinkTrialChestsAndStartingItems(lines);
                }

                if (GetVersion(lines) < 7)
                {
                    AddGreatFairies(lines);
                }

                if (GetVersion(lines) < 8)
                {
                    AddMagicRequirements(lines);
                }

                if (GetVersion(lines) < 9)
                {
                    AddCows(lines);
                }

                if (GetVersion(lines) < 10)
                {
                    AddSkulltulaTokens(lines);
                }

                if (GetVersion(lines) < 11)
                {
                    AddStrayFairies(lines);
                }

                if (GetVersion(lines) < 12)
                {
                    AddMundaneRewards(lines);
                }

                if (GetVersion(lines) < 13)
                {
                    RemoveGormanBrosRaceDayThree(lines);
                }

                if (GetVersion(lines) < 14)
                {
                    AddTricks(lines);
                }

                if (GetVersion(lines) < 15)
                {
                    AddGossipStones(lines);
                }

                if (GetVersion(lines) < 16)
                {
                    AddRupeesAndFixedDrops(lines);
                }

                if (GetVersion(lines) < 17)
                {
                    AddRupeesAndFixedDrops2(lines);
                }

                if (GetVersion(lines) < 18)
                {
                    AddRupeesAndFixedDrops3(lines);
                }

                if (GetVersion(lines) < 19)
                {
                    AddRupeesAndFixedDrops4(lines);
                }

                logicObject = ConvertToJson(lines);
            }

            if (logicObject.Version < 1)
            {
                throw new FormatException("Unexpected logic version number.");
            }

            if (logicObject.Version < 2)
            {
                AddBossRemains(logicObject);
            }

            if (logicObject.Version < 3)
            {
                AddOcarinaAndSongOfTime(logicObject);
            }

            if (logicObject.Version < 4)
            {
                AddMultiLocations(logicObject);
            }

            if (logicObject.Version < 5)
            {
                AddMultiLocationGoronShop(logicObject);
            }

            if (logicObject.Version < 6)
            {
                AddOtherMagicBean(logicObject);
            }

            if (logicObject.Version < 7)
            {
                AddMultiLocationBusinessScrubs(logicObject);
            }

            if (logicObject.Version < 8)
            {
                AddOtherTimeTravel(logicObject);
            }

            if (logicObject.Version < 9)
            {
                AddBeansAndDekuPlayground(logicObject);
            }

            if (logicObject.Version < 10)
            {
                AddRoyalWallet(logicObject);
            }

            if (logicObject.Version < 11)
            {
                AddMultiLocationClockTownFairy(logicObject);
            }

            return JsonSerializer.Serialize(logicObject);
        }

        public static int GetVersion(List<string> lines)
        {
            foreach (var line in lines)
            {
                if (line.StartsWith("#"))
                {
                    continue;
                }
                if (line.StartsWith("-version"))
                {
                    return int.Parse(line.Split(' ')[1]);
                }
                else
                {
                    break;
                }
            }
            return -1;
        }

        private static void AddVersionNumber(List<string> lines)
        {
            lines.Insert(0, "-version 0");
        }

        private static void AddItemNames(List<string> lines)
        {
            if (lines[1] == "- Deku Mask")
            {
                lines[0] = "-version 1";
                return;
            }
            lines.RemoveAll(line => line.StartsWith("-"));
            var itemNames = new string[] {"Deku Mask", "Hero's Bow", "Fire Arrow", "Ice Arrow", "Light Arrow", "Bomb Bag (20)", "Magic Bean",
                "Powder Keg", "Pictobox", "Lens of Truth", "Hookshot", "Great Fairy's Sword", "Witch Bottle", "Aliens Bottle", "Gold Dust",
                "Beaver Race Bottle", "Dampe Bottle", "Chateau Bottle", "Bombers' Notebook", "Razor Sword", "Gilded Sword", "Mirror Shield",
                "Town Archery Quiver (40)", "Swamp Archery Quiver (50)", "Town Bomb Bag (30)", "Mountain Bomb Bag (40)", "Town Wallet (200)", "Ocean Wallet (500)", "Moon's Tear",
                "Land Title Deed", "Swamp Title Deed", "Mountain Title Deed", "Ocean Title Deed", "Room Key", "Letter to Kafei", "Pendant of Memories",
                "Letter to Mama", "Mayor Dotour HP", "Postman HP", "Rosa Sisters HP", "??? HP", "Grandma Short Story HP", "Grandma Long Story HP",
                "Keaton Quiz HP", "Deku Playground HP", "Town Archery HP", "Honey and Darling HP", "Swordsman's School HP", "Postbox HP",
                "Termina Field Gossips HP", "Termina Field Business Scrub HP", "Swamp Archery HP", "Pictograph Contest HP", "Boat Archery HP",
                "Frog Choir HP", "Beaver Race HP", "Seahorse HP", "Fisherman Game HP", "Evan HP", "Dog Race HP", "Poe Hut HP",
                "Treasure Chest Game HP", "Peahat Grotto HP", "Dodongo Grotto HP", "Woodfall Chest HP", "Twin Islands Chest HP",
                "Ocean Spider House HP", "Graveyard Iron Knuckle HP", "Postman's Hat", "All Night Mask", "Blast Mask", "Stone Mask", "Great Fairy's Mask",
                "Keaton Mask", "Bremen Mask", "Bunny Hood", "Don Gero's Mask", "Mask of Scents", "Romani Mask", "Circus Leader's Mask", "Kafei's Mask",
                "Couple's Mask", "Mask of Truth", "Kamaro's Mask", "Gibdo Mask", "Garo Mask", "Captain's Hat", "Giant's Mask", "Goron Mask", "Zora Mask",
                "Song of Soaring", "Epona's Song", "Song of Storms", "Sonata of Awakening", "Goron Lullaby", "New Wave Bossa Nova",
                "Elegy of Emptiness", "Oath to Order", "Poison swamp access", "Woodfall Temple access", "Woodfall clear", "North access", "Snowhead Temple access",
                "Snowhead clear", "Epona access", "West access", "Pirates' Fortress access", "Great Bay Temple access", "Great Bay clear", "East access",
                "Ikana Canyon access", "Stone Tower Temple access", "Inverted Stone Tower Temple access", "Ikana clear", "Explosives", "Arrows", "(Unused)", "(Unused)",
                "(Unused)", "(Unused)", "(Unused)",
                "Woodfall Map", "Woodfall Compass", "Woodfall Boss Key", "Woodfall Key 1", "Snowhead Map", "Snowhead Compass", "Snowhead Boss Key",
                "Snowhead Key 1 - block room", "Snowhead Key 2 - icicle room", "Snowhead Key 3 - bridge room", "Great Bay Map", "Great Bay Compass", "Great Bay Boss Key", "Great Bay Key 1",
                "Stone Tower Map", "Stone Tower Compass", "Stone Tower Boss Key", "Stone Tower Key 1 - armos room", "Stone Tower Key 2 - eyegore room", "Stone Tower Key 3 - updraft room",
                "Stone Tower Key 4 - death armos maze", "Trading Post Red Potion", "Trading Post Green Potion", "Trading Post Shield", "Trading Post Fairy",
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
                "Map: Snowhead", "Map: Romani Ranch", "Map: Great Bay", "Map: Stone Tower", "Goron Racetrack Grotto" };
            for (var i = 0; i < itemNames.Length; i++)
            {
                lines.Insert(i * 5, $"- {itemNames[i]}");
            }
            lines.Insert(0, "-version 1");
        }

        private static void AddMoonItems(List<string> lines)
        {
            lines[0] = "-version 2";
            var newItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 255,
                    Conditionals = Enumerable.Range(68, 20).Select(i => new List<int> { i }).ToList()
                },
                new MigrationItem
                {
                    ID = 256,
                    Conditionals = Enumerable.Range(68, 20).Combinations(2).Select(a => a.ToList()).ToList()
                },
                new MigrationItem
                {
                    ID = 257,
                    Conditionals = Enumerable.Range(68, 20).Combinations(3).Select(a => a.ToList()).ToList()
                },
                new MigrationItem
                {
                    ID = 258,
                    Conditionals = Enumerable.Range(68, 20).Combinations(4).Select(a => a.ToList()).ToList()
                },
                new MigrationItem
                {
                    ID = 259,
                    DependsOnItems = new List<int>
                    {
                        97, 100, 103, 108, 113
                    }
                },
                new MigrationItem
                {
                    ID = 260,
                    DependsOnItems = new List<int>
                    {
                        259, 0, 255
                    }
                },
                new MigrationItem
                {
                    ID = 261,
                    DependsOnItems = new List<int>
                    {
                        259, 88, 256
                    }
                },
                new MigrationItem
                {
                    ID = 262,
                    DependsOnItems = new List<int>
                    {
                        259, 89, 257
                    }
                },
                new MigrationItem
                {
                    ID = 263,
                    DependsOnItems = new List<int>
                    {
                        259, 258, 114, 115, 2, 10
                    }
                },
                new MigrationItem
                {
                    ID = 264,
                    DependsOnItems = new List<int>
                    {
                        259, 0, 88, 89, 114, 115, 2, 10
                    }
                    .Concat(Enumerable.Range(68, 20))
                    .ToList()
                }
            };
            var itemNames = new string[]
            {
                "One Mask", "Two Masks", "Three Masks", "Four Masks", "Moon Access", "Deku Trial HP", "Goron Trial HP", "Zora Trial HP", "Link Trial HP", "Fierce Deity's Mask"
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id => 
                        {
                            var itemId = int.Parse(id);
                            if (itemId >= 255)
                            {
                                itemId += newItems.Length;
                            }
                            return itemId;
                        }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 255]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, "0");
                lines.Insert(item.ID * 5 + 5, "0");
            }
        }

        private static void AddRequirementsForSongOath(List<string> lines)
        {
            lines[0] = "-version 3";
            var oathIndex = lines.FindIndex(s => s == "- Oath to Order");
            lines[oathIndex + 1] = "";
            lines[oathIndex + 2] = $"100;103;108;113";
            lines[oathIndex + 3] = "0";
            lines[oathIndex + 4] = "0";
        }

        private static void AddSongOfHealing(List<string> lines)
        {
            lines[0] = "-version 4";
            var newItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 90
                }
            };
            var itemNames = new string[]
            {
                "Song of Healing"
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 90)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 90]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, "0");
                lines.Insert(item.ID * 5 + 5, "0");
            }
            var requireSongOfHealing = new int[] { 83, 84, 88, 89 }; // kamaro, gidbo, goron, zora masks
            foreach (var id in requireSongOfHealing)
            {
                lines[id * 5 + 2] = lines[id * 5 + 2].Length == 0 ? "90" : "90," + lines[id * 5 + 2];
            }
        }

        private static void AddIkanaScrubGoldRupee(List<string> lines)
        {
            lines[0] = "-version 5";
            var newItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 256,
                    DependsOnItems = new List<int> { 110, 89, 32 } // east access, zora mask, ocean deed
                }
            };
            var itemNames = new string[]
            {
                "Ikana Scrub Gold Rupee"
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 256)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 256]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, "0");
                lines.Insert(item.ID * 5 + 5, "0");
            }
        }

        private static void AddPreClocktownChestLinkTrialChestsAndStartingItems(List<string> lines)
        {
            lines[0] = "-version 6";
            var newItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 267,
                    DependsOnItems = new List<int> { 261, 260, 10 }
                },
                new MigrationItem
                {
                    ID = 268,
                    DependsOnItems = new List<int> { 261, 260, 10 }
                },
                new MigrationItem
                {
                    ID = 269,
                    DependsOnItems = new List<int> { 261 }
                },
                new MigrationItem
                {
                    ID = 270,
                },
                new MigrationItem
                {
                    ID = 271,
                },
                new MigrationItem
                {
                    ID = 272,
                },
                new MigrationItem
                {
                    ID = 273,
                },
            };
            var itemNames = new string[]
            {
                "Link Trial 30 Arrows",
                "Link Trial 10 Bombchu",
                "Pre-Clocktown 10 Deku Nuts",
                "Starting Sword",
                "Starting Shield",
                "Starting Heart 1",
                "Starting Heart 2",
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 267)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 267]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, "0");
                lines.Insert(item.ID * 5 + 5, "0");
            }
        }

        private static void AddGreatFairies(List<string> lines)
        {
            lines[0] = "-version 7";
            var newItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 11,
                    Conditionals = new List<List<int>>
                    {
                        new List<int> { 0 },
                        new List<int> { 92 },
                        new List<int> { 93 },
                    }
                },
                new MigrationItem
                {
                    ID = 12,
                    DependsOnItems = new List<int> { 104 },
                },
                new MigrationItem
                {
                    ID = 13,
                    DependsOnItems = new List<int> { 107 },
                },
                new MigrationItem
                {
                    ID = 14,
                    DependsOnItems = new List<int> { 112 },
                },
            };
            var itemNames = new string[]
            {
                "Great Fairy Magic Meter",
                "Great Fairy Spin Attack",
                "Great Fairy Extended Magic",
                "Great Fairy Double Defense"
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line) || (i % 5 != 2 && i % 5 != 3))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 11)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 11]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, "0");
                lines.Insert(item.ID * 5 + 5, "0");
            }

            var updateItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 76, // Great Fairy's Mask
                    // remove requirements
                }
            };
            foreach (var item in updateItems)
            {
                lines[item.ID * 5 + 2] = string.Join(",", item.DependsOnItems);
                lines[item.ID * 5 + 3] = string.Join(";", item.Conditionals.Select(c => string.Join(",", c)));
            }
        }

        private static void AddMagicRequirements(List<string> lines)
        {
            lines[0] = "-version 8";
            var newItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 278,
                    Conditionals = new List<List<int>>
                    {
                        new List<int> { 11 },
                        new List<int> { 13 },
                    }
                },
            };
            var itemNames = new string[]
            {
                "Magic Meter"
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 278)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 278]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, "0");
                lines.Insert(item.ID * 5 + 5, "0");
            }
            
            var requireMagic = new int[] { 2, 3, 4, 9 }; // fire arrow, ice arrow, light arrow, lens of truth
            for (var i = 0; i < lines.Count; i++)
            {
                if (i%5 != 2 && i%5 != 3)
                {
                    continue;
                }
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section =>
                    {
                        if (section.Split(',').Select(int.Parse).Intersect(requireMagic).Any())
                        {
                            section += ",278";
                        }
                        return section;
                    }).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
        }

        private static void AddCows(List<string> lines)
        {
            lines[0] = "-version 9";
            var newItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 278,
                    DependsOnItems = new List<int> { 96, 109 } // epona's song, epona access
                },
                new MigrationItem
                {
                    ID = 279,
                    DependsOnItems = new List<int> { 96, 109 } // epona's song, epona access
                },
                new MigrationItem
                {
                    ID = 280,
                    DependsOnItems = new List<int> { 265 } // moon access / unaccessible
                },
                new MigrationItem
                {
                    ID = 281,
                    DependsOnItems = new List<int> { 96, 115, 88, 173 }, // epona's song, ikana canyon access, gibdo mask, hot spring water, 
                    Conditionals = new List<List<int>>
                    {
                        new List<int> { 4 }, // light arrow
                        new List<int> { 0, 103 }, // deku mask, poison swamp access
                    },
                },
                new MigrationItem
                {
                    ID = 282,
                    DependsOnItems = new List<int> { 96, 119 } // epona's song, explosives
                },
                new MigrationItem
                {
                    ID = 283,
                    DependsOnItems = new List<int> { 96, 119 } // epona's song, explosives
                },
                new MigrationItem
                {
                    ID = 284,
                    DependsOnItems = new List<int> { 96, 110, 10 } // epona's song, west access, hookshot
                },
                new MigrationItem
                {
                    ID = 285,
                    DependsOnItems = new List<int> { 96, 110, 10 } // epona's song, west access, hookshot
                },
            };
            var itemNames = new string[]
            {
                "Ranch Cow #1 Milk",
                "Ranch Cow #2 Milk",
                "Ranch Cow #3 Milk",
                "Well Cow Milk",
                "Termina Grotto Cow #1 Milk",
                "Termina Grotto Cow #2 Milk",
                "Great Bay Coast Grotto Cow #1 Milk",
                "Great Bay Coast Grotto Cow #2 Milk",
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 278)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 278]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, "0");
                lines.Insert(item.ID * 5 + 5, "0");
            }
        }

        private static void AddSkulltulaTokens(List<string> lines)
        {
            lines[0] = "-version 10";
            var newItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 286,
                    DependsOnItems = new List<int> { 103 }, // Poison swamp access
                },
                new MigrationItem
                {
                    ID = 287,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 288,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 289,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 290,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 291,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 292,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 293,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 294,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 295,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 296,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 297,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 298,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 299,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 300,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 301,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 302,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 303,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 304,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 305,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 306,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 307,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 308,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 309,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 310,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 311,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 312,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 313,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 314,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 315,
                    DependsOnItems = new List<int> { 103 },
                },
                new MigrationItem
                {
                    ID = 316,
                    DependsOnItems = new List<int> { 110, 119 }, // West access, Explosives
                },
                new MigrationItem
                {
                    ID = 317,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 318,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 319,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 320,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 321,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 322,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 323,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 324,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 325,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 326,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 327,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 328,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 329,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 330,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 331,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 332,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 333,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 334,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 335,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 336,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 337,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 338,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 339,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 340,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 341,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 342,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 343,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 344,
                    DependsOnItems = new List<int> { 110, 119 },
                },
                new MigrationItem
                {
                    ID = 345,
                    DependsOnItems = new List<int> { 110, 119 },
                },
            };
            var itemNames = new string[]
            {
                "Swamp Skulltula Main Room Near Ceiling", "Swamp Skulltula Gold Room Near Ceiling", "Swamp Skulltula Monument Room Torch", "Swamp Skulltula Gold Room Pillar", "Swamp Skulltula Pot Room Jar",

                "Swamp Skulltula Tree Room Grass 1", "Swamp Skulltula Tree Room Grass 2", "Swamp Skulltula Main Room Water", "Swamp Skulltula Main Room Lower Left Soft Soil", "Swamp Skulltula Monument Room Crate 1",

                "Swamp Skulltula Main Room Upper Soft Soil", "Swamp Skulltula Main Room Lower Right Soft Soil", "Swamp Skulltula Monument Room Lower Wall", "Swamp Skulltula Monument Room On Monument", "Swamp Skulltula Main Room Pillar",

                "Swamp Skulltula Pot Room Pot 1", "Swamp Skulltula Pot Room Pot 2", "Swamp Skulltula Gold Room Hive", "Swamp Skulltula Main Room Upper Pillar", "Swamp Skulltula Pot Room Behind Vines",

                "Swamp Skulltula Tree Room Tree 1", "Swamp Skulltula Pot Room Wall", "Swamp Skulltula Pot Room Hive 1", "Swamp Skulltula Tree Room Tree 2", "Swamp Skulltula Gold Room Wall",

                "Swamp Skulltula Tree Room Hive", "Swamp Skulltula Monument Room Crate 2", "Swamp Skulltula Pot Room Hive 2", "Swamp Skulltula Tree Room Tree 3", "Swamp Skulltula Main Room Jar",

                "Ocean Skulltula Storage Room Behind Boat", "Ocean Skulltula Library Hole Behind Picture", "Ocean Skulltula Library Hole Behind Cabinet", "Ocean Skulltula Library On Corner Bookshelf", "Ocean Skulltula 2nd Room Ceiling Edge",

                "Ocean Skulltula 2nd Room Ceiling Plank", "Ocean Skulltula Colored Skulls Ceiling Edge", "Ocean Skulltula Library Ceiling Edge", "Ocean Skulltula Storage Room Ceiling Web", "Ocean Skulltula Storage Room Behind Crate",

                "Ocean Skulltula 2nd Room Jar", "Ocean Skulltula Entrance Right Wall", "Ocean Skulltula Entrance Left Wall", "Ocean Skulltula 2nd Room Webbed Hole", "Ocean Skulltula Entrance Web",

                "Ocean Skulltula Colored Skulls Chandelier 1", "Ocean Skulltula Colored Skulls Chandelier 2", "Ocean Skulltula Colored Skulls Chandelier 3", "Ocean Skulltula Colored Skulls Behind Picture", "Ocean Skulltula Library Behind Picture",

                "Ocean Skulltula Library Behind Bookcase 1", "Ocean Skulltula Storage Room Crate", "Ocean Skulltula 2nd Room Webbed Pot", "Ocean Skulltula 2nd Room Upper Pot", "Ocean Skulltula Colored Skulls Pot",

                "Ocean Skulltula Storage Room Jar", "Ocean Skulltula 2nd Room Lower Pot", "Ocean Skulltula Library Behind Bookcase 2", "Ocean Skulltula 2nd Room Behind Skull 1", "Ocean Skulltula 2nd Room Behind Skull 2"
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 286)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 286]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, $"{item.TimeNeeded}");
                lines.Insert(item.ID * 5 + 5, "0");
            }
        }

        private static void AddStrayFairies(List<string> lines)
        {
            lines[0] = "-version 11";
            var newItems = new MigrationItem[]
            {
                new MigrationItem
                {
                    ID = 346,
                },
                new MigrationItem
                {
                    ID = 347,
                },
                new MigrationItem
                {
                    ID = 348,
                },
                new MigrationItem
                {
                    ID = 349,
                },
                new MigrationItem
                {
                    ID = 350,
                },
                new MigrationItem
                {
                    ID = 351,
                },
                new MigrationItem
                {
                    ID = 352,
                },
                new MigrationItem
                {
                    ID = 353,
                },
                new MigrationItem
                {
                    ID = 354,
                },
                new MigrationItem
                {
                    ID = 355,
                },
                new MigrationItem
                {
                    ID = 356,
                },
                new MigrationItem
                {
                    ID = 357,
                },
                new MigrationItem
                {
                    ID = 358,
                },
                new MigrationItem
                {
                    ID = 359,
                },
                new MigrationItem
                {
                    ID = 360,
                },
                new MigrationItem
                {
                    ID = 361,
                },
                new MigrationItem
                {
                    ID = 362,
                },
                new MigrationItem
                {
                    ID = 363,
                },
                new MigrationItem
                {
                    ID = 364,
                },
                new MigrationItem
                {
                    ID = 365,
                },
                new MigrationItem
                {
                    ID = 366,
                },
                new MigrationItem
                {
                    ID = 367,
                },
                new MigrationItem
                {
                    ID = 368,
                },
                new MigrationItem
                {
                    ID = 369,
                },
                new MigrationItem
                {
                    ID = 370,
                },
                new MigrationItem
                {
                    ID = 371,
                },
                new MigrationItem
                {
                    ID = 372,
                },
                new MigrationItem
                {
                    ID = 373,
                },
                new MigrationItem
                {
                    ID = 374,
                },
                new MigrationItem
                {
                    ID = 375,
                },
                new MigrationItem
                {
                    ID = 376,
                },
                new MigrationItem
                {
                    ID = 377,
                },
                new MigrationItem
                {
                    ID = 378,
                },
                new MigrationItem
                {
                    ID = 379,
                },
                new MigrationItem
                {
                    ID = 380,
                },
                new MigrationItem
                {
                    ID = 381,
                },
                new MigrationItem
                {
                    ID = 382,
                },
                new MigrationItem
                {
                    ID = 383,
                },
                new MigrationItem
                {
                    ID = 384,
                },
                new MigrationItem
                {
                    ID = 385,
                },
                new MigrationItem
                {
                    ID = 386,
                },
                new MigrationItem
                {
                    ID = 387,
                },
                new MigrationItem
                {
                    ID = 388,
                },
                new MigrationItem
                {
                    ID = 389,
                },
                new MigrationItem
                {
                    ID = 390,
                },
                new MigrationItem
                {
                    ID = 391,
                },
                new MigrationItem
                {
                    ID = 392,
                },
                new MigrationItem
                {
                    ID = 393,
                },
                new MigrationItem
                {
                    ID = 394,
                },
                new MigrationItem
                {
                    ID = 395,
                },
                new MigrationItem
                {
                    ID = 396,
                },
                new MigrationItem
                {
                    ID = 397,
                },
                new MigrationItem
                {
                    ID = 398,
                },
                new MigrationItem
                {
                    ID = 399,
                },
                new MigrationItem
                {
                    ID = 400,
                },
                new MigrationItem
                {
                    ID = 401,
                },
                new MigrationItem
                {
                    ID = 402,
                },
                new MigrationItem
                {
                    ID = 403,
                },
                new MigrationItem
                {
                    ID = 404,
                },
                new MigrationItem
                {
                    ID = 405,
                },
                new MigrationItem
                {
                    ID = 406,
                },
            };
            var itemNames = new string[]
            {
                "Clock Town Stray Fairy",
                "Woodfall Pre-Boss Room Bubble 1",
                "Woodfall Entrance Fairy",
                "Woodfall Pre-Boss Room Bubble 2",
                "Woodfall Pre-Boss Room Bubble 3",
                "Woodfall Deku Baba",
                "Woodfall Poison Water Bubble",
                "Woodfall Main Room Bubble",
                "Woodfall Skulltula",
                "Woodfall Pre-Boss Room Bubble 4",
                "Woodfall Main Room Switch",
                "Woodfall Entrance Platform",
                "Woodfall Dark Room",
                "Woodfall Jar Fairy",
                "Woodfall Bridge Room Hive",
                "Woodfall Platform Room Hive",
                "Snowhead Snow Room Bubble",
                "Snowhead Ceiling Bubble",
                "Snowhead Dinolfos 1",
                "Snowhead Bridge Room Bubble 1",
                "Snowhead Bridge Room Bubble 2",
                "Snowhead Dinolfos 2",
                "Snowhead Map Room Fairy",
                "Snowhead Map Room Ledge",
                "Snowhead Basement",
                "Snowhead Twin Block",
                "Snowhead Icicle Room Wall",
                "Snowhead Main Room Wall",
                "Snowhead Torches",
                "Snowhead Ice Puzzle",
                "Snowhead Crate",
                "Great Bay Skulltula",
                "Great Bay Pre-Boss Room Underwater Bubble",
                "Great Bay Water Control Room Underwater Bubble",
                "Great Bay Pre-Boss Room Bubble",
                "Great Bay Waterwheel Room",
                "Great Bay Green Valve",
                "Great Bay Seesaw Room",
                "Great Bay Waterwheel Room",
                "Great Bay Entrance Torches",
                "Great Bay Bio Babas",
                "Great Bay Underwater Barrel",
                "Great Bay Whirlpool Jar",
                "Great Bay Whirlpool Barrel",
                "Great Bay Dexihands Jar",
                "Great Bay Ledge Jar",
                "Stone Tower Mirror Sun Block",
                "Stone Tower Eyegore",
                "Stone Tower Lava Room Fire Ring", // todo check location name
                "Stone Tower Updraft Fire Ring",
                "Stone Tower Mirror Sun Switch",
                "Stone Tower Boss Warp",
                "Stone Tower Wizzrobe",
                "Stone Tower Death Armos",
                "Stone Tower Updraft Frozen Eye",
                "Stone Tower Thin Bridge",
                "Stone Tower Basement Ledge",
                "Stone Tower Statue Eye",
                "Stone Tower Underwater",
                "Stone Tower Bridge Crystal",
                "Stone Tower Lava Room Ledge", // todo check location name
            };
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 346)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 346]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, $"{item.TimeNeeded}");
                lines.Insert(item.ID * 5 + 5, "0");
            }
        }

        private static void AddMundaneRewards(List<string> lines)
        {
            lines[0] = "-version 12";
            var itemNames = new string[]
            {
                "Lottery 50r", "Bank 5r", "Milk Bar Chateau", "Milk Bar Milk", "Deku Playground 50r", "Honey and Darling 50r", "Kotake Mushroom Sale 20r", "Pictograph Contest 5r",
                "Pictograph Contest 20r", "Swamp Scrub Magic Bean", "Ocean Scrub Green Potion", "Canyon Scrub Blue Potion", "Zora Hall Stage Lights 5r", "Gorman Bros Purchase Milk",
                "Gorman Bros Race Milk", "Ocean Spider House 50r", "Ocean Spider House 20", "Lulu Pictograph 5r", "Lulu Pictograph 20r", "Treasure Chest Game 50r", "Treasure Chest Game 20r",
                "Treasure Chest Game Deku Nuts", "Curiosity Shop 5r", "Curiosity Shop 20r", "Curiosity Shop 50r", "Curiosity Shop 200r", "Seahorse",
            };
            var newItems = itemNames.Select((itemName, index) => new MigrationItem
            {
                ID = 407 + index
            }).ToArray();
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 407)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 5 + 1, $"- {itemNames[item.ID - 407]}");
                lines.Insert(item.ID * 5 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 5 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 5 + 4, $"{item.TimeNeeded}");
                lines.Insert(item.ID * 5 + 5, "0");
            }
        }

        private static void RemoveGormanBrosRaceDayThree(List<string> lines)
        {
            lines[0] = "-version 13";

            var itemsToRemove = new List<int>
            {
                421
            };

            foreach (var removeId in itemsToRemove.OrderByDescending(id => id))
            {
                for (var i = 0; i < lines.Count; i++)
                {
                    var line = lines[i];
                    if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }
                    var updatedItemSections = line
                        .Split(';')
                        .Select(section => section.Split(',').Select(int.Parse).Where(id => id != removeId).Select(id =>
                        {
                            if (id > removeId)
                            {
                                id--;
                            }
                            return id;
                        }).ToList()).Where(section => section.Any()).ToList();
                    lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
                }

                lines.RemoveRange(removeId * 5 + 1, 5);
            }
        }

        private static void AddTricks(List<string> lines)
        {
            lines[0] = "-version 14";

            for (var i = 1; i < lines.Count; i += 6)
            {
                lines.Insert(i + 5, "");
            }
        }

        private static void AddGossipStones(List<string> lines)
        {
            lines[0] = "-version 15";
            var itemNames = new string[]
            {
                "GossipTerminaSouth",
                "GossipSwampPotionShop",
                "GossipMountainSpringPath",
                "GossipMountainPath",
                "GossipOceanZoraGame",
                "GossipCanyonRoad",
                "GossipCanyonDock",
                "GossipCanyonSpiritHouse",
                "GossipTerminaMilk",
                "GossipTerminaWest",
                "GossipTerminaNorth",
                "GossipTerminaEast",
                "GossipRanchTree",
                "GossipRanchBarn",
                "GossipMilkRoad",
                "GossipOceanFortress",
                "GossipSwampRoad",
                "GossipTerminaObservatory",
                "GossipRanchCuccoShack",
                "GossipRanchRacetrack",
                "GossipRanchEntrance",
                "GossipCanyonRavine",
                "GossipMountainSpringFrog",
                "GossipSwampSpiderHouse",
                "GossipTerminaGossipLarge",
                "GossipTerminaGossipGuitar",
                "GossipTerminaGossipPipes",
                "GossipTerminaGossipDrums",
            };
            var newItems = itemNames.Select((itemName, index) => new MigrationItem
            {
                ID = 433 + index
            }).ToArray();
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 433)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 6 + 1, $"- {itemNames[item.ID - 433]}");
                lines.Insert(item.ID * 6 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 6 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 6 + 4, $"{item.TimeNeeded}");
                lines.Insert(item.ID * 6 + 5, "0");
                lines.Insert(item.ID * 6 + 6, "");
            }
        }

        private static void AddRupeesAndFixedDrops(List<string> lines)
        {
            lines[0] = "-version 16";
            var itemNames = new string[]
            {
                "Ikana Castle Courtyard Grass",
                "Ikana Castle Courtyard Grass 2",
                "Night 1 Grave Pot",
                "Night 2 Grave Pot",
                "Night 1 Grave Pot 2",
                "Cucco Shack Crate",
                "Dampe's Basement Pot",
                "Dampe's Basement Pot 2",
                "Dampe's Basement Pot 3",
                "Dampe's Basement Pot 4",
                "Goron Village Small Snowball",
                "Goron Village Small Snowball 2",
                "Great Bay Coast Pot",
                "Great Bay Coast Pot 2",
                "Great Bay Coast Pot 3",
                "Great Bay Coast Pot 4",
                "Great Bay Temple Red Valve Barrel",
                "Ikana King Pot",
                "Ikana King Pot 2",
                "Ikana King Entry Pot",
                "Ikana King Entry Pot 2",
                "Ikana Graveyard Grass",
                "Oceanside Spider House Entrance Pot",
                "Oceanside Spider House Entrance Pot 2",
                "Pirates' Fortress Sewer Gate Pot",
                "Pirates' Fortress Guarded Egg Pot",
                "Pirates' Fortress Barrel Maze Egg Pot",
                "Pirates' Fortress Sewer Exit Pot",
                "Secret Shrine Underwater Pot",
                "Secret Shrine Underwater Pot 2",
                "Snowhead Temple Icicle Room Snowball",
                "Snowhead Temple Icicle Room Snowball 2",
                "Stone Tower Upper Scarecrow Pot",
                "Stone Tower Upper Scarecrow Pot 2",
                "Great Bay Coast Pot 5",
                "Great Bay Temple Seesaw Room Pot",
                "Great Bay Temple Green Pump Barrel",
                "Ikana Canyon Grass",
                "Milk Road Grass",
                "Mountain Village Spring Snowball",
                "Mountain Village Winter Small Snowball",
                "Pirates' Fortress Lone Guard Egg Pot",
                "Pirates' Fortress Cage Pot",
                "Ranch Crate",
                "Snowhead Small Snowball",
                "Stone Tower Owl Pot",
                "Zora Cape Owl Pot",
                "Observatory Scarecrow Pot",
                "Observatory Scarecrow Pot 2",
                "Deku Palace Item",
                "Deku Palace Item 2",
                "Deku Palace Item 3",
                "Deku Palace Item 4",
                "Deku Palace Item 5",
                "Doggy Racetrack Pot",
                "Doggy Racetrack Pot 2",
                "Doggy Racetrack Pot 3",
                "Doggy Racetrack Pot 4",
                "Goron Village Large Snowball",
                "Goron Village Large Snowball 2",
                "Goron Village Large Snowball 3",
                "Great Bay Coast Ledge Pot",
                "Great Bay Coast Ledge Pot 2",
                "Great Bay Coast Ledge Pot 3",
                "Great Bay Temple Water Control Room Item",
                "Great Bay Temple Water Control Room Item 2",
                "Bio Baba Grotto Hive",
                "Laundry Pool Crate",
                "Mountain Village Day 3 Snowball",
                "Mountain Village Day 2 Snowball",
                "Twin Islands Item",
                "Twin Islands Item 2",
                "Twin Islands Item 3",
                "Twin Islands Item 4",
                "Pirates' Fortress Barrel Maze Egg Pot 2",
                "Pirates' Fortress Sewer Exit Barrel",
                "Pirates' Fortress Sewer Exit Barrel 2",
                "Pirates' Fortress Sewer Exit Barrel 3",
                "Pirates' Fortress Cage Room Barrel",
                "Ranch Barn Hay Item",
                "Ranch Barn Hay Item 2",
                "Snowhead Temple Icicle Room Snowball 3",
                "Snowhead Temple Icicle Room Snowball 4",
                "Snowhead Temple Icicle Room Snowball 5",
                "Snowhead Temple Elevator Room Crate",
                "Snowhead Temple Elevator Room Crate 2",
                "Snowhead Temple Elevator Room Crate 3",
                "Snowhead Temple Elevator Room Crate 4",
                "Snowhead Temple Elevator Room Crate 5",
                "Snowhead Temple Safety Bridge Pot",
                "Snowhead Temple Safety Bridge Pot 2",
                "Cleared Swamp Potion Shop Pot",
                "Swamp Near Frog Item",
                "Swamp Near Frog Item 2",
                "Potion Shop Pot",
                "Stone Tower Temple Lava Room Item",
                "Stone Tower Temple Lava Room Item 2",
                "Stone Tower Temple Thin Bridge Item",
                "Stone Tower Temple Thin Bridge Item 2",
                "Stone Tower Temple Thin Bridge Item 3",
                "Stone Tower Temple Thin Bridge Item 4",
                "Stone Tower Temple Thin Bridge Item 5",
                "Stone Tower Temple Thin Bridge Item 6",
                "Stone Tower Temple Thin Bridge Item 7",
                "Stone Tower Temple Thin Bridge Item 8",
                "Inverted Stone Tower Temple Dexihand Item",
                "Inverted Stone Tower Temple Pre-Boss Closest Item",
                "Inverted Stone Tower Temple Pre-Boss 2nd Closest Item",
                "Inverted Stone Tower Temple Pre-Boss Item",
                "Inverted Stone Tower Temple Pre-Boss Item 2",
                "Inverted Stone Tower Temple Pre-Boss Item 3",
                "Inverted Stone Tower Temple Pre-Boss Furthest Item",
                "Inverted Stone Tower Temple Pre-Boss Furthest Item 2",
                "Inverted Stone Tower Temple Pre-Boss 2nd Furthest Item",
                "Inverted Stone Tower Temple Pre-Boss 2nd Furthest Item 2",
                "Inverted Stone Tower Temple Pre-Boss Closest Item 2",
                "Inverted Stone Tower Temple Pre-Boss 2nd Closest Item 2",
                "Swordsman's School Pot",
                "Swordsman's School Pot 2",
                "Swordsman's School Pot 3",
                "Swordsman's School Pot 4",
                "Swordsman's School Pot 5",
                "Woodfall Item",
                "Woodfall Temple Entrance Hive",
                "Woodfall Temple Gekko Room Pot",
                "Woodfall Temple Gekko Room Pot 2",
                "Woodfall Temple Gekko Room Pot 3",
                "Woodfall Temple Gekko Room Pot 4",
                "Woodfall Temple Pre-Boss Platform Item",
                "Woodfall Temple Pre-Boss Platform Item 2",
                "Woodfall Temple Pre-Boss Platform Item 3",
                "Woodfall Temple Pre-Boss Platform Item 4",
                "Well Left Path Pot",
                "Well Left Path Pot 2",
                "Well Left Path Pot 3",
                "Well Left Path Pot 4",
                "Well Left Path Pot 5",
                "Goron Village Small Snowball 3",
                "Goron Village Small Snowball 4",
                "Great Bay Coast Pot 6",
                "Great Bay Temple Red Valve Barrel 2",
                "Great Bay Temple Green Pump Barrel 2",
                "Ikana Canyon Grass 2",
                "Mountain Village Spring Snowball 2",
                "Mountain Village Winter Small Snowball 2",
                "Snowhead Small Snowball 2",
                "Stone Tower Owl Pot 2",
                "Inverted Stone Tower Pot",
                "Zora Cape Owl Pot 2",
                "Ikana Castle Left Staircase Pot",
                "Goron Village Small Snowball 5",
                "Goron Village Small Snowball 6",
                "Pirates' Fortress Sewer Exit Pot 2",
                "Woodfall Pot",
                "Goron Shrine Pot",
                "Goron Shrine Pot 2",
                "Goron Shrine Pot 3",
                "Goron Shrine Pot 4",
                "Goron Shrine Pot 5",
                "Goron Village Small Snowball 7",
                "Goron Village Small Snowball 8",
                "Cleared Swamp Owl Grass",
                "Southern Swamp Owl Grass",
                "Woodfall Pot 2",
                "Dampe's Basement Pot 5",
                "Dampe's Basement Pot 6",
                "Dampe's Basement Pot 7",
                "Deku Palace Item 6",
                "Deku Palace Item 7",
                "Deku Palace Item 8",
                "Deku Palace Item 9",
                "Deku Palace Item 10",
                "Deku Palace Item 11",
                "Deku Palace Item 12",
                "Deku Palace Item 13",
                "Deku Palace Item 14",
                "Deku Palace Out of Bounds Item",
                "Deku Palace Item 15",
                "Deku Palace Item 16",
                "Deku Palace Item 17",
                "Butler Race Pillar Item",
                "Butler Race Pillar Item 2",
                "Butler Race Pillar Item 3",
                "Butler Race Pillar Item 4",
                "Butler Race Pillar Item 5",
                "Butler Race Pillar Item 6",
                "Butler Race River Item",
                "Butler Race River Item 2",
                "Butler Race River Item 3",
                "Butler Race River Item 4",
                "Butler Race River Item 5",
                "Butler Race River Item 6",
                "Butler Race Right Path Item",
                "Butler Race Right Path Item 2",
                "Butler Race Right Path Item 3",
                "Butler Race Right Path Item 4",
                "Butler Race Right Path Item 5",
                "Butler Race Right Path Item 6",
                "Butler Race Final Room Item",
                "Butler Race Final Room Item 2",
                "Butler Race Final Room Item 3",
                "Butler Race Final Room Item 4",
                "Butler Race Final Room Item 5",
                "Butler Race Final Room Item 6",
                "Butler Race Final Room Item 7",
                "Butler Race Final Room Item 8",
                "Butler Race Final Room Item 9",
                "Butler Race Final Room Item 10",
                "Butler Race Dual Pot",
                "East Clock Town Crate",
                "Great Bay Temple Water Control Room Item 3",
                "Great Bay Temple Water Control Room Item 4",
                "Ikana Graveyard Grass 2",
                "Potion Shop Item",
                "Pirates' Fortress Cage Room Barrel 2",
                "Pirates' Fortress Cage Room Barrel 3",
                "Pirates' Fortress Cage Room Barrel 4",
                "Pirates' Fortress Cage Room Barrel 5",
                "Secret Shrine Floating Item",
                "Secret Shrine Floating Item 2",
                "Secret Shrine Floating Item 3",
                "Secret Shrine Floating Item 4",
                "Secret Shrine Floating Item 5",
                "Secret Shrine Floating Item 6",
                "Secret Shrine Floating Item 7",
                "Secret Shrine Floating Item 8",
                "Secret Shrine Floating Item 9",
                "Secret Shrine Floating Item 10",
                "Secret Shrine Floating Item 11",
                "Secret Shrine Floating Item 12",
                "Secret Shrine Floating Item 13",
                "Secret Shrine Floating Item 14",
                "Secret Shrine Floating Item 15",
                "Secret Shrine Floating Item 16",
                "Secret Shrine Floating Item 17",
                "Cleared Swamp Potion Shop Pot 2",
                "Potion Shop Pot 2",
                "Stone Tower Temple Lava Room Item 3",
                "Stone Tower Temple Lava Room Item 4",
                "Stone Tower Temple Lava Room Item 5",
                "Inverted Stone Tower Temple Dexihand Item 2",
                "Clock Tower Rooftop Pot",
                "Clock Tower Rooftop Pot 2",
                "Clock Tower Rooftop Pot 3",
                "Clock Tower Rooftop Pot 4",
                "Goron Racetrack Pot",
                "Goron Racetrack Pot 2",
                "Goron Racetrack Pot 3",
                "Goron Racetrack Pot 4",
                "Goron Racetrack Pot 5",
                "Goron Racetrack Pot 6",
                "Goron Racetrack Pot 7",
                "Goron Racetrack Pot 8",
                "Goron Racetrack Pot 9",
                "Goron Racetrack Pot 10",
                "Goron Racetrack Pot 11",
                "Goron Racetrack Pot 12",
                "Goron Racetrack Pot 13",
                "Goron Racetrack Pot 14",
                "Goron Racetrack Pot 15",
                "Goron Racetrack Pot 16",
                "Goron Racetrack Pot 17",
                "Goron Racetrack Pot 18",
                "Goron Racetrack Pot 19",
                "Goron Racetrack Pot 20",
                "Goron Racetrack Pot 21",
                "Goron Racetrack Pot 22",
                "Goron Racetrack Pot 23",
                "Goron Racetrack Pot 24",
                "Goron Racetrack Pot 25",
                "Goron Racetrack Pot 26",
                "Goron Racetrack Pot 27",
                "Goron Shrine Pot 6",
                "Goron Shrine Pot 7",
                "Goron Shrine Pot 8",
                "Goron Shrine Pot 9",
                "Great Bay Coast Pot 7",
                "Great Bay Temple Red Valve Crate",
                "Ikana King Pot 3",
                "Ikana Canyon Grass 3",
                "Milk Road Grass 2",
                "Mountain Village Spring Snowball 3",
                "Goron Graveyard Snowball",
                "Goron Graveyard Snowball 2",
                "Mountain Village Winter Small Snowball 3",
                "Snowhead Small Snowball 3",
                "Stone Tower Owl Pot 3",
                "Inverted Stone Tower Pot 2",
                "Link Trial Pot",
                "Link Trial Pot 2",
                "Link Trial Pot 3",
                "Link Trial Pot 4",
                "Zora Cape Owl Pot 3",
                "Dampe's Basement Pot 8",
                "Pirates' Fortress Item",
                "Pirates' Fortress Item 2",
                "Pirates' Fortress Item 3",
                "Butler Race Pillar Item 7",
                "Butler Race Pillar Item 8",
                "Great Bay Temple Water Control Room Item 5",
                "Great Bay Temple Dexihand Item",
                "Great Bay Temple Dexihand Item 2",
                "Great Bay Temple Green Pump Item",
                "Great Bay Temple Green Pump Item 2",
                "Laundry Pool Item",
                "Laundry Pool Item 2",
                "Laundry Pool Item 3",
                "Mountain Village Spring Stair Item",
                "Snowhead Temple Icicle Room Frozen Item",
                "Snowhead Temple Icicle Room Frozen Item 2",
                "Snowhead Temple Icicle Room Frozen Item 3",
                "Swamp Near Frog Hive",
                "Stone Tower Temple Lava Room Item 6",
                "Stone Tower Temple Eyegore Room Item",
                "Stone Tower Temple Mirror Room Crate",
                "Stone Tower Temple Mirror Room Crate 2",
                "Stone Tower Temple Eyegore Room Item 2",
                "Inverted Stone Tower Temple Dexihand Item 3",
                "Inverted Stone Tower Temple Updraft Room Item",
                "Inverted Stone Tower Temple Updraft Room Item 2",
                "Termina Field Pillar Item",
                "Woodfall Temple Pre-Boss Left Pillar Item",
                "Woodfall Temple Pre-Boss Right Pillar Item",
                "Ikana Castle Courtyard Grass 3",
                "Ikana Castle Courtyard Grass 4",
                "Ikana Castle Fire Ceiling Room Pot",
                "Ikana Castle Hole Room Pot",
                "Ikana Castle Hole Room Pot 2",
                "Observatory Balloon Pot",
                "Observatory Balloon Pot 2",
                "Observatory Scarecrow Pot 3",
                "Night 2 Grave Pot 2",
                "Deku Palace Pot",
                "Deku Palace Pot 2",
                "Goron Racetrack Pot 28",
                "Goron Racetrack Pot 29",
                "Goron Racetrack Pot 30",
                "Goron Shrine Pot 10",
                "Goron Shrine Pot 11",
                "Goron Village Large Snowball 4",
                "Goron Village Large Snowball 5",
                "Goron Village Large Snowball 6",
                "Goron Village Small Snowball 9",
                "Goron Village Small Snowball 10",
                "Ikana King Entry Pot 3",
                "Ikana Graveyard Grass 3",
                "Mountain Village Winter Small Snowball 4",
                "Mountain Village Winter Small Snowball 5",
                "Mountain Village Day 1 Snowball",
                "Mountain Village Day 2 Snowball 2",
                "Oceanside Spider House Main Room Pot",
                "Oceanside Spider House Entrance Pot 3",
                "Oceanside Spider House Main Room Pot 2",
                "Oceanside Spider House Storage Room Pot",
                "Twin Islands Day 3 Snowball",
                "Twin Islands Day 3 Snowball 2",
                "Twin Islands Day 3 Snowball 3",
                "Twin Islands Day 3 Snowball 4",
                "Twin Islands Day 3 Snowball 5",
                "Twin Islands Day 2 Snowball",
                "Twin Islands Day 2 Snowball 2",
                "Twin Islands Day 2 Snowball 3",
                "Twin Islands Day 2 Snowball 4",
                "Twin Islands Day 1 Snowball",
                "Twin Islands Day 1 Snowball 2",
                "Twin Islands Day 1 Snowball 3",
                "Twin Islands Day 1 Snowball 4",
                "Twin Islands Day 1 Snowball 5",
                "Twin Islands Small Snowball",
                "Twin Islands Small Snowball 2",
                "Twin Islands Ramp Snowball",
                "Path to Mountain Village Small Snowball",
                "Path to Snowhead Large Snowball",
                "Path to Snowhead Large Snowball 2",
                "Path to Snowhead Large Snowball 3",
                "Path to Snowhead Large Snowball 4",
                "Pinnacle Rock Pot",
                "Pinnacle Rock Pot 2",
                "Pinnacle Rock Pot 3",
                "Pinnacle Rock Pot 4",
                "Secret Shrine Underwater Pot 3",
                "Secret Shrine Underwater Pot 4",
                "Snowhead Large Snowball",
                "Snowhead Large Snowball 2",
                "Snowhead Large Snowball 3",
                "Snowhead Large Snowball 4",
                "Snowhead Large Snowball 5",
                "Snowhead Large Snowball 6",
                "Stone Tower Lower Scarecrow Pot",
                "Stone Tower Lower Scarecrow Pot 2",
                "Stone Tower Upper Scarecrow Pot 3",
                "Stone Tower Upper Scarecrow Pot 4",
                "Stone Tower Lower Scarecrow Pot 3",
                "Zora Cape Waterfall Pot",
                "Ranch Fence Item",
                "Ranch Fence Item 2",
                "Ranch Fence Item 3",
                "Ranch Fence Item 4",
                "Ranch Fence Item 5",
                "Ranch Fence Item 6",
                "Termina Field Above Cow Grotto Invisible Item",
                "Termina Field Invisible Item 2",
                "Termina Field Invisible Item 3",
                "Termina Field Invisible Item 4",
                "Termina Field Invisible Item 5",
                "Termina Field Invisible Item 6",
                "Termina Field Invisible Item 7",
                "Termina Field Invisible Item 8",
                "Termina Field Northern Ramp Invisible Item",
                "Termina Field Invisible Item 10",
                "Termina Field Invisible Item 11",
                "Swamp Spider House Invisible Item",
                "Swamp Spider House Invisible Item 2",
                "Swamp Spider House Invisible Item 3",
                "Swamp Spider House Invisible Item 4",
                "Swamp Spider House Invisible Item 5",
                "Termina Field Tree Item",
                "Termina Field Pillar Spawned Item",
                "Termina Field Telescope Guay",
                "Swordsman School Gong",
                "Bean Grotto Soft Soil",
                "Deku Palace Soft Soil",
                "Doggy Racetrack Soft Soil",
                "Great Bay Coast Soft Soil",
                "Ranch Day 1 Soil",
                "Ranch Day 2 or 3 Soil",
                "Secret Shrine Soft Soil",
                "Stone Tower Soft Soil Lower",
                "Stone Tower Soft Soil Upper",
                "Swamp Spider House Rock Soft Soil",
                "Swamp Spider House Gold Room Soft Soil",
                "Termina Field Stump Soft Soil",
                "Termina Field Observatory Soft Soil",
                "Termina Field South Wall Soft Soil",
                "Termina Field Pillar Soft Soil",
                "Termina Field Guay #1",
                "Termina Field Guay #2",
                "Termina Field Guay #3",
                "Termina Field Guay #4",
                "Termina Field Guay #5",
                "Termina Field Guay #6",
                "Termina Field Guay #7",
                "Termina Field Guay #8",
                "Termina Field Guay #9",
                "Termina Field Guay #10",
                "Termina Field Guay #11",
                "Termina Field Guay #12",
                "Termina Field Guay #13",
                "Termina Field Guay #14",
                "Termina Field Guay #15",
                "Termina Field Guay #16",
                "Termina Field Guay #17",
                "Termina Field Guay #18",
                "Termina Field Guay #19",
                "Termina Field Guay #20",
                "Termina Field Guay #5a",
                "Termina Field Guay #10a",
                "Termina Field Guay #15a",
                "Deku Palace Rupee Cluster #1",
                "Deku Palace Rupee Cluster #2",
                "Deku Palace Rupee Cluster #3",
                "Deku Palace Rupee Cluster #4",
                "Deku Palace Rupee Cluster #5",
                "Deku Palace Rupee Cluster #6",
                "Deku Palace Rupee Cluster #7",
                "Ikana Graveyard Rupee Cluster",
                "Ikana Graveyard Rupee Cluster 2",
                "Ikana Graveyard Rupee Cluster 3",
                "Ikana Graveyard Rupee Cluster 4",
                "Ikana Graveyard Rupee Cluster 5",
                "Ikana Graveyard Rupee Cluster 6",
                "Ikana Graveyard Rupee Cluster 7",
                "Termina Field Song Wall Dawn",
                "Termina Field Song Wall Dawn 2",
                "Termina Field Song Wall Dawn 3",
                "Termina Field Song Wall 0 / 8 / 12 / 16",
                "Termina Field Song Wall 0 / 8 / 12 / 16 2",
                "Termina Field Song Wall 0 / 8 / 12 / 16 3",
                "Termina Field Song Wall 2 / 10 / 14 / 18 / 22",
                "Termina Field Song Wall 2 / 10 / 14 / 18 / 22 2",
                "Termina Field Song Wall 2 / 10 / 14 / 18 / 22 3",
                "Termina Field Song Wall 4 / 20",
                "Termina Field Song Wall 4 / 20 2",
                "Termina Field Song Wall 4 / 20 3",
                "Termina Field Song Wall Odd Hours",
                "Termina Field Song Wall Odd Hours 2",
                "Termina Field Song Wall Odd Hours 3",
                "Deku Playground Day 2 Item",
                "Deku Playground Day 2 Item 2",
                "Deku Playground Day 2 Item 3",
                "Deku Playground Day 2 Item 4",
                "Deku Playground Day 2 Item 5",
                "Deku Playground Day 2 Item 6",
                "Deku Playground Day 1 Item",
                "Deku Playground Day 1 Item 2",
                "Deku Playground Day 1 Item 3",
                "Deku Playground Day 1 Item 4",
                "Deku Playground Day 1 Item 5",
                "Deku Playground Day 1 Item 6",
                "Deku Playground Day 3 Item",
                "Deku Playground Day 3 Item 2",
                "Deku Playground Day 3 Item 3",
                "Deku Playground Day 3 Item 4",
                "Deku Playground Day 3 Item 5",
                "Deku Playground Day 3 Item 6",
                "Pirates' Fortress Skull Flag Left Eye",
                "Pirates' Fortress Skull Flag Left Eye 2",
                "Pirates' Fortress Skull Flag Left Eye 3",
                "Pirates' Fortress Skull Flag Right Eye",
                "Pirates' Fortress Skull Flag Right Eye 2",
                "Pirates' Fortress Skull Flag Right Eye 3",
                "Hookshot Room Skull Flag Forehead",
                "Hookshot Room Skull Flag Forehead 2",
                "Hookshot Room Skull Flag Forehead 3",
                "Swamp Spider House Blue Gem",
                "Swamp Spider House Blue Gem 2",
                "Swamp Spider House Blue Gem 3",
                "Swamp Spider House Blue Gem 4",
                "Swamp Spider House Blue Gem 5",
                "Swamp Spider House Blue Gem 6",
                "Swamp Spider House Blue Gem 7",
                "Swamp Spider House Blue Gem 8",
                "Swamp Spider House Blue Gem 9",
                "Swamp Spider House Blue Gem 10",
                "Swamp Spider House Blue Gem 11",
                "Swamp Spider House Blue Gem 12",
                "Oceanside Spider House Mask",
                "Oceanside Spider House Mask 2",
                "Oceanside Spider House Mask 3",
                "Oceanside Spider House Mask 4",
                "Oceanside Spider House Mask 5",
                "Oceanside Spider House Mask 6",
                "Oceanside Spider House Mask 7",
                "Oceanside Spider House Mask 8",
                "Oceanside Spider House Mask 9",
                "Termina Field Clam",
                "Termina Field Clam 2",
                "Termina Field Clam 3",
                "Termina Field Wall",
                "Termina Field Wall 2",
                "Termina Field Wall 3",
                "Termina Field Skull Kid Drawing",
                "Termina Field Skull Kid Drawing 2",
                "Termina Field Skull Kid Drawing 3",
                "Cucco Shack Diamond Hole",
                "Cucco Shack Diamond Hole 2",
                "Cucco Shack Diamond Hole 3",
                "Cucco Shack Diamond Hole 4",
                "Cucco Shack Diamond Hole 5",
                "Cucco Shack Diamond Hole 6",
                "Ikana Graveyard Lantern",
                "Ikana Graveyard Lantern 2",
                "Ikana Graveyard Lantern 3",
                "Ikana Graveyard Lantern 4",
                "Ikana Graveyard Lantern 5",
                "Ikana Graveyard Lantern 6",
                "Ikana Graveyard Lantern 7",
                "Ikana Graveyard Lantern 8",
                "Ikana Graveyard Lantern 9",
                "Ikana Graveyard Lantern 10",
                "Ikana Graveyard Lantern 11",
                "Ikana Graveyard Lantern 12",
                "Stock Pot Inn Mask",
                "Stock Pot Inn Mask 2",
                "Stock Pot Inn Mask 3",
                "East Clock Town Target",
                "East Clock Town Target 2",
                "East Clock Town Target 3",
                "East Clock Town Target 4",
                "East Clock Town Target 5",
                "East Clock Town Target 6",
                "East Clock Town Basket",
                "East Clock Town Basket 2",
                "East Clock Town Basket 3",
                "Clock Tower Clock",
                "Clock Tower Clock 2",
                "Clock Tower Clock 3",
                "Takkuri",
                "Hookshot Room Pot",
                "Hookshot Room Pot 2",
                "Termina Field Rock",
                "Termina Field Rock 2",
                "Ikana Graveyard Highest Rock",
                "Ikana Graveyard Lowest Rock",
                "Ikana Graveyard 2nd Lowest Rock",
                "Termina Field Rock 3",
                "Termina Field Rock 4",
                "Termina Field Rock 5",
                "Termina Field Rock 6",
                "Termina Field Rock 7",
                "Ikana Graveyard 2nd Highest Rock",
                "Ikana Graveyard Middle Rock",
                "Termina Field Rock 8",
                "Termina Field Rock 9",
                "Milk Road Keaton Grass",
                "Milk Road Keaton Grass 2",
                "Milk Road Keaton Grass 3",
                "Milk Road Keaton Grass 4",
                "Milk Road Keaton Grass 5",
                "Milk Road Keaton Grass 6",
                "Milk Road Keaton Grass 7",
                "Milk Road Keaton Grass 8",
                "Milk Road Keaton Grass 9",
                "North Clock Town Keaton Grass",
                "North Clock Town Keaton Grass 2",
                "North Clock Town Keaton Grass 3",
                "North Clock Town Keaton Grass 4",
                "North Clock Town Keaton Grass 5",
                "North Clock Town Keaton Grass 6",
                "North Clock Town Keaton Grass 7",
                "North Clock Town Keaton Grass 8",
                "North Clock Town Keaton Grass 9",
                "Mountain Village Spring Keaton Grass",
                "Mountain Village Spring Keaton Grass 2",
                "Mountain Village Spring Keaton Grass 3",
                "Mountain Village Spring Keaton Grass 4",
                "Mountain Village Spring Keaton Grass 5",
                "Mountain Village Spring Keaton Grass 6",
                "Mountain Village Spring Keaton Grass 7",
                "Mountain Village Spring Keaton Grass 8",
                "Mountain Village Spring Keaton Grass 9",
                "Oceanside Spider House Mask Room Pot",
                "Oceanside Spider House Mask Room Pot 2",
            };
            var newItems = itemNames.Select((itemName, index) => new MigrationItem
            {
                ID = 433 + index
            }).ToArray();
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= 433)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 6 + 1, $"- {itemNames[item.ID - 433]}");
                lines.Insert(item.ID * 6 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 6 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 6 + 4, $"{item.TimeNeeded}");
                lines.Insert(item.ID * 6 + 5, "0");
                lines.Insert(item.ID * 6 + 6, "");
            }
        }

        private static void AddRupeesAndFixedDrops2(List<string> lines)
        {
            const int startIndex = 1056;
            lines[0] = "-version 17";
            var itemNames = new string[]
            {
                "Ikana Canyon Cleared Grass",
                "Ikana Canyon Cleared Grass 2",
                "Ikana Canyon Cleared Grass 3",
                "Path to Snowhead Spring Snowball",
                "Path to Snowhead Spring Snowball 2",
                "Path to Snowhead Spring Snowball 3",
                "Path to Snowhead Spring Snowball 4",
                "Path to Mountain Village Spring Snowball",
                "Path to Mountain Village Spring Snowball 2",
                "Path to Mountain Village Spring Snowball 3",
            };
            var newItems = itemNames.Select((itemName, index) => new MigrationItem
            {
                ID = startIndex + index
            }).ToArray();
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= startIndex)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 6 + 1, $"- {itemNames[item.ID - startIndex]}");
                lines.Insert(item.ID * 6 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 6 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 6 + 4, $"{item.TimeNeeded}");
                lines.Insert(item.ID * 6 + 5, "0");
                lines.Insert(item.ID * 6 + 6, "");
            }
        }

        private static void AddRupeesAndFixedDrops3(List<string> lines)
        {
            const int startIndex = 1066;
            lines[0] = "-version 18";
            var itemNames = new string[]
            {
                "Zora Cape Jar Game",
                "Ikana Graveyard Day 2 Bats",
            };
            var newItems = itemNames.Select((itemName, index) => new MigrationItem
            {
                ID = startIndex + index
            }).ToArray();
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= startIndex)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 6 + 1, $"- {itemNames[item.ID - startIndex]}");
                lines.Insert(item.ID * 6 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 6 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 6 + 4, $"{item.TimeNeeded}");
                lines.Insert(item.ID * 6 + 5, "0");
                lines.Insert(item.ID * 6 + 6, "");
            }
        }

        private static void AddRupeesAndFixedDrops4(List<string> lines)
        {
            const int startIndex = 1068;
            lines[0] = "-version 19";
            var itemNames = new string[]
            {
                "Cucco Shack Potted Plant",
            };
            var newItems = itemNames.Select((itemName, index) => new MigrationItem
            {
                ID = startIndex + index
            }).ToArray();
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-") || string.IsNullOrWhiteSpace(line) || line.StartsWith(";"))
                {
                    continue;
                }
                var updatedItemSections = line
                    .Split(';')
                    .Select(section => section.Split(',').Select(id =>
                    {
                        var itemId = int.Parse(id);
                        if (itemId >= startIndex)
                        {
                            itemId += newItems.Length;
                        }
                        return itemId;
                    }).ToList()).ToList();
                lines[i] = string.Join(";", updatedItemSections.Select(section => string.Join(",", section)));
            }
            foreach (var item in newItems)
            {
                lines.Insert(item.ID * 6 + 1, $"- {itemNames[item.ID - startIndex]}");
                lines.Insert(item.ID * 6 + 2, string.Join(",", item.DependsOnItems));
                lines.Insert(item.ID * 6 + 3, string.Join(";", item.Conditionals.Select(c => string.Join(",", c))));
                lines.Insert(item.ID * 6 + 4, $"{item.TimeNeeded}");
                lines.Insert(item.ID * 6 + 5, "0");
                lines.Insert(item.ID * 6 + 6, "");
            }
        }

        private static JsonFormatLogic ConvertToJson(List<string> lines)
        {
            var itemNames = new List<string>
            {
                "MaskDeku",
                "ItemBow",
                "ItemFireArrow",
                "ItemIceArrow",
                "ItemLightArrow",
                "ItemBombBag",
                "ItemMagicBean",
                "ItemPowderKeg",
                "ItemPictobox",
                "ItemLens",
                "ItemHookshot",
                "FairyMagic",
                "FairySpinAttack",
                "FairyDoubleMagic",
                "FairyDoubleDefense",
                "ItemFairySword",
                "ItemBottleWitch",
                "ItemBottleAliens",
                "ItemBottleGoronRace",
                "ItemBottleBeavers",
                "ItemBottleDampe",
                "ItemBottleMadameAroma",
                "ItemNotebook",
                "UpgradeRazorSword",
                "UpgradeGildedSword",
                "UpgradeMirrorShield",
                "UpgradeBigQuiver",
                "UpgradeBiggestQuiver",
                "UpgradeBigBombBag",
                "UpgradeBiggestBombBag",
                "UpgradeAdultWallet",
                "UpgradeGiantWallet",
                "TradeItemMoonTear",
                "TradeItemLandDeed",
                "TradeItemSwampDeed",
                "TradeItemMountainDeed",
                "TradeItemOceanDeed",
                "TradeItemRoomKey",
                "TradeItemKafeiLetter",
                "TradeItemPendant",
                "TradeItemMamaLetter",
                "HeartPieceNotebookMayor",
                "HeartPieceNotebookPostman",
                "HeartPieceNotebookRosa",
                "HeartPieceNotebookHand",
                "HeartPieceNotebookGran1",
                "HeartPieceNotebookGran2",
                "HeartPieceKeatonQuiz",
                "HeartPieceDekuPlayground",
                "HeartPieceTownArchery",
                "HeartPieceHoneyAndDarling",
                "HeartPieceSwordsmanSchool",
                "HeartPiecePostBox",
                "HeartPieceTerminaGossipStones",
                "HeartPieceTerminaBusinessScrub",
                "HeartPieceSwampArchery",
                "HeartPiecePictobox",
                "HeartPieceBoatArchery",
                "HeartPieceChoir",
                "HeartPieceBeaverRace",
                "HeartPieceSeaHorse",
                "HeartPieceFishermanGame",
                "HeartPieceEvan",
                "HeartPieceDogRace",
                "HeartPiecePoeHut",
                "HeartPieceTreasureChestGame",
                "HeartPiecePeahat",
                "HeartPieceDodong",
                "HeartPieceWoodFallChest",
                "HeartPieceTwinIslandsChest",
                "HeartPieceOceanSpiderHouse",
                "HeartPieceKnuckle",
                "MaskPostmanHat",
                "MaskAllNight",
                "MaskBlast",
                "MaskStone",
                "MaskGreatFairy",
                "MaskKeaton",
                "MaskBremen",
                "MaskBunnyHood",
                "MaskDonGero",
                "MaskScents",
                "MaskRomani",
                "MaskCircusLeader",
                "MaskKafei",
                "MaskCouple",
                "MaskTruth",
                "MaskKamaro",
                "MaskGibdo",
                "MaskGaro",
                "MaskCaptainHat",
                "MaskGiant",
                "MaskGoron",
                "MaskZora",
                "SongHealing",
                "SongSoaring",
                "SongEpona",
                "SongStorms",
                "SongSonata",
                "SongLullaby",
                "SongNewWaveBossaNova",
                "SongElegy",
                "SongOath",
                "AreaSouthAccess",
                "AreaWoodFallTempleAccess",
                "AreaWoodFallTempleClear",
                "AreaNorthAccess",
                "AreaSnowheadTempleAccess",
                "AreaSnowheadTempleClear",
                "OtherEpona",
                "AreaWestAccess",
                "AreaPiratesFortressAccess",
                "AreaGreatBayTempleAccess",
                "AreaGreatBayTempleClear",
                "AreaEastAccess",
                "AreaIkanaCanyonAccess",
                "AreaStoneTowerTempleAccess",
                "AreaInvertedStoneTowerTempleAccess",
                "AreaStoneTowerClear",
                "OtherExplosive",
                "OtherArrow",
                "AreaWoodfallNew",
                "AreaSnowheadNew",
                "AreaGreatBayNew",
                "AreaLANew",
                "AreaInvertedStoneTowerNew",
                "ItemWoodfallMap",
                "ItemWoodfallCompass",
                "ItemWoodfallBossKey",
                "ItemWoodfallKey1",
                "ItemSnowheadMap",
                "ItemSnowheadCompass",
                "ItemSnowheadBossKey",
                "ItemSnowheadKey1",
                "ItemSnowheadKey2",
                "ItemSnowheadKey3",
                "ItemGreatBayMap",
                "ItemGreatBayCompass",
                "ItemGreatBayBossKey",
                "ItemGreatBayKey1",
                "ItemStoneTowerMap",
                "ItemStoneTowerCompass",
                "ItemStoneTowerBossKey",
                "ItemStoneTowerKey1",
                "ItemStoneTowerKey2",
                "ItemStoneTowerKey3",
                "ItemStoneTowerKey4",
                "ShopItemTradingPostRedPotion",
                "ShopItemTradingPostGreenPotion",
                "ShopItemTradingPostShield",
                "ShopItemTradingPostFairy",
                "ShopItemTradingPostStick",
                "ShopItemTradingPostArrow30",
                "ShopItemTradingPostNut10",
                "ShopItemTradingPostArrow50",
                "ShopItemWitchBluePotion",
                "ShopItemWitchRedPotion",
                "ShopItemWitchGreenPotion",
                "ShopItemBombsBomb10",
                "ShopItemBombsBombchu10",
                "ShopItemGoronBomb10",
                "ShopItemGoronArrow10",
                "ShopItemGoronRedPotion",
                "ShopItemZoraShield",
                "ShopItemZoraArrow10",
                "ShopItemZoraRedPotion",
                "BottleCatchFairy",
                "BottleCatchPrincess",
                "BottleCatchFish",
                "BottleCatchBug",
                "BottleCatchPoe",
                "BottleCatchBigPoe",
                "BottleCatchSpringWater",
                "BottleCatchHotSpringWater",
                "BottleCatchEgg",
                "BottleCatchMushroom",
                "ChestLensCaveRedRupee",
                "ChestLensCavePurpleRupee",
                "ChestBeanGrottoRedRupee",
                "ChestHotSpringGrottoRedRupee",
                "ChestBadBatsGrottoPurpleRupee",
                "ChestIkanaSecretShrineGrotto",
                "ChestPiratesFortressRedRupee1",
                "ChestPiratesFortressRedRupee2",
                "ChestInsidePiratesFortressTankRedRupee",
                "ChestInsidePiratesFortressGuardSilverRupee",
                "ChestInsidePiratesFortressHeartPieceRoomRedRupee",
                "ChestInsidePiratesFortressHeartPieceRoomBlueRupee",
                "ChestInsidePiratesFortressMazeRedRupee",
                "ChestPinacleRockRedRupee1",
                "ChestPinacleRockRedRupee2",
                "ChestBomberHideoutSilverRupee",
                "ChestTerminaGrottoBombchu",
                "ChestTerminaGrottoRedRupee",
                "ChestTerminaUnderwaterRedRupee",
                "ChestTerminaGrassRedRupee",
                "ChestTerminaStumpRedRupee",
                "ChestGreatBayCoastGrotto",
                "ChestGreatBayCapeLedge1",
                "ChestGreatBayCapeLedge2",
                "ChestGreatBayCapeGrotto",
                "ChestGreatBayCapeUnderwater",
                "ChestPiratesFortressEntranceRedRupee1",
                "ChestPiratesFortressEntranceRedRupee2",
                "ChestPiratesFortressEntranceRedRupee3",
                "ChestToSwampGrotto",
                "ChestDogRacePurpleRupee",
                "ChestGraveyardGrotto",
                "ChestSwampGrotto",
                "ChestWoodfallBlueRupee",
                "ChestWoodfallRedRupee",
                "ChestWellRightPurpleRupee",
                "ChestWellLeftPurpleRupee",
                "ChestMountainVillage",
                "ChestMountainVillageGrottoRedRupee",
                "ChestToIkanaRedRupee",
                "ChestToIkanaGrotto",
                "ChestInvertedStoneTowerSilverRupee",
                "ChestInvertedStoneTowerBombchu10",
                "ChestInvertedStoneTowerBean",
                "ChestToSnowheadGrotto",
                "ChestToGoronVillageRedRupee",
                "ChestSecretShrineHeartPiece",
                "ChestSecretShrineDinoGrotto",
                "ChestSecretShrineWizzGrotto",
                "ChestSecretShrineWartGrotto",
                "ChestSecretShrineGaroGrotto",
                "ChestInnStaffRoom",
                "ChestInnGuestRoom",
                "ChestWoodsGrotto",
                "ChestEastClockTownSilverRupee",
                "ChestSouthClockTownRedRupee",
                "ChestSouthClockTownPurpleRupee",
                "HeartPieceBank",
                "HeartPieceSouthClockTown",
                "HeartPieceNorthClockTown",
                "HeartPieceToSwamp",
                "HeartPieceSwampScrub",
                "HeartPieceDekuPalace",
                "HeartPieceGoronVillageScrub",
                "HeartPieceZoraGrotto",
                "HeartPieceLabFish",
                "HeartPieceGreatBayCapeLikeLike",
                "HeartPiecePiratesFortress",
                "HeartPieceZoraHallScrub",
                "HeartPieceToSnowhead",
                "HeartPieceGreatBayCoast",
                "HeartPieceIkana",
                "HeartPieceCastle",
                "HeartContainerWoodfall",
                "HeartContainerSnowhead",
                "HeartContainerGreatBay",
                "HeartContainerStoneTower",
                "ItemTingleMapTown",
                "ItemTingleMapWoodfall",
                "ItemTingleMapSnowhead",
                "ItemTingleMapRanch",
                "ItemTingleMapGreatBay",
                "ItemTingleMapStoneTower",
                "ChestToGoronRaceGrotto",
                "IkanaScrubGoldRupee",
                "OtherOneMask",
                "OtherTwoMasks",
                "OtherThreeMasks",
                "OtherFourMasks",
                "AreaMoonAccess",
                "HeartPieceDekuTrial",
                "HeartPieceGoronTrial",
                "HeartPieceZoraTrial",
                "HeartPieceLinkTrial",
                "MaskFierceDeity",
                "ChestLinkTrialArrow30",
                "ChestLinkTrialBombchu10",
                "ChestPreClocktownDekuNut",
                "StartingSword",
                "StartingShield",
                "StartingHeartContainer1",
                "StartingHeartContainer2",
                "ItemRanchBarnMainCowMilk",
                "ItemRanchBarnOtherCowMilk1",
                "ItemRanchBarnOtherCowMilk2",
                "ItemWellCowMilk",
                "ItemTerminaGrottoCowMilk1",
                "ItemTerminaGrottoCowMilk2",
                "ItemCoastGrottoCowMilk1",
                "ItemCoastGrottoCowMilk2",
                "CollectibleSwampSpiderToken1",
                "CollectibleSwampSpiderToken2",
                "CollectibleSwampSpiderToken3",
                "CollectibleSwampSpiderToken4",
                "CollectibleSwampSpiderToken5",
                "CollectibleSwampSpiderToken6",
                "CollectibleSwampSpiderToken7",
                "CollectibleSwampSpiderToken8",
                "CollectibleSwampSpiderToken9",
                "CollectibleSwampSpiderToken10",
                "CollectibleSwampSpiderToken11",
                "CollectibleSwampSpiderToken12",
                "CollectibleSwampSpiderToken13",
                "CollectibleSwampSpiderToken14",
                "CollectibleSwampSpiderToken15",
                "CollectibleSwampSpiderToken16",
                "CollectibleSwampSpiderToken17",
                "CollectibleSwampSpiderToken18",
                "CollectibleSwampSpiderToken19",
                "CollectibleSwampSpiderToken20",
                "CollectibleSwampSpiderToken21",
                "CollectibleSwampSpiderToken22",
                "CollectibleSwampSpiderToken23",
                "CollectibleSwampSpiderToken24",
                "CollectibleSwampSpiderToken25",
                "CollectibleSwampSpiderToken26",
                "CollectibleSwampSpiderToken27",
                "CollectibleSwampSpiderToken28",
                "CollectibleSwampSpiderToken29",
                "CollectibleSwampSpiderToken30",
                "CollectibleOceanSpiderToken1",
                "CollectibleOceanSpiderToken2",
                "CollectibleOceanSpiderToken3",
                "CollectibleOceanSpiderToken4",
                "CollectibleOceanSpiderToken5",
                "CollectibleOceanSpiderToken6",
                "CollectibleOceanSpiderToken7",
                "CollectibleOceanSpiderToken8",
                "CollectibleOceanSpiderToken9",
                "CollectibleOceanSpiderToken10",
                "CollectibleOceanSpiderToken11",
                "CollectibleOceanSpiderToken12",
                "CollectibleOceanSpiderToken13",
                "CollectibleOceanSpiderToken14",
                "CollectibleOceanSpiderToken15",
                "CollectibleOceanSpiderToken16",
                "CollectibleOceanSpiderToken17",
                "CollectibleOceanSpiderToken18",
                "CollectibleOceanSpiderToken19",
                "CollectibleOceanSpiderToken20",
                "CollectibleOceanSpiderToken21",
                "CollectibleOceanSpiderToken22",
                "CollectibleOceanSpiderToken23",
                "CollectibleOceanSpiderToken24",
                "CollectibleOceanSpiderToken25",
                "CollectibleOceanSpiderToken26",
                "CollectibleOceanSpiderToken27",
                "CollectibleOceanSpiderToken28",
                "CollectibleOceanSpiderToken29",
                "CollectibleOceanSpiderToken30",
                "CollectibleStrayFairyClockTown",
                "CollectibleStrayFairyWoodfall1",
                "CollectibleStrayFairyWoodfall2",
                "CollectibleStrayFairyWoodfall3",
                "CollectibleStrayFairyWoodfall4",
                "CollectibleStrayFairyWoodfall5",
                "CollectibleStrayFairyWoodfall6",
                "CollectibleStrayFairyWoodfall7",
                "CollectibleStrayFairyWoodfall8",
                "CollectibleStrayFairyWoodfall9",
                "CollectibleStrayFairyWoodfall10",
                "CollectibleStrayFairyWoodfall11",
                "CollectibleStrayFairyWoodfall12",
                "CollectibleStrayFairyWoodfall13",
                "CollectibleStrayFairyWoodfall14",
                "CollectibleStrayFairyWoodfall15",
                "CollectibleStrayFairySnowhead1",
                "CollectibleStrayFairySnowhead2",
                "CollectibleStrayFairySnowhead3",
                "CollectibleStrayFairySnowhead4",
                "CollectibleStrayFairySnowhead5",
                "CollectibleStrayFairySnowhead6",
                "CollectibleStrayFairySnowhead7",
                "CollectibleStrayFairySnowhead8",
                "CollectibleStrayFairySnowhead9",
                "CollectibleStrayFairySnowhead10",
                "CollectibleStrayFairySnowhead11",
                "CollectibleStrayFairySnowhead12",
                "CollectibleStrayFairySnowhead13",
                "CollectibleStrayFairySnowhead14",
                "CollectibleStrayFairySnowhead15",
                "CollectibleStrayFairyGreatBay1",
                "CollectibleStrayFairyGreatBay2",
                "CollectibleStrayFairyGreatBay3",
                "CollectibleStrayFairyGreatBay4",
                "CollectibleStrayFairyGreatBay5",
                "CollectibleStrayFairyGreatBay6",
                "CollectibleStrayFairyGreatBay7",
                "CollectibleStrayFairyGreatBay8",
                "CollectibleStrayFairyGreatBay9",
                "CollectibleStrayFairyGreatBay10",
                "CollectibleStrayFairyGreatBay11",
                "CollectibleStrayFairyGreatBay12",
                "CollectibleStrayFairyGreatBay13",
                "CollectibleStrayFairyGreatBay14",
                "CollectibleStrayFairyGreatBay15",
                "CollectibleStrayFairyStoneTower1",
                "CollectibleStrayFairyStoneTower2",
                "CollectibleStrayFairyStoneTower3",
                "CollectibleStrayFairyStoneTower4",
                "CollectibleStrayFairyStoneTower5",
                "CollectibleStrayFairyStoneTower6",
                "CollectibleStrayFairyStoneTower7",
                "CollectibleStrayFairyStoneTower8",
                "CollectibleStrayFairyStoneTower9",
                "CollectibleStrayFairyStoneTower10",
                "CollectibleStrayFairyStoneTower11",
                "CollectibleStrayFairyStoneTower12",
                "CollectibleStrayFairyStoneTower13",
                "CollectibleStrayFairyStoneTower14",
                "CollectibleStrayFairyStoneTower15",
                "MundaneItemLotteryPurpleRupee",
                "MundaneItemBankBlueRupee",
                "ShopItemMilkBarChateau",
                "ShopItemMilkBarMilk",
                "MundaneItemDekuPlaygroundPurpleRupee",
                "MundaneItemHoneyAndDarlingPurpleRupee",
                "MundaneItemKotakeMushroomSaleRedRupee",
                "MundaneItemPictographContestBlueRupee",
                "MundaneItemPictographContestRedRupee",
                "ShopItemBusinessScrubMagicBean",
                "ShopItemBusinessScrubGreenPotion",
                "ShopItemBusinessScrubBluePotion",
                "MundaneItemZoraStageLightsBlueRupee",
                "ShopItemGormanBrosMilk",
                "MundaneItemOceanSpiderHouseDay2PurpleRupee",
                "MundaneItemOceanSpiderHouseDay3RedRupee",
                "MundaneItemLuluBadPictographBlueRupee",
                "MundaneItemLuluGoodPictographRedRupee",
                "MundaneItemTreasureChestGamePurpleRupee",
                "MundaneItemTreasureChestGameRedRupee",
                "MundaneItemTreasureChestGameDekuNuts",
                "MundaneItemCuriosityShopBlueRupee",
                "MundaneItemCuriosityShopRedRupee",
                "MundaneItemCuriosityShopPurpleRupee",
                "MundaneItemCuriosityShopGoldRupee",
                "MundaneItemSeahorse",
                "CollectableAncientCastleOfIkanaCastleExteriorGrass1",
                "CollectableAncientCastleOfIkanaCastleExteriorGrass2",
                "CollectableBeneathTheGraveyardMainAreaPot1",
                "CollectableBeneathTheGraveyardInvisibleRoomPot1",
                "CollectableBeneathTheGraveyardBadBatRoomPot1",
                "CollectableCuccoShackWoodenCrateLarge1",
                "CollectableDampesHouseBasementPot1",
                "CollectableDampesHouseBasementPot2",
                "CollectableDampesHouseBasementPot3",
                "CollectableDampesHouseBasementPot4",
                "CollectableGoronVillageWinterSmallSnowball1",
                "CollectableGoronVillageWinterSmallSnowball2",
                "CollectableGreatBayCoastPot1",
                "CollectableGreatBayCoastPot2",
                "CollectableGreatBayCoastPot3",
                "CollectableGreatBayCoastPot4",
                "CollectableGreatBayTempleBlueChuchuValveRoomBarrel1",
                "CollectableIgosDuIkanaSLairIgosDuIkanaSRoomPot1",
                "CollectableIgosDuIkanaSLairIgosDuIkanaSRoomPot2",
                "CollectableIgosDuIkanaSLairPreBossRoomPot1",
                "CollectableIgosDuIkanaSLairPreBossRoomPot2",
                "CollectableIkanaGraveyardIkanaGraveyardLowerGrass1",
                "CollectableOceansideSpiderHouseEntrancePot1",
                "CollectableOceansideSpiderHouseEntrancePot2",
                "CollectablePiratesFortressInteriorWaterCurrentRoomPot1",
                "CollectablePiratesFortressInterior100RupeeEggRoomPot1",
                "CollectablePiratesFortressInteriorBarrelRoomEggPot1",
                "CollectablePiratesFortressInteriorTelescopeRoomPot1",
                "CollectableSecretShrineMainRoomPot1",
                "CollectableSecretShrineMainRoomPot2",
                "CollectableSnowheadTempleIceBlockRoomSmallSnowball1",
                "CollectableSnowheadTempleIceBlockRoomSmallSnowball2",
                "CollectableStoneTowerPot1",
                "CollectableStoneTowerPot2",
                "CollectableGreatBayCoastPot5",
                "CollectableGreatBayTempleSeesawRoomPot1",
                "CollectableGreatBayTempleTopmostRoomWithGreenValveBarrel1",
                "CollectableIkanaCanyonMainAreaGrass1",
                "CollectableMilkRoadGrass1",
                "CollectableMountainVillageSpringSmallSnowball1",
                "CollectableMountainVillageWinterSmallSnowball1",
                "CollectablePiratesFortressInteriorTwinBarrelEggRoomPot1",
                "CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartPot1",
                "CollectableRomaniRanchWoodenCrateLarge1",
                "CollectableSnowheadSmallSnowball1",
                "CollectableStoneTowerPot3",
                "CollectableZoraCapePot1",
                "CollectableAstralObservatoryObservatoryBombersHideoutPot1",
                "CollectableAstralObservatoryObservatoryBombersHideoutPot2",
                "CollectableDekuPalaceWestInnerGardenItem1",
                "CollectableDekuPalaceEastInnerGardenItem1",
                "CollectableDekuPalaceEastInnerGardenItem2",
                "CollectableDekuPalaceWestInnerGardenItem2",
                "CollectableDekuPalaceWestInnerGardenItem3",
                "CollectableDoggyRacetrackPot1",
                "CollectableDoggyRacetrackPot2",
                "CollectableDoggyRacetrackPot3",
                "CollectableDoggyRacetrackPot4",
                "CollectableGoronVillageWinterLargeSnowball1",
                "CollectableGoronVillageWinterLargeSnowball2",
                "CollectableGoronVillageWinterLargeSnowball3",
                "CollectableGreatBayCoastPot6",
                "CollectableGreatBayCoastPot7",
                "CollectableGreatBayCoastPot8",
                "CollectableGreatBayTempleWaterControlRoomItem1",
                "CollectableGreatBayTempleWaterControlRoomItem2",
                "CollectableGrottosOceanHeartPieceGrottoBeehive1",
                "CollectableLaundryPoolWoodenCrateSmall1",
                "CollectableMountainVillageWinterLargeSnowball1",
                "CollectableMountainVillageWinterLargeSnowball2",
                "CollectablePathToGoronVillageWinterItem1",
                "CollectablePathToGoronVillageWinterItem2",
                "CollectablePathToGoronVillageWinterItem3",
                "CollectablePathToGoronVillageWinterItem4",
                "CollectablePiratesFortressInteriorBarrelRoomEggPot2",
                "CollectablePiratesFortressInteriorTelescopeRoomItem1",
                "CollectablePiratesFortressInteriorTelescopeRoomItem2",
                "CollectablePiratesFortressInteriorTelescopeRoomItem3",
                "CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem1",
                "CollectableRanchHouseBarnBarnItem1",
                "CollectableRanchHouseBarnBarnItem2",
                "CollectableSnowheadTempleIceBlockRoomSmallSnowball3",
                "CollectableSnowheadTempleIceBlockRoomSmallSnowball4",
                "CollectableSnowheadTempleIceBlockRoomSmallSnowball5",
                "CollectableSnowheadTempleMapRoomWoodenCrateLarge1",
                "CollectableSnowheadTempleMapRoomWoodenCrateLarge2",
                "CollectableSnowheadTempleMapRoomWoodenCrateLarge3",
                "CollectableSnowheadTempleMapRoomWoodenCrateLarge4",
                "CollectableSnowheadTempleMapRoomWoodenCrateLarge5",
                "CollectableSnowheadTempleMainRoomPot1",
                "CollectableSnowheadTempleMainRoomPot2",
                "CollectableSouthernSwampClearMagicHagsPotionShopExteriorPot1",
                "CollectableSouthernSwampPoisonedCentralSwampItem1",
                "CollectableSouthernSwampPoisonedCentralSwampItem2",
                "CollectableSouthernSwampPoisonedMagicHagsPotionShopExteriorPot1",
                "CollectableStoneTowerTempleLavaRoomItem1",
                "CollectableStoneTowerTempleLavaRoomItem2",
                "CollectableStoneTowerTempleRoomAfterLightArrowsItem1",
                "CollectableStoneTowerTempleRoomAfterLightArrowsItem2",
                "CollectableStoneTowerTempleRoomAfterLightArrowsItem3",
                "CollectableStoneTowerTempleRoomAfterLightArrowsItem4",
                "CollectableStoneTowerTempleRoomAfterLightArrowsItem5",
                "CollectableStoneTowerTempleRoomAfterLightArrowsItem6",
                "CollectableStoneTowerTempleRoomAfterLightArrowsItem7",
                "CollectableStoneTowerTempleRoomAfterLightArrowsItem8",
                "CollectableStoneTowerTempleInvertedEyegoreRoomItem1",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem1",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem2",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem3",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem4",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem5",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem6",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem7",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem8",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem9",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem10",
                "CollectableStoneTowerTempleInvertedPreBossRoomItem11",
                "CollectableSwordsmanSSchoolPot1",
                "CollectableSwordsmanSSchoolPot2",
                "CollectableSwordsmanSSchoolPot3",
                "CollectableSwordsmanSSchoolPot4",
                "CollectableSwordsmanSSchoolPot5",
                "CollectableWoodfallItem1",
                "CollectableWoodfallTempleEntranceRoomBeehive1",
                "CollectableWoodfallTempleGekkoRoomPot1",
                "CollectableWoodfallTempleGekkoRoomPot2",
                "CollectableWoodfallTempleGekkoRoomPot3",
                "CollectableWoodfallTempleGekkoRoomPot4",
                "CollectableWoodfallTemplePreBossRoomItem1",
                "CollectableWoodfallTemplePreBossRoomItem2",
                "CollectableWoodfallTemplePreBossRoomItem3",
                "CollectableWoodfallTemplePreBossRoomItem4",
                "CollectableBeneathTheWellBugAndBombRoomPot1",
                "CollectableBeneathTheWellBugAndBombRoomPot2",
                "CollectableBeneathTheWellBugAndBombRoomPot3",
                "CollectableBeneathTheWellBugAndBombRoomPot4",
                "CollectableBeneathTheWellBugAndBombRoomPot5",
                "CollectableGoronVillageWinterSmallSnowball3",
                "CollectableGoronVillageWinterSmallSnowball4",
                "CollectableGreatBayCoastPot9",
                "CollectableGreatBayTempleBlueChuchuValveRoomBarrel2",
                "CollectableGreatBayTempleTopmostRoomWithGreenValveBarrel2",
                "CollectableIkanaCanyonMainAreaGrass2",
                "CollectableMountainVillageSpringSmallSnowball2",
                "CollectableMountainVillageWinterSmallSnowball2",
                "CollectableSnowheadSmallSnowball2",
                "CollectableStoneTowerPot4",
                "CollectableStoneTowerInvertedStoneTowerFlippedPot1",
                "CollectableZoraCapePot2",
                "CollectableAncientCastleOfIkana1FWestStaircasePot1",
                "CollectableGoronVillageWinterSmallSnowball5",
                "CollectableGoronVillageWinterSmallSnowball6",
                "CollectablePiratesFortressInteriorTelescopeRoomPot2",
                "CollectableWoodfallPot1",
                "CollectableGoronShrineGoronKidSRoomPot1",
                "CollectableGoronShrineGoronKidSRoomPot2",
                "CollectableGoronShrineMainRoomPot1",
                "CollectableGoronShrineMainRoomPot2",
                "CollectableGoronShrineMainRoomPot3",
                "CollectableGoronVillageWinterSmallSnowball7",
                "CollectableGoronVillageWinterSmallSnowball8",
                "CollectableSouthernSwampClearCentralSwampGrass1",
                "CollectableSouthernSwampPoisonedCentralSwampGrass1",
                "CollectableWoodfallPot2",
                "CollectableDampesHouseBasementPot5",
                "CollectableDampesHouseBasementPot6",
                "CollectableDampesHouseBasementPot7",
                "CollectableDekuPalaceEastInnerGardenItem3",
                "CollectableDekuPalaceEastInnerGardenItem4",
                "CollectableDekuPalaceEastInnerGardenItem5",
                "CollectableDekuPalaceEastInnerGardenItem6",
                "CollectableDekuPalaceEastInnerGardenItem7",
                "CollectableDekuPalaceEastInnerGardenItem8",
                "CollectableDekuPalaceWestInnerGardenItem4",
                "CollectableDekuPalaceWestInnerGardenItem5",
                "CollectableDekuPalaceWestInnerGardenItem6",
                "CollectableDekuPalaceWestInnerGardenItem7",
                "CollectableDekuPalaceWestInnerGardenItem8",
                "CollectableDekuPalaceWestInnerGardenItem9",
                "CollectableDekuPalaceWestInnerGardenItem10",
                "CollectableDekuShrineGiantRoomFloor1Item1",
                "CollectableDekuShrineGiantRoomFloor1Item2",
                "CollectableDekuShrineGiantRoomFloor1Item3",
                "CollectableDekuShrineGiantRoomFloor1Item4",
                "CollectableDekuShrineGiantRoomFloor1Item5",
                "CollectableDekuShrineGiantRoomFloor1Item6",
                "CollectableDekuShrineWaterRoomWithPlatformsItem1",
                "CollectableDekuShrineWaterRoomWithPlatformsItem2",
                "CollectableDekuShrineWaterRoomWithPlatformsItem3",
                "CollectableDekuShrineWaterRoomWithPlatformsItem4",
                "CollectableDekuShrineWaterRoomWithPlatformsItem5",
                "CollectableDekuShrineWaterRoomWithPlatformsItem6",
                "CollectableDekuShrineRoomBeforeFlameWallsItem1",
                "CollectableDekuShrineRoomBeforeFlameWallsItem2",
                "CollectableDekuShrineRoomBeforeFlameWallsItem3",
                "CollectableDekuShrineRoomBeforeFlameWallsItem4",
                "CollectableDekuShrineRoomBeforeFlameWallsItem5",
                "CollectableDekuShrineRoomBeforeFlameWallsItem6",
                "CollectableDekuShrineDekuButlerSRoomItem1",
                "CollectableDekuShrineDekuButlerSRoomItem2",
                "CollectableDekuShrineDekuButlerSRoomItem3",
                "CollectableDekuShrineDekuButlerSRoomItem4",
                "CollectableDekuShrineDekuButlerSRoomItem5",
                "CollectableDekuShrineDekuButlerSRoomItem6",
                "CollectableDekuShrineDekuButlerSRoomItem7",
                "CollectableDekuShrineDekuButlerSRoomItem8",
                "CollectableDekuShrineDekuButlerSRoomItem9",
                "CollectableDekuShrineDekuButlerSRoomItem10",
                "CollectableDekuShrineGreyBoulderRoomPot1",
                "CollectableEastClockTownWoodenCrateSmall1",
                "CollectableGreatBayTempleWaterControlRoomItem3",
                "CollectableGreatBayTempleWaterControlRoomItem4",
                "CollectableIkanaGraveyardIkanaGraveyardLowerGrass2",
                "CollectableMagicHagsPotionShopItem1",
                "CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem2",
                "CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem3",
                "CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem4",
                "CollectablePiratesFortressInteriorCellRoomWithPieceOfHeartItem5",
                "CollectableSecretShrineEntranceRoomItem1",
                "CollectableSecretShrineEntranceRoomItem2",
                "CollectableSecretShrineEntranceRoomItem3",
                "CollectableSecretShrineEntranceRoomItem4",
                "CollectableSecretShrineEntranceRoomItem5",
                "CollectableSecretShrineEntranceRoomItem6",
                "CollectableSecretShrineEntranceRoomItem7",
                "CollectableSecretShrineEntranceRoomItem8",
                "CollectableSecretShrineEntranceRoomItem9",
                "CollectableSecretShrineEntranceRoomItem10",
                "CollectableSecretShrineEntranceRoomItem11",
                "CollectableSecretShrineEntranceRoomItem12",
                "CollectableSecretShrineEntranceRoomItem13",
                "CollectableSecretShrineEntranceRoomItem14",
                "CollectableSecretShrineEntranceRoomItem15",
                "CollectableSecretShrineEntranceRoomItem16",
                "CollectableSecretShrineEntranceRoomItem17",
                "CollectableSouthernSwampClearMagicHagsPotionShopExteriorPot2",
                "CollectableSouthernSwampPoisonedMagicHagsPotionShopExteriorPot2",
                "CollectableStoneTowerTempleLavaRoomItem3",
                "CollectableStoneTowerTempleLavaRoomItem4",
                "CollectableStoneTowerTempleLavaRoomItem5",
                "CollectableStoneTowerTempleInvertedEyegoreRoomItem2",
                "CollectableClockTowerRooftopPot1",
                "CollectableClockTowerRooftopPot2",
                "CollectableClockTowerRooftopPot3",
                "CollectableClockTowerRooftopPot4",
                "CollectableGoronRacetrackPot1",
                "CollectableGoronRacetrackPot2",
                "CollectableGoronRacetrackPot3",
                "CollectableGoronRacetrackPot4",
                "CollectableGoronRacetrackPot5",
                "CollectableGoronRacetrackPot6",
                "CollectableGoronRacetrackPot7",
                "CollectableGoronRacetrackPot8",
                "CollectableGoronRacetrackPot9",
                "CollectableGoronRacetrackPot10",
                "CollectableGoronRacetrackPot11",
                "CollectableGoronRacetrackPot12",
                "CollectableGoronRacetrackPot13",
                "CollectableGoronRacetrackPot14",
                "CollectableGoronRacetrackPot15",
                "CollectableGoronRacetrackPot16",
                "CollectableGoronRacetrackPot17",
                "CollectableGoronRacetrackPot18",
                "CollectableGoronRacetrackPot19",
                "CollectableGoronRacetrackPot20",
                "CollectableGoronRacetrackPot21",
                "CollectableGoronRacetrackPot22",
                "CollectableGoronRacetrackPot23",
                "CollectableGoronRacetrackPot24",
                "CollectableGoronRacetrackPot25",
                "CollectableGoronRacetrackPot26",
                "CollectableGoronRacetrackPot27",
                "CollectableGoronShrineGoronKidSRoomPot3",
                "CollectableGoronShrineMainRoomPot4",
                "CollectableGoronShrineMainRoomPot5",
                "CollectableGoronShrineMainRoomPot6",
                "CollectableGreatBayCoastPot10",
                "CollectableGreatBayTempleBlueChuchuValveRoomWoodenCrateLarge1",
                "CollectableIgosDuIkanaSLairIgosDuIkanaSRoomPot3",
                "CollectableIkanaCanyonMainAreaGrass3",
                "CollectableMilkRoadGrass2",
                "CollectableMountainVillageSpringSmallSnowball3",
                "CollectableMountainVillageWinterSmallSnowball3",
                "CollectableMountainVillageWinterSmallSnowball4",
                "CollectableMountainVillageWinterSmallSnowball5",
                "CollectableSnowheadSmallSnowball3",
                "CollectableStoneTowerPot5",
                "CollectableStoneTowerInvertedStoneTowerFlippedPot2",
                "CollectableTheMoonLinkTrialEntrancePot1",
                "CollectableTheMoonLinkTrialEntrancePot2",
                "CollectableTheMoonLinkTrialEntrancePot3",
                "CollectableTheMoonLinkTrialEntrancePot4",
                "CollectableZoraCapePot3",
                "CollectableDampesHouseBasementPot8",
                "CollectablePiratesFortressItem1",
                "CollectablePiratesFortressItem2",
                "CollectablePiratesFortressItem3",
                "CollectableDekuShrineGiantRoomFloor1Item7",
                "CollectableDekuShrineGiantRoomFloor1Item8",
                "CollectableGreatBayTempleWaterControlRoomItem5",
                "CollectableGreatBayTempleCompassBossKeyRoomItem1",
                "CollectableGreatBayTempleCompassBossKeyRoomItem2",
                "CollectableGreatBayTempleTopmostRoomWithGreenValveItem1",
                "CollectableGreatBayTempleTopmostRoomWithGreenValveItem2",
                "CollectableLaundryPoolItem1",
                "CollectableLaundryPoolItem2",
                "CollectableLaundryPoolItem3",
                "CollectableMountainVillageWinterMountainVillageSpringItem1",
                "CollectableSnowheadTempleIceBlockRoomItem1",
                "CollectableSnowheadTempleIceBlockRoomItem2",
                "CollectableSnowheadTempleIceBlockRoomItem3",
                "CollectableSouthernSwampPoisonedCentralSwampBeehive1",
                "CollectableStoneTowerTempleLavaRoomItem6",
                "CollectableStoneTowerTempleEyegoreRoomItem1",
                "CollectableStoneTowerTempleMirrorRoomWoodenCrateLarge1",
                "CollectableStoneTowerTempleMirrorRoomWoodenCrateLarge2",
                "CollectableStoneTowerTempleEyegoreRoomItem2",
                "CollectableStoneTowerTempleInvertedEyegoreRoomItem3",
                "CollectableStoneTowerTempleInvertedAirRoomItem1",
                "CollectableStoneTowerTempleInvertedAirRoomItem2",
                "CollectableTerminaFieldItem1",
                "CollectableWoodfallTemplePreBossRoomItem5",
                "CollectableWoodfallTemplePreBossRoomItem6",
                "CollectableAncientCastleOfIkanaCastleExteriorGrass3",
                "CollectableAncientCastleOfIkanaCastleExteriorGrass4",
                "CollectableAncientCastleOfIkanaFireCeilingRoomPot1",
                "CollectableAncientCastleOfIkanaHoleRoomPot1",
                "CollectableAncientCastleOfIkanaHoleRoomPot2",
                "CollectableAstralObservatorySewerPot1",
                "CollectableAstralObservatorySewerPot2",
                "CollectableAstralObservatoryObservatoryBombersHideoutPot3",
                "CollectableBeneathTheGraveyardMainAreaPot2",
                "CollectableDekuPalaceEastInnerGardenPot1",
                "CollectableDekuPalaceEastInnerGardenPot2",
                "CollectableGoronRacetrackPot28",
                "CollectableGoronRacetrackPot29",
                "CollectableGoronRacetrackPot30",
                "CollectableGoronShrineMainRoomPot7",
                "CollectableGoronShrineMainRoomPot8",
                "CollectableGoronVillageWinterLargeSnowball4",
                "CollectableGoronVillageWinterLargeSnowball5",
                "CollectableGoronVillageWinterLargeSnowball6",
                "CollectableGoronVillageWinterSmallSnowball9",
                "CollectableGoronVillageWinterSmallSnowball10",
                "CollectableIgosDuIkanaSLairPreBossRoomPot3",
                "CollectableIkanaGraveyardIkanaGraveyardLowerGrass3",
                "CollectableMountainVillageWinterSmallSnowball6",
                "CollectableMountainVillageWinterSmallSnowball7",
                "CollectableMountainVillageWinterLargeSnowball3",
                "CollectableMountainVillageWinterLargeSnowball4",
                "CollectableOceansideSpiderHouseMainRoomPot1",
                "CollectableOceansideSpiderHouseEntrancePot3",
                "CollectableOceansideSpiderHouseMainRoomPot2",
                "CollectableOceansideSpiderHouseStorageRoomPot1",
                "CollectablePathToGoronVillageWinterLargeSnowball1",
                "CollectablePathToGoronVillageWinterLargeSnowball2",
                "CollectablePathToGoronVillageWinterLargeSnowball3",
                "CollectablePathToGoronVillageWinterLargeSnowball4",
                "CollectablePathToGoronVillageWinterLargeSnowball5",
                "CollectablePathToGoronVillageWinterLargeSnowball6",
                "CollectablePathToGoronVillageWinterLargeSnowball7",
                "CollectablePathToGoronVillageWinterLargeSnowball8",
                "CollectablePathToGoronVillageWinterLargeSnowball9",
                "CollectablePathToGoronVillageWinterLargeSnowball10",
                "CollectablePathToGoronVillageWinterLargeSnowball11",
                "CollectablePathToGoronVillageWinterLargeSnowball12",
                "CollectablePathToGoronVillageWinterLargeSnowball13",
                "CollectablePathToGoronVillageWinterLargeSnowball14",
                "CollectablePathToGoronVillageWinterSmallSnowball1",
                "CollectablePathToGoronVillageWinterSmallSnowball2",
                "CollectablePathToGoronVillageWinterSmallSnowball3",
                "CollectablePathToMountainVillageSmallSnowball1",
                "CollectablePathToSnowheadLargeSnowball1",
                "CollectablePathToSnowheadLargeSnowball2",
                "CollectablePathToSnowheadLargeSnowball3",
                "CollectablePathToSnowheadLargeSnowball4",
                "CollectablePinnacleRockPot1",
                "CollectablePinnacleRockPot2",
                "CollectablePinnacleRockPot3",
                "CollectablePinnacleRockPot4",
                "CollectableSecretShrineMainRoomPot3",
                "CollectableSecretShrineMainRoomPot4",
                "CollectableSnowheadLargeSnowball1",
                "CollectableSnowheadLargeSnowball2",
                "CollectableSnowheadLargeSnowball3",
                "CollectableSnowheadLargeSnowball4",
                "CollectableSnowheadLargeSnowball5",
                "CollectableSnowheadLargeSnowball6",
                "CollectableStoneTowerPot6",
                "CollectableStoneTowerPot7",
                "CollectableStoneTowerPot8",
                "CollectableStoneTowerPot9",
                "CollectableStoneTowerPot10",
                "CollectableZoraCapePot4",
                "CollectableRomaniRanchInvisibleItem1",
                "CollectableRomaniRanchInvisibleItem2",
                "CollectableRomaniRanchInvisibleItem3",
                "CollectableRomaniRanchInvisibleItem4",
                "CollectableRomaniRanchInvisibleItem5",
                "CollectableRomaniRanchInvisibleItem6",
                "CollectableTerminaFieldInvisibleItem1",
                "CollectableTerminaFieldInvisibleItem2",
                "CollectableTerminaFieldInvisibleItem3",
                "CollectableTerminaFieldInvisibleItem4",
                "CollectableTerminaFieldInvisibleItem5",
                "CollectableTerminaFieldInvisibleItem6",
                "CollectableTerminaFieldInvisibleItem7",
                "CollectableTerminaFieldInvisibleItem8",
                "CollectableTerminaFieldInvisibleItem9",
                "CollectableTerminaFieldInvisibleItem10",
                "CollectableTerminaFieldInvisibleItem11",
                "CollectableSwampSpiderHouseInvisibleItem1",
                "CollectableSwampSpiderHouseInvisibleItem2",
                "CollectableSwampSpiderHouseInvisibleItem3",
                "CollectableSwampSpiderHouseInvisibleItem4",
                "CollectableSwampSpiderHouseInvisibleItem5",
                "CollectableTerminaFieldTreeItem1",
                "CollectableTerminaFieldPillarItem1",
                "CollectableTerminaFieldTelescopeGuay1",
                "CollectableSwordsmanSchoolGong1",
                "CollectableBeanGrottoSoftSoil1",
                "CollectableDekuPalaceSoftSoil1",
                "CollectableDoggyRacetrackSoftSoil1",
                "CollectableGreatBayCoastSoftSoil1",
                "CollectableRomaniRanchSoftSoil1",
                "CollectableRomaniRanchSoftSoil2",
                "CollectableSecretShrineSoftSoil1",
                "CollectableStoneTowerSoftSoil1",
                "CollectableStoneTowerSoftSoil2",
                "CollectableSwampSpiderHouseSoftSoil1",
                "CollectableSwampSpiderHouseSoftSoil2",
                "CollectableTerminaFieldSoftSoil1",
                "CollectableTerminaFieldSoftSoil2",
                "CollectableTerminaFieldSoftSoil3",
                "CollectableTerminaFieldSoftSoil4",
                "CollectableTerminaFieldGuay1",
                "CollectableTerminaFieldGuay2",
                "CollectableTerminaFieldGuay3",
                "CollectableTerminaFieldGuay4",
                "CollectableTerminaFieldGuay5",
                "CollectableTerminaFieldGuay6",
                "CollectableTerminaFieldGuay7",
                "CollectableTerminaFieldGuay8",
                "CollectableTerminaFieldGuay9",
                "CollectableTerminaFieldGuay10",
                "CollectableTerminaFieldGuay11",
                "CollectableTerminaFieldGuay12",
                "CollectableTerminaFieldGuay13",
                "CollectableTerminaFieldGuay14",
                "CollectableTerminaFieldGuay15",
                "CollectableTerminaFieldGuay16",
                "CollectableTerminaFieldGuay17",
                "CollectableTerminaFieldGuay18",
                "CollectableTerminaFieldGuay19",
                "CollectableTerminaFieldGuay20",
                "CollectableTerminaFieldGuay21",
                "CollectableTerminaFieldGuay22",
                "CollectableTerminaFieldGuay23",
                "CollectableDekuPalaceRupeeCluster1",
                "CollectableDekuPalaceRupeeCluster2",
                "CollectableDekuPalaceRupeeCluster3",
                "CollectableDekuPalaceRupeeCluster4",
                "CollectableDekuPalaceRupeeCluster5",
                "CollectableDekuPalaceRupeeCluster6",
                "CollectableDekuPalaceRupeeCluster7",
                "CollectableBeneathTheGraveyardRupeeCluster1",
                "CollectableBeneathTheGraveyardRupeeCluster2",
                "CollectableBeneathTheGraveyardRupeeCluster3",
                "CollectableBeneathTheGraveyardRupeeCluster4",
                "CollectableBeneathTheGraveyardRupeeCluster5",
                "CollectableBeneathTheGraveyardRupeeCluster6",
                "CollectableBeneathTheGraveyardRupeeCluster7",
                "CollectableTerminaFieldSongWall1",
                "CollectableTerminaFieldSongWall2",
                "CollectableTerminaFieldSongWall3",
                "CollectableTerminaFieldSongWall4",
                "CollectableTerminaFieldSongWall5",
                "CollectableTerminaFieldSongWall6",
                "CollectableTerminaFieldSongWall7",
                "CollectableTerminaFieldSongWall8",
                "CollectableTerminaFieldSongWall9",
                "CollectableTerminaFieldSongWall10",
                "CollectableTerminaFieldSongWall11",
                "CollectableTerminaFieldSongWall12",
                "CollectableTerminaFieldSongWall13",
                "CollectableTerminaFieldSongWall14",
                "CollectableTerminaFieldSongWall15",
                "CollectableDekuPlaygroundItem1",
                "CollectableDekuPlaygroundItem2",
                "CollectableDekuPlaygroundItem3",
                "CollectableDekuPlaygroundItem4",
                "CollectableDekuPlaygroundItem5",
                "CollectableDekuPlaygroundItem6",
                "CollectableDekuPlaygroundItem7",
                "CollectableDekuPlaygroundItem8",
                "CollectableDekuPlaygroundItem9",
                "CollectableDekuPlaygroundItem10",
                "CollectableDekuPlaygroundItem11",
                "CollectableDekuPlaygroundItem12",
                "CollectableDekuPlaygroundItem13",
                "CollectableDekuPlaygroundItem14",
                "CollectableDekuPlaygroundItem15",
                "CollectableDekuPlaygroundItem16",
                "CollectableDekuPlaygroundItem17",
                "CollectableDekuPlaygroundItem18",
                "CollectablePiratesFortressHitTag1",
                "CollectablePiratesFortressHitTag2",
                "CollectablePiratesFortressHitTag3",
                "CollectablePiratesFortressHitTag4",
                "CollectablePiratesFortressHitTag5",
                "CollectablePiratesFortressHitTag6",
                "CollectablePiratesFortressInteriorHookshotRoomHitTag1",
                "CollectablePiratesFortressInteriorHookshotRoomHitTag2",
                "CollectablePiratesFortressInteriorHookshotRoomHitTag3",
                "CollectableSwampSpiderHouseHitTag1",
                "CollectableSwampSpiderHouseHitTag2",
                "CollectableSwampSpiderHouseHitTag3",
                "CollectableSwampSpiderHouseHitTag4",
                "CollectableSwampSpiderHouseHitTag5",
                "CollectableSwampSpiderHouseHitTag6",
                "CollectableSwampSpiderHouseHitTag7",
                "CollectableSwampSpiderHouseHitTag8",
                "CollectableSwampSpiderHouseHitTag9",
                "CollectableSwampSpiderHouseHitTag10",
                "CollectableSwampSpiderHouseHitTag11",
                "CollectableSwampSpiderHouseHitTag12",
                "CollectableOceansideSpiderHouseHitTag1",
                "CollectableOceansideSpiderHouseHitTag2",
                "CollectableOceansideSpiderHouseHitTag3",
                "CollectableOceansideSpiderHouseHitTag4",
                "CollectableOceansideSpiderHouseHitTag5",
                "CollectableOceansideSpiderHouseHitTag6",
                "CollectableOceansideSpiderHouseHitTag7",
                "CollectableOceansideSpiderHouseHitTag8",
                "CollectableOceansideSpiderHouseHitTag9",
                "CollectableTerminaFieldHitTag1",
                "CollectableTerminaFieldHitTag2",
                "CollectableTerminaFieldHitTag3",
                "CollectableTerminaFieldHitTag4",
                "CollectableTerminaFieldHitTag5",
                "CollectableTerminaFieldHitTag6",
                "CollectableTerminaFieldHitTag7",
                "CollectableTerminaFieldHitTag8",
                "CollectableTerminaFieldHitTag9",
                "CollectableCuccoShackHitTag1",
                "CollectableCuccoShackHitTag2",
                "CollectableCuccoShackHitTag3",
                "CollectableCuccoShackHitTag4",
                "CollectableCuccoShackHitTag5",
                "CollectableCuccoShackHitTag6",
                "CollectableIkanaGraveyardHitTag1",
                "CollectableIkanaGraveyardHitTag2",
                "CollectableIkanaGraveyardHitTag3",
                "CollectableIkanaGraveyardHitTag4",
                "CollectableIkanaGraveyardHitTag5",
                "CollectableIkanaGraveyardHitTag6",
                "CollectableIkanaGraveyardHitTag7",
                "CollectableIkanaGraveyardHitTag8",
                "CollectableIkanaGraveyardHitTag9",
                "CollectableIkanaGraveyardHitTag10",
                "CollectableIkanaGraveyardHitTag11",
                "CollectableIkanaGraveyardHitTag12",
                "CollectableStockPotInnHitTag1",
                "CollectableStockPotInnHitTag2",
                "CollectableStockPotInnHitTag3",
                "CollectableEastClockTownHitTag1",
                "CollectableEastClockTownHitTag2",
                "CollectableEastClockTownHitTag3",
                "CollectableEastClockTownHitTag4",
                "CollectableEastClockTownHitTag5",
                "CollectableEastClockTownHitTag6",
                "CollectableEastClockTownHitTag7",
                "CollectableEastClockTownHitTag8",
                "CollectableEastClockTownHitTag9",
                "CollectableSouthClockTownHitTag1",
                "CollectableSouthClockTownHitTag2",
                "CollectableSouthClockTownHitTag3",
                "CollectableTerminaFieldEnemy1",
                "CollectablePiratesFortressInteriorHookshotRoomPot1",
                "CollectablePiratesFortressInteriorHookshotRoomPot2",
                "CollectableTerminaFieldRock1",
                "CollectableTerminaFieldRock2",
                "CollectableIkanaGraveyardIkanaGraveyardUpperRock1",
                "CollectableIkanaGraveyardIkanaGraveyardUpperRock2",
                "CollectableIkanaGraveyardIkanaGraveyardUpperRock3",
                "CollectableTerminaFieldRock3",
                "CollectableTerminaFieldRock4",
                "CollectableTerminaFieldRock5",
                "CollectableTerminaFieldRock6",
                "CollectableTerminaFieldRock7",
                "CollectableIkanaGraveyardIkanaGraveyardUpperRock4",
                "CollectableIkanaGraveyardIkanaGraveyardUpperRock5",
                "CollectableTerminaFieldRock8",
                "CollectableTerminaFieldRock9",
                "CollectableMilkRoadKeatonGrass1",
                "CollectableMilkRoadKeatonGrass2",
                "CollectableMilkRoadKeatonGrass3",
                "CollectableMilkRoadKeatonGrass4",
                "CollectableMilkRoadKeatonGrass5",
                "CollectableMilkRoadKeatonGrass6",
                "CollectableMilkRoadKeatonGrass7",
                "CollectableMilkRoadKeatonGrass8",
                "CollectableMilkRoadKeatonGrass9",
                "CollectableNorthClockTownKeatonGrass1",
                "CollectableNorthClockTownKeatonGrass2",
                "CollectableNorthClockTownKeatonGrass3",
                "CollectableNorthClockTownKeatonGrass4",
                "CollectableNorthClockTownKeatonGrass5",
                "CollectableNorthClockTownKeatonGrass6",
                "CollectableNorthClockTownKeatonGrass7",
                "CollectableNorthClockTownKeatonGrass8",
                "CollectableNorthClockTownKeatonGrass9",
                "CollectableMountainVillageSpringKeatonGrass1",
                "CollectableMountainVillageSpringKeatonGrass2",
                "CollectableMountainVillageSpringKeatonGrass3",
                "CollectableMountainVillageSpringKeatonGrass4",
                "CollectableMountainVillageSpringKeatonGrass5",
                "CollectableMountainVillageSpringKeatonGrass6",
                "CollectableMountainVillageSpringKeatonGrass7",
                "CollectableMountainVillageSpringKeatonGrass8",
                "CollectableMountainVillageSpringKeatonGrass9",
                "CollectableOceansideSpiderHouseMaskRoomPot1",
                "CollectableOceansideSpiderHouseMaskRoomPot2",
                "CollectableIkanaCanyonMainAreaGrass4",
                "CollectableIkanaCanyonMainAreaGrass5",
                "CollectableIkanaCanyonMainAreaGrass6",
                "CollectablePathToSnowheadSmallSnowball1",
                "CollectablePathToSnowheadSmallSnowball2",
                "CollectablePathToSnowheadSmallSnowball3",
                "CollectablePathToSnowheadSmallSnowball4",
                "CollectablePathToMountainVillageSmallSnowball2",
                "CollectablePathToMountainVillageSmallSnowball3",
                "CollectablePathToMountainVillageSmallSnowball4",
                "CollectableZoraCapeJarGame1",
                "CollectableIkanaGraveyardDay2Bats1",
                "CollectableCuccoShackPottedPlant1",
                "GossipTerminaSouth",
                "GossipSwampPotionShop",
                "GossipMountainSpringPath",
                "GossipMountainPath",
                "GossipOceanZoraGame",
                "GossipCanyonRoad",
                "GossipCanyonDock",
                "GossipCanyonSpiritHouse",
                "GossipTerminaMilk",
                "GossipTerminaWest",
                "GossipTerminaNorth",
                "GossipTerminaEast",
                "GossipRanchTree",
                "GossipRanchBarn",
                "GossipMilkRoad",
                "GossipOceanFortress",
                "GossipSwampRoad",
                "GossipTerminaObservatory",
                "GossipRanchCuccoShack",
                "GossipRanchRacetrack",
                "GossipRanchEntrance",
                "GossipCanyonRavine",
                "GossipMountainSpringFrog",
                "GossipSwampSpiderHouse",
                "GossipTerminaGossipLarge",
                "GossipTerminaGossipGuitar",
                "GossipTerminaGossipPipes",
                "GossipTerminaGossipDrums",
            };

            var itemNameIndex = 0;
            foreach (var line in lines)
            {
                if (line.StartsWith("- "))
                {
                    if (itemNameIndex >= itemNames.Count)
                    {
                        var itemName = line.Substring(2);
                        itemNames.Add(itemName);
                    }
                    itemNameIndex++;
                }
            }

            var items = new List<JsonFormatLogicItem>();

            int i = 0;
            while (true)
            {
                if (i >= lines.Count)
                {
                    break;
                }
                if (lines[i].StartsWith("#"))
                {
                    i++;
                    continue;
                }
                if (lines[i].Contains("-"))
                {
                    i++;
                    continue;
                }
                else
                {
                    var item = new JsonFormatLogicItem();
                    item.RequiredItems = new List<string>();
                    if (lines[i] != "")
                    {
                        foreach (var j in lines[i].Split(','))
                        {
                            item.RequiredItems.Add(itemNames[Convert.ToInt32(j)]);
                        }
                    }
                    item.ConditionalItems = new List<List<string>>();
                    if (lines[i + 1] != "")
                    {
                        foreach (var j in lines[i + 1].Split(';'))
                        {
                            var conditionals = new List<string>();
                            foreach (var k in j.Split(','))
                            {
                                conditionals.Add(itemNames[Convert.ToInt32(k)]);
                            }
                            item.ConditionalItems.Add(conditionals);
                        }
                    }
                    item.TimeNeeded = (TimeOfDay)Convert.ToInt32(lines[i + 2]);
                    item.TimeAvailable = (TimeOfDay)Convert.ToInt32(lines[i + 3]);
                    var trickInfo = lines[i + 4].Split(new char[] { ';' }, 2);
                    item.IsTrick = trickInfo.Length > 1;
                    item.TrickTooltip = item.IsTrick ? trickInfo[1] : null;
                    if (string.IsNullOrWhiteSpace(item.TrickTooltip))
                    {
                        item.TrickTooltip = null;
                    }
                    item.Id = itemNames[items.Count];
                    items.Add(item);
                    i += 5;
                }
            }

            //items.Remove("AreaWoodfallNew");
            //items.Remove("AreaSnowheadNew");
            //items.Remove("AreaGreatBayNew");
            //items.Remove("AreaLANew");
            //items.Remove("AreaInvertedStoneTowerNew");

            return new JsonFormatLogic
            {
                Version = 1,
                Logic = items,
            };
        }

        private static void AddBossRemains(JsonFormatLogic logicObject)
        {
            const int startIndex = 1068;
            var itemNames = new string[]
            {
                "RemainsOdolwa",
                "RemainsGoht",
                "RemainsGyorg",
                "RemainsTwinmold",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 2;
        }

        private static void AddOcarinaAndSongOfTime(JsonFormatLogic logicObject)
        {
            const int startIndex = 94;
            var itemNames = new string[]
            {
                "ItemOcarina",
                "SongTime",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 3;
        }

        private static void AddMultiLocations(JsonFormatLogic logicObject)
        {
            const int startIndex = 1075;
            var itemNames = new string[]
            {
                "ItemTingleMapTownInTown",
                "ItemTingleMapTownInCanyon",
                "ItemTingleMapWoodfallInSwamp",
                "ItemTingleMapWoodfallInTown",
                "ItemTingleMapSnowheadInMountain",
                "ItemTingleMapSnowheadInSwamp",
                "ItemTingleMapRanchInRanch",
                "ItemTingleMapRanchInMountain",
                "ItemTingleMapGreatBayInOcean",
                "ItemTingleMapGreatBayInRanch",
                "ItemTingleMapStoneTowerInCanyon",
                "ItemTingleMapStoneTowerInOcean",
                "HeartPiecePostBoxInSCT",
                "HeartPiecePostBoxInNCT",
                "HeartPiecePostBoxInECT",
                "HeartPieceKeatonQuizInNCT",
                "HeartPieceKeatonQuizInMilkRoad",
                "HeartPieceKeatonQuizInMountainVillage",
                "SongOathInWFT",
                "SongOathInSHT",
                "SongOathInGBT",
                "SongOathInISTT",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 4;
        }

        private static void AddMultiLocationGoronShop(JsonFormatLogic logicObject)
        {
            const int startIndex = 1097;
            var itemNames = new string[]
            {
                "ShopItemGoronBomb10InWinter",
                "ShopItemGoronBomb10InSpring",
                "ShopItemGoronArrow10InWinter",
                "ShopItemGoronArrow10InSpring",
                "ShopItemGoronRedPotionInWinter",
                "ShopItemGoronRedPotionInSpring",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 5;
        }

        private static void AddOtherMagicBean(JsonFormatLogic logicObject)
        {
            const int startIndex = 123;
            var itemNames = new string[]
            {
                "OtherMagicBean",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 6;
        }

        private static void AddMultiLocationBusinessScrubs(JsonFormatLogic logicObject)
        {
            const int startIndex = 1104;
            var itemNames = new string[]
            {
                "ShopItemBusinessScrubMagicBeanInSwamp",
                "ShopItemBusinessScrubMagicBeanInTown",
                "UpgradeBiggestBombBagInMountain",
                "UpgradeBiggestBombBagInSwamp",
                "ShopItemBusinessScrubGreenPotionInOcean",
                "ShopItemBusinessScrubGreenPotionInMountain",
                "ShopItemBusinessScrubBluePotionInCanyon",
                "ShopItemBusinessScrubBluePotionInOcean",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 7;
        }

        private static void AddOtherTimeTravel(JsonFormatLogic logicObject)
        {
            const int startIndex = 124;
            var itemNames = new string[]
            {
                "OtherTimeTravel",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 8;
        }

        private static void AddBeansAndDekuPlayground(JsonFormatLogic logicObject)
        {
            const int startIndex = 124;
            var itemNames = new string[]
            {
                "OtherLimitlessBeans",
                "OtherPlayDekuPlayground",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 9;
        }

        private static void AddRoyalWallet(JsonFormatLogic logicObject)
        {
            const int startIndex = 32;
            var itemNames = new string[]
            {
                "UpgradeRoyalWallet",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 10;
        }

        private static void AddMultiLocationClockTownFairy(JsonFormatLogic logicObject)
        {
            const int startIndex = 1116;
            var itemNames = new string[]
            {
                "CollectibleStrayFairyClockTownInLaundryPool",
                "CollectibleStrayFairyClockTownInECT",
            };

            logicObject.Logic.InsertRange(startIndex, itemNames.Select(name => new JsonFormatLogicItem
            {
                Id = name,
                RequiredItems = new List<string>(),
                ConditionalItems = new List<List<string>>(),
            }));
            logicObject.Version = 11;
        }

        private class MigrationItem
        {
            public int ID;
            public List<List<int>> Conditionals = new List<List<int>>();
            public List<int> DependsOnItems = new List<int>();
            public int TimeNeeded = 0;
        }

        private class JsonFormatLogic
        {
            public int Version { get; set; }
            public List<JsonFormatLogicItem> Logic { get; set; }
        }

        private class JsonFormatLogicItem
        {
            public string Id { get; set; }
            public List<string> RequiredItems { get; set; }
            public List<List<string>> ConditionalItems { get; set; }
            public TimeOfDay TimeNeeded { get; set; }
            public TimeOfDay TimeAvailable { get; set; }
            public TimeOfDay TimeSetup { get; set; }
            public bool IsTrick { get; set; }
            public string TrickTooltip { get; set; }
            public string TrickCategory { get; set; }
        }

        [Flags]
        private enum TimeOfDay
        {
            None = 0,
            Day1 = 1,
            Night1 = 2,
            Day2 = 4,
            Night2 = 8,
            Day3 = 16,
            Night3 = 32,
            Any = 63,
        }
    }
}
