using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarTraining.Classes
{
    public enum ChordQuality
    {
        [Description("Major")]
        Major = 1,
        [Description("Minor")]
        Minor = 2,
        [Description("Dominant 7th")]
        Dominant7th = 3,
        [Description("Major 7th")]
        Major7th = 4,
        [Description("Minor 7th")]
        Minor7th = 5,
        [Description("Minor 7th Flat 5")]
        Minor7Flat5 = 6,
    }
}
