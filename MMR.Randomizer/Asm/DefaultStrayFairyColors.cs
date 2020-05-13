using System;
using System.Drawing;

namespace MMR.Randomizer.Asm
{
    internal static class DefaultStrayFairyColors
    {
        public static readonly ExtendedObjects.StrayFairyColors ClockTown = new ExtendedObjects.StrayFairyColors
        {
            OuterPrimColor = Color.FromArgb(0xFF, 0xA5, 0x00),
            InnerPrimColor = Color.FromArgb(0xFF, 0xFF, 0xDC),
            InnerEnvColor = Color.FromArgb(0xFF, 0x80, 0x00),
        };

        public static readonly ExtendedObjects.StrayFairyColors Woodfall = new ExtendedObjects.StrayFairyColors
        {
            OuterPrimColor = Color.FromArgb(0xFF, 0x69, 0xB4),
            InnerPrimColor = Color.FromArgb(0xFF, 0xDC, 0xFF),
            InnerEnvColor = Color.FromArgb(0xFF, 0x00, 0x64),
        };

        public static readonly ExtendedObjects.StrayFairyColors Snowhead = new ExtendedObjects.StrayFairyColors
        {
            OuterPrimColor = Color.FromArgb(0x00, 0xC0, 0x00),
            InnerPrimColor = Color.FromArgb(0xDC, 0xFF, 0xFF),
            InnerEnvColor = Color.FromArgb(0x00, 0xFF, 0x32),
        };

        public static readonly ExtendedObjects.StrayFairyColors GreatBay = new ExtendedObjects.StrayFairyColors
        {
            OuterPrimColor = Color.FromArgb(0x18, 0x74, 0xCD),
            InnerPrimColor = Color.FromArgb(0xDC, 0xFF, 0xFF),
            InnerEnvColor = Color.FromArgb(0x00, 0x64, 0xFF),
        };

        public static readonly ExtendedObjects.StrayFairyColors StoneTower = new ExtendedObjects.StrayFairyColors
        {
            OuterPrimColor = Color.FromArgb(0xFF, 0xFF, 0x00),
            InnerPrimColor = Color.FromArgb(0xFF, 0xFF, 0xDC),
            InnerEnvColor = Color.FromArgb(0xFF, 0xFF, 0x00),
        };
    }

    internal static class RandomStrayFairColors
    {
        private static readonly Random Rng = new Random();

        public static ExtendedObjects.StrayFairyColors Generate()
        {
            byte[] randomBytes = new byte[9];
            Rng.NextBytes(randomBytes);

            return new ExtendedObjects.StrayFairyColors
            {
                OuterPrimColor = Color.FromArgb(randomBytes[0], randomBytes[1], randomBytes[2]),
                InnerPrimColor = Color.FromArgb(randomBytes[3], randomBytes[4], randomBytes[5]),
                InnerEnvColor = Color.FromArgb(randomBytes[6], randomBytes[7], randomBytes[8]),
            };
        }
    }
}
