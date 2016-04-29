using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarTraining.Classes
{
    public class Bar
    {
        public IList<Chord> Chords { get; set; }

        public Bar()
        {
            Chords = new List<Chord>();
        }

        public Bar(IList<Chord> chords)
        {
            Chords = chords;
        }
    }
}
