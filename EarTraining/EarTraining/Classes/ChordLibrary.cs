using System.Collections.Generic;

namespace EarTraining.Classes
{
    public static class ChordLibrary
    {
        // Major Chords
        // ============

        public static Chord AMajor = new Chord
        {
            Name = "A",
            BassNote = "A",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.0f,
        };

        public static Chord CMajor = new Chord
        {
            Name = "C",
            BassNote = "C",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.2f,
        };

        public static Chord DMajor = new Chord
        {
            Name = "D",
            BassNote = "D",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.0f,
        };

        public static Chord EMajor = new Chord
        {
            Name = "E",
            BassNote = "E",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.0f,
        };

        public static Chord GMajor = new Chord
        {
            Name = "G",
            BassNote = "G",
            Quality = ChordQuality.Major,
            NormalTempoDelta = 1.2f,
        };

        // Minor Chords
        // ============

        public static Chord AMinor = new Chord
        {
            Name = "Am",
            BassNote = "A",
            Quality = ChordQuality.Minor,
            NormalTempoDelta = 1.2f,
        };

        public static Chord DMinor = new Chord
        {
            Name = "Dm",
            BassNote = "D",
            Quality = ChordQuality.Minor,
            NormalTempoDelta = 1.25f,
        };

        public static Chord EMinor = new Chord
        {
            Name = "Em",
            BassNote = "E",
            Quality = ChordQuality.Minor,
            NormalTempoDelta = 1.2f,
        };

        // Dominant 7th Chords
        // ===================

        public static Chord A7 = new Chord
        {
            Name = "A7",
            BassNote = "A",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.2f,
        };

        public static Chord B7 = new Chord
        {
            Name = "B7",
            BassNote = "B",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.25f,
        };

        public static Chord C7 = new Chord
        {
            Name = "C7",
            BassNote = "C",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.25f,
        };

        public static Chord D7 = new Chord
        {
            Name = "D7",
            BassNote = "D",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.4f,
        };

        public static Chord E7 = new Chord
        {
            Name = "E7",
            BassNote = "E",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.1f,
        };

        public static Chord G7 = new Chord
        {
            Name = "G7",
            BassNote = "G",
            Quality = ChordQuality.Dominant7th,
            NormalTempoDelta = 1.2f,
        };

        // Major 7th Chords
        // ================

        public static Chord FMajor7 = new Chord
        {
            Name = "Fmaj7",
            BassNote = "F",
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

        public static List<Chord> Stage4ChordPalleteModified = new List<Chord>
        {
            AMajor, DMajor, EMajor, AMinor, DMinor, EMinor, CMajor, GMajor,
            G7, C7, B7,
        };

        public static List<Chord> Stage5ChordPalette = new List<Chord>
        {
            AMajor, DMajor, EMajor, AMinor, DMinor, EMinor, CMajor, GMajor,
            G7, C7, B7, FMajor7, D7, E7, A7,
        };

        public static List<Chord> Stage5ChordPaletteModified = new List<Chord>
        {
            AMajor, DMajor, EMajor, AMinor, DMinor, EMinor, CMajor, GMajor,
            G7, C7, B7, D7, E7, A7,
        };
    }
}
