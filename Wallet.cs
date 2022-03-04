using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace appData
{
    public partial class Library
    {
        // ===========================================================================================
        /// <summary>
        /// Class library for accessing WalletProfile table.
        /// </summary>
        //
        // Must add reference via NuGet:
        // - System.Data.SqlClient
        // - System.Configuration.ConfigurationManager
        // ===========================================================================================
        public class Wallet
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Deletes Wallet Profile of a given Wallet ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public void Delete(Int32 walletID)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("WalletDelete", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@WalletID", SqlDbType.Int).Value = walletID;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves Wallet Profile of a given Wallet ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.WalletProfile Get(Int32 walletID)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("WalletGet", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@WalletID", SqlDbType.Int).Value = walletID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@CardanoID", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@RecoveryPhrase", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@MnemonicPassword", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }

                Data.WalletProfile walletProfile = new Data.WalletProfile();

                walletProfile.WalletID = walletID;
                walletProfile.UserID = Convert.ToInt32(cmd.Parameters["@UserID"].Value);
                walletProfile.CardanoID = (String)cmd.Parameters["@CardanoID"].Value;
                walletProfile.Password = (String)cmd.Parameters["@Password"].Value;
                walletProfile.RecoveryPhrase = (String)cmd.Parameters["@RecoveryPhrase"].Value;
                walletProfile.MnemonicPassword = (String)cmd.Parameters["@MnemonicPassword"].Value;

                return walletProfile;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves Wallet ID of a given User ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 GetID(Int32 userID)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("WalletGetID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@WalletID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }

                return Convert.ToInt32(cmd.Parameters["@WalletID"].Value);
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves list of all Wallets.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public DataView GetList()
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("WalletList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                DataTable dt = new DataTable("WalletList");

                dt.Columns.Add(new DataColumn("WalletID"));
                dt.Columns.Add(new DataColumn("UserID"));
                dt.Columns.Add(new DataColumn("CardanoID"));
                dt.Columns.Add(new DataColumn("Password"));
                dt.Columns.Add(new DataColumn("RecoveryPhrase"));
                dt.Columns.Add(new DataColumn("MnemonicPassword"));

                while (dr.Read())
                {
                    DataRow row = dt.NewRow();

                    row["WalletID"] = Convert.ToInt32(dr.GetValue(0));
                    row["UserID"] = Convert.ToInt32(dr.GetValue(1));
                    row["CardanoID"] = dr.GetString(2);
                    row["Password"] = dr.GetString(3);
                    row["RecoveryPhrase"] = dr.GetString(4);
                    row["MnemonicPassword"] = dr.GetString(5);

                    dt.Rows.Add(row);
                }

                dr.Close();

                DataView dv = new DataView(dt);
                dv.Sort = "WalletID ASC";
                return dv;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves the list of wallets in object array format sorted in ascending order by id.
            /// If hideKeys is true (default setting), this will hide both Password and Recovery Phrase.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.List.Wallets List()
            {
                return List(true);
            }

            public Data.List.Wallets List(Boolean hideKeys)
            {
                DataView dv = GetList();

                List<Data.WalletProfile> walletList = new List<Data.WalletProfile>();
                walletList = (from DataRow dr in dv.ToTable().Rows
                            select new Data.WalletProfile()
                            {
                                WalletID = Convert.ToInt32(dr["WalletID"]),
                                UserID = Convert.ToInt32(dr["UserID"]),
                                CardanoID = (String)dr["CardanoID"],
                                Password = (hideKeys ? "" : (String)dr["Password"]),
                                RecoveryPhrase = (hideKeys ? "" : (String)dr["RecoveryPhrase"]),
                                MnemonicPassword = (hideKeys ? "" : (String)dr["MnemonicPassword"])
                            }).ToList();

                Data.List.Wallets listWallets = new Data.List.Wallets();
                listWallets.WalletProfiles = walletList.ToArray();

                return listWallets;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Saves Wallet Profile and returns the assigned Wallet ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 Save(Library.Data.WalletProfile walletProfile)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("WalletSave", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = walletProfile.UserID;
                cmd.Parameters.Add("@CardanoID", SqlDbType.NVarChar, 250).Value = walletProfile.CardanoID;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Value = walletProfile.Password;
                cmd.Parameters.Add("@RecoveryPhrase", SqlDbType.NVarChar, 250).Value = walletProfile.RecoveryPhrase;
                cmd.Parameters.Add("@MnemonicPassword", SqlDbType.NVarChar, 250).Value = walletProfile.MnemonicPassword;
                cmd.Parameters.Add("@WalletID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }

                return Convert.ToInt32(cmd.Parameters["@WalletID"].Value);
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Updates Wallet Password of the given Wallet ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public void Update(Int32 WalletID, String Password)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("WalletUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@WalletID", SqlDbType.Int).Value = WalletID;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Value = Password;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }
            }
        }
    }
}