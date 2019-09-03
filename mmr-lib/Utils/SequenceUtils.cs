using MMRando.Constants;
using MMRando.Models.Rom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace MMRando.Utils
{
    public class SequenceUtils
    {
        const int AUDIO_SEQ_TABLE = 0xC77B80;
        const int INSTRUMENT_SET_MAP = 0xC77A60;

        public static List<SequenceInfo> ReadSequenceInfo()
        {
            var json = Resources.GetTextFile("sequences.json");
            List<SequenceInfo> sequences = JsonConvert.DeserializeObject<List<SequenceInfo>>(json);

            var songOfTime = sequences.Single(x => x.Name == "mmr-f-sot");
            songOfTime.Replaces = 0x33;

            return sequences;
        }


        public static void RebuildAudioSeq(List<SequenceInfo> sequences)
        {
            List<MMSequence> OldSeq = new List<MMSequence>();
            int f = RomUtils.GetFileIndexForWriting(AUDIO_SEQ_TABLE);
            int basea = RomData.MMFileList[f].Addr;

            for (int i = 0; i < 128; i++)
            {
                MMSequence entry = new MMSequence();

                if (i == 0x1E)
                {
                    entry.Addr = 2;
                    entry.Size = 0;
                    OldSeq.Add(entry);
                    continue;
                }

                int entryaddr = AUDIO_SEQ_TABLE + (i * 0x10);
                entry.Addr = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, entryaddr - basea);
                entry.Size = (int)ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, entryaddr - basea + 4);
                if (entry.Size > 0)
                {
                    entry.Data = new byte[entry.Size];
                    Array.Copy(RomData.MMFileList[4].Data, entry.Addr, entry.Data, 0, entry.Size);
                }
                else
                {
                    var sequence = sequences.SingleOrDefault(u => u.Replaces == i);
                    if (sequence != null
                        && (entry.Addr > 0) && (entry.Addr < 128)) //not actually necessary
                    {
                        if (sequence.Replaces != 0x28)
                        {
                            sequence.Replaces = entry.Addr;
                        }
                        else
                        {
                            entry.Data = OldSeq[0x18].Data;
                            entry.Size = OldSeq[0x18].Size;
                        }

                    }
                }
                OldSeq.Add(entry);
            }

            List<MMSequence> NewSeq = new List<MMSequence>();
            int addr = 0;
            byte[] NewAudioSeq = new byte[0];
            for (int i = 0; i < 128; i++)
            {
                MMSequence newentry = new MMSequence();
                if (OldSeq[i].Size == 0)
                {
                    newentry.Addr = OldSeq[i].Addr;
                }
                else
                {
                    newentry.Addr = addr;
                }

                int j = sequences.FindIndex(u => u.Replaces == i);
                if (j != -1)
                {
                    if (sequences[j].SequenceId != -1)
                    {
                        newentry.Size = OldSeq[sequences[j].SequenceId].Size;
                        newentry.Data = OldSeq[sequences[j].SequenceId].Data;
                    }
                    else
                    {
                        BinaryReader sequence = new BinaryReader(File.Open(sequences[j].Name, FileMode.Open));
                        int len = (int)sequence.BaseStream.Length;
                        byte[] data = new byte[len];
                        sequence.Read(data, 0, len);
                        sequence.Close();

                        if (data[1] != 0x20)
                        {
                            data[1] = 0x20;
                        }

                        newentry.Size = len;
                        newentry.Data = data;
                    }
                }
                else
                {
                    newentry.Size = OldSeq[i].Size;
                    newentry.Data = OldSeq[i].Data;
                }
                NewSeq.Add(newentry);

                if (newentry.Data != null)
                {
                    NewAudioSeq = NewAudioSeq.Concat(newentry.Data).ToArray();
                }

                addr += newentry.Size;
            }

            if (addr > (RomData.MMFileList[4].End - RomData.MMFileList[4].Addr))
            {
                MMFile newa = new MMFile();
                newa.Addr = RomData.MMFileList[RomData.MMFileList.Count - 1].End;
                newa.End = newa.Addr + addr;
                newa.IsCompressed = false;
                newa.Data = NewAudioSeq;
                RomData.MMFileList.Add(newa);
                ResourceUtils.ApplyHack(Values.ModsDirectory + "reloc-audio");
                RomData.MMFileList[4].Data = new byte[0];
                RomData.MMFileList[4].Cmp_Addr = -1;
                RomData.MMFileList[4].Cmp_End = -1;
            }
            else
            {
                RomData.MMFileList[4].Data = NewAudioSeq;
            }

            //update pointer table
            f = RomUtils.GetFileIndexForWriting(AUDIO_SEQ_TABLE);
            for (int i = 0; i < 128; i++)
            {
                ReadWriteUtils.Arr_WriteU32(RomData.MMFileList[f].Data, AUDIO_SEQ_TABLE + (i * 16) - basea, (uint)NewSeq[i].Addr);
                ReadWriteUtils.Arr_WriteU32(RomData.MMFileList[f].Data, AUDIO_SEQ_TABLE + (i * 16) + 4 - basea, (uint)NewSeq[i].Size);
            }

            //update inst sets
            f = RomUtils.GetFileIndexForWriting(INSTRUMENT_SET_MAP);
            basea = RomData.MMFileList[f].Addr;
            for (int i = 0; i < 128; i++)
            {
                int paddr = (INSTRUMENT_SET_MAP - basea) + (i * 2) + 2;
                int j = -1;

                if (NewSeq[i].Size == 0)
                {
                    j = sequences.FindIndex(u => u.Replaces == NewSeq[i].Addr);
                }
                else
                {
                    j = sequences.FindIndex(u => u.Replaces == i);
                }

                if (j != -1)
                {
                    RomData.MMFileList[f].Data[paddr] = (byte)sequences[j].Instrument;
                }

            }
        }

    }
}
