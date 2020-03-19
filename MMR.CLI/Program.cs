using MMR.Randomizer.Models.Settings;
using MMR.Randomizer.Models;
using MMR.Randomizer;
using MMR.Randomizer.GameObjects;
using System.Collections.Generic;
using System;
using MMR.Randomizer.Utils;
using System.Linq;
using MMR.Randomizer.Extensions;
using MMR.Common.Utils;
using System.IO;
using System.Linq.Expressions;
using MMR.Randomizer.Models.Colors;

namespace MMR.CLI
{
    partial class Program
    {
        static int Main(string[] args)
        {
            var argsDictionary = DictionaryHelper.FromProgramArguments(args);
            if (argsDictionary.ContainsKey("-help"))
            {
                Console.WriteLine("All arguments are optional.");
                var helpTexts = new Dictionary<string, string>
                {
                    { "-help", "See this help text." },
                    { "-settings <path>", "Path to a settings JSON file. Only the GameplaySettings will be loaded. Other settings will be loaded from your default settings.json file." },
                    { "-patch", "Output a .mmr patch file." },
                    { "-spoiler", "Output a .txt spoiler log." },
                    { "-html", "Output a .html item tracker." },
                    { "-rom", "Output a .z64 ROM file." },
                    { "-seed", "Set the seed for the randomizer." },
                    { "-output <path>", "Path to output the output ROM. Other outputs will be based on the filename in this path. If omitted, will output to \"output/{timestamp}.z64\"" },
                    { "-input <path>", "Path to the input Majora's Mask (U) ROM. If omitted, will try to use \"input.z64\"." },
                    { "-save", "Save the settings to the default settings.json file." },
                };
                foreach (var kvp in helpTexts)
                {
                    Console.WriteLine("{0, -17} {1}", kvp.Key, kvp.Value);
                }
                Console.WriteLine("settings.json details:");
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.GameplaySettings.LogicMode));
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.GameplaySettings.DamageMode));
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.GameplaySettings.DamageEffect));
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.GameplaySettings.MovementMode));
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.GameplaySettings.FloorType));
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.GameplaySettings.ClockSpeed));
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.GameplaySettings.BlastMaskCooldown));
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.GameplaySettings.GossipHintStyle));
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.CosmeticSettings.TatlColorSchema));
                Console.WriteLine(GetEnumSettingDescription(cfg => cfg.CosmeticSettings.Music));
                Console.WriteLine(GetEnumArraySettingDescription(cfg => cfg.CosmeticSettings.DPad.Pad.Values) + " Array length of 4.");
                Console.WriteLine(GetArrayValueDescription(nameof(CosmeticSettings.HeartsSelection), ColorSelectionManager.Hearts.GetItems().Select(csi => csi.Name)));
                Console.WriteLine(GetArrayValueDescription(nameof(CosmeticSettings.MagicSelection), ColorSelectionManager.MagicMeter.GetItems().Select(csi => csi.Name)));
                return 0;
            }
            var configuration = LoadSettings();
            if (configuration == null)
            {
                Console.WriteLine("Default settings file not found. Generating...");
                configuration = new Configuration
                {
                    CosmeticSettings = new CosmeticSettings(),
                    GameplaySettings = new GameplaySettings(),
                    OutputSettings = new OutputSettings()
                    {
                        InputROMFilename = "input.z64",
                    },
                };
                SaveSettings(configuration);
                Console.WriteLine($"Generated {Path.ChangeExtension(DEFAULT_SETTINGS_FILENAME, SETTINGS_EXTENSION)}. Edit it to set your settings.");
            }
            var settingsPath = argsDictionary.GetValueOrDefault("-settings")?.FirstOrDefault();
            if (settingsPath != null)
            {
                var loadedConfiguration = LoadSettings(settingsPath);
                if (loadedConfiguration == null)
                {
                    Console.WriteLine($"File not found \"{settingsPath}\".");
                    return -1;
                }
                if (loadedConfiguration.GameplaySettings == null)
                {
                    Console.WriteLine($"Error loading GameplaySettings from \"{settingsPath}\".");
                    return -1;
                }
                configuration.GameplaySettings = loadedConfiguration.GameplaySettings;
                Console.WriteLine($"Loaded GameplaySettings from \"{settingsPath}\".");
            }

            configuration.GameplaySettings.CustomItemList = ConvertIntString(configuration.GameplaySettings.CustomItemListString ?? string.Empty);
            configuration.GameplaySettings.CustomStartingItemList = ConvertItemString(ItemUtils.StartingItems().Where(item => !item.Name().Contains("Heart")).ToList(), configuration.GameplaySettings.CustomStartingItemListString ?? string.Empty);
            configuration.GameplaySettings.CustomJunkLocations = ConvertItemString(ItemUtils.AllLocations().ToList(), configuration.GameplaySettings.CustomJunkLocationsString ?? string.Empty);

            configuration.OutputSettings.GeneratePatch |= argsDictionary.ContainsKey("-patch");
            configuration.OutputSettings.GenerateSpoilerLog |= argsDictionary.ContainsKey("-spoiler");
            configuration.OutputSettings.GenerateHTMLLog |= argsDictionary.ContainsKey("-html");
            configuration.OutputSettings.GenerateROM |= argsDictionary.ContainsKey("-rom");

            int seed;
            if (argsDictionary.ContainsKey("-seed"))
            {
                seed = int.Parse(argsDictionary["-seed"][0]);
            }
            else
            {
                seed = new Random().Next();
            }

            var outputArg = argsDictionary.GetValueOrDefault("-output");
            if (outputArg != null)
            {
                if (outputArg.Count > 1)
                {
                    throw new ArgumentException("Invalid argument.", "-output");
                }
                configuration.OutputSettings.OutputROMFilename = outputArg.SingleOrDefault();
            }
            configuration.OutputSettings.OutputROMFilename ??= Path.Combine("output", FileUtils.MakeFilenameValid(DateTime.UtcNow.ToString("o")));
            if (Path.GetExtension(configuration.OutputSettings.OutputROMFilename) != ".z64")
            {
                configuration.OutputSettings.OutputROMFilename += ".z64";
            }

            var inputArg = argsDictionary.GetValueOrDefault("-input");
            if (inputArg != null)
            {
                if (inputArg.Count > 1)
                {
                    throw new ArgumentException("Invalid argument.", "-input");
                }
                configuration.OutputSettings.InputROMFilename = inputArg.SingleOrDefault();
            }
            configuration.OutputSettings.InputROMFilename ??= "input.z64";

            if (argsDictionary.ContainsKey("-save"))
            {
                SaveSettings(configuration);
            }

            var validationResult = configuration.GameplaySettings.Validate() ?? configuration.OutputSettings.Validate();
            if (validationResult != null)
            {
                Console.WriteLine(validationResult);
                return -1;
            }

            try
            {
                string result;
                using (var progressBar = new ProgressBar())
                {
                    //var progressReporter = new TextWriterProgressReporter(Console.Out);
                    var progressReporter = new ProgressBarProgressReporter(progressBar);
                    result = ConfigurationProcessor.Process(configuration, seed, progressReporter);
                }
                if (result != null)
                {
                    Console.Error.WriteLine(result);
                }
                else
                {
                    Console.WriteLine("Generation complete!");
                }
                return result == null ? 0 : -1;
            }
            catch (Exception e)
            {
                Console.Error.Write(e.Message);
                Console.Error.Write(e.StackTrace);
                return -1;
            }
        }

        private static List<int> ConvertIntString(string c)
        {
            var result = new List<int>();
            try
            {
                result.Clear();
                string[] v = c.Split('-');
                int[] vi = new int[13];
                if (v.Length != vi.Length)
                {
                    result.Add(-1);
                    return null;
                }
                for (int i = 0; i < 13; i++)
                {
                    if (v[12 - i] != "")
                    {
                        vi[i] = Convert.ToInt32(v[12 - i], 16);
                    }
                }
                for (int i = 0; i < 32 * 13; i++)
                {
                    int j = i / 32;
                    int k = i % 32;
                    if (((vi[j] >> k) & 1) > 0)
                    {
                        if (i >= ItemUtils.AllLocations().Count())
                        {
                            throw new IndexOutOfRangeException();
                        }
                        result.Add(i);
                    }
                }
            }
            catch
            {
                result.Clear();
                result.Add(-1);
            }
            return result;
        }

        private static List<Item> ConvertItemString(List<Item> items, string c)
        {
            var sectionCount = (int)Math.Ceiling(items.Count / 32.0);
            var result = new List<Item>();
            try
            {
                result.Clear();
                string[] v = c.Split('-');
                int[] vi = new int[sectionCount];
                if (v.Length != vi.Length)
                {
                    //result.Add(-1);
                    return null;
                }
                for (int i = 0; i < sectionCount; i++)
                {
                    if (v[sectionCount - 1 - i] != "")
                    {
                        vi[i] = Convert.ToInt32(v[sectionCount - 1 - i], 16);
                    }
                }
                for (int i = 0; i < 32 * sectionCount; i++)
                {
                    int j = i / 32;
                    int k = i % 32;
                    if (((vi[j] >> k) & 1) > 0)
                    {
                        if (i >= items.Count)
                        {
                            throw new IndexOutOfRangeException();
                        }
                        result.Add(items[i]);
                    }
                }
            }
            catch
            {
                result.Clear();
                //result.Add(-1);
            }
            return result;
        }

        private const string DEFAULT_SETTINGS_FILENAME = "settings";
        private const string SETTINGS_EXTENSION = "json";
        private static void SaveSettings(Configuration configuration, string filename = null)
        {
            var path = Path.ChangeExtension(filename ?? DEFAULT_SETTINGS_FILENAME, SETTINGS_EXTENSION);
            string logicFilePath = null;
            //if (filename != null)
            //{
            //    logicFilePath = configuration.GameplaySettings.UserLogicFileName;
            //    configuration.GameplaySettings.UserLogicFileName = null;
            //    if (configuration.GameplaySettings.LogicMode == LogicMode.UserLogic && logicFilePath != null && File.Exists(logicFilePath))
            //    {
            //        using (StreamReader Req = new StreamReader(File.Open(logicFilePath, FileMode.Open)))
            //        {
            //            configuration.GameplaySettings.Logic = Req.ReadToEnd();
            //            if (configuration.GameplaySettings.Logic.StartsWith("{"))
            //            {
            //                var logicConfiguration = Configuration.FromJson(configuration.GameplaySettings.Logic);
            //                configuration.GameplaySettings.Logic = logicConfiguration.GameplaySettings.Logic;
            //            }
            //        }
            //    }
            //    configurationToSave = new Configuration
            //    {
            //        GameplaySettings = configuration.GameplaySettings,
            //    };
            //}
            using (var settingsFile = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                settingsFile.Write(configuration.ToString());
            }
            //if (logicFilePath != null)
            //{
            //    configuration.GameplaySettings.UserLogicFileName = logicFilePath;
            //    configuration.GameplaySettings.Logic = null;
            //}
        }

        private static Configuration LoadSettings(string filename = null)
        {
            var path = Path.ChangeExtension(filename ?? DEFAULT_SETTINGS_FILENAME, SETTINGS_EXTENSION);
            if (File.Exists(path))
            {
                Configuration configuration;
                using (StreamReader Req = new StreamReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    configuration = Configuration.FromJson(Req.ReadToEnd());
                }

                if (configuration.GameplaySettings.Logic != null)
                {
                    configuration.GameplaySettings.UserLogicFileName = path;
                    configuration.GameplaySettings.Logic = null;
                }
                if (!File.Exists(configuration.GameplaySettings.UserLogicFileName))
                {
                    configuration.GameplaySettings.UserLogicFileName = string.Empty;
                }
                return configuration;
            }
            return null;
        }

        private static string GetEnumSettingDescription<T>(Expression<Func<Configuration, T>> propertySelector) where T : struct
        {
            return $"{((MemberExpression)propertySelector.Body).Member.Name, -17} {string.Join('|', Enum.GetNames(typeof(T)))}";
        }

        private static string GetEnumArraySettingDescription<T>(Expression<Func<Configuration, T[]>> propertySelector) where T : struct
        {
            return $"{((MemberExpression)propertySelector.Body).Member.Name, -17} [{string.Join('|', Enum.GetNames(typeof(T)))}]";
        }

        private static string GetArrayValueDescription(string name, IEnumerable<string> values)
        {
            return $"{name, -17} {string.Join('|', values)}";
        }
    }
}
