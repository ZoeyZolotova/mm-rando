using System;
using MMR.Randomizer.Models.Rom;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace MMR.Randomizer.Tests.Models.Rom
{
    public class MessageEntryBuilderTests
    {
        private MessageEntry SkulltulaEntry = new MessageEntry
        {
            Id = 0x52,
            Header = new byte[11] {0x02, 0x00, 0x52, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF},
            Message =
                $"\u0017You got a \u0006Swamp Gold Skulltula\u0011Spirit\0!\u0018\u001F\u0000\u0010 This is your \u0001\u000D\u0000 one!\u00BF",
        };

        [Test]
        public void HeaderBuilderTest()
        {
            var builder = new MessageEntryBuilder.HeaderBuilder();

            builder.FaintBlue().Y(0).Icon(0x52).NextMessage(0xFFFF).RupeeCost(0xFFFF);

            byte[] initialHeaderBytes = SkulltulaEntry.Header;
            byte[] builderHeaderBytes = builder.Build();

            Assert.That(builderHeaderBytes, Is.EqualTo(initialHeaderBytes));
        }

        [Test]
        public void MessageBuilderTest()
        {
            var builder = new MessageEntryBuilder.MessageBuilder();

            builder
                .QuickTextStart().Text("You got a ").StartPinkText().Text("Swamp Gold Skulltula").NewLine()
                .Text("Spirit").PopTextColor().Text("!").QuickTextStop().PauseText(0x0010).Text(" This is your ").StartRedText().SkulltulaCount().PopTextColor().Text(" one!").EndFinalTextBox();

            byte[] initialMessageBytes = Array.ConvertAll(SkulltulaEntry.Message.ToCharArray(), it => (byte) it);
            byte[] builderMessageBytes = Array.ConvertAll(builder.Build().ToCharArray(), it => (byte) it);

            Assert.That(builderMessageBytes, Is.EqualTo(initialMessageBytes));
        }

        [Test]
        public void MessageBuilderEntryTest()
        {
            var builder = new MessageEntryBuilder();

            MessageEntry entry = builder
                .Id(0x52)
                .Header(it => { it.FaintBlue().Y(0).Icon(0x52).NextMessage(0xFFFF).RupeeCost(0xFFFF); })
                .Message(it =>
                {
                    it
                        .QuickTextStart().Text("You got a ").StartPinkText().Text("Swamp Gold Skulltula").NewLine()
                        .Text("Spirit").PopTextColor().Text("!").QuickTextStop().PauseText(0x0010).Text(" This is your ").StartRedText().SkulltulaCount().PopTextColor().Text(" one!").EndFinalTextBox();
                })
                .Build();

            ClassicAssert.IsTrue(entry.Equals(SkulltulaEntry));
        }

        [Test]
        public void MessageBuilderDslTest()
        {
            var builder = new MessageEntryBuilder();

            MessageEntry entry = builder
                .Id(0x52)
                .Header(it => { it.FaintBlue().Y(0).Icon(0x52).NextMessage(0xFFFF).RupeeCost(0xFFFF); })
                .Message(it =>
                {
                    it.QuickText(() =>
                        {
                            it
                                .Text("You got a ")
                                .Pink(() =>
                                {
                                    it
                                        .Text("Swamp Gold Skulltula")
                                        .NewLine()
                                        .Text("Spirit");
                                })
                                .Text("!");
                        })
                        .PauseText(0x0010)
                        .Text(" This is your ")
                        .Red(() => { it.SkulltulaCount(); })
                        .Text(" one!")
                        .EndFinalTextBox();
                })
                .Build();

            ClassicAssert.IsTrue(entry.Equals(SkulltulaEntry));
        }
    }
}
