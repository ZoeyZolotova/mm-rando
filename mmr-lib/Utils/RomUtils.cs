using MMRando.Models.Rom;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using MMRando.Utils.Mzxrules;
using System.Diagnostics;
using System.Security.Cryptography;
using MMRando.Models;

namespace MMRando.Utils
{

    public static class RomUtils
    {
        const string COMPRESSED_NOFLIP_SHA256 = "EFB1365B3AE362604514C0F9A1A2D11F5DC8688BA5BE660A37DEBF5E3BE43F2B";
        const string COMPRESSED_FLIP32_SHA256 = "0C1FC983FA5CC010CF6441C22388CEAA81ACE85FFB0F0028755B209876E58070";
        const string COMPRESSED_FLIP16_SHA256 = "8DC31559174F958A938AB7ECCB25DD310A4167F98CB68A521181F4653B684431";

        public const int COMPRESSED_FILE_SIZE = 0x200_0000;


        const int FILE_TABLE = 0x1A500;
        const int SIGNATURE_ADDRESS = 0x1A4D0;
        public static void SetStrings(string filename, string ver, string setting)
        {
            ResourceUtils.ApplyHack(filename);
            int veraddr = 0xC44E30;
            int settingaddr = 0xC44E70;
            string verstring = $"MM Rando {ver}\x00";
            string settingstring = $"{setting.Substring(0,9)}\x00";

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
            int start = RomData.MMFileList[RomData.MMFileList.Count - 1].End;
            MMFile newfile = new MMFile
            {
                Addr = start,
                IsCompressed = false,
                Data = buffer,
                End = start + buffer.Length
            };
            RomData.MMFileList.Add(newfile);
            return newfile.Addr;
        }

