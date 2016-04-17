using System;
using System.Collections.Generic;
using System.IO;

namespace EarTraining.Classes
{
    public sealed class Chord : IEquatable<Chord>
    {
        public string Name { get; set; }
        public ChordQuality Quality { get; set; }
        public IDictionary<int, string> AudioFiles { get; set; }


        public override bool Equals(Object obj)
        {
            // Check for null values and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var other = (Chord)obj;
            return (Name.Equals(other.Name));
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Chord c1, Chord c2)
        {
            bool retVal;

            if (ReferenceEquals(c1, c2))
            {
                retVal = true;
            }
            else if (((object)c1 == null) || ((object)c2 == null))
            {
                retVal = false;
            }
            else
            {
                retVal = (c1.Equals(c2));
            }

            return retVal;
        }

        public static bool operator !=(Chord c1, Chord c2)
        {
            return !(c1 == c2);
        }

        public bool Equals(Chord other)
        {
            if (other == null)
            {
                return false;
            }

            return (Name.Equals(other.Name));
        }

        public override string ToString()
        {
            return Name ?? string.Empty; ;
        }
    }
}
