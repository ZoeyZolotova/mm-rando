using MMRando.Utils;
using System.Collections.Generic;
using System;

namespace MMRando.Models.Rom
{
    public class MessageTable
    {
        const int MESSAGE_DATA_ADDRESS = 0xAD1000;
        const int MESSAGE_TABLE_ADDRESS = 0xC5D0D8;
        const int MESSAGE_TABLE_END_ADDRESS = 0xC66040; //address of null record

        const int MAX_RECORDS = (MESSAGE_TABLE_END_ADDRESS - MESSAGE_TABLE_ADDRESS) / 8;
        const int MAX_SIZE = 0xB3B000 - MESSAGE_DATA_ADDRESS;

        Dictionary<ushort, MessageEntry> messages = new Dictionary<ushort, MessageEntry>();

        public void UpdateMessages(MessageEntry message)
        {
            if (messages.ContainsKey(message.Id))
            {
                if (message.Header == null)
                {
                    message.Header = messages[message.Id].Header;
                }
                messages[message.Id] = message;
            }
        }

        public void UpdateMessages(List<MessageEntry> messages)
        {
            foreach(var item in messages)
            {
                UpdateMessages(item);
            }
        }

        public MessageEntry GetMessage(ushort id)
        {
            if (messages.ContainsKey(id))
            {
                return messages[id];
            }
            return null;
        }

        public void InitializeTable()
        {
            Dictionary<ushort, MessageEntry> messageTable = new Dictionary<ushort, MessageEntry>();

            int fileIndex = RomUtils.GetFileIndexForWriting(MESSAGE_TABLE_ADDRESS);
            MMFile file = RomData.MMFileList[fileIndex];
            int code_baseAddr = MESSAGE_TABLE_ADDRESS - file.Addr;
            var code_data = file.Data;

            fileIndex = RomUtils.GetFileIndexForWriting(MESSAGE_DATA_ADDRESS);
            file = RomData.MMFileList[fileIndex];

            var message_data = file.Data;

            while (true)
            {
                ushort textId = ReadWriteUtils.Arr_ReadU16(code_data, code_baseAddr);
                if (textId >= 0xFFFF)
                {
                    break;
                }

                int address = ReadWriteUtils.Arr_ReadS32(code_data, code_baseAddr + 4) & 0xFFFFFF;

                byte[] header = new byte[11];
                Array.Copy(message_data, address, header, 0, 11);

                int cur = address + 11 - 1;
                string message = "";

                do
                {
                    cur++;
                    message += (char)message_data[cur];
                }
                while (message_data[cur] != 0xBF);


                MessageEntry messageEntry = new MessageEntry()
                {
                    Id = textId,
                    Header = header,
                    Message = message
                };
                messageTable.Add(textId, messageEntry);
                code_baseAddr += 8;
            }

            messages = messageTable;
        }

        public byte[] Rebuild()
        {
            if (messages.Values.Count > MAX_RECORDS)
            {
                throw new Exception($"Message table contains {messages.Values.Count} records, when max is {MAX_RECORDS}");
            }
            int new_message_data_size = 0;
            foreach (var item in messages.Values)
            {
                new_message_data_size += item.Size;
            }
            new_message_data_size = (new_message_data_size + 0xF) & -0x10;

            if (new_message_data_size > MAX_SIZE)
            {
                throw new Exception($"Message data bigger than 0x{MAX_SIZE:X} bytes");
            }
            byte[] new_message_data = new byte[new_message_data_size];

            int cur = 0;
            foreach(var item in messages.Values)
            {
                item.address = 0x0800_0000 | cur;
                Array.Copy(item.Data, 0, new_message_data, cur, item.Size);
                cur += item.Size;
            }
            return new_message_data;
        }

        public static void WriteMessageTable(MessageTable table)
        {
            byte[] new_message_data = table.Rebuild();

            int fileIndex = RomUtils.GetFileIndexForWriting(MESSAGE_DATA_ADDRESS);
            var file = RomData.MMFileList[fileIndex];

            file.Data = new_message_data;
            file.End = file.Addr + new_message_data.Length;


            fileIndex = RomUtils.GetFileIndexForWriting(MESSAGE_TABLE_ADDRESS);
            file = RomData.MMFileList[fileIndex];
            int code_baseAddr = MESSAGE_TABLE_ADDRESS - file.Addr;
            var code_data = file.Data;

            foreach(var item in table.messages.Values)
            {
                ReadWriteUtils.Arr_WriteU16(code_data, code_baseAddr, item.Id);
                ReadWriteUtils.Arr_WriteU32(code_data, code_baseAddr + 4, (uint)item.address);
                code_baseAddr += 8;
            }

            //write terminator record
            ReadWriteUtils.Arr_WriteU16(code_data, code_baseAddr, 0xFFFF);
            ReadWriteUtils.Arr_WriteU32(code_data, code_baseAddr + 4, 0);
        }
    }
}
