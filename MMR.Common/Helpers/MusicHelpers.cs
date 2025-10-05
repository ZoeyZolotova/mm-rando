using System.IO;

namespace MMR.Common.Helpers
{
    public static class MusicHelpers
    {
        /// <summary>
        /// Determines if the input file is a Majora's Mask Randomizer music file.
        /// </summary>
        public static bool IsMMRSFile(string filepath)
        {
            return Path.GetExtension(filepath) == ".mmrs";
        }

        /// <summary>
        /// Determines if the input file is an Ocarina of Time Randomizer music file.
        /// </summary>
        public static bool IsOOTRSFile(string filepath)
        {
            return Path.GetExtension(filepath) == ".ootrs";
        }
    }
}
