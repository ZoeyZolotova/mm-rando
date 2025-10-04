using MMR.Common.Utils;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace MMR.Common.Tests.Utils
{
    public class BitPackerTests
    {
        [Test]
        public void TestBooleans()
        {
            var bitPacker = new BitPacker();
            bitPacker.Write(false);
            bitPacker.Write(false);
            bitPacker.Write(true);
            bitPacker.Write(false);
            bitPacker.Write(false);
            bitPacker.Write(true);
            bitPacker.Write(false);
            bitPacker.Write(false);

            var result = bitPacker.ToByteArray();
            CollectionAssert.AreEquivalent(new byte[] { 0b00100100 }, result);

            var bitUnpacker = new BitUnpacker(result);

            ClassicAssert.AreEqual(false, bitUnpacker.ReadBool());
            ClassicAssert.AreEqual(false, bitUnpacker.ReadBool());
            ClassicAssert.AreEqual(true, bitUnpacker.ReadBool());
            ClassicAssert.AreEqual(false, bitUnpacker.ReadBool());
            ClassicAssert.AreEqual(false, bitUnpacker.ReadBool());
            ClassicAssert.AreEqual(true, bitUnpacker.ReadBool());
            ClassicAssert.AreEqual(false, bitUnpacker.ReadBool());
            ClassicAssert.AreEqual(false, bitUnpacker.ReadBool());
        }

        [Test]
        public void TestBytesWithSize()
        {
            var bitPacker = new BitPacker();
            bitPacker.Write(1, 3);
            bitPacker.Write(1, 2);
            bitPacker.Write(1, 1);
            bitPacker.Write(1, 2);

            var result = bitPacker.ToByteArray();
            CollectionAssert.AreEquivalent(new byte[] { 0b00101101 }, result);

            var bitUnpacker = new BitUnpacker(result);

            ClassicAssert.AreEqual(1, bitUnpacker.ReadS32(3));
            ClassicAssert.AreEqual(1, bitUnpacker.ReadS32(2));
            ClassicAssert.AreEqual(1, bitUnpacker.ReadS32(1));
            ClassicAssert.AreEqual(1, bitUnpacker.ReadS32(2));
        }

        [Test]
        public void TestMultipleBytesWithSize()
        {
            var bitPacker = new BitPacker();
            bitPacker.Write(1, 3);
            bitPacker.Write(1, 3);
            bitPacker.Write(1, 3);
            bitPacker.Write(1, 3);

            var result = bitPacker.ToByteArray();
            CollectionAssert.AreEquivalent(new byte[] { 0b00100100, 0b10010000 }, result);

            var bitUnpacker = new BitUnpacker(result);

            ClassicAssert.AreEqual(1, bitUnpacker.ReadS32(3));
            ClassicAssert.AreEqual(1, bitUnpacker.ReadS32(3));
            ClassicAssert.AreEqual(1, bitUnpacker.ReadS32(3));
            ClassicAssert.AreEqual(1, bitUnpacker.ReadS32(3));
        }

    }
}
