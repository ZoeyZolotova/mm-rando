namespace MMR.Randomizer.Constants
{
    /// <summary>
    /// Stores the ROM addresses of audio-related MMFiles.
    /// </summary>
    public static class Addresses
    {
        // not sure where DB got the instrument set map pointer, its not in seq64
        //  looks like its part of the Sequence Banks Map file, as that starts C77960 and has len 210
        public const int INST_SET_MAP = 0x00C77A60; // pointer table: sequence -> instrument bank

        // Majora's Mask Audiobin
        /// <summary>
        /// The address of the 'audioseq index' on the ROM.
        /// </summary>
        public const int AUDIOSEQ_INDEX_ADDR   = 0x00C77B70;
        public const int AUDIOSEQ_TABLE        = 0x00C77B80; // 0x00C77B70 + 0x10 because the first 16 bytes is the number of sequences

        /// <summary>
        /// The address of the 'audioseq' MMFile.
        /// </summary>
        public const int AUDIOSEQ_ADDR         = 0x00046AF0;

        /// <summary>
        /// The address of the 'audiobank index' on the ROM.
        /// </summary>
        public const int AUDIOBANK_INDEX_ADDR  = 0x00C776C0;
        public const int AUDIOBANK_TABLE       = 0x00C776D0; // 0x00C776C0 + 0x10 because the first 16 bytes if the number of instrument banks
        /// <summary>
        /// The size of the 'audiobank index' in the 'code' MMFile.
        /// </summary>
        public const int AUDIOBANK_INDEX_SIZE  = 0x000002A0;

        /// <summary>
        /// The address of the 'audiobank' MMFile.
        /// </summary>
        public const int AUDIOBANK_ADDR        = 0x00020700;
        /// <summary>
        /// The size of the 'audiobank' MMFile.
        /// </summary>
        public const int AUDIOBANK_SIZE        = 0x000263F0;

        /// <summary>
        /// The address of the 'audiotable index' on the ROM.
        /// </summary>
        public const int AUDIOTABLE_INDEX_ADDR = 0x00C78380;
        /// <summary>
        /// The size of the 'audiotable index' in the 'code' MMFile.
        /// </summary>
        public const int AUDIOTABLE_INDEX_SIZE = 0x000002A0;

        /// <summary>
        /// The address of the 'audiotable' MMFile.
        /// </summary>
        public const int AUDIOTABLE_ADDR       = 0x00097F70;
        /// <summary>
        /// The size of the 'audiotable' MMFile.
        /// </summary>
        public const int AUDIOTABLE_SIZE       = 0x00548770;

        /// <summary>
        /// The address of the 'code' MMFile.
        /// </summary>
        public const int CODE_ADDR             = 0x00B3C000;
    }
}
