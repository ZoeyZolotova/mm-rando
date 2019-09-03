using System.Collections.Generic;

namespace MMRando.Utils
{

    public class Yaz0Utils
    {

        //compression
        // reference: https://github.com/aboood40091/libyaz0

        public static int FindMatch(byte[] src, byte b, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                if (src[i] == b)
                {
                    return i;
                }
            }
            return -1;
        }

        public static int[] Search(byte[] src, int pos, int end)
        {
            int[] found = { 0, 1 };
            if ((pos + 2) < end)
            {
                int search = pos - 0x1000;
                if (search < 0) { search = 0; };
                int cmp_end = pos + 0x111;
                if (cmp_end > end) { cmp_end = end; };
                while (search < pos)
                {
                    search = FindMatch(src, src[pos], search, pos);
                    if (search == -1) { break; };
                    int cmp1 = search + 1;
                    int cmp2 = pos + 1;
                    while ((cmp2 < cmp_end) && (src[cmp1] == src[cmp2]))
                    {
                        cmp1++;
                        cmp2++;
                    }
                    int len = cmp2 - pos;
                    if (found[1] < len)
                    {
                        found[1] = len;
                        found[0] = search;
                        if (found[1] == 0x111)
                        {
                            break;
                        }
                    }
                    search++;
                }
            }
            return found;
        }

        public static int[] LA(byte[] src, int pos, int end, ref int[] LAfound, ref bool LAprev)
        {
            if (LAprev)
            {
                LAprev = false;
                return LAfound;
            }
            LAprev = false;
            int[] found = Search(src, pos, end);
            if (found[1] > 2)
            {
                LAfound = Search(src, pos + 1, end);
                if (LAfound[1] > found[1] + 2)
                {
                    found[1] = 1;
                    LAprev = true;
                }
            }
            return found;
        }

        public static byte[] Compress(byte[] src)
        {
            int[] LAfound = { 0, 1 };
            bool LAprev = false;
            int src_pos = 0;
            int src_end = src.Length;
            List<byte> dest = new List<byte>();
            //header
            dest.Add((byte)0x59);
            dest.Add((byte)0x61);
            dest.Add((byte)0x7A);
            dest.Add((byte)0x30);
            dest.Add((byte)((src_end & 0xFF000000) >> 24));
            dest.Add((byte)((src_end & 0xFF0000) >> 16));
            dest.Add((byte)((src_end & 0xFF00) >> 8));
            dest.Add((byte)(src_end & 0xFF));
            for (int i = 0; i < 8; i++)
            {
                dest.Add((byte)0);
            }
            //do compression
            while (src_pos < src_end)
            {
                int code_byte_pos = dest.Count;
                dest.Add((byte)0);
                for (int i = 0; i < 8; i++)
                {
                    if (src_pos >= src_end) { break; };
                    int[] found = LA(src, src_pos, src_end, ref LAfound, ref LAprev);
                    if (found[1] > 2)
                    {
                        int delta = src_pos - found[0] - 1;
                        if (found[1] < 0x12)
                        {
                            dest.Add((byte)((delta >> 8) | ((found[1] - 2) << 4)));
                            dest.Add((byte)(delta & 0xFF));
                        }
                        else
                        {
                            dest.Add((byte)(delta >> 8));
                            dest.Add((byte)(delta & 0xFF));
                            dest.Add((byte)((found[1] - 0x12) & 0xFF));
                        }
                        src_pos += found[1];
                    }
                    else
                    {
                        dest[code_byte_pos] |= (byte)(1 << (7 - i));
                        dest.Add(src[src_pos]);
                        src_pos++;
                    }
                }
            }
            while (dest.Count % 16 != 0)
            {
                dest.Add((byte)0);
            }
            return dest.ToArray();
        }

        public static byte[] Decompress(byte[] CmpFile)
        {
            if (ReadWriteUtils.Arr_ReadU32(CmpFile, 0) != 0x59617A30)
            {
                return null;
            }

            int dest_len = (int)ReadWriteUtils.Arr_ReadU32(CmpFile, 4);
            byte[] dest = new byte[dest_len];
            int src_pos = 16;
            int src_len = CmpFile.Length;
            int dest_pos = 0;
            while ((src_pos < src_len) && (dest_pos < dest_len))
            {
                byte code = CmpFile[src_pos];
                src_pos++;
                for (int i = 0; i < 8; i++)
                {
                    if ((src_pos >= src_len) || (dest_pos >= dest_len))
                    {
                        break;
                    };
                    if ((code & 0x80) > 0)
                    {
                        dest[dest_pos] = CmpFile[src_pos];
                        dest_pos++;
                        src_pos++;
                    }
                    else
                    {
                        byte b1 = CmpFile[src_pos];
                        src_pos++;
                        byte b2 = CmpFile[src_pos];
                        src_pos++;
                        int copy_from = dest_pos - (((b1 & 0xF) << 8) | b2) - 1;
                        int n = b1 >> 4;
                        if (n == 0)
                        {
                            n = CmpFile[src_pos] + 0x12;
                            src_pos++;
                        }
                        else
                        {
                            n += 2;
                        }
                        for (int j = 0; j < n; j++)
                        {
                            dest[dest_pos] = dest[copy_from];
                            dest_pos++;
                            copy_from++;
                        }
                    }
                    code <<= 1;
                }
            }
            return dest;
        }

    }

}