using MMR.Randomizer.Models.Rom;
using System.Collections.Generic;

namespace MMR.Randomizer
{
    /// <summary>
    /// Stores the SequenceList, InstrumentSetList, TargetSequences list, PointerizedSequences list, ListOfSamples, MMFileList, SceneList, GetItemList, and BottleList.
    /// </summary>
    public static class RomData
    {
        /// <summary>
        /// A list of sequences available for injection and randomization.
        /// </summary>
        public static List<SequenceInfo> SequenceList { get; set; }
        /// <summary>
        /// A list of instrument sets available for injection.
        /// </summary>
        public static List<InstrumentSetInfo> InstrumentSetList { get; set; }
        /// <summary>
        /// A list of sequences to be replaced.
        /// </summary>
        public static List<SequenceInfo> TargetSequences { get; set; }
        /// <summary>
        /// A list of sequences that have been pointerized.
        /// </summary>
        public static List<SequenceInfo> PointerizedSequences { get; set; }
        /// <summary>
        /// A list of binary Zelda64 ADPCM audio sample files for injection.
        /// </summary>
        public static List<SequenceSoundSampleBinaryData> ListOfSamples { get; set; }
        public static int SamplesFileID { get; set; } = 0;
        public static List<MMFile> MMFileList { get; set; }
        public static List<Scene> SceneList { get; set; }
        public static Dictionary<int, GetItemEntry> GetItemList { get; set; }
        public static Dictionary<int, BottleCatchEntry> BottleList { get; set; }
    }
}
