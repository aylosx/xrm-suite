namespace Shared.Models.Responses.Account
{
    using System;
    using System.Runtime.Serialization;

    using Microsoft.Xrm.Sdk;

    using Shared.Models.Domain;

    [DataContract]
    public class InitializedEntity
    {
        #region Properties

        [DataMember]
        public EntityReference AccountEntityReference { get; private set; }

        [DataMember]
        public string AccountNumber { get; private set; }

        #endregion

        #region Constructor

        public InitializedEntity(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            AccountEntityReference = new EntityReference(Account.EntityLogicalName, account.Id);
            AccountNumber = account.AccountNumber;
        }

        #endregion
    }
}
