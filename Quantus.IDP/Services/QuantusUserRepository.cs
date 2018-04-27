using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Quantus.IDP.Entities;
using Quantus.IDP.Entities.Default;

namespace Quantus.IDP.Services
{
    public class QuantusUserRepository : IQuantusUserRepository
    {
        QuantusUserContext _context;

        public QuantusUserRepository(QuantusUserContext context)
        {
            _context = context;
        }

        public bool AreUserCredentialsValid(string username, string password)
        {
            // get the user
            var user = GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            return (user.Password == password && !string.IsNullOrWhiteSpace(password));
        }

        public QuantusUser GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Claims.Any(c => c.ClaimType == "email" && c.ClaimValue == email));
        }

        public QuantusUser GetUserByProvider(string loginProvider, string providerKey)
        {
            return _context.Users
                .FirstOrDefault(u => 
                    u.Logins.Any(l => l.LoginProvider == loginProvider && l.ProviderKey == providerKey));
        }

        public QuantusUser GetUserBySubjectId(Guid subjectId)
        {
            return _context.Users.FirstOrDefault(u => u.Id == subjectId);
        }

        public QuantusUser GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username);
        }

        public IEnumerable<QuantusUserClaim> GetUserClaimsBySubjectId(Guid subjectId)
        {
            // get user with claims
            var user = _context.Users.Include("Claims").FirstOrDefault(u => u.Id == subjectId);
            if (user == null)
            {
                return new List<QuantusUserClaim>();
            }
            return user.Claims.ToList();
        }

        public IEnumerable<QuantusUserLogin> GetUserLoginsBySubjectId(Guid subjectId)
        {
            var user = _context.Users.Include("Logins").FirstOrDefault(u => u.Id == subjectId);
            if (user == null)
            {
                return new List<QuantusUserLogin>();
            }
            return user.Logins.ToList();
        }

        public bool IsUserActive(Guid subjectId)
        {
            var user = GetUserBySubjectId(subjectId);
            return user.IsActive;
         }

        public void AddUser(QuantusUser quantusUser)
        {
            _context.Users.Add(quantusUser);
        }

        public void AddUserLogin(Guid subjectId, string loginProvider, string providerKey)
        {
            var user = GetUserBySubjectId(subjectId);
            if (user == null)
            {
                throw new ArgumentException("User with given subjectId not found.", subjectId.ToString());
            }

            user.Logins.Add(new QuantusUserLogin()
            {
                UserId = subjectId,
                LoginProvider = loginProvider,
                ProviderKey = providerKey
            });
        }

        public void AddUserClaim(Guid subjectId, string claimType, string claimValue)
        {          
            var user = GetUserBySubjectId(subjectId);
            if (user == null)
            {
                throw new ArgumentException("User with given subjectId not found.", subjectId.ToString());
            }

            user.Claims.Add(new QuantusUserClaim() { ClaimType = claimType, ClaimValue = claimValue});         
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_context != null)
                {
                    _context.Dispose();
                    _context = null;
                }
            }
        }

       
    }
}
