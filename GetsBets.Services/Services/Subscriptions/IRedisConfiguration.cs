namespace GetsBets.Services
{
    public interface IRedisConfiguration
    {
        public string ConnectionString { get; }
        public string ExtractionChannel { get; }
    }
}