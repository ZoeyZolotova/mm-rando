using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace MMR.Randomizer.Utils
{
    public class AudiobankUtils
    {
        // Should only need the sample offsets, and int should be fine for bank addresses
        // because there shouldn't be a bank out there longer than 0x7FFFFFFF bytes

        public class Audiobank
        {
            public List<Instrument> Instruments = new();
            public List<Drum> Drums = new();
            public List<Effect> Effects = new();

            public Audiobank(byte[] metadata, byte[] bankData)
            {
                int numInsts = metadata[12];
                int numDrums = metadata[13];
                int numEffects = BinaryPrimitives.ReadUInt16BigEndian(metadata.AsSpan(14, 2));

                uint drumListAddr = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(0, 4));
                for (int i = 0; i < numDrums; i++)
                {
                    uint offset = drumListAddr + (uint)(4 * i);
                    offset = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan((int)offset, 4));
                    Drum drum = offset != 0 ? new Drum(i, bankData, (int)offset) : null;
                    Drums.Add(drum);
                }

                uint effectListAddr = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(4, 4));
                for (int i = 0; i < numEffects; i++)
                {
                    uint offset = effectListAddr + (uint)(8 * i);
                    Effect effect = offset != 0 ? new Effect(i, bankData, (int)offset) : null;
                    Effects.Add(effect);
                }

                for (int i = 0; i < numInsts; i++)
                {
                    uint offset = 0x08 + (uint)(4 * i);
                    offset = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan((int)offset, 4));
                    Instrument instrument = offset != 0 ? new Instrument(i, bankData, (int)offset) : null;
                    Instruments.Add(instrument);
                }
            }
        }

        public class Sample<TParent>
        {
            public TParent Parent;
            public uint BankOffset {  get; set; }
            public uint Address {  get; set; }

            public Sample(byte[] bankData, uint sampleOffset, TParent parent)
            {
                Parent = parent;
                BankOffset = sampleOffset;

                byte[] sampleHeader = new byte[0x10];
                Array.Copy(bankData, sampleOffset, sampleHeader, 0, 0x10);
                Address = BinaryPrimitives.ReadUInt32BigEndian(sampleHeader.AsSpan(4, 4));
            }
        }

        public class Instrument
        {
            public int InstrumentID {  get; set; }
            public uint LowSampleAddress { get; set; }
            public uint PrimSampleAddress { get; set; }
            public uint HighSampleAddress { get; set; }

            public Sample<Instrument> LowSample { get; set; }
            public Sample<Instrument> PrimSample { get; set; }
            public Sample<Instrument> HighSample { get; set; }

            public Instrument(int instrumentId, byte[] bankData, int instrumentOffset)
            {
                InstrumentID = instrumentId;

                LowSampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(instrumentOffset + 8, 4));
                PrimSampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(instrumentOffset + 16, 4));
                HighSampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(instrumentOffset + 24, 4));

                LowSample = LowSampleAddress != 0 ? new Sample<Instrument>(bankData, LowSampleAddress, this) : null;
                PrimSample = PrimSampleAddress != 0 ? new Sample<Instrument>(bankData, PrimSampleAddress, this) : null;
                HighSample = HighSampleAddress != 0 ? new Sample<Instrument>(bankData, HighSampleAddress, this) : null;
            }
        }

        public class Drum
        {
            public int DrumId { get; set; }
            public uint SampleAddress { get; set; }
            public Sample<Drum> Sample { get; set; }

            public Drum(int drumId, byte[] bankData, int drumOffset)
            {
                DrumId = drumId;

                SampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(drumOffset + 4, 4));

                // Need to figure out how to pass the name so the error can report which song... should be good enough for sinlge song testing though...
                Sample = SampleAddress != 0 ? new Sample<Drum>(bankData, SampleAddress, this) : throw new Exception($"Error: Drum sample address is 0x00000000 for audiobank, audio engine will crash!");
            }
        }

        public class Effect
        {
            public int EffectId { get; set; }
            public uint SampleAddress { get; set; }
            public Sample<Effect> Sample { get; set; }

            public Effect(int effectId, byte[] bankData, int sampleOffset)
            {
                EffectId = effectId;

                SampleAddress = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(sampleOffset, 4));

                // Unsure if this also crashes the audio engine, but it should never be 0 nonetheless...
                Sample = SampleAddress != 0 ? new Sample<Effect>(bankData, SampleAddress, this) : throw new Exception($"Error: Effect sample address is 0x00000000 for audiobank, audio engine will crash!");
            }
        }
    }
}
