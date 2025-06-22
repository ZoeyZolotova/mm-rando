using MMR.Randomizer.GameObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using MMR.Randomizer.Models.Rom;
using MMR.Randomizer.Constants;
using MMR.Common.Utils;

namespace MMR.Randomizer.Utils
{

    public abstract class PlandoCombo
    {
        public string Name { get; set; }
        public string Notes { get; set; }
        public int ItemDrawCount { get; set; } = -1; // plural item list: how many items are randomly selected instead of all
        public bool SkipIfError { get; set; } = false;
    }

    public class PlandoItemCombo : PlandoCombo
    {
        public List<Item> ItemList { get; set; }
        public List<Item> CheckList { get; set; }
        public bool SkipLogic { get; set; } = false;

        public static PlandoItemCombo Copy(PlandoItemCombo pic)
        {
            return new PlandoItemCombo
            {
                // to list makes a copy
                ItemList = [.. pic.ItemList],
                CheckList = [.. pic.CheckList],
                SkipLogic = pic.SkipLogic,
                ItemDrawCount = pic.ItemDrawCount,
                Name = pic.Name,
                Notes = pic.Notes
            };
        }
    }

    public class PlandoMusicCombo : PlandoCombo
    {
        public List<string> SongsList { get; set; }
        public List<string> SlotsList { get; set; }
    }

    // will allow you to select which hints you want
    // public class PlandoHintCombo

    // will allow you to select which entrances you want
    // public class PlandoEntranceCombo

    // will allow you to select which enemies you want
    // public class PlandoEnemyCombo

    class PlandoUtils
    {
        // read plando list(s) from file
        public static List<PlandoItemCombo> ReadAllItemPlandoFiles(List<Item> randomizerItemList)
        {
            // any file with FILEName_Plando.json in the base directory is a plando file
            // resource folders getting nuked, cannot use, just assume base directory is best places for now
            var itemPlandoList = new List<PlandoItemCombo>();
            foreach (var filePath in Directory.GetFiles(Values.MainDirectory, "*ItemPlando.json"))
            {
                var fileName = Path.GetFileName(filePath);
                try
                {
                    string filetext = File.ReadAllText(fileName);
                    // the string enum converter reads the item enumerators as strings rather than their int values, so we can read item/checks by enum
                    // eg, the json can have ItemList: ["MaskBunnyHood"] instead of ItemList: [ 22 ]
                    var workingList = JsonSerializer.Deserialize<List<PlandoItemCombo>>(filetext);
                    // for item in workingList, get object reference from ItemList, beacuse we need to modify these later
                    foreach (PlandoItemCombo pic in workingList)
                    {
                        for (int i = 0; i < pic.ItemList.Count; i++)
                        {
                            if (randomizerItemList.Contains(pic.ItemList[i]))
                            {
                                pic.ItemList[i] = randomizerItemList.Find(u => u == pic.ItemList[i]);
                            }
                        }
                        for (int i = 0; i < pic.CheckList.Count; i++)
                        {
                            if (randomizerItemList.Contains(pic.CheckList[i]))
                            {
                                pic.CheckList[i] = randomizerItemList.Find(u => u == pic.CheckList[i]);
                            }
                        }
                    }
                    itemPlandoList = [.. itemPlandoList, .. workingList];
                }
                catch (System.Text.Json.JsonException e)
                {
                    Debug.Print("Error: exception occurred reading plando file: " + e.ToString());
                    throw new Exception("The following plando file failed to parse:\n"
                                      + Path.GetFileName(filePath) + "\n\n"
                                      + "That means it was not in acceptable json format.\n"
                                      + "Common reasons are missing punctuation or characters,\n"
                                      + "   like a missing comma separating items in a list\n"
                                      + "   or a missing comma separating parts of a single combo\n"
                                      + "   or a missing \" character at the start/end of an item\n"
                                      + "Sometimes the line number of the error is below the actual issue\n\n"
                                      + "The location of the parse error was reported at\n"
                                      + "line number: " + e.LineNumber + ", " + e.BytePositionInLine + " characters deep.");
                }
                catch (Exception e)
                {
                    Debug.Print("Error: exception occurred reading plando file: " + e.ToString());
                    throw new Exception("plando file read exception:\n" + e.ToString() + "\n file: " + Path.GetFileName(filePath));
                }
            }
            return itemPlandoList;
        }

        public static List<PlandoMusicCombo> ReadAllMusicPlandoFiles(string directory = "Resources/music")
        {
            var musicPlandoList = new List<PlandoMusicCombo>();
            foreach (String filePath in Directory.GetFiles(directory, "*MusicPlando.json"))
            {
                try
                {
                    var fileText = File.ReadAllText(filePath);
                    var workingList = JsonSerializer.Deserialize<List<PlandoMusicCombo>>(fileText) ?? throw new Exception($"MusicPlando: Plando file [{filePath}] failed to parse");
                    foreach (var plandoEvent in workingList)
                    {
                        if (plandoEvent.SongsList == null)
                        {
                            throw new Exception($"MusicPlando: Plando file\n [{filePath}]\n" +
                                $" has a broken SongsList for event:\n [{plandoEvent.Name}]");
                        }
                        if (plandoEvent.SlotsList == null)
                        {
                            throw new Exception($"MusicPlando: Plando file\n [{filePath}]\n" +
                                $" has a broken SlotsList for event:\n [{plandoEvent.Name}]");
                        }

                    }

                    musicPlandoList = [.. musicPlandoList, .. workingList];
                }
                catch (Exception ex)
                {
                    Debug.Print("Error: exception occurred reading plando file: " + ex.ToString());
#if DEBUG
                      throw new Exception($"plando file read error: " + ex.ToString() + " file: " + Path.GetFileName(filePath));
#endif
                }
            }
            return musicPlandoList;
        }

