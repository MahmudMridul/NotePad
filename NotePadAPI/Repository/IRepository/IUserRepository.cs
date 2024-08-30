namespace NotePadAPI.Repository.IRepository
{
    public interface IUserRepository
    {
        Task<bool> EmailExists(string email);
    }
}
