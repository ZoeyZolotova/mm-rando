using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using MMR.Randomizer.Constants;
using MMR.Randomizer.Models.Rom;
using MMR.Common.Utils;

namespace MMR.Randomizer.Utils.Music
{
    /// <summary>
    /// Handles caching of the files in the music folder and the sequence list.
    /// </summary>
    public static class MusicCacheUtils
    {
        /// <summary>
        /// The path to the 'music.cache' JSON file.
        /// </summary>
        public static readonly string CachePath = Path.Combine(Values.MusicDirectory, "music.cache");

        /// <summary>
        /// Loads the 'music.cache' file into a dictionary.
        /// </summary>
        /// <returns>Empty music cache if there's no cache.</returns>
        public static MusicCache Load()
        {
            if (!File.Exists(CachePath))
                return new MusicCache
                {
                    FileHashes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                    SequenceList = []
                };

            var json = File.ReadAllText(CachePath);
            return JsonSerializer.Deserialize<MusicCache>(json) ?? new MusicCache
            {
                FileHashes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
                SequenceList = []
            };
        }

        /// <summary>
        /// Saves a MusicCache object to a 'music.cache' JSON file.
        /// </summary>
        public static void Save(MusicCache cache)
        {
            var content = JsonSerializer.Serialize(cache);
            File.WriteAllText(CachePath, content);
        }

        /// <summary>
        /// Calculates the SHA256 for a given file.
        /// </summary>
        /// <returns>SHA256 hash string.</returns>
        public static string GetFileHash(string filePath)
        {
            using var hash = SHA256.Create();
            using var stream = File.OpenRead(filePath);
            return Convert.ToHexStringLower(hash.ComputeHash(stream));
        }

        /// <summary>
        /// Represents a 'music.cache' JSON file storing a dictionary of filepaths and SHA256 hash strings, and the sequence list.
        /// </summary>
        public class MusicCache
        {
            public Dictionary<string, string> FileHashes { get; set; } = new(StringComparer.OrdinalIgnoreCase);
            public List<SequenceInfo> SequenceList { get; set; } = [];
        }
    }
}
