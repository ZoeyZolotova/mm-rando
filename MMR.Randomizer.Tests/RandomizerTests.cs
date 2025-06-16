using System;
using System.Linq;
using MMR.Randomizer.GameObjects;
using MMR.Randomizer.Models;
using MMR.Randomizer.Models.Rom;
using MMR.Randomizer.Models.Settings;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace MMR.Randomizer.Tests
{
    public class RandomizerTests
    {
        private GameplaySettings _settings;
        private const int seed = 0;

        [SetUp]
        public void SetupRandomizerTests()
        {
            _settings = new GameplaySettings
            {
                LogicMode = LogicMode.NoLogic,
            };
        }

        [Test]
        public void ShouldResultInAsManyMessageCostsAsThereAreMessageCosts()
        {
            _settings.PriceMode = PriceMode.None;
            var randomizer = new Randomizer(_settings, seed);
            var result = randomizer.Randomize(new NoProgressReporter());

            ClassicAssert.AreEqual(MessageCost.MessageCosts.Length, result.MessageCosts.Count);
        }

        [Test]
        [TestCase(PriceMode.Purchases)]
        [TestCase(PriceMode.Purchases | PriceMode.Minigames)]
        [TestCase(PriceMode.Purchases | PriceMode.Misc)]
        [TestCase(PriceMode.Purchases | PriceMode.Minigames | PriceMode.Misc)]
        [TestCase(PriceMode.Minigames)]
        [TestCase(PriceMode.Minigames | PriceMode.Misc)]
        [TestCase(PriceMode.Misc)]
        [TestCase(PriceMode.None)]
        public void ShouldHaveCorrespondingNonNullMessageCostsForEachMessageCostPurchasesDependingOnPriceMode(PriceMode priceMode)
        {
            _settings.PriceMode = priceMode;
            var randomizer = new Randomizer(_settings, seed);
            var result = randomizer.Randomize(new NoProgressReporter());

            var purchaseCount = MessageCost.MessageCosts.Count(mc => priceMode.HasFlag(mc.Category));

            for (var i = 0; i < MessageCost.MessageCosts.Length; i++)
            {
                ClassicAssert.AreEqual(priceMode.HasFlag(MessageCost.MessageCosts[i].Category), result.MessageCosts[i] != null);
            }
        }

        [Test]
        [TestCase(PriceMode.Purchases)]
        [TestCase(PriceMode.Purchases | PriceMode.Minigames)]
        [TestCase(PriceMode.Purchases | PriceMode.Misc)]
        [TestCase(PriceMode.Purchases | PriceMode.Minigames | PriceMode.Misc)]
        [TestCase(PriceMode.Minigames)]
        [TestCase(PriceMode.Minigames | PriceMode.Misc)]
        [TestCase(PriceMode.Misc)]
        [TestCase(PriceMode.None)]
        public void ShouldResultInMessageCostsBeingSequentiallyEquivalentToVanillaCostsIfPriceModeHasShuffleOnly(PriceMode priceMode)
        {
            _settings.PriceMode = priceMode | PriceMode.ShuffleOnly;
            var randomizer = new Randomizer(_settings, seed);
            var result = randomizer.Randomize(new NoProgressReporter());

            var vanillaCosts = MessageCost.MessageCosts.Where(mc => priceMode.HasFlag(mc.Category)).Select(mc => mc.Cost).ToList();

            CollectionAssert.AreEquivalent(vanillaCosts, result.MessageCosts.Where(c => c != null));
        }

        [Test]
        public void ShouldResultInMessageCostsBeingTwiceAsLargeAsVanillaCostsIfPriceModeIsAccountForRoyalWallet()
        {
            _settings.PriceMode = PriceMode.AccountForRoyalWallet;
            _settings.CustomItemList.Add(Item.UpgradeRoyalWallet);
            _settings.CustomItemList.Add(Item.ChestTerminaGrassRedRupee);
            _settings.CustomItemList.Add(Item.ChestTerminaGrottoRedRupee);

            var randomizer = new Randomizer(_settings, seed);
            var result = randomizer.Randomize(new NoProgressReporter());

            for (var i = 0; i < MessageCost.MessageCosts.Length; i++)
            {
                var expectedCost = MessageCost.MessageCosts[i].Cost << 1;
                if (expectedCost > 999)
                {
                    expectedCost = 999;
                }
                ClassicAssert.AreEqual(expectedCost, result.MessageCosts[i]);
            }
        }

        [Test]
        [TestCase(PriceMode.Purchases)]
        [TestCase(PriceMode.Purchases | PriceMode.Minigames)]
        [TestCase(PriceMode.Purchases | PriceMode.Misc)]
        [TestCase(PriceMode.Purchases | PriceMode.Minigames | PriceMode.Misc)]
        [TestCase(PriceMode.Minigames)]
        [TestCase(PriceMode.Minigames | PriceMode.Misc)]
        [TestCase(PriceMode.Misc)]
        [TestCase(PriceMode.None)]
        public void ShouldResultInMessageCostsBeingTwiceAsLargeAsVanillaCostsIfPriceModeHasAccountForRoyalWalletAndShuffleOnly(PriceMode priceMode)
        {
            _settings.PriceMode = priceMode | PriceMode.AccountForRoyalWallet | PriceMode.ShuffleOnly;
            _settings.CustomItemList.Add(Item.UpgradeRoyalWallet);
            _settings.CustomItemList.Add(Item.ChestTerminaGrassRedRupee);
            _settings.CustomItemList.Add(Item.ChestTerminaGrottoRedRupee);

            var randomizer = new Randomizer(_settings, seed);
            var result = randomizer.Randomize(new NoProgressReporter());

            var shuffledCosts = MessageCost.MessageCosts
                .Where(mc => priceMode.HasFlag(mc.Category))
                .Select(mc =>
                {
                    var cost = mc.Cost << 1;
                    if (cost > 999)
                    {
                        cost = 999;
                    }
                    return cost;
                })
                .ToList();

            var shuffledResults = result.MessageCosts.Where((c, i) => priceMode.HasFlag(MessageCost.MessageCosts[i].Category));

            CollectionAssert.AreEquivalent(shuffledCosts, shuffledResults);

            for (var i = 0; i < MessageCost.MessageCosts.Length; i++)
            {
                var messageCost = MessageCost.MessageCosts[i];
                if (priceMode.HasFlag(messageCost.Category))
                {
                    continue;
                }
                var expectedCost = MessageCost.MessageCosts[i].Cost << 1;
                if (expectedCost > 999)
                {
                    expectedCost = 999;
                }
                ClassicAssert.AreEqual(expectedCost, result.MessageCosts[i], "Cost {0} was incorrect.", messageCost.Name);
            }
        }
    }
}