        public static int AddrToFile(int addr)
        {
            return RomData.MMFileList.FindIndex(
                file => addr >= file.Addr && addr < file.End);
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

        public static int GetFileIndexForWriting(int addr)
        {
            int index = AddrToFile(addr);
            CheckCompressed(index);
            return index;
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

        public static void CreatePatch(string filename, List<MMFile> originalMMFiles)
        {
            using (var filestream = File.Open(Path.ChangeExtension(filename, "mmr"), FileMode.Create))
            using (var compressStream = new GZipStream(filestream, CompressionMode.Compress))
            using (var writer = new BinaryWriter(compressStream))
            {
                for (var fileIndex = 0; fileIndex < RomData.MMFileList.Count; fileIndex++)
                {
                    var file = RomData.MMFileList[fileIndex];
                    if (file.Data == null || (file.IsCompressed && !file.WasEdited))
                    {
                        continue;
                    }
                    if (fileIndex >= originalMMFiles.Count)
                    {
                        writer.Write(ReadWriteUtils.Byteswap32((uint)fileIndex));
                        writer.Write(ReadWriteUtils.Byteswap32((uint)0));
                        writer.Write(ReadWriteUtils.Byteswap32((uint)file.Data.Length));
                        writer.Write(file.Data);
                        continue;
                    }
                    CheckCompressed(fileIndex, originalMMFiles);
                    var originalFile = originalMMFiles[fileIndex];
                    if (file.Data.Length != originalFile.Data.Length)
                    {
                        writer.Write(ReadWriteUtils.Byteswap32((uint)fileIndex));
                        writer.Write(-1);
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
                                writer.Write(ReadWriteUtils.Byteswap32((uint)modifiedIndex.Value));
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
        }

        public static void ApplyPatch(string filename)
        {
            using (var filestream = File.Open(filename, FileMode.Open))
            using (var compressStream = new GZipStream(filestream, CompressionMode.Decompress))
            using (var memoryStream = new MemoryStream())
            {
                compressStream.CopyTo(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new BinaryReader(memoryStream))
                {
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        var fileIndex = ReadWriteUtils.ReadS32(reader);
                        var index = ReadWriteUtils.ReadS32(reader);
                        var length = ReadWriteUtils.ReadS32(reader);
                        var data = reader.ReadBytes(length);
                        if (fileIndex >= RomData.MMFileList.Count)
                        {
                            var start = RomData.MMFileList[RomData.MMFileList.Count - 1].End;
                            var newFile = new MMFile
                            {
                                Addr = start,
                                IsCompressed = false,
                                Data = data,
                                End = start + data.Length
                            };
                            RomData.MMFileList.Add(newFile);
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
            }
        }

        public static byte[] BuildROM()
        {
            Parallel.ForEach(RomData.MMFileList, file =>
            {
                if (file.IsCompressed && file.WasEdited)
                {
                    byte[] result;
                    var newSize = Yaz.Encode(file.Data, file.Data.Length, out result);
                    if (newSize >= 0)
                    {
                        file.Data = new byte[newSize];
                        ReadWriteUtils.Arr_Insert(result, 0, newSize, file.Data, 0);
                    }
                }
            });
            byte[] ROM = new byte[COMPRESSED_FILE_SIZE];
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
                if (RomData.MMFileList[i].Cmp_Addr == -1)
                {
                    continue;
                }
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

        public static void LoadROM(BinaryReader baseROM, ValidateRomResult format)
        {
            if (format == ValidateRomResult.ValidFile)
            {
                ReadFileTable(baseROM);
            }
            else
            {
                byte[] rom = new byte[COMPRESSED_FILE_SIZE];
                baseROM.Read(rom, 0, COMPRESSED_FILE_SIZE);
                if (format == ValidateRomResult.Swap16)
                {
                    SwapByteOrder(rom, 2);
                }
                else if (format == ValidateRomResult.Swap32)
                {
                    SwapByteOrder(rom, 4);
                }

                using (BinaryReader br = new BinaryReader(new MemoryStream(rom)))
                {
                    ReadFileTable(br);
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

        public static ValidateRomResult ValidateROM(string filename)
        {
            if (!File.Exists(filename))
            {
                return ValidateRomResult.NoFile;
            }

            byte[] file;
            using (BinaryReader ROM = new BinaryReader(File.Open(filename, FileMode.Open, FileAccess.Read)))
            {
                if (ROM.BaseStream.Length != COMPRESSED_FILE_SIZE)
                {
                    return ValidateRomResult.InvalidFile;
                }

                file = ROM.ReadBytes(COMPRESSED_FILE_SIZE);
            }

            var hash = GetSha256Hash(file);
            if (hash.SequenceEqual(ToByteArray(COMPRESSED_NOFLIP_SHA256)))
            {
                return ValidateRomResult.ValidFile;
            }
            if (hash.SequenceEqual(ToByteArray(COMPRESSED_FLIP32_SHA256)))
            {
                return ValidateRomResult.Swap32;
            }
            if (hash.SequenceEqual(ToByteArray(COMPRESSED_FLIP16_SHA256)))
            {
                return ValidateRomResult.Swap16;
            }

            return ValidateRomResult.InvalidFile;
        }

        private static byte[] GetSha256Hash(byte[] file)
        {
            byte[] hash;
            using (var sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(file);
            }

            Debug.WriteLine(
                string.Concat(hash.Select(x => x.ToString("X2"))));

            return hash;
        }

        /// <summary>
        /// Swaps the byte ordering of a file
        /// </summary>
        /// <param name="input">The input byte array</param>
        /// <param name="flipsize">Size in bytes of each chunk that will be reversed. Must be a positive power of 2</param>
        /// <returns></returns>
        public static byte[] SwapByteOrder(byte[] input, int flipsize)
        {
            int xor = flipsize - 1;
            if (xor == 0)
            {
                return input;
            }

            byte[] output = new byte[input.Length];

            for(var i = 0; i < input.Length; i++)
            {
                output[i] = input[i ^ xor];
            }
            return output;
        }

        private static byte[] ToByteArray(string hexLiteral)
        {
            if (hexLiteral.Length %2 != 0)
            {
                throw new Exception("String contains an uneven number of characters");
            }

            byte[] result = new byte[hexLiteral.Length / 2];

            for (int i = 0; i < hexLiteral.Length; i += 2)
            {
                result[i/2] = Convert.ToByte(hexLiteral.Substring(i, 2), 16);
            }
            return result;
        }
    }

}