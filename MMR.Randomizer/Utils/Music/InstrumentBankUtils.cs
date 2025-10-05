using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Linq;

namespace MMR.Randomizer.Utils.Music
{
    /// <summary>
    /// Stores classes that parse Zelda64 binary instrument bank data.
    /// </summary>
    public class InstrumentBankUtils
    {
        public enum AudioSampleCodec : int
        {
            ADPCM = 0,
            S8 = 1,
            S16_INMEM = 2,
            SMALL_ADPCM = 3,
            REVERB = 4,
            S16 = 5,
            UNK6 = 6,
            UNK7 = 7
        }

        public enum AudioStorageMedium : int
        {
            RAM = 0,
            UNK = 1,
            CART = 2,
            DISK_DRIVE = 3,
            RAM_UNLOADED = 5
        }

        public enum AudioCacheLoadType : int
        {
            PERMANENT = 0,
            PERSISTENT = 1,
            TEMPORARY = 2,
            EITHER = 3,
            EITHER_NOSYNC = 4
        }

        public interface ISample
        {
            string ParentString { get; }
            int ParentIndex { get; }
            string KeyRegion { get; }
            uint? Address { get; }
            uint BankOffset { get; }
            byte[] Data { get; }
        }

        public class Audiobin
        {
            public byte[] Audiobank { get; set; }
            public byte[] AudiobankIndex { get; set; }
            public byte[] Audiotable {  get; set; }
            public byte[] AudiotableIndex { get; set; }
            public List<InstrumentBank> Banks { get; set; } = [];

            public Audiobin(byte[] audiobank, byte[] audiobankIndex, byte[] audiotable, byte[] audiotableIndex)
            {
                Audiobank = audiobank;
                AudiobankIndex = audiobankIndex;
                Audiotable = audiotable;
                AudiotableIndex = audiotableIndex;

                var numBanks = BinaryPrimitives.ReadUInt16BigEndian(AudiobankIndex.AsSpan(0, 2));
                for (int i = 0; i < numBanks; i++)
                {
                    int index = 0x10 + (0x10 * i);
                    byte[] currentEntry = new byte[0x10];
                    Array.Copy(AudiobankIndex, index, currentEntry, 0, 0x10);
                    var instrumentBank = new InstrumentBank(currentEntry, Audiobank, Audiotable, AudiotableIndex);
                    Banks.Add(instrumentBank);
                }
            }

            public ISample FindSampleInBanks(byte[] sampleData)
            {
                foreach (var bank in Banks)
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

        public class InstrumentBank
        {
            public uint BankOffset { get; set; }
            public uint BankLength { get; set; }
            public AudioStorageMedium SampleMedium { get; set; }
            public AudioCacheLoadType CacheLoadType { get; set; }
            public int SampleBankId_1 { get; set; }
            public int SampleBankId_2 { get; set; }
            public int NumInsts { get; set; }
            public int NumDrums { get; set; }
            public int NumEffects { get; set; }
            public byte[] BankData { get; set; }
            public byte[] BankMetadata { get; set; }

            public List<Instrument> Instruments = [];
            public List<Drum> Drums = [];
            public List<Effect> Effects = [];

            public InstrumentBank(byte[] tableEntry, byte[] audiobankFile, byte[] audiotableFile, byte[] audiotableIndex)
            {
                byte[] bankMetadata = new byte[8];
                switch (tableEntry.Length)
                {
                    case 0x08:
                        BankOffset = 0;
                        BankLength = 0;
                        SampleMedium = CheckForValidEnum<AudioStorageMedium>(tableEntry[0]);
                        CacheLoadType = CheckForValidEnum<AudioCacheLoadType>(tableEntry[1]);
                        SampleBankId_1 = tableEntry[2];
                        SampleBankId_2 = tableEntry[3];
                        NumInsts = tableEntry[4];
                        NumDrums = tableEntry[5];
                        NumEffects = BinaryPrimitives.ReadUInt16BigEndian(tableEntry.AsSpan(6, 2));

                        bankMetadata = tableEntry;
                        break;

                    case 0x10:
                        BankOffset = BinaryPrimitives.ReadUInt32BigEndian(tableEntry.AsSpan(0, 4));
                        BankLength = BinaryPrimitives.ReadUInt32BigEndian(tableEntry.AsSpan(4, 4));;
                        SampleMedium = CheckForValidEnum<AudioStorageMedium>(tableEntry[8]);
                        CacheLoadType = CheckForValidEnum<AudioCacheLoadType>(tableEntry[9]);
                        SampleBankId_1 = tableEntry[10];
                        SampleBankId_2 = tableEntry[11];
                        NumInsts = tableEntry[12];
                        NumDrums = tableEntry[13];
                        NumEffects = BinaryPrimitives.ReadInt16BigEndian(tableEntry.AsSpan(14, 2));

                        Array.Copy(tableEntry, 8, bankMetadata, 0, 8);
                        break;

                    default:
                        throw new Exception($"Object Instantiation Error: Invalid length for bank metadata binary — expected '8' or '16' bytes, but got '{tableEntry.Length}' bytes instead");
                }

                // If the bank metadata is 8 bytes, the audiobankFile should be the zbank file
                // Because of this, the length is just 0, so the length is the zbank length
                var length = BankLength == 0 ? audiobankFile.Length : (int)BankLength;
                byte[] bankData = new byte[length];
                Array.Copy(audiobankFile, BankOffset, bankData, 0, length);
                BankData = bankData;

                BankMetadata = bankMetadata;

                // Instantiate all drums, effects, and instruments in the instrument bank
                uint drumListAddr = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan(0, 4));
                for (int i = 0; i < NumDrums; i++)
                {
                    uint offset = drumListAddr + (uint)(4 * i);
                    offset = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan((int)offset, 4));
                    Drum drum = offset != 0 ? new Drum(i, BankData, audiotableFile, audiotableIndex, (int)offset, SampleBankId_1) : null;
                    Drums.Add(drum);
                }

                uint effectListAddr = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan(4, 4));
                for (int i = 0; i < NumEffects; i++)
                {
                    uint offset = effectListAddr + (uint)(8 * i);
                    Effect effect = offset != 0 ? new Effect(i, BankData, audiotableFile, audiotableIndex, (int)offset, SampleBankId_1) : null;
                    Effects.Add(effect);
                }

