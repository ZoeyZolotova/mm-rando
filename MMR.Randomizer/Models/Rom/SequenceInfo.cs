using System.Collections.Generic;
using System.IO;
using MMR.Randomizer.Constants;

namespace MMR.Randomizer.Models.Rom
{
    //[System.Diagnostics.DebuggerDisplay("[{Name}][{Replaces.ToString(\"X2\")}]")] // slower but more debug
    [System.Diagnostics.DebuggerDisplay("{Name}")]
    public class SequenceInfo
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Directory { get; set; } = Values.MusicDirectory;
        public string Filename => Path.Combine(Directory, Name);
        public int Replaces { get; set; } = -1;
        public int SeqId { get; set; } = -1; // MM_seq -> SeqId
        public List<int> Categories { get; set; } = new List<int>();
        public int Instrument { get; set; }
        public List<SequenceBinaryData> SequenceBinaryList { get; set; }
        public int PreviousSlot { get; set; } = -1;
        public List<SequenceSoundSampleBinaryData> InstrumentSamples { get; set; } = null;

        public void ClearUnavailableBanks()
        {
            /// custom instrument sets relies on overwriting a vanilla bank with a new bank that has the old instruments, with additions
            /// this feature does NOT support two banks stuffing into one slot, first bank to take a slot claims it and other songs cannot use for banks
            /// this function should clear away banks that are unavailable because they are already taken

            // get list of banks that: their slot has not been modified, or their bank is already used by another song and can be reused
            this.SequenceBinaryList = this.SequenceBinaryList.FindAll(u => u.InstrumentSet == null
                                                                  || (RomData.InstrumentSetList[u.InstrumentSet.BankSlot].Modified == 0
                                                                  || (u.InstrumentSet.Hash != 0
                                                                  && u.InstrumentSet.Hash == RomData.InstrumentSetList[u.InstrumentSet.BankSlot].Hash)));
        }

        public bool CheckAvailableBanks()
        {
            // get list of banks that: their slot has not been modified, or their bank is already used by another song and can be reused
            var banks = this.SequenceBinaryList.FindAll(u => u.InstrumentSet == null
                                                                  || (RomData.InstrumentSetList[u.InstrumentSet.BankSlot].Modified == 0
                                                                  || (u.InstrumentSet.Hash != 0
                                                                  && u.InstrumentSet.Hash == RomData.InstrumentSetList[u.InstrumentSet.BankSlot].Hash)));
            if (banks.Count == 0)
            {
                return false;
            }
            return true;
        }

    }
}
