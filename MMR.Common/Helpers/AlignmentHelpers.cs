namespace MMR.Common.Helpers
{
    public static class AlignmentHelpers
    {
        /// <summary>
        /// Aligns an int to the nearest 8-byte boundary.
        /// </summary>
        public static int AlignTo8(int value) => (value + 0x7) & ~0x7;

        /// <summary>
        /// Aligns a long to the nearest 8-byte boundary.
        /// </summary>
        public static long AlignTo8(long value) => (value + 0x7) & ~0x7;

        /// <summary>
        /// Aligns an int to the nearest 16-byte boundary.
        /// </summary>
        public static int AlignTo16(int value) => (value + 0xF) & ~0xF;

        /// <summary>
        /// Aligns a long to the nearest 16-byte boundary.
        /// </summary>
        public static long AlignTo16(long value) => (value + 0xF) & ~0xF;
    }
}