                for (int i = 0; i < NumInsts; i++)
                {
                    uint offset = 0x08 + (uint)(4 * i);
                    offset = BinaryPrimitives.ReadUInt32BigEndian(BankData.AsSpan((int)offset, 4));
                    Instrument instrument = offset != 0 ? new Instrument(i, BankData, audiotableFile, audiotableIndex, (int)offset, SampleBankId_1) : null;
                    Instruments.Add(instrument);
                }
            }

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

        public class Sample<TParent> : ISample
        {
            public TParent Parent;
            public string ParentString { get; set; }
            public int ParentIndex { get; set; }
            public string KeyRegion { get; set; }
            public uint BankOffset { get; set; }
            public uint Unk0 { get; set; }
            public AudioSampleCodec Codec { get; set; }
            public AudioStorageMedium Medium { get; set; }
            public bool IsCached { get; set; }
            public bool IsRelocated { get; set; }
            public uint Size { get; set; }
            public uint? Address { get; set; }
            public uint? AudiotableAddress { get; set; }
            public byte[] Data { get; set; }

            public Sample(byte[] bankData, byte[] audiotable, byte[] audiotableIndex, uint sampleOffset, int sampleBankId, TParent parent, int parentIndex, string keyRegion = null)
            {
                Parent = parent;
                ParentString = parent switch
                {
                    Instrument => "INST",
                    Drum => "DRUM",
                    Effect => "SFX",
                    _ => null
                };
                ParentIndex = parentIndex;
                KeyRegion = keyRegion;

                ReadOnlySpan<byte> sampleHeader = bankData.AsSpan((int)sampleOffset, 0x10);
                Bitfield bitfield = Bitfield.Parse(sampleHeader);

                Unk0 = bitfield.Unk0;
                Codec = bitfield.Codec;
                Medium = bitfield.Medium;
                IsCached = bitfield.IsCached;
                IsRelocated = bitfield.IsRelocated;
                Size = bitfield.Size;
                Address = BinaryPrimitives.ReadUInt32BigEndian(sampleHeader.Slice(4, 4));

                if (Codec != AudioSampleCodec.ADPCM && Codec != AudioSampleCodec.SMALL_ADPCM)
                    throw new InvalidOperationException($"InstrumentBankUtils Error: Expected Codec of 'ADPCM' or 'SMALL_ADPCM', but got '{Codec}' instead.");

                if (Medium != AudioStorageMedium.RAM)
                    throw new InvalidOperationException($"InstrumentBankUtils Error: Expected Medium of 'RAM', but got '{Medium}' instead.");

                if (IsRelocated)
                    throw new InvalidOperationException($"InstrumentBankUtils Error: Expected IsRelocated of 'false', but got '{IsRelocated}' instead.");

                if (audiotable != null && Address > audiotable.Length) { Data = null; Address = null; return; }

                if (audiotable != null && audiotableIndex != null)
                {
                    int atOffset = 0x10 + (sampleBankId * 0x10);
                    byte[] audiotableEntry = new byte[0x10];
                    Array.Copy(audiotableIndex, atOffset, audiotableEntry, 0, 0x10);
                    uint audiotableOffset = BinaryPrimitives.ReadUInt32BigEndian(audiotableEntry.AsSpan(0, 4));
                    uint? sampleAddress = audiotableOffset + Address;
                    AudiotableAddress = sampleAddress;

                    byte[] sampleData = new byte[Size];
                    Array.Copy(audiotable, (int)AudiotableAddress, sampleData, 0, Size);
                    Data = sampleData;
                }
                else
                {
                    Data = null;
                    AudiotableAddress = null;
                }
            }

