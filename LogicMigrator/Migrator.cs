using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MMRando.Extensions;

namespace MMRando.LogicMigrator
{
    public static partial class Migrator
    {
        public const int CurrentVersion = 13;

        public static string ApplyMigrations(string logic)
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

            return string.Join("\r\n", lines);
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

        private class MigrationItem
        {
            public int ID;
            public List<List<int>> Conditionals = new List<List<int>>();
            public List<int> DependsOnItems = new List<int>();
            public int TimeNeeded = 0;
        }
    }
}
