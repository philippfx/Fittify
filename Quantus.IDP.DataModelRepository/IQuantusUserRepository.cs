using System;
using System.Collections.Generic;
using Quantus.IDP.DataModels.Models.Default;

namespace Quantus.IDP.DataModelRepository
{
    public interface IQuantusUserRepository
    {
        QuantusUser GetUserByUsername(string username);
        QuantusUser GetUserBySubjectId(Guid subjectId);
        QuantusUser GetUserByEmail(string email);
        QuantusUser GetUserByProvider(string loginProvider, string providerKey);
        IEnumerable<QuantusUserLogin> GetUserLoginsBySubjectId(Guid subjectId);
        IEnumerable<QuantusUserClaim> GetUserClaimsBySubjectId(Guid subjectId);
        bool AreUserCredentialsValid(string username, string password);
        bool IsUserActive(Guid subjectId);
        void AddUser(QuantusUser quantusUser);
        void AddUserLogin(Guid subjectId, string loginProvider, string providerKey);
        void AddUserClaim(Guid subjectId, string claimType, string claimValue);
        bool Save();
    }
}