            private readonly struct Bitfield(uint raw)
            {
                public uint Unk0 { get; } = (raw >> 31) & 0b1;
                public AudioSampleCodec Codec { get; } = (AudioSampleCodec)((raw >> 28) & 0b111);
                public AudioStorageMedium Medium { get; } = (AudioStorageMedium)((raw >> 26) & 0b11);
                public bool IsCached { get; } = ((raw >> 25) & 1) != 0;
                public bool IsRelocated { get; } = ((raw >> 24) & 1) != 0;
                public uint Size { get; } = raw & 0b111111111111111111111111;

                public static Bitfield Parse(ReadOnlySpan<byte> data)
                {
                    uint raw = BinaryPrimitives.ReadUInt32BigEndian(data.Slice(0, 4));
                    return new Bitfield(raw);
                }
            }

        }

        public class Drum
        {
            public int DrumIndex { get; set; }
            public int DecayIndex { get; set; }
            public int Pan { get; set; }
            public uint SampleAddress { get; set; }
            public float SampleTuning { get; set; }
            public uint EnvelopeAddress { get; set; }

            public Sample<Drum> Sample { get; set; } = null;

            public Drum(int drumIndex, byte[] bankData, byte[] audiotable, byte[] audiotableIndex, int drumOffset, int sampleBankId)
            {
                DrumIndex = drumIndex;

                DecayIndex = bankData[drumOffset];
                Pan = bankData[drumOffset + 1];
                SampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(drumOffset + 4, 4));
                SampleTuning = BinaryPrimitives.ReadSingleBigEndian(bankData.AsSpan(drumOffset + 8, 4));
                EnvelopeAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(drumOffset + 12, 4));

                Sample = SampleAddress != 0 ? new Sample<Drum>(bankData, audiotable, audiotableIndex, SampleAddress, sampleBankId, this, DrumIndex) : throw new Exception($"Drum Instantiation Error: Drum sample address is 0x00000000 for audiobank, audio engine will crash!");
            }
        }

        public class Effect
        {
            public int EffectIndex { get; set; }
            public uint SampleAddress { get; set; }
            public float SampleTuning { get; set; }

            public Sample<Effect> Sample { get; set; } = null;

            public Effect(int effectIndex, byte[] bankData, byte[] audiotable, byte[] audiotableIndex, int sampleOffset, int sampleBankId)
            {
                EffectIndex = effectIndex;
                SampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(sampleOffset, 4));
                SampleTuning = BinaryPrimitives.ReadSingleBigEndian(bankData.AsSpan(sampleOffset + 4, 4));

                Sample = new Sample<Effect>(bankData, audiotable, audiotableIndex, SampleAddress, sampleBankId, this, EffectIndex);
            }
        }

        public class Instrument
        {
            public int InstrumentIndex { get; set; }
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

            public Instrument(int instrumentIndex, byte[] bankData, byte[] audiotable, byte[] audiotableIndex, int instrumentOffset, int sampleBankId)
            {
                InstrumentIndex = instrumentIndex;

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

                LowSample = LowSampleAddress != 0 ? new Sample<Instrument>(bankData, audiotable, audiotableIndex, LowSampleAddress, sampleBankId, this, InstrumentIndex, "LOW") : null;
                PrimSample = PrimSampleAddress != 0 ? new Sample<Instrument>(bankData, audiotable, audiotableIndex, PrimSampleAddress, sampleBankId, this, InstrumentIndex, "PRIM") : null;
                HighSample = HighSampleAddress != 0 ? new Sample<Instrument>(bankData, audiotable, audiotableIndex, HighSampleAddress, sampleBankId, this, InstrumentIndex, "HIGH") : null;
            }
        }

        private static TEnum CheckForValidEnum<TEnum>(int value) where TEnum : Enum
        {
            if (!Enum.IsDefined(typeof(TEnum), value))
                throw new InvalidOperationException($"Object Instantiation Error: Invalid {typeof(TEnum).Name} value: {value}");

            return (TEnum)(object)value;
        }

    }
}
