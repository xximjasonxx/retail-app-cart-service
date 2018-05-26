
using CartApi.Models;

namespace CartApi
{
    public interface IUserDataContext
    {
        // readonly fields
        User CurrentUser { get; }

        // setter methods
        void SetCurrentUser(User user);
    }
}