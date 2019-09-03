using System;

namespace MMRando.Models.Rom
{
    public class MessageEntry
    {
        public ushort Id;
        public byte[] Header = new byte[11];
        public string Message;

        /// <summary>
        /// For MessageTable access only
        /// </summary>
        public int address;


        public int Size => (Message.Length + Header.Length + 3) & -4;

        public byte[] Data => GetData();

        private byte[] GetData()
        {
            byte[] data = new byte[Size];
            byte[] msg = Array.ConvertAll(Message.ToCharArray(), item => (byte)item);

            Header.CopyTo(data, 0);
            msg.CopyTo(data, Header.Length);
            return data;
        }
    }
}
