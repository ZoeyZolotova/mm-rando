using System;
using System.Buffers.Binary;
using System.Collections.Generic;

namespace MMR.Randomizer.Utils
{
    public class AudiobankUtils
    {
        // Parses through a binary instrument bank (.zbank, decompressed) file and stores data

        public interface ISample
        {
            uint Address { get; set; }
            uint BankOffset { get; set; }
        }

        // Should only need the sample offsets, and int should be fine for bank addresses
        // because there shouldn't be a bank out there longer than 0x7FFFFFFF bytes

        public class Audiobank
        {
            public List<Instrument> Instruments = new();
            public List<Drum> Drums = new();
            public List<Effect> Effects = new();

            public Audiobank(byte[] metadata, byte[] bankData)
            {
                // The metadata stored in SequenceSoundSampleBinaryData is not the full 0x10 bytes
                // it's the .bankmeta from the music file.

                int numInsts = metadata[4];
                int numDrums = metadata[5];
                int numEffects = BinaryPrimitives.ReadUInt16BigEndian(metadata.AsSpan(6, 2));

                // Find all the drums and instantiate them
                uint drumListAddr = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(0, 4));
                for (int i = 0; i < numDrums; i++)
                {
                    uint offset = drumListAddr + (uint)(4 * i);
                    offset = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan((int)offset, 4));
                    Drum drum = offset != 0 ? new Drum(i, bankData, (int)offset) : null;
                    Drums.Add(drum);
                }

                // Find all the effects and instantiate them
                uint effectListAddr = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan(4, 4));
                for (int i = 0; i < numEffects; i++)
                {
                    uint offset = effectListAddr + (uint)(8 * i);
                    Effect effect = offset != 0 ? new Effect(i, bankData, (int)offset) : null;
                    Effects.Add(effect);
                }

                // Find all the instruments and instantiante them
                for (int i = 0; i < numInsts; i++)
                {
                    uint offset = 0x08 + (uint)(4 * i);
                    offset = BinaryPrimitives.ReadUInt32BigEndian(bankData.AsSpan((int)offset, 4));
                    Instrument instrument = offset != 0 ? new Instrument(i, bankData, (int)offset) : null;
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
            // Gets data from an Instrument struct in an instrument bank

            public int InstrumentId {  get; set; }
            public uint LowSampleAddress { get; set; }
            public uint PrimSampleAddress { get; set; }
            public uint HighSampleAddress { get; set; }

            public Sample<Instrument> LowSample { get; set; } = null;
            public Sample<Instrument> PrimSample { get; set; } = null;
            public Sample<Instrument> HighSample { get; set; } = null;

            public Instrument(int instrumentId, byte[] bankData, int instrumentOffset)
            {
                InstrumentId = instrumentId;

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
            // Gets data from a Drum struct in an instrument bank

            public int DrumId { get; set; }
            public uint SampleAddress { get; set; }
            public Sample<Drum> Sample { get; set; } = null;

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
            // Gets data from a TunedSample struct in the Effects list in an instrument bank

            public int EffectId { get; set; }
            public uint SampleAddress { get; set; }
            public Sample<Effect> Sample { get; set; } = null;

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
