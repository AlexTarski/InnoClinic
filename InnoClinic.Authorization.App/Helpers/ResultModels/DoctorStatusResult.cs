using System.Net;

namespace InnoClinic.Authorization.Business.Helpers.ResultModels
{
    public record DoctorStatusResult(
        bool IsActive,
        HttpStatusCode StatusCode,
        string? Message
    );
}