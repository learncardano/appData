using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace appData
{
    public partial class Library
    {
        // ===========================================================================================
        /// <summary>
        /// Class library for accessing Credential table.
        /// </summary>
        //
        // Must add reference via NuGet:
        // - System.Data.SqlClient
        // - System.Configuration.ConfigurationManager
        // ===========================================================================================
        public class Credential
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves Credential of a given Credential ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.Credential Get(Int32 credentialID)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("CredentialGet", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@CredentialID", SqlDbType.Int).Value = credentialID;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@TokenID", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@TokenExpiry", SqlDbType.DateTime).Direction = ParameterDirection.Output;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }

                Data.Credential credential = new Data.Credential();

                credential.CredentialID = credentialID;
                credential.UserName = (String)cmd.Parameters["@UserName"].Value;
                credential.Password = (String)cmd.Parameters["@Password"].Value;
                credential.TokenID = (String)cmd.Parameters["@TokenID"].Value;
                credential.TokenExpiry = Convert.ToDateTime(cmd.Parameters["@TokenExpiry"].Value);

                return credential;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves Credential ID of a given User Name and Password.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 GetID(String userName, String password)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("CredentialGetID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Value = password;
                cmd.Parameters.Add("@CredentialID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }

                return Convert.ToInt32(cmd.Parameters["@CredentialID"].Value);
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Saves User Name and Password and returns the assigned Credential ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 Save(String userName, String password)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("CredentialSave", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50).Value = userName;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Value = password;
                cmd.Parameters.Add("@CredentialID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }

                return Convert.ToInt32(cmd.Parameters["@CredentialID"].Value);
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Updates Token ID and Token Expiry of a given Credential ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public void Update(Int32 credentialID, String tokenID, String tokenExpiry)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("CredentialUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@CredentialID", SqlDbType.Int).Value = credentialID;
                cmd.Parameters.Add("@TokenID", SqlDbType.NVarChar, 250).Value = tokenID;
                cmd.Parameters.Add("@TokenExpiry", SqlDbType.DateTime).Value = tokenExpiry;

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