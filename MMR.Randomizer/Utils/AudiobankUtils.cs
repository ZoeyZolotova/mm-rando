using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;

namespace MMR.Randomizer.Utils
{
    /// <summary>
    /// Stores classes that parse Zelda64 binary audio file and instrument bank data.
    /// </summary>
    public class AudiobankUtils
    {
        /// <summary>
        /// Represents the possible Zelda64 audio sample codecs.
        /// </summary>
        public enum AudioSampleCodec : int
        {
            CODEC_ADPCM,
            CODEC_S8,
            CODEC_S16_INMEM,
            CODEC_SMALL_ADPCM,
            CODEC_REVERB,
            CODEC_S16,
            CODEC_UNK6,
            CODEC_UNK7
        }

        /// <summary>
        /// Represents the possible Zelda64 audio sample storage mediums.
        /// </summary>
        public enum AudioStorageMedium: int
        {
            MEDIUM_RAM,
            MEDIUM_UNK,
            MEDIUM_CART,
            MEDIUM_DISK_DRIVE,
            MEDIUM_RAM_UNLOADED = 5
        }

        /// <summary>
        /// Represents the possible Zelda64 caching policies for instrument banks, audio sequences, and audio samples.
        /// </summary>
        public enum AudioCacheLoadType: int
        {
            CACHE_LOAD_PERMANENT,
            CACHE_LOAD_PERSISTENT,
            CACHE_LOAD_TEMPORARY,
            CACHE_LOAD_EITHER,
            CACHE_LOAD_EITHER_NOSYNC
        }

        /// <summary>
        /// Interface to return data from a sample that requires a parent object.
        /// </summary>
        public interface ISample
        {
            // For information on the fields, read the Sample class
            string ParentString { get; }
            int ParentId { get; }
            string KeyRegion { get; }
            uint? Address { get; }
            uint BankOffset { get; }
            byte[] Data { get; }
        }

        /// <summary>
        /// Represents the audiobank, audiobank index, audiotable, and audiotable index Zelda64 binary files.
        /// </summary>
        public class Audiobin
        {
            public byte[] AudiobankTable { get; set; } // Audiobank data
            public byte[] AudiobankIndex { get; set; } // Audiobank index data
            public byte[] Audiotable { get; set; } // Audiotable data
            public byte[] AudiotableIndex { get; set; } // Audiotable index data
            public List<Audiobank> Audiobanks { get; set; } = []; // A list of all audiobanks in the audio binary

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

