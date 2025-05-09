using MMR.Randomizer.Models.Rom;

namespace MMR.Randomizer.Constants
{
    /// <summary>
    /// Stores the ROM addresses of audio-related MMFiles.
    /// </summary>
    public static class Addresses
    {
        // these are addresses for tables that point to the specific objects for audio


        // not sure where DB got the instrument set map pointer, its not in seq64
        //  looks like its part of the Sequence Banks Map file, as that starts C77960 and has len 210
        public const int InstSetMap         = 0xC77A60; // pointer table: sequence -> instrumentset
        public const int AudioSequence      = 0x046AF0; // audioseq
        public const int SeqTable           = 0xC77B80; // audioseq table (70 + 0x10)
        public const int AudiobankTable     = 0xC776D0; // audiobank index (c0 + 0x10)
        public const int Audiobank          = 0x020700;
        // TODO add audiobank and soundbank pointers

        // Majora's Mask Audiobin
        /// <summary>
        /// The address of the 'audiobank index' on the ROM.
        /// </summary>
        public const int AUDIOBANK_INDEX_ADDR    = 0x00C776C0;
        /// <summary>
        /// The size of the 'audiobank index' in the 'code' MMFile.
        /// </summary>
        public const int AUDIOBANK_INDEX_SIZE    = 0x000002A0;

        /// <summary>
        /// The address of the 'audiobank' MMFile.
        /// </summary>
        public const int AUDIOBANK_ADDR          = 0x00020700;
        /// <summary>
        /// The size of the 'audiobank' MMFile.
        /// </summary>
        public const int AUDIOBANK_SIZE          = 0x000263F0;

        /// <summary>
        /// The address of the 'audiotable index' on the ROM.
        /// </summary>
        public const int AUDIOTABLE_INDEX_ADDR   = 0x00C78380;
        /// <summary>
        /// The size of the 'audiotable index' in the 'code' MMFile.
        /// </summary>
        public const int AUDIOTABLE_INDEX_SIZE   = 0x000002A0;

        /// <summary>
        /// The address of the 'audiotable' MMFile.
        /// </summary>
        public const int AUDIOTABLE_ADDR         = 0x00097F70;
        /// <summary>
        /// The size of the 'audiotable' MMFile.
        /// </summary>
        public const int AUDIOTABLE_SIZE         = 0x00548770;

        /// <summary>
        /// The address of the 'code' MMFile.
        /// </summary>
        public const int CODE_ADDR               = 0x00B3C000;
    }
}
