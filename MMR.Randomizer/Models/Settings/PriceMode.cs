using System;
using System.ComponentModel;

namespace MMR.Randomizer.Models
{
    [Flags]
    [Description("Randomize Prices")]
    public enum PriceMode
    {
        None,

        [Description("Prices for item purchases will be randomized.")]
        Purchases = 1,

        [Description("Prices for minigames will be randomized.")]
        Minigames = 2,

        [Description("Prices for other miscellaneous spending will be randomized.")]
        Misc = 4,

        [Description("Price randomization will range from 1-999 instead of 1-500.")]
        AccountForRoyalWallet = 8,
    }
}
