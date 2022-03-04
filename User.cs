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
        /// Class library for accessing UserProfile table.
        /// </summary>
        //
        // Must add reference via NuGet:
        // - System.Data.SqlClient
        // - System.Configuration.ConfigurationManager
        // ===========================================================================================
        public class User
        {
            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Deletes User Profile of a given User ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public void Delete(Int32 userID)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("UserDelete", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;

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
            /// Retrieves User Profile of a given User ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.UserProfile Get(Int32 userID)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("UserGet", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userID;
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 100).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@DateRegistered", SqlDbType.DateTime).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Country", SqlDbType.Char, 2).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@CypherKey", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }

                Data.UserProfile userProfile = new Data.UserProfile();

                userProfile.UserID = userID;
                userProfile.LoginID = (String)cmd.Parameters["@LoginID"].Value;
                userProfile.Password = (String)cmd.Parameters["@Password"].Value;
                userProfile.FirstName = (String)cmd.Parameters["@FirstName"].Value;
                userProfile.LastName = (String)cmd.Parameters["@LastName"].Value;
                userProfile.DateRegistered = Convert.ToDateTime(cmd.Parameters["@DateRegistered"].Value);
                userProfile.Country = (String)cmd.Parameters["@Country"].Value;
                userProfile.CypherKey = (String)cmd.Parameters["@CypherKey"].Value;

                return userProfile;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves User ID of a given Login ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 GetID(String loginID)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("UserGetID", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 100).Value = loginID;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }

                return Convert.ToInt32(cmd.Parameters["@UserID"].Value);
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves list of all Users.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public DataView GetList()
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("UserList", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                DataTable dt = new DataTable("UserList");

                dt.Columns.Add(new DataColumn("UserID"));
                dt.Columns.Add(new DataColumn("LoginID"));
                dt.Columns.Add(new DataColumn("Password"));
                dt.Columns.Add(new DataColumn("FirstName"));
                dt.Columns.Add(new DataColumn("LastName"));
                dt.Columns.Add(new DataColumn("DateRegistered"));
                dt.Columns.Add(new DataColumn("Country"));
                dt.Columns.Add(new DataColumn("CypherKey"));

                while (dr.Read())
                {
                    DataRow row = dt.NewRow();

                    row["UserID"] = Convert.ToInt32(dr.GetValue(0));
                    row["LoginID"] = dr.GetString(1);
                    row["Password"] = dr.GetString(2);
                    row["FirstName"] = dr.GetString(3);
                    row["LastName"] = dr.GetString(4);
                    row["DateRegistered"] = Convert.ToDateTime(dr.GetValue(5));
                    row["Country"] = dr.GetString(6);
                    row["CypherKey"] = dr.GetString(7);

                    dt.Rows.Add(row);
                }

                dr.Close();

                DataView dv = new DataView(dt);
                dv.Sort = "DateRegistered ASC";
                return dv;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Retrieves the list of users in object array format sorted in ascending order by date or id.
            /// If hideKeys is true (default setting), this will hide both Password and Cypher Key.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Data.List.Users List()
            {
                return List(true);
            }

            public Data.List.Users List(Boolean hideKeys)
            {
                DataView dv = GetList();

                List<Data.UserProfile> userList = new List<Data.UserProfile>();
                userList = (from DataRow dr in dv.ToTable().Rows
                              select new Data.UserProfile()
                              {
                                  UserID = Convert.ToInt32(dr["UserID"]),
                                  LoginID = (String)dr["LoginID"],
                                  Password = (hideKeys ? "" : (String)dr["Password"]),
                                  FirstName = (String)dr["FirstName"],
                                  LastName = (String)dr["LastName"],
                                  DateRegistered = Convert.ToDateTime(dr["DateRegistered"]),
                                  Country = (String)dr["Country"],
                                  CypherKey = (hideKeys ? "" : (String)dr["CypherKey"])
                              }).ToList();

                Data.List.Users listUsers = new Data.List.Users();
                listUsers.UserProfiles = userList.ToArray();

                return listUsers;
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Saves User Profile and returns the assigned User ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public Int32 Save(Library.Data.UserProfile userProfile)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("UserSave", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 100).Value = userProfile.LoginID;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Value = userProfile.Password;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = userProfile.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = userProfile.LastName;
                cmd.Parameters.Add("@DateRegistered", SqlDbType.DateTime).Value = userProfile.DateRegistered;
                cmd.Parameters.Add("@Country", SqlDbType.Char, 2).Value = userProfile.Country;
                cmd.Parameters.Add("@CypherKey", SqlDbType.NVarChar, 50).Value = userProfile.CypherKey;
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Direction = ParameterDirection.Output;

                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    con.Close();
                }

                return Convert.ToInt32(cmd.Parameters["@UserID"].Value);
            }

            // ---------------------------------------------------------------------------------------
            /// <summary>
            /// Updates User Profile of the given User ID.
            /// </summary>
            // ---------------------------------------------------------------------------------------
            public void Update(Library.Data.UserProfile userProfile)
            {
                // Create Instance of Connection and Command Object
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConnection"].ConnectionString);
                SqlCommand cmd = new SqlCommand("UserUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Add Parameters to SPROC
                cmd.Parameters.Add("@UserID", SqlDbType.Int).Value = userProfile.UserID;
                cmd.Parameters.Add("@LoginID", SqlDbType.NVarChar, 100).Value = userProfile.LoginID;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 250).Value = userProfile.Password;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50).Value = userProfile.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 50).Value = userProfile.LastName;
                cmd.Parameters.Add("@DateRegistered", SqlDbType.DateTime).Value = userProfile.DateRegistered;
                cmd.Parameters.Add("@Country", SqlDbType.Char, 2).Value = userProfile.Country;

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