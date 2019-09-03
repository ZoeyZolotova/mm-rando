using System.ComponentModel;

namespace MMRando.Models
{
    public enum TatlColorSchema
    {
        Default,
        Dark,
        Hot,
        Cool,
        [Description("Rainbow (cycle)")]
        Rainbow,
        Random,
    }
}
