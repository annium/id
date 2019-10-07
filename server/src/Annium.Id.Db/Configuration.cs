namespace Annium.Id.Db
{
    internal class Configuration
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Name { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool LogQueries { get; set; }
    }
}