namespace AdeAuth.Services.Extensions
{
    public static class Extract
    {
        private static Encryption ExtractEncryption(this IConfiguration configuration)
        {
            return configuration.GetSection("Encryption").Get<Encryption>();
        }
        public static AppConfiguration AppProperties(this IConfiguration configuration)
        {
            return new AppConfiguration()
            {
                Encryption = ExtractEncryption(configuration)
            };
        }
    }
}
