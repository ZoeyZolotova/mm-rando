using System;
using System.Collections.Generic;
using System.Linq;
using MMRando.Extensions;

namespace MMRando.LogicMigrator
{
    public static partial class Migrator
    {
        public const int CurrentVersion = 8;

        public static string[] ApplyMigrations(string[] logic)
        {
            var lines = logic.ToList();

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

            return lines.ToArray();
        }

        public static int GetVersion(List<string> lines)
        {
            if (!lines[0].StartsWith("-version"))
            {
                return -1;
            }
            return int.Parse(lines[0].Split(' ')[1]);
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

        private class MigrationItem
        {
            public int ID;
            public List<List<int>> Conditionals = new List<List<int>>();
            public List<int> DependsOnItems = new List<int>();
        }
    }
}
