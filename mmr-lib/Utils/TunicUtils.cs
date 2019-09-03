using System;
using System.Collections.Generic;
using System.Drawing;

namespace MMRando.Utils
{

    public class TunicUtils
    {

        static int[] sizes = new int[] { 14, 128, 33, 512, 16 };
        static bool[] grad = new bool[] { false, false, false, true, false };
        static bool[] zora = new bool[] { false, false, true, true, false };
        static bool[] fd = new bool[] { false, false, false, false, true };

        private static float Interpolate(float y0, float y1, float x0, float x1, float x)
        {
            if (x0 == x1)
            {
                return y0;
            }
            else
            {
                return y0 * ((x1 - x) / (x1 - x0)) + y1 * ((x - x0) / (x1 - x0));
            }
        }

        private static ushort ToRGBA5551(Color c)
        {
            byte r = (byte)((c.R >> 3) & 0x1F);
            byte g = (byte)((c.G >> 3) & 0x1F);
            byte b = (byte)((c.B >> 3) & 0x1F);
            byte a;
            if (c.A > 0x7F)
            {
                a = 1;
            }
            else
            {
                a = 0;
            }
            return (ushort)((r << 11) | (g << 6) | (b << 1) | a);
        }

        private static Color FromRGBA5551(ushort c)
        {
            float r = (c & 0xF800) >> 11;
            float g = (c & 0x7C0) >> 6;
            float b = (c & 0x3E) >> 1;
            int a = (c & 1) * 255;
            return Color.FromArgb(a, (int)(r * 255.0f / 31.0f), (int)(g * 255.0f / 31.0f), (int)(b * 255.0f / 31.0f));
        }

        private static Color GetAverageColour(Color[] c, int count)
        {
            float r = 0f;
            float g = 0f;
            float b = 0f;
            float d = 1.0f / count;
            for (int i = 0; i < count; i++)
            {
                r += c[i].R * d;
                g += c[i].G * d;
                b += c[i].B * d;
            }
            return Color.FromArgb((int)Math.Round(r), (int)Math.Round(g), (int)Math.Round(b));
        }

        private static Color[] ShiftHue(Color[] c, Color target, int count, bool zora, bool grad, bool fd)
        {
            Color[] s = new Color[count];
            Color a;
            if (zora && !grad)
            {
                a = GetAverageColour(c, 16);
            }
            else
            {
                a = GetAverageColour(c, count);
            }
            float rot = target.GetHue() - a.GetHue();
            float avgb = a.GetBrightness();
            float avgs = a.GetSaturation();
            for (int i = 0; i < count; i++)
            {
                if ((i == 12) && (count == 14))
                {
                    s[i] = c[i];
                    continue;
                }
                float h = c[i].GetHue();
                float b = c[i].GetBrightness();
                float sat = c[i].GetSaturation();
                b -= avgb;
                sat -= avgs;
                b += target.GetBrightness();
                sat += target.GetSaturation();
                h += rot;
                if (fd)
                {
                    sat = target.GetSaturation();
                    h = target.GetHue();
                }
                if (zora && grad)
                {
                    if (i > 351)
                    {
                        float x0 = c[352].GetBrightness();
                        float x1 = c[511].GetBrightness();
                        float x = c[i].GetBrightness();
                        h = target.GetHue();
                        sat = target.GetSaturation();
                        b = Interpolate(target.GetBrightness(), c[511].GetBrightness(), x0, x1, x);
                    }
                }
                h %= 360f;
                if (h < 0f)
                {
                    h += 360f;
                }
                if (b < 0.0f)
                {
                    b = 0.0f;
                }
                if (b > 1.0f)
                {
                    b = 1.0f;
                }
                if (sat < 0.0f)
                {
                    sat = 0.0f;
                }
                if (sat > 1.0f)
                {
                    sat = 1.0f;
                }
                s[i] = ColorUtils.FromAHSB(c[i].A, h, sat, b);
                //this code is a mess
                if (zora && grad)
                {
                    if (i < 96)
                    {
                        s[i] = c[i];
                    }
                    else if (i < 352)
                    {
                        float x0 = c[95].GetBrightness();
                        float x1 = c[352].GetBrightness();
                        float x = c[i].GetBrightness();
                        int rr = (int)Interpolate(c[95].R, target.R, x0, x1, x);
                        int gg = (int)Interpolate(c[95].G, target.G, x0, x1, x);
                        int bb = (int)Interpolate(c[95].B, target.B, x0, x1, x);
                        if (rr < 0)
                        {
                            rr = 0;
                        }
                        if (rr > 255)
                        {
                            rr = 255;
                        }
                        if (gg < 0)
                        {
                            gg = 0;
                        }
                        if (gg > 255)
                        {
                            gg = 255;
                        }
                        if (bb < 0)
                        {
                            bb = 0;
                        }
                        if (bb > 255)
                        {
                            bb = 255;
                        }
                        s[i] = Color.FromArgb(rr, gg, bb);
                    }
                }
                else if (zora)
                {
                    if (i > 15)
                    {
                        float x0 = c[14].GetBrightness();
                        float x1 = c[31].GetBrightness();
                        float x = c[i].GetBrightness();
                        int rr = (int)Interpolate(s[14].R, c[31].R, x0, x1, x);
                        int gg = (int)Interpolate(s[14].G, c[31].G, x0, x1, x);
                        int bb = (int)Interpolate(s[14].B, c[31].B, x0, x1, x);
                        if (rr < 0)
                        {
                            rr = 0;
                        }
                        if (rr > 255)
                        {
                            rr = 255;
                        }
                        if (gg < 0)
                        {
                            gg = 0;
                        }
                        if (gg > 255)
                        {
                            gg = 255;
                        }
                        if (bb < 0)
                        {
                            bb = 0;
                        }
                        if (bb > 255)
                        {
                            bb = 255;
                        }
                        s[i] = Color.FromArgb(rr, gg, bb);
                    }
                }
            }
            return s;
        }

        private static Color[] ReadColours(int file, int addr, int count)
        {
            Color[] c = new Color[count];
            for (int i = 0; i < count; i++)
            {
                int ca = addr + (i * 2);
                ushort rgba = (ushort)((RomData.MMFileList[file].Data[ca] << 8) 
                    | RomData.MMFileList[file].Data[ca + 1]);
                c[i] = FromRGBA5551(rgba);
            }
            return c;
        }

        private static void WriteColours(int file, int addr, int count, Color[] c)
        {
            for (int i = 0; i < count; i++)
            {
                int ca = addr + (i * 2);
                ushort rgba = ToRGBA5551(c[i]);
                var data = new byte[]
                {
                    (byte)(rgba >> 8),
                    (byte)(rgba & 0xFF),
                };
                ReadWriteUtils.Arr_Insert(data, 0, data.Length, RomData.MMFileList[file].Data, ca);
            }
        }

        public static void UpdateFormTunics(List<int[]> addresses, Color target)
        {
            for (int i = 0; i < addresses.Count; i++)
            {
                for (int j = 0; j < addresses[i].Length; j++)
                {
                    int f = RomUtils.GetFileIndexForWriting(addresses[i][j]);
                    int a = addresses[i][j] - RomData.MMFileList[f].Addr;
                    Color[] c = ReadColours(f, a, sizes[i]);
                    c = ShiftHue(c, target, sizes[i], zora[i], grad[i], fd[i]);
                    WriteColours(f, a, sizes[i], c);
                }
            }
        }

    }

}