using MMR.Randomizer.Models;
using MMR.Randomizer.Models.Settings;
using MMR.Randomizer.Utils;
using System;

namespace MMR.Randomizer
{
    public static class ConfigurationProcessor
    {
        public static string Process(Configuration configuration, int seed, IProgressReporter progressReporter)
        {
            if (configuration.OutputSettings.InputROMFilename == "")
            {
                return $"Please choose a valid input Majora's Mask (U) .z64 rom";
            }

            var randomizer = new Randomizer(configuration.GameplaySettings, seed);
            RandomizedResult randomized = null;
            if (string.IsNullOrWhiteSpace(configuration.OutputSettings.InputPatchFilename))
            {
                try
                {
                    randomized = randomizer.Randomize(progressReporter);
                }
                catch (RandomizationException ex)
                {
                    string nl = Environment.NewLine;
                    return $"Error randomizing logic: {ex.Message}{nl}{nl}Please try a different seed";
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }

                if (configuration.OutputSettings.GenerateSpoilerLog
                    && configuration.GameplaySettings.LogicMode != LogicMode.Vanilla)
                {
                    SpoilerUtils.CreateSpoilerLog(randomized, configuration.GameplaySettings, configuration.OutputSettings);
                }
            }

            if (configuration.OutputSettings.GenerateROM || configuration.OutputSettings.OutputVC || configuration.OutputSettings.GeneratePatch)
            {
                if (!RomUtils.ValidateROM(configuration.OutputSettings.InputROMFilename))
                {
                    return "Cannot verify input ROM is Majora's Mask (U).";
                }

                var builder = new Builder(randomized, configuration.CosmeticSettings);

                try
                {
                    builder.MakeROM(configuration.OutputSettings, progressReporter);
                }
                catch (PatchMagicException)
                {
                    return $"Error applying patch: Not a valid patch file";
                }
                catch (PatchVersionException ex)
                {
                    return $"Error applying patch: {ex.Message}";
                }
                catch (Exception ex)
                {
                    string nl = Environment.NewLine;
                    return $"Error building ROM: {ex.Message}{nl}{nl}Please contact the development team and provide them more information";
                }
            }

            //settings.InputPatchFilename = null;

            return null;
            //return "Generation complete!";
        }
    }

    public interface IProgressReporter
    {
        void ReportProgress(int percentProgress, string message);
    }
}
