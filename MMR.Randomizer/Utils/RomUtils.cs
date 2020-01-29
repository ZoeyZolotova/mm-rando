using MMR.Randomizer.Models.Rom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using MMR.Randomizer.Utils.Mzxrules;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;

namespace MMR.Randomizer.Utils
{

    public static class RomUtils
    {
        const int FILE_TABLE = 0x1A500;
        const int SIGNATURE_ADDRESS = 0x1A4D0;
        public static void SetStrings(string filename, string ver, string setting)
        {
            ResourceUtils.ApplyHack(filename);
            int veraddr = 0xC44E30;
            int settingaddr = 0xC44E70;
            string verstring = $"MM Rando {ver}\x00";
            string settingstring = $"{setting}\x00";

            int f = GetFileIndexForWriting(veraddr);
            var file = RomData.MMFileList[f];

            byte[] buffer = Encoding.ASCII.GetBytes(verstring);
            int addr = veraddr - file.Addr;
            ReadWriteUtils.Arr_Insert(buffer, 0, buffer.Length, file.Data, addr);

            buffer = Encoding.ASCII.GetBytes(settingstring);
            addr = settingaddr - file.Addr;
            ReadWriteUtils.Arr_Insert(buffer, 0, buffer.Length, file.Data, addr);
        }

        public static int AddNewFile(string filename)
        {
            byte[] buffer;
            using (BinaryReader data = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                int len = (int)data.BaseStream.Length;
                buffer = new byte[len];
                data.Read(buffer, 0, len);
            }
            int index = RomUtils.AppendFile(buffer);
            return RomData.MMFileList[index].Addr;
        }

        public static int AddrToFile(int RAddr)
        {
            return RomData.MMFileList.FindIndex(
                file => RAddr >= file.Addr && RAddr < file.End);
        }

        public static void CheckCompressed(int fileIndex, List<MMFile> mmFileList = null)
        {
            if (mmFileList == null)
            {
                mmFileList = RomData.MMFileList;
            }
            var file = mmFileList[fileIndex];
            if (file.IsCompressed && !file.WasEdited)
            {
                using (var stream = new MemoryStream(file.Data))
                {
                    file.Data = Yaz.Decode(stream, file.Data.Length);
                }
                file.WasEdited = true;
            }
        }

        public static int GetFileIndexForWriting(int rAddr)
        {
            int index = AddrToFile(rAddr);
            CheckCompressed(index);
            return index;
        }

        public static int ByteswapROM(string filename)
        {
            using (BinaryReader ROM = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                if (ROM.BaseStream.Length % 4 != 0)
                {
                    return -1;
                }

                byte[] buffer = new byte[4];
                ROM.Read(buffer, 0, 4);
                // very hacky
                ROM.BaseStream.Seek(0, 0);
                if (buffer[0] == 0x80)
                {
                    return 1;
                }
                else if (buffer[1] == 0x80)
                {
                    using (BinaryWriter newROM = new BinaryWriter(File.Open(filename + ".z64", FileMode.Create)))
                    {
                        while (ROM.BaseStream.Position < ROM.BaseStream.Length)
                        {
                            newROM.Write(ReadWriteUtils.Byteswap16(ReadWriteUtils.ReadU16(ROM)));
                        }
                    }
                    return 0;
                }
                else if (buffer[3] == 0x80)
                {
                    using (BinaryWriter newROM = new BinaryWriter(File.Open(filename + ".z64", FileMode.Create)))
                    {
                        while (ROM.BaseStream.Position < ROM.BaseStream.Length)
                        {
                            newROM.Write(ReadWriteUtils.Byteswap32(ReadWriteUtils.ReadU32(ROM)));
                        }
                    }
                    return 0;
                }
            }
            return -1;
        }

        private static void UpdateFileTable(byte[] ROM)
        {
            for (int i = 0; i < RomData.MMFileList.Count; i++)
            {
                int offset = FILE_TABLE + (i * 16);
                ReadWriteUtils.Arr_WriteU32(ROM, offset, (uint)RomData.MMFileList[i].Addr);
                ReadWriteUtils.Arr_WriteU32(ROM, offset + 4, (uint)RomData.MMFileList[i].End);
                ReadWriteUtils.Arr_WriteU32(ROM, offset + 8, (uint)RomData.MMFileList[i].Cmp_Addr);
                ReadWriteUtils.Arr_WriteU32(ROM, offset + 12, (uint)RomData.MMFileList[i].Cmp_End);
            }
        }

