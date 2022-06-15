using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MMR.Common.Utils;

namespace MMR.DiscordBot.Services
{
    public class MMRService
    {
        private const string MMR_CLI = "MMR_CLI";
        private readonly string _cliPath;
        private readonly HttpClient _httpClient;
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly Random _random = new Random();

        public MMRService()
        {
            _cliPath = Environment.GetEnvironmentVariable(MMR_CLI);
            if (string.IsNullOrWhiteSpace(_cliPath))
            {
                throw new Exception($"Environment Variable '{MMR_CLI}' is missing.");
            }
            if (!Directory.Exists(_cliPath))
            {
                throw new Exception($"'{_cliPath}' is not a valid MMR.CLI path.");
            }
            Console.WriteLine($"{nameof(MMR_CLI)}: {_cliPath}");

            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "zoey.zolotova at gmail.com");
        }

        public string GetSpoilerLogPath(string filenameWithoutExtension)
        {
            return Path.Combine(_cliPath, "output", $"{filenameWithoutExtension}_SpoilerLog.txt");
        }

        public string GetSettingsPath(ulong guildId, string settingName)
        {
            var settingsRoot = Path.Combine(_cliPath, "settings");
            if (!Directory.Exists(settingsRoot))
            {
                Directory.CreateDirectory(settingsRoot);
            }
            var guildRoot = Path.Combine(settingsRoot, $"{guildId}");
            if (!Directory.Exists(guildRoot))
            {
                Directory.CreateDirectory(guildRoot);
            }
            return Path.Combine(guildRoot, $"{FileUtils.MakeFilenameValid(settingName)}.json");
        }

        public IEnumerable<string> GetSettingsPaths(ulong guildId)
        {
            var settingsRoot = Path.Combine(_cliPath, "settings");
            if (!Directory.Exists(settingsRoot))
            {
                Directory.CreateDirectory(settingsRoot);
            }
            var guildRoot = Path.Combine(settingsRoot, $"{guildId}");
            if (!Directory.Exists(guildRoot))
            {
                Directory.CreateDirectory(guildRoot);
            }

            return Directory.EnumerateFiles(guildRoot);
        }

        public async Task<(string patchPath, string hashIconPath, string spoilerLogPath)> GenerateSeed(string filenameWithoutExtension, string settingsPath, int seed)
        {
            var success = false;
            while (!success)
            {
                try
                {
                    success = await RunMMRCLI(filenameWithoutExtension, settingsPath, seed);
                    if (success)
                    {
                        var patchPath = Path.Combine(_cliPath, "output", $"{filenameWithoutExtension}.mmr");
                        var hashIconPath = Path.ChangeExtension(patchPath, "png");
                        var spoilerLogPath = GetSpoilerLogPath(filenameWithoutExtension);
                        if (File.Exists(patchPath) && File.Exists(hashIconPath) && File.Exists(spoilerLogPath))
                        {
                            return (patchPath, hashIconPath, spoilerLogPath);
                        }
                        else
                        {
                            success = false;
                        }
                    }
                }
                catch
                {
                    // TODO log error
                    success = false;
                }
                if (!success)
                {
                    seed += _random.Next(int.MinValue, int.MaxValue);
                }
            }
            throw new Exception("Failed to generate seed.");
        }

        private async Task<bool> RunMMRCLI(string filenameWithoutExtension, string settingsPath, int seed)
        {
            var output = Path.Combine("output", filenameWithoutExtension);
            var processInfo = new ProcessStartInfo("dotnet");
            processInfo.WorkingDirectory = _cliPath;
            processInfo.Arguments = $"{Path.Combine(_cliPath, @"MMR.CLI.dll")} -output \"{output}.z64\" -seed {seed} -spoiler -patch";
            if (!string.IsNullOrWhiteSpace(settingsPath))
            {
                processInfo.Arguments += $" -settings \"{settingsPath}\"";
            }
            processInfo.ErrorDialog = false;
            processInfo.UseShellExecute = false;
            processInfo.RedirectStandardOutput = true;
            processInfo.RedirectStandardError = true;

            var proc = Process.Start(processInfo);
            proc.ErrorDataReceived += (sender, errorLine) => { if (errorLine.Data != null) Trace.WriteLine(errorLine.Data); };
            proc.OutputDataReceived += (sender, outputLine) => { if (outputLine.Data != null) Trace.WriteLine(outputLine.Data); };
            proc.BeginErrorReadLine();
            proc.BeginOutputReadLine();

            proc.WaitForExit();
            return proc.ExitCode == 0;
        }

        public async Task<IEnumerable<int>> GetSeed(int num = 1)
        {
            await _semaphore.WaitAsync();
            IEnumerable<int> seeds;
            try
            {
                var response = await _httpClient.GetStringAsync($"https://www.random.org/integers/?num={num * 31}&min=0&max=1&col=31&base=2&format=plain&rnd=new");
                seeds = response.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(s => Convert.ToInt32(s.Replace("\t", "").Trim(), 2));
            }
            catch (HttpRequestException e)
            {
                seeds = Enumerable.Range(0, num).Select(_ => _random.Next());
            }
            finally
            {
                _semaphore.Release();
            }
            return seeds;
        }
    }
}
