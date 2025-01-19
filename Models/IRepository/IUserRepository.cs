namespace NEWS_App.Models.IRepository
{
    public interface IUserRepository
    {
      
            Task<User> GetByIdAsync(int id);
            Task<User> GetByUsernameAsync(string username);
            Task AddAsync(User user);
            Task<bool> ValidateUser(string username, string password);
            Task<bool> ToggleNotifications(int userId);

        Task updateUser(User user);
    }
}
