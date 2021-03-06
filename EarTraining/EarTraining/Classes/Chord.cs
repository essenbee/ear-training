﻿using System;

namespace EarTraining.Classes
{
    public sealed class Chord : IEquatable<Chord>
    {
        public string Name { get; set; }
        public string BassNote { get; set; }
        public ChordQuality Quality { get; set; }
        public float NormalTempoDelta { get; set; }

        private const string Strums4 = "FourStrums";
        private const string Strums2 = "TwoStrums";
        private const string Strums1 = "SingleStrum";

        public string GetAudioResourceName(int numStrums = 4)
        {
            var strums = string.Empty;
            switch (numStrums)
            {
                case 2 : strums = Strums2;
                    break;
                case 1:
                    strums = Strums1;
                    break;
                default:
                    strums = Strums4;
                    break;
            } 

            return $"{Name}Chord{strums}";
        }

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
