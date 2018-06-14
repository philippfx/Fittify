using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quantus.DbResetter;

namespace Fittify.Api.Services
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
