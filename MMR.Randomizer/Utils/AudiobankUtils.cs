using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Diagnostics;

namespace MMR.Randomizer.Utils
{
    public class AudiobankUtils
    {
        // Parses through a binary instrument bank (.zbank, decompressed) file and stores data

        public enum AudioSampleCodec : int
        {
            CODEC_ADPCM,
            CODEC_S8,
            CODEC_S16_INMEM,
            CODEC_SMALL_ADPCM,
            CODEC_REVERB,
            CODEC_S16
        }

        public enum AudioStorageMedium: int
        {
            MEDIUM_RAM,
            MEDIUM_UNK,
            MEDIUM_CART,
            MEDIUM_DISK_DRIVE
        }

        public interface ISample
        {
            uint? Address { get; set; }
            uint BankOffset { get; set; }
            byte[] Data { get; set; }
        }

        // Should only need the sample offsets, and int should be fine for bank addresses
        // because there shouldn't be a bank out there longer than 0x7FFFFFFF bytes

        public class Audiobin
        {
            public byte[] AudiobankTable { get; set; }
            public byte[] AudiobankIndex { get; set; }
            public byte[] Audiotable { get; set; }
            public byte[] AudiotableIndex { get; set; }
            public List<Audiobank> Audiobanks { get; set; } = new();

            public Audiobin(byte[] audiobankTable, byte[] audiobankIndex, byte[] audiotable, byte[] audiotableIndex)
            {
                AudiobankTable = audiobankTable;
                AudiobankIndex = audiobankIndex;
                Audiotable = audiotable;
                AudiotableIndex = audiotableIndex;

                var numBanks = BinaryPrimitives.ReadUInt16BigEndian(AudiobankIndex.AsSpan(0, 2));
                for (int i = 0; i < numBanks; i++)
                {
                    int index = 0x10 + (0x10 * i);
                    byte[] currentEntry = new byte[0x10];
                    Array.Copy(AudiobankIndex, index, currentEntry, 0, 0x10);
                    var audiobank = new Audiobank(currentEntry, AudiobankTable, Audiotable, AudiotableIndex);
                    Audiobanks.Add(audiobank);
                }
            }

            public ISample FindSampleInBanks(byte[] sampleData)
            {
                foreach (var bank in Audiobanks)
                {
                    foreach (var instrument in bank.Instruments)
                    {
                        if (instrument?.LowSample?.Data == sampleData)
                            return instrument.LowSample;

                        if (instrument?.PrimSample?.Data == sampleData)
                            return instrument.PrimSample;

                        if (instrument?.HighSample?.Data == sampleData)
                            return instrument.HighSample;
                    }

                    foreach (var drum in bank.Drums)
                    {
                        if (drum?.Sample?.Data == sampleData)
                            return drum.Sample;
                    }

                    foreach (var effect in bank.Effects)
                    {
                        if (effect?.Sample?.Data == sampleData)
                            return effect.Sample;
                    }
                }

                return null;
            }
        }

        public class Audiobank {

            public uint BankOffset { get; set; }
            public uint BankLength { get; set; }
            public int SampleMedium { get; set; }
            public int SequencePlayer { get; set; }
            public int AudiotableId { get; set; }
            public int BankId { get; set; }
            public int NumInsts { get; set; }
            public int NumDrums { get; set; }
            public int NumEffects { get; set; }
            public byte[] BankData { get; set; }
            public byte[] Bankmeta { get; set; }

            public List<Instrument> Instruments = new();
            public List<Drum> Drums = new();
            public List<Effect> Effects = new();

