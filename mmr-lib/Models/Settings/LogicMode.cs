using System.ComponentModel;

namespace MMRando.Models
{
    public enum LogicMode
    {
        [Description("Casual Logic")]
        Casual,
        [Description("Glitched Logic")]
        Glitched,
        [Description("Glitched No Setups")]
        GlitchedNoSetups,
        [Description("Glitched Common Tricks")]
        GlitchedCommonTricks,
        [Description("User Logic")]
        User,
        [Description("No Logic")]
        None,
        [Description("Vanilla Item Placement")]
        Vanilla,
    }
}
