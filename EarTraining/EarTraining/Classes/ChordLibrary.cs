using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EarTraining.Classes
{
    public static class ChordLibrary
    {
        // Major Chords
        // ============

        public static Chord AMajor = new Chord
        {
            Name = "A",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.0f,
        };

        public static Chord CMajor = new Chord
        {
            Name = "C",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.2f,
        };

        public static Chord DMajor = new Chord
        {
            Name = "D",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.0f,
        };

        public static Chord EMajor = new Chord
        {
            Name = "E",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.0f,
        };

        public static Chord GMajor = new Chord
        {
            Name = "G",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.2f,
        };

        // Minor Chords
        // ============

        public static Chord AMinor = new Chord
        {
            Name = "Am",
            Quality = ChordQuality.Minor,
            NormalTempoDelta = 1.2f,
        };

        public static Chord DMinor = new Chord
        {
            Name = "Dm",
            Quality = ChordQuality.Minor,
            NormalTempoDelta = 1.25f,
        };

        public static Chord EMinor = new Chord
        {
            Name = "Em",
            Quality = ChordQuality.Minor,
            NormalTempoDelta = 1.2f,
        };

        // Dominant 7th Chords
        // ===================

        public static Chord A7 = new Chord
        {
            Name = "A7",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.2f,
        };

        public static Chord B7 = new Chord
        {
            Name = "B7",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.25f,
        };

        public static Chord C7 = new Chord
        {
            Name = "C7",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.25f,
        };

        public static Chord D7 = new Chord
        {
            Name = "D7",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.4f,
        };

        public static Chord E7 = new Chord
        {
            Name = "E7",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.1f,
        };

        public static Chord G7 = new Chord
        {
            Name = "G7",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.2f,
        };

        // Major 7th Chords
        // ================

        public static Chord FMajor7 = new Chord
        {
            Name = "Fmaj7",
            Quality = ChordQuality.Major7th,
            NormalTempoDelta = 1.15f,
        };

        // Chord Palettes
        // ==============

        public static List<Chord> Stage1ChordPalette = new List<Chord>
        {
            AMajor, DMajor, EMajor,
        };

        public static List<Chord> Stage2ChordPalette = new List<Chord>
        {
            AMajor, DMajor, EMajor, AMinor, DMinor, EMinor,
        };

        public static List<Chord> Stage3ChordPalette = new List<Chord>
        {
            AMajor, DMajor, EMajor, AMinor, DMinor, EMinor, CMajor, GMajor,
        };

        public static List<Chord> Stage4ChordPalette = new List<Chord>
        {
            AMajor, DMajor, EMajor, AMinor, DMinor, EMinor, CMajor, GMajor,
            G7, C7, B7, FMajor7,
        };

        public static List<Chord> Stage5ChordPalette = new List<Chord>
        {
            AMajor, DMajor, EMajor, AMinor, DMinor, EMinor, CMajor, GMajor,
            G7, C7, B7, FMajor7, D7, E7, A7,
        };
    }
}
