namespace InnoClinic.Authorization.Business.Configuration
{
    public class EmailSettings
    {
        public string From { get; set; }
        public string DisplayName { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string CredUserName { get; set; }
        public string CredPassword { get; set; }
        public bool EnableSsl { get; set; } = true;
    }
}