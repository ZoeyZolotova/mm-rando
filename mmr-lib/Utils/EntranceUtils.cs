namespace MMRando.Utils
{

    public static class EntranceUtils
    {
        private static int GetEntranceAddr(int ent)
        {
            int offset = ((ent >> 9) * 12) + 0xC5BC64;
            int f = RomUtils.GetFileIndexForWriting(offset);

            offset -= RomData.MMFileList[f].Addr;
            uint p1 = ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, offset);
            offset = ((ent >> 4) & 0x1F) * 4;

            p1 = (uint)((p1 & 0xFFFFFF) + 0xA96540 - RomData.MMFileList[f].Addr);
            p1 = ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, (int)(p1 + offset));
            p1 = (uint)((p1 & 0xFFFFFF) + 0xA96540 - RomData.MMFileList[f].Addr);

            offset = (ent & 0xF) << 2;
            return (int)p1 + offset;
        }
        public static void WriteEntrances(int[] olde, int[] newe)
        {
            int f = RomUtils.GetFileIndexForWriting(0xC5BC64);
            uint[] data = new uint[newe.Length];

            for (int i = 0; i < newe.Length; i++)
            {
                data[i] = ReadWriteUtils.Arr_ReadU32(RomData.MMFileList[f].Data, GetEntranceAddr(newe[i]));
            }

            for (int i = 0; i < newe.Length; i++)
            {
                ReadWriteUtils.Arr_WriteU32(RomData.MMFileList[f].Data, GetEntranceAddr(olde[i]), data[i]);
            }
        }

    }

}