        public static List<(SequenceInfo, SequenceInfo)> GetRandomizedSongPlacements(Random random, System.Text.StringBuilder songLog)
        {
            void DebugOut(string s)
            {
                Debug.WriteLine(s);
                songLog.AppendLine(s);
            }

            var returnSongTupleList = new List<(SequenceInfo, SequenceInfo)>();
            var allPlandoMusicCombos = ReadAllMusicPlandoFiles(Values.MusicDirectory);

            foreach (PlandoMusicCombo musicCombo in allPlandoMusicCombos)
            {
                // shuffle songs and slots based on our random seed
                musicCombo.SongsList = [.. musicCombo.SongsList.OrderBy(x => random.Next())];
                musicCombo.SlotsList = [.. musicCombo.SlotsList.OrderBy(x => random.Next())];

                // clean combo of already placed items and checks
                var previouslyUsedSongs = returnSongTupleList.Select(u => u.Item1.Name).ToList();
                foreach (string i in musicCombo.SongsList.ToList())
                {
                    if (previouslyUsedSongs.Contains(i))
                    {
                        DebugOut("Song already placed, removed from combo: " + i);
                        musicCombo.SongsList.Remove(i);
                    }
                    else if (!RomData.SequenceList.Any(u => u.Name == i))
                    {
                        throw new Exception("Music Plando Error: " +
                            "Song does not exist in sequence pool, did you misspell the song Name? \n" + i);
                    }
                }

                var previouslyUsedSlots = returnSongTupleList.Select(u => u.Item2.PlandoName).ToList();
                foreach (string i in musicCombo.SlotsList.ToList())
                {
                    if (previouslyUsedSlots.Contains(i))
                    {
                        DebugOut("Slot already used, removed from combo: " + i);
                        musicCombo.SlotsList.Remove(i);
                    }
                    else if (!RomData.TargetSequences.Any(u => u.PlandoName == i))
                    {
                        throw new Exception("Music Plando Error: " +
                            "Slot does not exist in slot pool, did you misspell the slot Name or forget to add it to seqs.txt? \n" + i);
                    }
                }

                if (musicCombo.SongsList.Count == 0)
                {
                    throw new Exception("Music Plando Error: " +
                        "Plando Music Combo is starved, all songs have already been placed: \n" + musicCombo.Name);
                }
                if (musicCombo.SlotsList.Count == 0)
                {
                    throw new Exception("Music Plando Error: " +
                        "Plando Music Combo is starved, all slots are already filled: \n" + musicCombo.Name);
                }

                if (musicCombo.ItemDrawCount <= -1 || musicCombo.ItemDrawCount > musicCombo.SongsList.Count)
                    musicCombo.ItemDrawCount = musicCombo.SongsList.Count;

                for (int i = 0; i < musicCombo.ItemDrawCount && i < musicCombo.SlotsList.Count; i++)
                {
                    SequenceInfo song = RomData.SequenceList.Find(u => u.Name == musicCombo.SongsList[i]);
                    SequenceInfo slot = RomData.TargetSequences.Find(u => u.PlandoName == musicCombo.SlotsList[i]);

                    returnSongTupleList.Add((song, slot));
                    DebugOut("* Song placed: " + song.Name + " placed in slot " + slot.Name);
                }
            }

            return returnSongTupleList;
        }

        // remove items and checks already taken
        public static PlandoItemCombo CleanItemCombo(PlandoItemCombo itemCombo, Random random, List<Item> randomizerItemPool, ItemList randomizerItemList)
        {
            PlandoItemCombo returnCombo = new()
            {
                ItemList = [.. itemCombo.ItemList.OrderBy(x => random.Next())],
                CheckList = [.. itemCombo.CheckList.OrderBy(x => random.Next())],
                SkipLogic = itemCombo.SkipLogic,
                ItemDrawCount = itemCombo.ItemDrawCount,
                Name = itemCombo.Name,
                Notes = itemCombo.Notes
            };

            // clean combo of already placed items and checks
            foreach (Item item in returnCombo.ItemList.ToList())
            {
                var itemVar = randomizerItemList[item];
                if (itemVar.NewLocation.HasValue)
                {
                    Debug.WriteLine("Item has already been placed. " + item);
                    returnCombo.ItemList.Remove(item);
                }
            }

            foreach (Item check in returnCombo.CheckList.ToList())
            {
                if (!randomizerItemPool.Contains(check))
                {
                    Debug.WriteLine("Check does not exist in randomized item pool, either already taken or not randomized: " + check);
                    returnCombo.CheckList.Remove(check);
                }
            }

            if (returnCombo.ItemList.Count == 0)
            {
                Debug.WriteLine("Plando Item Combo is starved, all items have already been placed: " + returnCombo.Name);
                return null;
            }
            if (returnCombo.CheckList.Count == 0)
            {
                Debug.WriteLine("Plando Item Combo is starved, all checks are already filled: " + returnCombo.Name);
                return null;
            }

            if (returnCombo.ItemDrawCount <= -1)
                returnCombo.ItemDrawCount = returnCombo.ItemList.Count;

            return returnCombo;
        }

    }
}