            /// <summary>
            /// Runs through each audiobank in the audio binary to find a sample with matching data.
            /// </summary>
            /// <returns>
            /// Returns the matched sample, otherwise returns 'null'.
            /// </returns>
            public ISample FindSampleInBanks(byte[] sampleData)
            {
                foreach (var bank in Audiobanks)
                {
                    foreach (var instrument in bank.Instruments)
                    {
                        if (instrument?.LowSample?.Data.SequenceEqual(sampleData) == true)
                            return instrument.LowSample;

                        if (instrument?.PrimSample?.Data.SequenceEqual(sampleData) == true)
                            return instrument.PrimSample;

                        if (instrument?.HighSample?.Data.SequenceEqual(sampleData) == true)
                            return instrument.HighSample;
                    }

                    foreach (var drum in bank.Drums)
                    {
                        if (drum?.Sample?.Data.SequenceEqual(sampleData) == true)
                            return drum.Sample;
                    }

                    foreach (var effect in bank.Effects)
                    {
                        if (effect?.Sample?.Data.SequenceEqual(sampleData) == true)
                            return effect.Sample;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Represents a Zelda64 binary instrument bank.
        /// </summary>
        public class Audiobank {

            public uint BankOffset { get; set; } // Offset of the bank in the audiotable
            public uint BankLength { get; set; } // Length of the bank in th audiotable
            public AudioStorageMedium SampleMedium { get; set; } // The storage medium for samples, default is RAM (u8)
            public AudioCacheLoadType CachePolicy { get; set; } // The cache policy the bank uses (u8)
            public int AudiotableId { get; set; } // The ID of the audiotable the samples use (u8)
            public int BankId { get; set; } // The ID of the bank, default is 0xFF (u8)
            public int NumInsts { get; set; } // The number of instruments in the bank (u8)
            public int NumDrums { get; set; } // The number of drums in the bank (u8)
            public int NumEffects { get; set; } // The number of effects in the bank (u16)
            public byte[] BankData { get; set; } // The bank's binary data
            public byte[] Bankmeta { get; set; } // 8 byte long bytearray, not 16 bytes

            public List<Instrument> Instruments = [];
            public List<Drum> Drums = [];
            public List<Effect> Effects = [];

            public Audiobank(byte[] tableEntry, byte[] audiobankFile, byte[] audiotableFile, byte[] audiotableIndex)
            {
                byte[] bankmetaData = new byte[8];
                switch (tableEntry.Length)
                {
                    case 0x08: // 8 Bytes (.bankmeta): [Sample Medium, Sequence Player, Audiotable, ID, Num Inst, Num Drum, Num Effect MSB, Num Effect LSB]
                        BankOffset = 0;
                        BankLength = 0;
                        SampleMedium = CheckForValidEnum<AudioStorageMedium>(tableEntry[0]);
                        CachePolicy = CheckForValidEnum<AudioCacheLoadType>(tableEntry[1]);
                        AudiotableId = tableEntry[2];
                        BankId = tableEntry[3];
                        NumInsts = tableEntry[4];
                        NumDrums = tableEntry[5];
                        NumEffects = BinaryPrimitives.ReadUInt16BigEndian(tableEntry.AsSpan(6, 2));

                        bankmetaData = tableEntry;
                        break;

                    case 0x10: // 16 Bytes: [4-byte Address, 4-byte Length, Sample Medium, Sequence Player, Audiotable, ID, Num Inst, Num Drum, 2-byte Num Effects]
                        BankOffset = BinaryPrimitives.ReadUInt32BigEndian(tableEntry.AsSpan(0, 4));
                        BankLength = BinaryPrimitives.ReadUInt32BigEndian(tableEntry.AsSpan(4, 4));
                        SampleMedium = CheckForValidEnum<AudioStorageMedium>(tableEntry[8]);
                        CachePolicy = CheckForValidEnum<AudioCacheLoadType>(tableEntry[9]);
                        AudiotableId = tableEntry[10];
                        BankId = tableEntry[11];
                        NumInsts = tableEntry[12];
                        NumDrums = tableEntry[13];
                        NumEffects = BinaryPrimitives.ReadUInt16BigEndian(tableEntry.AsSpan(14, 2));

                        Array.Copy(tableEntry, 8, bankmetaData, 0, 8);
                        break;

                    default: // When reading .bankmeta there's already a check for 8 bytes, but never hurts to be extra safe
                        throw new Exception($"Audiobank Instantiation Error: Invalid length for bankmeta binary - expected '8' or '16' bytes, but got '{tableEntry.Length}' bytes instead");
                }

                // If the bankmeta is just the 8 bytes, the audiobankFile should be the zbank file
                // Because of this, BankLength is 0 so use the length of the zbank getting passed in
                var length = BankLength == 0 ? audiobankFile.Length : (int)BankLength;
                byte[] bankData = new byte[length];
                Array.Copy(audiobankFile, BankOffset, bankData, 0, length);
                BankData = bankData;

                Bankmeta = bankmetaData;

                // Find all the drums and instantiate them
                uint drumListAddr = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan(0, 4));
                for (int i = 0; i < NumDrums; i++)
                {
                    uint offset = drumListAddr + (uint)(4 * i);
                    offset = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan((int)offset, 4));
                    Drum drum = offset != 0 ? new Drum(i, BankData, audiotableFile, audiotableIndex, (int)offset, AudiotableId) : null;
                    Drums.Add(drum);
                }

                // Find all the effects and instantiate them
                uint effectListAddr = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan(4, 4));
                for (int i = 0; i < NumEffects; i++)
                {
                    uint offset = effectListAddr + (uint)(8 * i);
                    Effect effect = offset != 0 ? new Effect(i, BankData, audiotableFile, audiotableIndex, (int)offset, AudiotableId) : null;
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

            /// <summary>
            /// Runs through all the instruments, drums, and effects in the instrument bank,
            /// then adds any sample objects it finds to a list so their addresses can be
            /// searched when updating custom audio sample pointers.
            /// </summary>
            /// <returns>
            /// List of all samples in the instrument bank.
            /// </returns>
            public List<ISample> GetBankSamples()
            {
                List<ISample> allSamples = [];

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

        /// <summary>
        /// Represents a Sample struct for a Zelda64 binary instrument bank.
        /// </summary>
        public class Sample<TParent> : ISample
        {
            // The parent object is stored for fallback to get the index value in Instruments, Drums, or Effects

            public TParent Parent; // The parent structure as its memory object
            public string ParentString { get; set; } // The type of struct the parent is as a string: INST, DRUM, SFx
            public int ParentId { get; set; } // The index of the parent struct in the corresponding list
            public string KeyRegion { get; set; } // The audio sample's key region in the parent struct
            public uint BankOffset {  get; set; } // Offset of the sample struct in the bank
            public byte[] SampleHeader { get; set; }
            public uint Unk0 { get; set; }
            public AudioSampleCodec Codec {  get; set; } // Audio codec of the audio sample
            public AudioStorageMedium Medium {  get; set; } // Storage medium of the audio sample
            public bool IsCached { get; set; } // Whether the sample is cached or not
            public bool IsRelocated { get; set; } // Whether the sample is relocated in memory or not
            public uint Size { get; set; } // Size of the binary ADPCM audio sample
            public uint? Address {  get; set; } // Sample address if it was in audiotable 0 or 1
            public uint? AudiotableAddress { get; set; } // Sample address in the bank's corresponding audiotable
            public byte[] Data { get; set; } // Binary ADPCM audio sample data

            public Sample(byte[] bankData, byte[] audiotable, byte[] audiotableIndex, uint sampleOffset, int audiotableId, TParent parent, int parentId, string keyRegion = null)
            {
                Parent = parent;
                ParentString = parent switch
                {
                    Instrument => "INST",
                    Drum => "DRUM",
                    Effect => "SFX",
                    _ => null
                };
                ParentId = parentId;
                KeyRegion = keyRegion;

                BankOffset = sampleOffset;

                byte[] sampleHeader = new byte[0x10];
                Array.Copy(bankData, sampleOffset, sampleHeader, 0, 0x10);
                SampleHeader = sampleHeader;

                // The first 4 bytes of the sample struct are a bitfield, the size is required
                // when copying the data into a bytearray and is the last 24 bits of data,
                // so the bitfield needs to be unpacked properly - not unpacking it properly will cause issues
                uint bits = BinaryPrimitives.ReadUInt32BigEndian(sampleHeader.AsSpan(0, 4));

                Unk0 = (bits >> 31) & 0b1;
                Codec = (AudioSampleCodec)((bits >> 28) & 0b111);
                Medium = (AudioStorageMedium)((bits >> 26) & 0b11);
                IsCached = ((bits >> 25) & 1) != 0;
                IsRelocated = ((bits >> 24) & 1) != 0;
                Size = bits & 0b111111111111111111111111; // Extract only the last 24 bits
                Address = BinaryPrimitives.ReadUInt32BigEndian(sampleHeader.AsSpan(4, 4));

                // Samples should always be ADPCM or small ADPCM, using RAM, and not be relocated
                if (Codec != AudioSampleCodec.CODEC_ADPCM && Codec != AudioSampleCodec.CODEC_SMALL_ADPCM)
                    throw new InvalidOperationException($"AudiobankUtils Error: Expected Codec of 'CODEC_ADPCM' or 'CODEC_SMALL_ADPCM', but got '{Codec}' instead.");

                if (Medium != AudioStorageMedium.MEDIUM_RAM)
                    throw new InvalidOperationException($"AudiobankUtils Error: Expected Medium of 'MEDIUM_RAM', but got '{Medium}' instead.");

                if (IsRelocated)
                    throw new InvalidOperationException($"AudiobankUtils Error: Expected IsRelocated of 'false', but got '{IsRelocated}' instead.");

                // If the data is outside the audiotable, it does not exist
                if (audiotable != null && Address > audiotable.Length)
                {
                    Data = null;
                    Address = null;
                    return;
                }

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
                else // There was no audiotable, so we can't get the data
                {
                    Data = null;
                    AudiotableAddress = null;
                }
            }
        }

        /// <summary>
        /// Represents a Drum struct for a Zelda64 binary instrument bank.
        /// </summary>
        public class Drum
        {
            public int DrumId { get; set; } // The index of the struct in the drum list
            public int DecayIndex { get; set; } // The index of the note release decay rate in the adsr decay table
            public int Pan {  get; set; } // Individual drum panning
            public uint SampleAddress { get; set; } // Offset to the sample struct in the bank
            public float SampleTuning { get; set; } // The tuning float for the audio sample
            public uint EnvelopeAddress { get; set; } // Offset to the envelope point array in the bank
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
                Sample = SampleAddress != 0 ? new Sample<Drum>(bankData, audiotable, audiotableIndex, SampleAddress, audiotableId, this, DrumId) : throw new Exception($"Drum Instantiation Error: Drum sample address is 0x00000000 for audiobank, audio engine will crash!");
            }
        }

        /// <summary>
        /// Represents a TunedSample struct in the effect list for a Zelda64 binary instrument bank.
        /// </summary>
        public class Effect
        {
            public int EffectId { get; set; } // The index of the effect in the effect list
            public uint SampleAddress { get; set; } // Offset to the sample struct in the bank
            public float SampleTuning { get; set; } // The tuning float for the audio sample
            public Sample<Effect> Sample { get; set; } = null;

            public Effect(int effectId, byte[] bankData, byte[] audiotable, byte[] audiotableIndex, int sampleOffset, int audiotableId)
            {
                EffectId = effectId;

                // Read and store tunedsample struct data
                SampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(sampleOffset, 4));
                SampleTuning = BinaryPrimitives.ReadSingleBigEndian(bankData.AsSpan(sampleOffset + 4, 4));

                // Unsure if this also crashes the audio engine, but it should never be 0 nonetheless...
                //Sample = SampleAddress != 0 ? new Sample<Effect>(bankData, audiotable, audiotableIndex, SampleAddress, audiotableId, this) : throw new Exception($"Effect Instantiation Error: Effect sample address is 0x00000000 for audiobank, audio engine will crash!");
                Sample = new Sample<Effect>(bankData, audiotable, audiotableIndex, SampleAddress, audiotableId, this, EffectId);
            }
        }

        /// <summary>
        /// Represents an Instrument struct for a Zelda64 binary instrument bank.
        /// </summary>
        public class Instrument
        {
            public int InstrumentId {  get; set; } // The index of the instrument in the instrument list
            public int LowKeyRegion { get; set; } // The max range for the instrument's low key region
            public int HighKeyRegion { get; set; } // The min range for the instrument's high key region
            public int DecayIndex { get; set; } // The index of the note release decay rate in the adsr decay table
            public uint EnvelopeAddress { get; set; } // Offset to the envelope point array in the bank
            public uint LowSampleAddress { get; set; } // Offset to the sample struct in the bank for the low key region sample
            public float LowSampleTuning { get; set; } // The tuning float for the low key region's audio sample
            public uint PrimSampleAddress { get; set; } // Offset to the sample struct in the bank for the primary key region sample
            public float PrimSampleTuning { get; set; } // The tuning float for the primary key region's audio sample
            public uint HighSampleAddress { get; set; } // Offset to the sample struct in the bank for the high key region sample
            public float HighSampleTuning { get; set; } // The tuning float for the high key region's audio sample

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
                LowSample = LowSampleAddress != 0 ? new Sample<Instrument>(bankData, audiotable, audiotableIndex, LowSampleAddress, audiotableId, this, InstrumentId, "LOW") : null;
                PrimSample = PrimSampleAddress != 0 ? new Sample<Instrument>(bankData, audiotable, audiotableIndex, PrimSampleAddress, audiotableId, this, InstrumentId, "PRIM") : null;
                HighSample = HighSampleAddress != 0 ? new Sample<Instrument>(bankData, audiotable, audiotableIndex, HighSampleAddress, audiotableId, this, InstrumentId, "HIGH") : null;
            }
        }

        private static TEnum CheckForValidEnum<TEnum>(int value) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
                throw new InvalidOperationException($"Audiobank Instantiation Error: Invalid {typeof(TEnum).Name} value in bankmeta binary: {value}");

            return (TEnum)(object)value;
        }
    }
}