            public Audiobank(byte[] tableEntry, byte[] audiobankFile, byte[] audiotableFile, byte[] audiotableIndex)
            {
                byte[] bankmetaData = new byte[8];
                switch (tableEntry.Length)
                {
                    case 0x08: // 8 Bytes (.bankmeta): [Sample Medium, Sequence Player, Audiotable, ID, Num Inst, Num Drum, Num Effect MSB, Num Effect LSB]
                        BankOffset = 0;
                        BankLength = 0;
                        SampleMedium = tableEntry[0];
                        SequencePlayer = tableEntry[1];
                        AudiotableId = tableEntry[2];
                        BankId = tableEntry[3];
                        NumInsts = tableEntry[4];
                        NumDrums = tableEntry[5];
                        NumEffects = BinaryPrimitives.ReadUInt16BigEndian(tableEntry.AsSpan(6, 2));

                        bankmetaData = tableEntry;
                        break;

                    case 0x10: // 16 Bytes: [Address, Length, Sample Medium, Sequence Player, Audiotable, ID, Num Inst, Num Drum, Num Effect MSB, Num Effect LSB]
                        BankOffset = BinaryPrimitives.ReadUInt32BigEndian(tableEntry.AsSpan(0, 4));
                        BankLength = BinaryPrimitives.ReadUInt32BigEndian(tableEntry.AsSpan(4, 4));
                        SampleMedium = tableEntry[8];
                        SequencePlayer = tableEntry[9];
                        AudiotableId = tableEntry[10];
                        BankId = tableEntry[11];
                        NumInsts = tableEntry[12];
                        NumDrums = tableEntry[13];
                        NumEffects = BinaryPrimitives.ReadUInt16BigEndian(tableEntry.AsSpan(14, 2));

                        Array.Copy(tableEntry, 8, bankmetaData, 0, 8);
                        break;

                    default: // When reading .bankmeta there's already a check for 8 bytes, but never hurts to be extra safe
                        throw new Exception($"Audiobank Instnatiation Error: Invalid length for bankmeta binary - expected '8' or '16' bytes, but got '{tableEntry.Length}' bytes instead");
                }

                // If the bankmeta is just the 8 bytes, the audiobankFile should be the zbank file
                // Because of this, BankLength is 0 so use the length of the zbank getting passed in
                if (BankLength == 0)
                {
                    byte[] bankData = new byte[audiobankFile.Length];
                    Array.Copy(audiobankFile, BankOffset, bankData, 0, audiobankFile.Length);
                    BankData = bankData;
                }
                else
                {
                    byte[] bankData = new byte[BankLength];
                    Array.Copy(audiobankFile, BankOffset, bankData, 0, BankLength);
                    BankData = bankData;
                }
                
                Bankmeta = bankmetaData;

                // Find all the drums and instantiate them
                uint drumListAddr = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan(0, 4));
                for (int i = 0; i < NumDrums; i++)
                {
                    uint offset = drumListAddr + (uint)(4 * i);
                    offset = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan((int)offset, 4));
                    Drum drum = offset != 0 ? new Drum(i, BankData, audiotableFile, audiotableIndex, (int)offset, AudiotableId) : null; // Maybe just leave 0...
                    Drums.Add(drum);
                }

                // Find all the effects and instantiate them
                uint effectListAddr = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan(4, 4));
                for (int i = 0; i < NumEffects; i++)
                {
                    uint offset = effectListAddr + (uint)(8 * i);
                    Effect effect = offset != 0 ? new Effect(i, BankData, audiotableFile, audiotableIndex, (int)offset, AudiotableId) : null; // Maybe just leave 0...
                    Effects.Add(effect);
                }

