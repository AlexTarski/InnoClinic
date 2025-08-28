namespace InnoClinic.Authorization.Business.Interfaces
{
    public interface IMessageService
    {
        public Task SendVerificationMessageAsync(string userAddress, string confirmationLink);
        public Task<bool> ConfirmUserContactMethod(string userId, string token);
    }
}