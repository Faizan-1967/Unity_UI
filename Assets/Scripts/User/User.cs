
using System;
using System.Threading.Tasks;
using Nakama;
using System.Linq;

class User
{
    public NakamaConnection instance;
    public User()
    {
        this.instance = NakamaConnection.Instance;
    }

    public async Task<IApiUsers> getUserAccount(string userId)
    {
        try
        {
            string[] ids = { userId };

            //awaiting
            IApiUsers userAcc = await instance.client.GetUsersAsync(instance.UserSession, ids);

            return userAcc;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
