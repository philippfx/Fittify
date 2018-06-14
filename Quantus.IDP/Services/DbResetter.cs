using System.Threading.Tasks;
using Quantus.DbResetter;

namespace Quantus.IDP.Services
{
    public class DbResetter : IDbResetter
    {
        public Task<bool> ResetDb()
        {
            return Task.Run(() =>
            {
                if (!Connection.DeleteDb())
                {
                    return false;
                }

                if (!Connection.EnsureCreatedDbContext())
                {
                    return false;
                }

                if (!Connection.Seed())
                {
                    return false;
                }
                return true;
            });
        }
    }
}