        public static byte[] CreatePatch(string filename, List<MMFile> originalMMFiles)
        {
            var hashAlg = new SHA256Managed();
            using (var outStream = filename != null ? (Stream) File.Open(Path.ChangeExtension(filename, "mmr"), FileMode.Create) : new MemoryStream())
            using (var cryptoStream = new CryptoStream(outStream, hashAlg, CryptoStreamMode.Write))
            {
                using (var compressStream = new GZipStream(cryptoStream, CompressionMode.Compress))
                using (var writer = new BinaryWriter(compressStream))
                {
                    writer.Write(ReadWriteUtils.Byteswap32(PatchUtil.PATCH_MAGIC));
                    writer.Write(ReadWriteUtils.Byteswap32((uint)PatchUtil.PATCH_VERSION));
                    for (var fileIndex = 0; fileIndex < RomData.MMFileList.Count; fileIndex++)
                    {
                        var file = RomData.MMFileList[fileIndex];
                        var fileIsStatic = file.IsStatic ? 1 : 0;
                        if (file.Data == null || (file.IsCompressed && !file.WasEdited))
                        {
                            continue;
                        }
                        if (fileIndex >= originalMMFiles.Count)
                        {
                            writer.Write(ReadWriteUtils.Byteswap32((uint)fileIndex));
                            writer.Write(ReadWriteUtils.Byteswap32((uint)file.Addr));
                            writer.Write(ReadWriteUtils.Byteswap32((uint)0));
                            writer.Write(ReadWriteUtils.Byteswap32((uint)fileIsStatic));
                            writer.Write(ReadWriteUtils.Byteswap32((uint)file.Data.Length));
                            writer.Write(file.Data);
                            continue;
                        }
                        CheckCompressed(fileIndex, originalMMFiles);
                        var originalFile = originalMMFiles[fileIndex];
                        if (file.Data.Length != originalFile.Data.Length)
                        {
                            writer.Write(ReadWriteUtils.Byteswap32((uint)fileIndex));
                            writer.Write(ReadWriteUtils.Byteswap32((uint)file.Addr));
                            writer.Write(-1);
                            writer.Write(ReadWriteUtils.Byteswap32((uint)fileIsStatic));
                            writer.Write(ReadWriteUtils.Byteswap32((uint)file.Data.Length));
                            writer.Write(file.Data);
                            continue;
                        }
                        int? modifiedIndex = null;
                        var modifiedBuffer = new List<byte>();
                        for (var i = 0; i <= file.Data.Length; i++)
                        {
                            if (i == file.Data.Length || file.Data[i] == originalFile.Data[i])
                            {
                                if (modifiedBuffer.Any())
                                {
                                    writer.Write(ReadWriteUtils.Byteswap32((uint)fileIndex));
                                    writer.Write(ReadWriteUtils.Byteswap32((uint)file.Addr));
                                    writer.Write(ReadWriteUtils.Byteswap32((uint)modifiedIndex.Value));
                                    writer.Write(ReadWriteUtils.Byteswap32((uint)fileIsStatic));
                                    writer.Write(ReadWriteUtils.Byteswap32((uint)modifiedBuffer.Count));
                                    writer.Write(modifiedBuffer.ToArray());
                                    modifiedBuffer.Clear();
                                    modifiedIndex = null;
                                    continue;
                                }
                            }
                            else
                            {
                                if (!modifiedIndex.HasValue)
                                {
                                    modifiedIndex = i;
                                }
                                modifiedBuffer.Add(file.Data[i]);
                            }
                        }
                    }
                }
                return hashAlg.Hash;
            }
        }

        /// <summary>
        /// Applies the given filename patch to the in-memory RomData
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>SHA256 hash of the patch.</returns>
        public static byte[] ApplyPatch(string filename)
        {
            var hashAlg = new SHA256Managed();
            using (var filestream = File.Open(filename, FileMode.Open))
            using (var cryptoStream = new CryptoStream(filestream, hashAlg, CryptoStreamMode.Read))
            using (var decompressStream = new GZipStream(cryptoStream, CompressionMode.Decompress))
            using (var memoryStream = new MemoryStream())
            {
                decompressStream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new BinaryReader(memoryStream))
                {
                    var magic = ReadWriteUtils.ReadU32(reader);
                    var version = ReadWriteUtils.ReadU32(reader);

                    // Make sure this is a patch file by checking the magic value
                    if (magic != PatchUtil.PATCH_MAGIC)
                    {
                        throw new PatchMagicException(magic);
                    }

                    // Check that this patch version is supported
                    if (version != (uint)PatchUtil.PATCH_VERSION)
                    {
                        throw new PatchVersionException(PatchUtil.PATCH_VERSION, (PatchVersion)version);
                    }

                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var fileIndex = ReadWriteUtils.ReadS32(reader);
                        var fileAddr = ReadWriteUtils.ReadS32(reader);
                        var index = ReadWriteUtils.ReadS32(reader);
                        var isStatic = ReadWriteUtils.ReadS32(reader) != 0 ? true : false;
                        var length = ReadWriteUtils.ReadS32(reader);
                        var data = reader.ReadBytes(length);
                        if (fileIndex >= RomData.MMFileList.Count)
                        {
                            var newFile = new MMFile
                            {
                                Addr = fileAddr,
                                IsCompressed = false,
                                Data = data,
                                End = fileAddr + data.Length,
                                IsStatic = isStatic,
                            };
                            RomUtils.AppendFile(newFile);
                        }
                        if (index == -1)
                        {
                            RomData.MMFileList[fileIndex].Data = data;
                            if (data.Length == 0)
                            {
                                RomData.MMFileList[fileIndex].Cmp_Addr = -1;
                                RomData.MMFileList[fileIndex].Cmp_End = -1;
                            }
                        }
                        else
                        {
                            CheckCompressed(fileIndex);
                            ReadWriteUtils.Arr_Insert(data, 0, data.Length, RomData.MMFileList[fileIndex].Data, index);
                        }
                    }
                }

