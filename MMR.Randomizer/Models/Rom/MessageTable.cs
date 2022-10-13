﻿using MMR.Randomizer.Models.SoundEffects;
using MMR.Randomizer.Utils;
using MMR.Rom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MMR.Randomizer.Models.Rom
{
    public class MessageTable
    {
        /// <summary>
        /// VROM range which contains all message table entries.
        /// </summary>
        public static readonly (uint, uint) MessageTableRange = (0xC5D0D8, 0xC66040);

        /// <summary>
        /// File index of file containing message data.
        /// </summary>
        public static readonly int MessageDataFile = 29;

        /// <summary>
        /// All messages in the table, mapped by Id.
        /// </summary>
        Dictionary<ushort, MessageEntry> Messages;

        /// <summary>
        /// Max number of entries in message table.
        /// </summary>
        public uint MaxEntryCount { get; }

        /// <summary>
        /// Maximum length of message data file, if any.
        /// </summary>
        public uint? MaxDataLength { get; } = null;

        MessageTable(Dictionary<ushort, MessageEntry> messages, uint? maxDataLength = null)
        {
            this.MaxDataLength = maxDataLength;
            this.MaxEntryCount = (uint)messages.Count;
            this.Messages = messages;
        }

        public MessageTable(uint maxEntryCount)
        {
            this.MaxEntryCount = maxEntryCount;
            this.Messages = new Dictionary<ushort, MessageEntry>((int)maxEntryCount);
        }

        /// <summary>
        /// Add new messages into the table.
        /// </summary>
        /// <param name="messages">Messages</param>
        public void AddMessage(params MessageEntry[] messages)
        {
            foreach (var message in messages)
            {
                this.Messages.Add(message.Id, message);
            }
        }

        /// <summary>
        /// Update all messages to apply "quick text" hack.
        /// </summary>
        public void ApplyQuickText()
        {
            var regex = new Regex("(?<!(?:\x1B|\x1C|\x1D|\x1E).?)(?:\x1F..|\x17|\x18)", RegexOptions.Singleline);
            foreach (var entry in this.Messages.Values)
            {
                entry.Message = regex.Replace(entry.Message, "");
            }
        }

        public void ApplyRandomSoundEffects(Dictionary<SoundEffect, SoundEffect> replacements)
        {
            var regex = new Regex("(?<!(?:\x1B|\x1C|\x1D|\x1E|\x1F).?)\x1E(..)", RegexOptions.Singleline);
            foreach (var entry in this.Messages.Values)
            {
                var match = regex.Match(entry.Message);
                if (match.Success)
                {
                    foreach (Capture capture in match.Groups[1].Captures)
                    {
                        var chars = capture.Value.Select(c => (byte)c).ToArray();
                        var oldSoundId = ReadWriteUtils.Arr_ReadU16(chars, 0);
                        var oldSoundEffectNo = (ushort)(oldSoundId & 0x0800);
                        var oldSoundEffect = (SoundEffect)(oldSoundId & ~0x0800);
                        var replacement = replacements.GetValueOrDefault(oldSoundEffect);
                        if (replacement != default)
                        {
                            var newSoundId = (ushort)((ushort)replacement | oldSoundEffectNo);
                            var stringBuilder = new StringBuilder(entry.Message);
                            stringBuilder[capture.Index] = (char)((newSoundId & 0xFF00) >> 8);
                            stringBuilder[capture.Index + 1] = (char)(newSoundId & 0xFF);
                            entry.Message = stringBuilder.ToString();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Build two <see cref="byte"/> arrays for the message table and message data respectively.
        /// </summary>
        /// <returns>Byte arrays</returns>
        public (byte[], byte[]) BuildData()
        {
            // Verify message count does not exceed maximum.
            if (this.Messages.Count > this.MaxEntryCount)
            {
                throw new Exception($"Message table contains {this.Messages.Count} records, when max is {this.MaxEntryCount}");
            }

            // Calculate total size and align to 0x10-byte boundary.
            var totalSum = this.Messages.Values.Sum(x => x.Size);
            var totalSize = (totalSum + 0xF) & -0x10;

            // Verify message data length does not exceed maximum.
            if (this.MaxDataLength != null && totalSize > this.MaxDataLength)
            {
                throw new Exception($"Message data bigger than 0x{this.MaxDataLength:X} bytes");
            }

            // Build full message data buffer.
            var dataBytes = new byte[totalSize];
            var tableBytes = new byte[(this.Messages.Count + 1) * 8];

            var index = 0;
            var offset = 0;
            foreach (var message in this.Messages.Values)
            {
                // Copy message table bytes.
                ReadWriteUtils.Arr_WriteU16(tableBytes, (index * 8), message.Id);
                ReadWriteUtils.Arr_WriteU32(tableBytes, (index * 8) + 4, (uint)offset | 0x0800_0000);

                // Copy message data bytes.
                var data = message.ToBytes();
                Buffer.BlockCopy(data, 0, dataBytes, offset, data.Length);

                index += 1;
                offset += data.Length;
            }

            // Write terminator record.
            ReadWriteUtils.Arr_WriteU16(tableBytes, (index * 8), 0xFFFF);
            ReadWriteUtils.Arr_WriteU32(tableBytes, (index * 8) + 4, 0);

            return (tableBytes, dataBytes);
        }

        /// <summary>
        /// Get existing message by Id, or null if none found.
        /// </summary>
        /// <param name="id">Message Id</param>
        /// <returns>Message, or null if none found</returns>
        public MessageEntry GetMessage(ushort id)
        {
            if (this.Messages.ContainsKey(id))
            {
                return this.Messages[id];
            }
            return null;
        }

        /// <summary>
        /// Read the default <see cref="MessageTable"/>.
        /// </summary>
        /// <returns>Message table</returns>
        public static MessageTable ReadDefault()
        {
            // Read message table from default addresses in ROM.
            var range = MessageTableRange;
            var index = MessageDataFile;
            return ReadFromROM(range.Item1, range.Item2, index);
        }

        /// <summary>
        /// Read a <see cref="MessageTable"/> from ROM.
        /// </summary>
        /// <param name="start">Table start VROM address</param>
        /// <param name="end">Table end VROM address</param>
        /// <param name="fileIndex">File index of message data file</param>
        /// <returns><see cref="MessageTable"/> read from ROM.</returns>
        public static MessageTable ReadFromROM(uint start, uint end, int fileIndex)
        {
            uint length = (end - start) / 8;
            var range = new ValueRange(start, end + 8);
            var span = RomData.Files.GetReadOnlySpanAt(range);
            var dataFile = RomData.Files.GetReadOnlySpan(fileIndex);
            var entries = new Dictionary<ushort, MessageEntry>();

            for (uint i = 0; i < length; i++)
            {
                // Read MessageTable entry data (text Id and offset).
                var slice = span.Slice((int)(i * 8));
                var textId = ReadWriteUtils.ReadU16(slice);
                var offset = ReadWriteUtils.ReadU32(slice, 4) & 0xFFFFFF;

                var nextOffset = ReadWriteUtils.ReadU32(slice, 0xC) & 0xFFFFFF;
                var size = nextOffset > 0 ? nextOffset - offset - 11 : 5; // "end!\xBF"
                
                // Check if terminator record.
                if (textId == 0xFFFF)
                {
                    break;
                }

                // Read MessageEntry from data file.
                var entry = MessageEntry.FromBytes(textId, dataFile, (int)offset, (int)size);

                entries.Add(textId, entry);
            }

            // Calculate maximum message data length from existing file.
            var maxDataLength = RomData.Files.GetAvailableAddressRange(fileIndex, AddressDirection.Right).Length;

            return new MessageTable(entries, maxDataLength);
        }

        /// <summary>
        /// Update with new <see cref="MessageEntry"/> message.
        /// </summary>
        /// <param name="message">Message</param>
        public void UpdateMessages(MessageEntry message)
        {
            if (this.Messages.ContainsKey(message.Id))
            {
                if (message.Header == null)
                {
                    message.UpdateHeader(this.Messages[message.Id].Header);
                }
                this.Messages[message.Id] = message;
            }
        }

        /// <summary>
        /// Update with multiple new <see cref="MessageEntry"/> messages.
        /// </summary>
        /// <param name="messages">Messages</param>
        public void UpdateMessages(IEnumerable<MessageEntry> messages)
        {
            foreach (var item in messages)
            {
                this.UpdateMessages(item);
            }
        }

        /// <summary>
        /// Overwrite default <see cref="MessageTable"/> in ROM along with message data.
        /// </summary>
        /// <param name="table"><see cref="MessageTable"/> to write</param>
        /// <param name="isQuickTextEnabled">Whether or not to apply "quick text" hacks</param>
        public static void WriteDefault(MessageTable table, bool isQuickTextEnabled)
        {
            // Apply "quick text" hack if specified.
            if (isQuickTextEnabled)
            {
                table.ApplyQuickText();
            }

            var (tableBytes, dataBytes) = table.BuildData();

            // Calculate offset of table in respective file.
            var (start, end) = MessageTableRange;
            var range = new ValueRange(start, end);
            var tableSpan = RomData.Files.GetSpan(FileIndex.code.ToInt(), range);

            // Write message table in-place.
            ReadWriteUtils.Copy(tableSpan, tableBytes);

            // Update message data file.
            RomData.Files.ResizeWithData(MessageDataFile, dataBytes);
        }

        /// <summary>
        /// Write an extended <see cref="MessageTable"/> to a specific address, and append a new file with the message data.
        /// </summary>
        /// <param name="table"><see cref="MessageTable"/> to write</param>
        /// <param name="span">Span to write to</param>
        /// <param name="addDummy">Whether or not to add a final dummy entry</param>
        /// <returns>Message data file index</returns>
        public static int WriteExtended(MessageTable table, Span<byte> span, bool addDummy = true)
        {
            if (addDummy)
            {
                // Add unused dummy message entry, so that the previous table entry can calculate its data length at runtime.
                table.AddMessage(new MessageEntry(0xFFFE, "\u00BF"));
            }

            // Build message table and data bytes.
            var (tableBytes, dataBytes) = table.BuildData();

            // Write message table in-place.
            ReadWriteUtils.Write(span, tableBytes);

            // Write extended message data as own file.
            var index = RomUtils.AppendFile(dataBytes);

            return index;
        }
    }
}
