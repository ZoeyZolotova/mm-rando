using Be.IO;
using MMR.Common.Extensions;
using MMR.Randomizer.Models.Rom;
using MMR.Randomizer.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using VCDiff.Decoders;
using VCDiff.Encoders;
using VCDiff.Includes;

namespace MMR.Randomizer.Patch
{
    public static class Patcher
    {
        /// <summary>
        /// Patch file magic number ("MMRP").
        /// </summary>
        public const uint PatchMagic = 0x4D4D5250;

        private static readonly byte[] key;
        private static readonly byte[] iv;

        static Patcher()
        {
            var bigInt = new BigInteger(typeof(Patcher).Assembly.ManifestModule.ModuleVersionId.ToByteArray());
            var random = new Random((int)(bigInt & int.MaxValue));
            var buffer = new byte[16];
            random.NextBytes(buffer);
            key = buffer.ToArray();
            random.NextBytes(buffer);
            iv = buffer.ToArray();
        }

        /// <summary>
        /// Apply a patch entry.
        /// </summary>
        /// <param name="header">Patch entry header.</param>
        /// <param name="data">Patch entry data.</param>
        static void ApplyPatchEntry(PatchHeader header, byte[] data)
        {
            var address = (int)header.Address;
            var index = (int)header.Index;
            if (header.Command == PatchCommand.NewFile)
            {
                var newFile = new MMFile
                {
                    Addr = address,
                    IsCompressed = false,
                    Data = data,
                    End = address + data.Length,
                    IsStatic = header.Flags.HasFlag(PatchFlags.IsStatic),
                };
                RomUtils.AppendFile(newFile);
            }
            else if (header.Command == PatchCommand.ExistingFile)
            {
                RomUtils.CheckCompressed(index);
                var original = RomData.MMFileList[index];
                original.Data = VcDiffDecodeManaged(original.Data, data);
                if (original.Data.Length == 0)
                {
                    original.Cmp_Addr = -1;
                    original.Cmp_End = -1;
                }
            }
        }

        /// <summary>
        /// Apply patch data from file at given path to the ROM.
        /// </summary>
        /// <param name="filePath">Patch file path.</param>
        /// <returns><see cref="SHA256"/> hash of the patch.</returns>
        public static byte[] ApplyPatch(string filePath)
        {
            using var outStream = File.OpenRead(filePath);
            return ApplyPatch(outStream);
        }