                // Find all the instruments and instantiante them
                for (int i = 0; i < NumInsts; i++)
                {
                    uint offset = 0x08 + (uint)(4 * i);
                    offset = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan((int)offset, 4));
                    Instrument instrument = offset != 0 ? new Instrument(i, BankData, audiotableFile, audiotableIndex, (int)offset, AudiotableId) : null;
                    Instruments.Add(instrument);
                }
            }

            public List<ISample> GetBankSamples()
            {
                // This runs through all the instruments, drums, and effects and adds any
                // sample structs it finds to a list so the addresses can be searched

                List<ISample> allSamples = new();

                foreach (var instrument in Instruments)
                {
                    if (instrument != null)
                    {
                        if (instrument.LowSample != null)
                            allSamples.Add(instrument.LowSample);

                        if (instrument.PrimSample != null)
                            allSamples.Add(instrument.PrimSample);

                        if (instrument.HighSample != null)
                            allSamples.Add(instrument.HighSample);
                    }
                }

                foreach (var drum in Drums)
                {
                    if (drum != null && drum.Sample != null)
                        allSamples.Add(drum.Sample);
                }

                foreach (var sfx in Effects)
                {
                    if (sfx != null && sfx.Sample != null)
                        allSamples.Add(sfx.Sample);
                }

                return allSamples;
            }
        }

        public class Sample<TParent> : ISample
        {
            // The parent object is stored for fallback to get the index value in Instruments, Drums, or Effects
            // The bitfield is ignored because it is unneeded currently

            public TParent Parent;
            public uint BankOffset {  get; set; }
            public byte[] SampleHeader { get; set; }
            public uint Unk0 { get; set; }
            public AudioSampleCodec Codec {  get; set; }
            public AudioStorageMedium Medium {  get; set; }
            public bool IsCached { get; set; }
            public bool IsRelocated { get; set; }
            public uint Size { get; set; }
            public uint? Address {  get; set; }
            public uint? AudiotableAddress { get; set; }
            public byte[] Data { get; set; }

            public Sample(byte[] bankData, byte[] audiotable, byte[] audiotableIndex, uint sampleOffset, int audiotableId, TParent parent)
            {
                Parent = parent;
                BankOffset = sampleOffset;

                byte[] sampleHeader = new byte[0x10];
                Array.Copy(bankData, sampleOffset, sampleHeader, 0, 0x10);
                SampleHeader = sampleHeader;

                uint bits = BinaryPrimitives.ReadUInt32BigEndian(sampleHeader.AsSpan(0, 4));

                Unk0 = (bits >> 31) & 0b1;
                Codec = (AudioSampleCodec)((bits >> 28) & 0b111);
                Medium = (AudioStorageMedium)((bits >> 26) & 0b11);
                IsCached = ((bits >> 25) & 1) != 0;
                IsRelocated = ((bits >> 24) & 1) != 0;
                Size = bits & 0b111111111111111111111111;
                Address = BinaryPrimitives.ReadUInt32BigEndian(sampleHeader.AsSpan(4, 4));

                // Read the sample data from the audiotable
                if (audiotable != null && audiotableIndex != null)
                {
                    int atOffset = 0x10 + (audiotableId * 0x10);
                    byte[] audiotableEntry = new byte[0x10];
                    Array.Copy(audiotableIndex, atOffset, audiotableEntry, 0, 0x10);
                    uint audiotableOffset = BinaryPrimitives.ReadUInt32BigEndian(audiotableEntry.AsSpan(0, 4));
                    uint? sampleAddress = audiotableOffset + Address;
                    AudiotableAddress = sampleAddress;

                    // Read and store the sample data
                    byte[] sampleData = new byte[Size];
                    Array.Copy(audiotable, (int)AudiotableAddress, sampleData, 0, Size);
                    Data = sampleData;
                }
            }
        }

        public class Drum
        {
            // Gets data from a Drum struct in an instrument bank

            public int DrumId { get; set; }
            public int DecayIndex { get; set; }
            public int Pan {  get; set; }
            public uint SampleAddress { get; set; }
            public float SampleTuning { get; set; }
            public uint EnvelopeAddress { get; set; }
            public Sample<Drum> Sample { get; set; } = null;

            public Drum(int drumId, byte[] bankData, byte[] audiotable, byte[] audiotableIndex, int drumOffset, int audiotableId)
            {
                DrumId = drumId;

                // Read and store drum struct data
                DecayIndex = bankData[drumOffset];
                Pan = bankData[drumOffset + 1];

                SampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(drumOffset + 4, 4));
                SampleTuning = BinaryPrimitives.ReadSingleBigEndian(bankData.AsSpan(drumOffset + 8, 4));

                EnvelopeAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(drumOffset + 12, 4));

                // Need to figure out how to pass the name so the error can report which song... should be good enough for sinlge song testing though...
                Sample = SampleAddress != 0 ? new Sample<Drum>(bankData, audiotable, audiotableIndex, SampleAddress, audiotableId, this) : throw new Exception($"Drum Instantiation Error: Drum sample address is 0x00000000 for audiobank, audio engine will crash!");
            }
        }

        public class Effect
        {
            // Gets data from a TunedSample struct in the Effects list in an instrument bank

            public int EffectId { get; set; }
            public uint SampleAddress { get; set; }
            public float SampleTuning { get; set; }
            public Sample<Effect> Sample { get; set; } = null;

            public Effect(int effectId, byte[] bankData, byte[] audiotable, byte[] audiotableIndex, int sampleOffset, int audiotableId)
            {
                EffectId = effectId;

                // Read and store tunedsample struct data
                SampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(sampleOffset, 4));
                SampleTuning = BinaryPrimitives.ReadSingleBigEndian(bankData.AsSpan(sampleOffset + 4, 4));

                // Unsure if this also crashes the audio engine, but it should never be 0 nonetheless...
                //Sample = SampleAddress != 0 ? new Sample<Effect>(bankData, audiotable, audiotableIndex, SampleAddress, audiotableId, this) : throw new Exception($"Effect Instantiation Error: Effect sample address is 0x00000000 for audiobank, audio engine will crash!");
                Sample = new Sample<Effect>(bankData, audiotable, audiotableIndex, SampleAddress, audiotableId, this);
            }
        }

        public class Instrument
        {
            // Gets data from an Instrument struct in an instrument bank

            public int InstrumentId {  get; set; }
            public int LowKeyRegion { get; set; }
            public int HighKeyRegion { get; set; }
            public int DecayIndex { get; set; }
            public uint EnvelopeAddress { get; set; }
            public uint LowSampleAddress { get; set; }
            public float LowSampleTuning { get; set; }
            public uint PrimSampleAddress { get; set; }
            public float PrimSampleTuning { get; set; }
            public uint HighSampleAddress { get; set; }
            public float HighSampleTuning { get; set; }

            public Sample<Instrument> LowSample { get; set; } = null;
            public Sample<Instrument> PrimSample { get; set; } = null;
            public Sample<Instrument> HighSample { get; set; } = null;

            public Instrument(int instrumentId, byte[] bankData, byte[] audiotable, byte[] audiotableIndex, int instrumentOffset, int audiotableId)
            {
                InstrumentId = instrumentId;

                // Read and store instrument struct data
                LowKeyRegion = bankData[instrumentOffset + 1];
                HighKeyRegion = bankData[instrumentOffset + 2];
                DecayIndex = bankData[instrumentOffset + 3];
                EnvelopeAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(instrumentOffset + 4, 4));

                LowSampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(instrumentOffset + 8, 4));
                LowSampleTuning = BinaryPrimitives.ReadSingleBigEndian(bankData.AsSpan(instrumentOffset + 12, 4));

                PrimSampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(instrumentOffset + 16, 4));
                PrimSampleTuning = BinaryPrimitives.ReadSingleBigEndian(bankData.AsSpan(instrumentOffset + 20, 4));

                HighSampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(instrumentOffset + 24, 4));
                HighSampleTuning = BinaryPrimitives.ReadSingleBigEndian(bankData.AsSpan(instrumentOffset + 28, 4));

                // Instantiate and store sample structs as objects
                LowSample = LowSampleAddress != 0 ? new Sample<Instrument>(bankData, audiotable, audiotableIndex, LowSampleAddress, audiotableId, this) : null;
                PrimSample = PrimSampleAddress != 0 ? new Sample<Instrument>(bankData, audiotable, audiotableIndex, PrimSampleAddress, audiotableId, this) : null;
                HighSample = HighSampleAddress != 0 ? new Sample<Instrument>(bankData, audiotable, audiotableIndex, HighSampleAddress, audiotableId, this) : null;
            }
        }
    }
}
