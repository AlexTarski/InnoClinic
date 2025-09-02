using InnoClinic.Authorization.Business.Models;

namespace InnoClinic.Authorization.Business
{
    /// <summary>
    /// Represents the result of an operation to retrieve a Client ID.
    /// </summary>
    /// <remarks>This class encapsulates the outcome of an operation, including whether it succeeded,  the
    /// retrieved client identifier (if successful), and an error message (if applicable).</remarks>
    public interface IClientIdResult
    {
        public bool IsSuccess { get; set; }
        public string? ClientId { get; set; }
        public MessageViewModel? ErrorMessage { get; set; }
    }
}