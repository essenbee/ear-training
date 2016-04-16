using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarTraining.Classes
{
    public class Chord
    {
        public string Name { get; set; }
        public ChordQuality Quality { get; set; }
        public IDictionary<int, UnmanagedMemoryStream> AudioStreams { get; set; }
    }
}
