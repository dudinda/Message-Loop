namespace MessageLoop.Common.Models.LongRun
{
    public record class LongRunToken
    {
        public LongRunToken(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}
