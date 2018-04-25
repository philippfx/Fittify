using System.Collections.Generic;
using Quantus.IDP.Entities;

namespace Quantus.IDP.Services
{
    public interface IQuantusUserRepository
    {
        QuantusUser GetUserByUsername(string username);
        QuantusUser GetUserBySubjectId(string subjectId);
        QuantusUser GetUserByEmail(string email);
        QuantusUser GetUserByProvider(string loginProvider, string providerKey);
        IEnumerable<UserLogin> GetUserLoginsBySubjectId(string subjectId);
        IEnumerable<UserClaim> GetUserClaimsBySubjectId(string subjectId);
        bool AreUserCredentialsValid(string username, string password);
        bool IsUserActive(string subjectId);
        void AddUser(QuantusUser quantusUser);
        void AddUserLogin(string subjectId, string loginProvider, string providerKey);
        void AddUserClaim(string subjectId, string claimType, string claimValue);
        bool Save();
    }
}