                return hashAlg.Hash;
            }
        }

        public static void WriteROM(string fileName, byte[] ROM)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Create)))
            {
                writer.Write(ROM, 0, ROM.Length);
            }
        }

        public static byte[] BuildROM()
        {
            Parallel.ForEach(RomData.MMFileList, file =>
            {
                if (file.IsCompressed && file.WasEdited)
                {
                    // lower priority so that the rando can't lock a badly scheduled CPU by using 100%
                    var previous_thread_priority = Thread.CurrentThread.Priority;
                    Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                    byte[] result;
                    var newSize = Yaz.Encode(file.Data, file.Data.Length, out result);
                    if (newSize >= 0)
                    {
                        file.Data = new byte[newSize];
                        ReadWriteUtils.Arr_Insert(result, 0, newSize, file.Data, 0);
                    }
                    // this thread is borrowed, we don't want it to always be the lowest priority, return to previous state
                    Thread.CurrentThread.Priority = previous_thread_priority;
                }
            });
            byte[] ROM = new byte[0x2000000];
            int ROMAddr = 0;
            for (int i = 0; i < RomData.MMFileList.Count; i++)
            {
                if (RomData.MMFileList[i].Cmp_Addr == -1)
                {
                    continue;
                }
                RomData.MMFileList[i].Cmp_Addr = ROMAddr;
                int file_len = RomData.MMFileList[i].Data.Length;
                if (RomData.MMFileList[i].IsCompressed)
                {
                    RomData.MMFileList[i].Cmp_End = ROMAddr + file_len;
                }
                ReadWriteUtils.Arr_Insert(RomData.MMFileList[i].Data, 0, file_len, ROM, ROMAddr);
                ROMAddr += file_len;
            }
            UpdateFileTable(ROM);
            SignROM(ROM);
            FixCRC(ROM);

            return ROM;
        }

        private static void SignROM(byte[] ROM)
        {
            string VersionString = "MajoraRando"; // ??????
            string DateString = DateTime.UtcNow.ToString("yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);
            for (int i = 0; i < VersionString.Length; i++)
            {
                ROM[SIGNATURE_ADDRESS + i] = (byte)VersionString[i];
            }
            for (int i = 0; i < DateString.Length; i++)
            {
                ROM[SIGNATURE_ADDRESS + i + 12] = (byte)DateString[i];
            }
        }

        private static void FixCRC(byte[] ROM)
        {
            // reference: http://n64dev.org/n64crc.html
            uint[] CRC = new uint[2];
            uint seed = 0xDF26F436;
            uint t1, t2, t3, t4, t5, t6, r, d;
            int i = 0x1000;
            t1 = t2 = t3 = t4 = t5 = t6 = seed;
            while (i < 0x101000)
            {
                d = ReadWriteUtils.Arr_ReadU32(ROM, i);
                if ((t6 + d) < t6) { t4++; }
                t6 += d;
                t3 ^= d;
                r = (d << (byte)(d & 0x1F)) | (d >> (byte)(32 - (d & 0x1F)));
                t5 += r;
                if (t2 < d)
                {
                    t2 ^= (t6 ^ d);
                }
                else
                {
                    t2 ^= r;
                }
                t1 += (ReadWriteUtils.Arr_ReadU32(ROM, 0x750 + (i & 0xFF)) ^ d);
                i += 4;
            }
            CRC[0] = t6 ^ t4 ^ t3;
            CRC[1] = t5 ^ t2 ^ t1;
            ReadWriteUtils.Arr_WriteU32(ROM, 16, CRC[0]);
            ReadWriteUtils.Arr_WriteU32(ROM, 20, CRC[1]);
        }

        private static void ExtractAll(BinaryReader ROM)
        {
            for (int i = 0; i < RomData.MMFileList.Count; i++)
            {
                if (RomData.MMFileList[i].Cmp_Addr == -1) { continue; }
                ROM.BaseStream.Seek(RomData.MMFileList[i].Cmp_Addr, 0);
                if (RomData.MMFileList[i].IsCompressed)
                {
                    byte[] CmpFile = new byte[RomData.MMFileList[i].Cmp_End - RomData.MMFileList[i].Cmp_Addr];
                    ROM.Read(CmpFile, 0, CmpFile.Length);
                    RomData.MMFileList[i].Data = CmpFile;
                }
                else
                {
                    var buffer = new byte[RomData.MMFileList[i].End - RomData.MMFileList[i].Addr];
                    ROM.Read(buffer, 0, buffer.Length);
                    RomData.MMFileList[i].Data = buffer;
                }
            }
        }

        public static void ReadFileTable(BinaryReader ROM)
        {
            RomData.MMFileList = new List<MMFile>();
            ROM.BaseStream.Seek(FILE_TABLE, SeekOrigin.Begin);
            while (true)
            {
                MMFile Current_File = new MMFile
                {
                    Addr = ReadWriteUtils.ReadS32(ROM),
                    End = ReadWriteUtils.ReadS32(ROM),
                    Cmp_Addr = ReadWriteUtils.ReadS32(ROM),
                    Cmp_End = ReadWriteUtils.ReadS32(ROM)
                };
                Current_File.IsCompressed = Current_File.Cmp_End != 0;
                if (Current_File.Addr == Current_File.End)
                {
                    break;
                }
                RomData.MMFileList.Add(Current_File);
            }
            ExtractAll(ROM);
        }

        public static bool CheckOldCRC(BinaryReader ROM)
        {
            ROM.BaseStream.Seek(16, 0);
            uint CRC1 = ReadWriteUtils.ReadU32(ROM);
            uint CRC2 = ReadWriteUtils.ReadU32(ROM);
            return (CRC1 == 0x5354631C) && (CRC2 == 0x03A2DEF0);
        }

        public static bool ValidateROM(string FileName)
        {
            bool res = false;
            using (BinaryReader ROM = new BinaryReader(File.Open(FileName, FileMode.Open, FileAccess.Read)))
            {
                if (ROM.BaseStream.Length == 0x2000000)
                {
                    res = CheckOldCRC(ROM);
                }
            }
            return res;
        }

        /// <summary>
        /// Get the index of the tail-most <see cref="MMFile"/> which does not use a static virtual address.
        /// </summary>
        /// <returns>Index</returns>
        public static int GetTailFileIndex()
        {
            var index = RomData.MMFileList.FindLastIndex(file => !file.IsStatic);
            var result = index >= 0 ? (int?)index : (int?)null;
            return result.Value;
        }

        /// <summary>
        /// Append a <see cref="MMFile"/> without a static virtual address to the end of the list.
        /// </summary>
        /// <param name="data">File data</param>
        /// <param name="isCompressed">Is file compressed</param>
        /// <returns>File index</returns>
        public static int AppendFile(byte[] data, bool isCompressed = false)
        {
            var index = GetTailFileIndex();
            var tail = RomData.MMFileList[index];
            return AppendFile(tail.End, data, isCompressed);
        }

        /// <summary>
        /// Append a <see cref="MMFile"/> to the list.
        /// </summary>
        /// <param name="addr">File address</param>
        /// <param name="data">File data</param>
        /// <param name="isCompressed">Is file compressed</param>
        /// <param name="isStatic">Is file address static</param>
        /// <returns>File index</returns>
        public static int AppendFile(int addr, byte[] data, bool isCompressed = false, bool isStatic = false)
        {
            var file = new MMFile
            {
                Addr = addr,
                End = addr + data.Length,
                IsCompressed = isCompressed,
                Data = data,
                IsStatic = isStatic,
            };

            return AppendFile(file);
        }

        /// <summary>
        /// Append a <see cref="MMFile"/> to the list.
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>File index</returns>
        public static int AppendFile(MMFile file)
        {
            if (!file.IsStatic)
            {
                // Insert before static files
                var index = GetTailFileIndex() + 1;
                RomData.MMFileList.Insert(index, file);
                return index;
            }
            else
            {
                RomData.MMFileList.Add(file);
                return RomData.MMFileList.Count - 1;
            }
        }
    }

}