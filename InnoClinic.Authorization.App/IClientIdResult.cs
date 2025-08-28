using InnoClinic.Authorization.Business.Models;

namespace InnoClinic.Authorization.Business
{
    public interface IClientIdResult
    {
        public bool IsSuccess { get; set; }
        public string? ClientId { get; set; }
        public MessageViewModel? ErrorMessage { get; set; }
    }
}