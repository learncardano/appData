using System;
using System.Globalization;

namespace appData
{
    public partial class Library
    {
        public partial class Data
        {
            public partial class List
            {
                public class Users
                {
                    public Data.UserProfile[] UserProfiles;
                }

                public class Wallets
                {
                    public Data.WalletProfile[] WalletProfiles;
                }
            }
        }
    }
}