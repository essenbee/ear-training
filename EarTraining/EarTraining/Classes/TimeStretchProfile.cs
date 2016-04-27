namespace EarTraining.Classes
{
    public class TimeStretchProfile
    {
        public string Id { get; set; }
        public string Description { get; set; }

        public bool UseAAFilter { get; set; }
        public int AAFilterLength { get; set; }
        public int Overlap { get; set; }
        public int Sequence { get; set; }
        public int SeekWindow { get; set; }

        public const float DefaultTempo = 1.0f; // 1 = 100%/regular speed
        public const int DefaultPitch = 0; // 0 = Regular pitch, in semi-tones
        public const int DefaultLatency = 125;

        public TimeStretchProfile()
        {
            Id = "Practice#_Optimum";
            UseAAFilter = true;
            AAFilterLength = 128;
            SeekWindow = 80;
            Sequence = 20;
            Overlap = 20;
            Description = "Optimum for Music and Speech";
        }

        public override string ToString()
        {
            return Description;
        }
    }
}