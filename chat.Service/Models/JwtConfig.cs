namespace chat.Service.Models
{
    public class JwtConfig
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public byte AccessTokenExpirationMinutes { get; set; }
        public byte RefreshTokenExpirationDays { get; set; }
    }
}
