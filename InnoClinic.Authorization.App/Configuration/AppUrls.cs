using Microsoft.Extensions.Configuration;

namespace InnoClinic.Authorization.Business.Configuration
{
    public static class AppUrls
    {
        public static string AuthUrl { get; set; }
        public static string ProfilesUrl { get; set; }
        public static string OfficesUrl { get; set; }
        public static string ServicesUrl { get; set; }
        public static string AppointmentsUrl { get; set; }
        public static string EmployeeUiUrl { get; set; }
        public static string ClientUiUrl { get; set; }

        public static void Initialize(IConfiguration configuration)
        {
            AuthUrl = configuration["AppUrls:AuthUrl"];
            ProfilesUrl = configuration["AppUrls:ProfilesUrl"];
            OfficesUrl = configuration["AppUrls:OfficesUrl"];
            ServicesUrl = configuration["AppUrls:ServicesUrl"];
            AppointmentsUrl = configuration["AppUrls:AppointmentsUrl"];
            EmployeeUiUrl = configuration["AppUrls:EmployeeUiUrl"];
            ClientUiUrl = configuration["AppUrls:ClientUiUrl"];
        }
    }
}