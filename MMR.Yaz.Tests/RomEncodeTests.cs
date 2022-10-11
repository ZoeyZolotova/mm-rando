using MMR.Rom;
using NUnit.Framework;
using System.Linq;

namespace MMR.Yaz.Tests
{
    public class RomEncodeTests
    {
        const string RomFilename = "Rom.z64";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestEncoding()
        {
            var rom = RomFile.From(RomFilename);
            var compressedFiles = rom.Files.Where(x => x.IsCompressed).ToArray();
            foreach (var entry in compressedFiles)
            {
                PerformCompression(rom, entry);
            }
        }

        static void PerformCompression(RomFile rom, VirtualFile entry)
        {
            var slice = rom.GetSpan(entry);
            var decoded = Yaz.Decode(slice);
            var encoded = Yaz.EncodeWithHeader(decoded, slice); // Yaz.EncodeAndCopy(decoded);
            var aligned = Yaz.AlignTo16(encoded);
            // Currently only compares compressed lengths, as compressed output is likely slightly different due to optimization.
            Assert.AreEqual(slice.Length, aligned);
        }
    }
}
