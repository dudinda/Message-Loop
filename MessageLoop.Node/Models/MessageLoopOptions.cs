namespace MessageLoop.Node.Models
{
    public record MessageLoopOptions
    {
        public int MaxFails { get; set; } = 5;
        public int LoopFrequencyMs { get; set; } = 1000;
        public int LoopTimeoutMs { get; set; } = 1000 * 60 * 5;
    }
}
