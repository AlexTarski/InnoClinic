using InnoClinic.Authorization.Business.Models;

namespace InnoClinic.Authorization.Business
{
    internal class ClientIdResult : IClientIdResult
    {
        public bool IsSuccess { get; set; }
        public string? ClientId { get; set; }
        public MessageViewModel? ErrorMessage { get; set; }
    }
}