        /// <summary>
        /// Apply patch data from given <see cref="Stream"/> to the ROM.
        /// </summary>
        /// <param name="inStream">Input stream.</param>
        /// <returns><see cref="SHA256"/> hash of the patch.</returns>
        public static byte[] ApplyPatch(Stream inStream)
        {
            try
            {
                var aes = Aes.Create();
                var hashAlg = SHA256.Create();
                using (var cryptoStream = new CryptoStream(inStream, aes.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                using (var hashStream = new CryptoStream(cryptoStream, hashAlg, CryptoStreamMode.Read))
                using (var decompressStream = new GZipStream(hashStream, CompressionMode.Decompress))
                using (var memoryStream = new MemoryStream())
                {
                    // Fully decompress into MemoryStream so that we can access Position to check for end of Stream.
                    decompressStream.CopyTo(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin);
                    using var reader = new BeBinaryReader(memoryStream);

                    // Validate patch magic.
                    var magic = reader.ReadUInt32();
                    ValidateMagic(magic);

                    Span<byte> headerBytes = stackalloc byte[PatchHeader.Size];
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        // Read header bytes into stack buffer to prevent allocation.
                        reader.ReadExact(headerBytes);
                        var header = PatchHeader.Read(headerBytes);
                        var data = reader.ReadBytes(header.Length);
                        ApplyPatchEntry(header, data);
                    }
                }
                return hashAlg.Hash;
            }
            catch
            {
                throw new IOException("Failed to apply patch. Patch may be invalid.");
            }
        }

        /// <summary>
        /// Create hash of patch data from current ROM state.
        /// </summary>
        /// <param name="originalMMFiles">Original <see cref="MMFile"/> collection.</param>
        /// <returns><see cref="SHA256"/> hash of the patch.</returns>
        public static byte[] CreatePatch(List<MMFile> originalMMFiles)
        {
            return CreatePatch(Stream.Null, originalMMFiles);
        }

        /// <summary>
        /// Create patch data from current ROM state and write to a file.
        /// </summary>
        /// <param name="filePath">Output file path.</param>
        /// <param name="originalMMFiles">Original <see cref="MMFile"/> collection.</param>
        /// <returns><see cref="SHA256"/> hash of the patch.</returns>
        public static byte[] CreatePatch(string filePath, List<MMFile> originalMMFiles)
        {
            using var outStream = File.Open(filePath, FileMode.Create);
            return CreatePatch(outStream, originalMMFiles);
        }

        /// <summary>
        /// Create patch data from current ROM state and write to <see cref="Stream"/>.
        /// </summary>
        /// <param name="outStream">Output stream.</param>
        /// <param name="originalMMFiles">Original <see cref="MMFile"/> collection.</param>
        /// <returns><see cref="SHA256"/> hash of the patch.</returns>
        public static byte[] CreatePatch(Stream outStream, List<MMFile> originalMMFiles)
        {
            var aes = Aes.Create();
            var hashAlg = SHA256.Create();
            using (var cryptoStream = new CryptoStream(outStream, aes.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            using (var hashStream = new CryptoStream(cryptoStream, hashAlg, CryptoStreamMode.Write))
            using (var compressStream = new GZipStream(hashStream, CompressionMode.Compress))
            using (var writer = new BeBinaryWriter(compressStream))
            {
                // Write magic value.
                writer.WriteUInt32(PatchMagic);

                Span<byte> headerBytes = stackalloc byte[PatchHeader.Size];
                for (var fileIndex = 0; fileIndex < RomData.MMFileList.Count; fileIndex++)
                {
                    var file = RomData.MMFileList[fileIndex];

                    // Check whether file should be included in the patch.
                    if (file.Data == null || (file.IsCompressed && !file.WasEdited))
                    {
                        continue;
                    }

                    if (fileIndex >= originalMMFiles.Count)
                    {
                        var index = (uint)fileIndex;
                        var address = (uint)file.Addr;

                        // Create header for appending new file.
                        var header = PatchHeader.CreateNew(index, address, file.Data.Length, file.IsStatic);
                        header.Write(headerBytes);

                        // Write header bytes and file contents.
                        writer.Write(headerBytes);
                        writer.Write(file.Data);
                    }
                    else
                    {
                        RomUtils.CheckCompressed(fileIndex, originalMMFiles);
                        var originalFile = originalMMFiles[fileIndex];

                        var index = (uint)fileIndex;
                        var address = (uint)file.Addr;
                        var diff = VcDiffEncodeManaged(originalFile.Data, file.Data);

                        // Create header for patching existing file.
                        var header = PatchHeader.CreateExisting(index, address, diff.Length, file.IsStatic);
                        header.Write(headerBytes);

                        // Write header bytes and diff bytes.
                        writer.Write(headerBytes);
                        writer.Write(diff);
                    }
                }
            }
            return hashAlg.Hash;
        }

        /// <summary>
        /// Validate magic value and throw a <see cref="PatchMagicException"/> if invalid.
        /// </summary>
        /// <param name="magic">Magic value</param>
        static void ValidateMagic(uint magic)
        {
            if (magic != PatchMagic)
            {
                throw new PatchMagicException(magic);
            }
        }

        /// <summary>
        /// Perform VCDiff decode (apply a diff).
        /// </summary>
        /// <param name="original">Original file data.</param>
        /// <param name="patch">Diff data.</param>
        /// <returns>Modified file data.</returns>
        static byte[] VcDiffDecodeManaged(byte[] original, byte[] patch)
        {
            using var output = new MemoryStream();
            using var dict = new MemoryStream(original);
            using var target = new MemoryStream(patch);

            // Decode using patch data.
            var decoder = new VcDecoder(dict, target, output);
            if (decoder.Decode(out var written) != VCDiffResult.SUCCESS)
            {
                throw new Exception("VCDiff decode failed.");
            }
            return output.ToArray();
        }

        /// <summary>
        /// Perform VCDiff encode (create a diff).
        /// </summary>
        /// <param name="original">Original file data.</param>
        /// <param name="modified">Modified file data.</param>
        /// <returns>Diff data.</returns>
        static byte[] VcDiffEncodeManaged(byte[] original, byte[] modified)
        {
            using var output = new MemoryStream();
            using var dict = new MemoryStream(original);
            using var target = new MemoryStream(modified);

            // Encode and write patch data.
            var encoder = new VcEncoder(dict, target, output);
            if (encoder.Encode(false) != VCDiffResult.SUCCESS)
            {
                throw new Exception("VCDiff encode failed.");
            }
            return output.ToArray();
        }
    }
}
