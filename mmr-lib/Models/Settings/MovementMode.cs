using System.ComponentModel;

namespace MMRando.Models
{
    public enum MovementMode
    {
        Default,
        [Description("High Speed (many softlocks)")]
        HighSpeed,
        [Description("Super Low Gravity")]
        SuperLowGravity,
        [Description("Low Gravity")]
        LowGravity,
        [Description("High Gravity")]
        HighGravity
    }
}
