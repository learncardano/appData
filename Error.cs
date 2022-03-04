using System;
using System.Globalization;

namespace appData
{
    public partial class Library
    {
        public partial class Error
        {
            public enum ErrorCodes
            {
                UserIDNotFound = 101,
                UserCantDelete = 102,
                UserLoginIDNotFound = 103,
                UserLoginIDExists = 104,
                UserProfileIncomplete = 105,
                UserCountryInvalid = 106,
                UserPasswordTooShort = 107,
                WalletMnemonicInvalid = 201,
                WalletIDNotFound = 202,
                WalletAlreadyExists = 203,
                AddressInvalid = 301,
                TransactionIDInvalid = 401,
                InvalidDateFormat = 901,
                InvalidAmount = 902,
                PasswordNotAccepted = 903,
                PasswordsMismatched = 904,
                APIError = 998,
                ExceptionError = 999
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Returns the description of an Error Code.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public String Description(ErrorCodes errorCode)
            {
                switch (errorCode)
                {
                    case Error.ErrorCodes.UserIDNotFound: 
                        return "User ID does not exist.";
                    case Error.ErrorCodes.UserCantDelete:
                        return "User can't be deleted. Possibly a Wallet exists for this user.";
                    case Error.ErrorCodes.UserLoginIDNotFound:
                        return "Login ID does not exist.";
                    case Error.ErrorCodes.UserLoginIDExists:
                        return "Login ID already exists.";
                    case Error.ErrorCodes.UserProfileIncomplete:
                        return "Missing User Information.";
                    case Error.ErrorCodes.UserCountryInvalid:
                        return "Invalid Country Code.";
                    case Error.ErrorCodes.UserPasswordTooShort:
                        return "Password must be at least 10 characters long.";
                    case Error.ErrorCodes.WalletMnemonicInvalid:
                        return "Invalid Mnemonic Sentence or Recovery Phrase.";
                    case Error.ErrorCodes.WalletIDNotFound:
                        return "Wallet ID does not exist.";
                    case Error.ErrorCodes.WalletAlreadyExists:
                        return "Wallet already exists for this user.";
                    case Error.ErrorCodes.AddressInvalid:
                        return "Invalid or Missing Wallet Address.";
                    case Error.ErrorCodes.TransactionIDInvalid:
                        return "Invalid Transaction ID.";
                    case Error.ErrorCodes.InvalidDateFormat:
                        return "Invalid Date Format.";
                    case Error.ErrorCodes.InvalidAmount:
                        return "Invalid Amount.";
                    case Error.ErrorCodes.PasswordNotAccepted:
                        return "Password is unaccepted or invalid.";
                    case Error.ErrorCodes.PasswordsMismatched:
                        return "Passwords do not match.";
                    case Error.ErrorCodes.APIError:
                        return "API Error: ";
                    case Error.ErrorCodes.ExceptionError: 
                        return "System Error Encountered: ";
                    default: 
                        return "";
                }
            }
        }
    }
}