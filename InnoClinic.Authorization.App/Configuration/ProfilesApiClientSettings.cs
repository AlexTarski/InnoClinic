namespace InnoClinic.Authorization.Business.Configuration
{
    public class ProfilesApiClientSettings
    {
        public required string BaseUrl { get; set; }
        public required string ProfilesEndpoint { get; set; }
        public required string DoctorsEndpoint { get; set; }
    }
}