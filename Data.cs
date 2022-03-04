using System;

namespace appData
{
    public partial class Library
    {
        public partial class Data
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// ApiError contains error code and description of error if the API encounters an error.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class ApiResponse
            {
                public Int32 ErrorCode = 0;
                public String Result = "";
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Credential holds security information of administrative user that can access
            /// the API and other non-public modules. Whenever access to the API is needed,
            /// a Token ID must be requested using the UserName and Password as credentials.
            /// If valid, a Token ID is generated with an expiry date. After the expiry, a new
            /// Token ID must be requested. This minimizes exposure of the UserName and Password.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class Credential
            {
                public Int32 CredentialID = 0;
                public String UserName = "";
                public String Password = "";
                public String TokenID = "";
                public DateTime TokenExpiry = DateTime.Now;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// User Profile contains basic login and personal information of account holders.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class UserProfile
            {
                public Int32 UserID = 0;
                public String LoginID = "";
                public String Password = "";
                public String FirstName = "";
                public String LastName = "";
                public DateTime DateRegistered = DateTime.Now;
                public String Country = "UK";
                public String CypherKey = "";
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Wallet Profile contains confidential Cardano wallet information of account holders.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public class WalletProfile
            {
                public Int32 WalletID = 0; // Internal database Wallet ID
                public Int32 UserID = 0; // Wallet Owner's User Profile
                public String CardanoID = ""; // Cardano Wallet ID
                public String Password = ""; // Cardano Wallet Passkey
                public String RecoveryPhrase = ""; // Cardano Wallet Recovery Phrase
                public String MnemonicPassword = ""; // Password to Encrypt Recovery Phrase
            }
        }
    }
}