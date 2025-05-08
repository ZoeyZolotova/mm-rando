using MMR.Randomizer.Constants;
using MMR.Randomizer.Models.Rom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using MMR.Common.Utils;

namespace MMR.Randomizer.Utils
{
    public static class MusicCacheUtils
    {
        // Caches the user's music folder, so only files not currently cached need to be processed and added to the sequence list

        private static readonly string CachePath = Path.Combine(Values.MusicDirectory, "music.cache");

        public static MusicCache Load()
        {
            if (!File.Exists(CachePath))
                return new MusicCache
                {
                    FileHashes = new Dictionary<string, string>(),
                    SequenceList = new List<SequenceInfo>()
                };

            var json = File.ReadAllText(CachePath);
            return JsonSerializer.Deserialize<MusicCache>(json) ?? new MusicCache
            {
                FileHashes = new Dictionary<string, string>(),
                SequenceList = new List<SequenceInfo>()
            };
        }

        public static void Save(MusicCache cache)
        {
            var content = JsonSerializer.Serialize(cache);
            File.WriteAllText(CachePath, content);
        }

        public static string GetFileHash(string filePath)
        {
            using var sha256 = SHA256.Create();
            using var stream = File.OpenRead(filePath);
            return BitConverter.ToString(sha256.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
        }

        public class MusicCache
        {
            // The music cache stores the filename and SHA256 of every custom music file
            // The sequence list containing all the sequence info added is also stored, that way if there are no
            // changes to the music folder, the randomizer can just load the cached list and skip looking through
            // the music directory
            public Dictionary<string, string> FileHashes { get; set; } = new();
            public List<SequenceInfo> SequenceList { get; set; } = new();
        }
    }
}
