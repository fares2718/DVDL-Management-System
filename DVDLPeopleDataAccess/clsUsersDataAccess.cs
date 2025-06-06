using DVDLDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLDataAccess
{
	public class clsUsersDataAccess
	{
		public static bool GetUserByID(int UserID,ref int PersonID,ref string Username,ref string Password,ref bool IsActive)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = @"SELECT * FROM Users
							 WHERE UserID=@UserID";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@UserID", UserID);

			try
			{
				connection.Open();

				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.Read())
				{
					isFound = true;
					int.TryParse(reader["PersonID"].ToString(), out PersonID);
					Username = reader["Username"].ToString();
					Password = reader["Password"].ToString();
					IsActive = Convert.ToBoolean(reader["IsActive"]);
				}
				else
					isFound = false;
				reader.Close();

			}
			catch (Exception ex)
			{
				isFound = false;
			}
			finally
			{
				connection.Close();
			}
			return isFound;
		}

		public static bool GetUserByUsername(ref int UserID, ref int PersonID,string Username, ref string Password, ref bool IsActive)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = @"SELECT * FROM Users 
							 where Username = @Username";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@Username", Username);

			try
			{
				connection.Open();

				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.Read())
				{
					isFound = true;
					int.TryParse(reader["UserID"].ToString().Trim(), out UserID);
					int.TryParse(reader["PersonID"].ToString(), out PersonID);
					Password = reader["Password"].ToString();
					IsActive = Convert.ToBoolean(reader["IsActive"]);

				}
				else
					isFound = false;
				reader.Close();

			}
			catch (Exception ex)
			{
				isFound = false;
			}
			finally
			{
				connection.Close();
			}
			return isFound;
		}

		public static int AddUser(int PersonID,string Username,string Password,bool IsActive)
		{
			if (isUserExists(Username))
			{
				return -1;
			}

			int UserID = -1;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = @"INSERT INTO Users (PersonID,Username,Password,IsActive)
							VALUES (@PersonID,@Username,@Password,@IsActive);
							SELECT SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@PersonID", PersonID);
			cmd.Parameters.AddWithValue("@Username", Username);
			cmd.Parameters.AddWithValue("@Password", Password);
			cmd.Parameters.AddWithValue("@IsActive", IsActive);
			try
			{
				connection.Open();
				object result = cmd.ExecuteScalar();

				if (result != null && int.TryParse(result.ToString(), out int insertedID))
				{
					UserID = insertedID;
				}
			}
			catch (Exception ex)
			{ }
			finally { connection.Close(); }


			return UserID;
		}

		public static bool UpdateUser(int UserID,string Username,string Password,bool isActive)
		{
			int rowAffected = 0;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"UPDATE Users
							set Username=@Username,
								Password=@Password
								WHERE UserID=@UserID";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@Username", Username);
			cmd.Parameters.AddWithValue("@Password", Password);
			cmd.Parameters.AddWithValue("@UserID", UserID);
			try
			{
				connection.Open();
				rowAffected = cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{

			}
			finally
			{
				connection.Close();
			}
			return (rowAffected > 0);
		}

		public static bool DeleteUser(int UserID)
		{
			int rowAffected = 0;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"DELETE Users 
									where UserID=@UserID";

			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@UserID", UserID);

			try
			{
				connection.Open();
				rowAffected = cmd.ExecuteNonQuery();

			}
			catch (Exception ex)
			{

			}
			finally
			{
				connection.Close();
			}
			return (rowAffected > 0);

		}

		public static DataTable GetAllUsers()
		{
			DataTable dt = new DataTable();
			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"SELECT Users.UserID, People.PersonID, (People.FirstName+' '+ People.LastName) AS FullName,
								Users.Username, Users.IsActive
								FROM People INNER JOIN
								Users ON People.PersonID = Users.PersonID";
			SqlCommand cmd = new SqlCommand(query, connection);

			try
			{
				connection.Open();
				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.HasRows)
				{
					dt.Load(reader);
				}
				reader.Close();
			}
			catch (Exception ex)
			{
			}
			finally
			{
				connection.Close();
			}
			return dt;
		}

		public static bool isUserExists(int UserID)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string qyery = @"SELECT FOUND=1 FROM Users
									where UserID=@UserID";

			SqlCommand cmd = new SqlCommand(qyery, connection);
			cmd.Parameters.AddWithValue("@UserID", UserID);

			try
			{
				connection.Open();
				SqlDataReader reader = cmd.ExecuteReader();

				isFound = reader.HasRows;
				reader.Close();
			}
			catch (Exception ex)
			{
				isFound = false;
			}
			finally
			{
				connection.Close();
			}

			return isFound;
		}

		public static bool isUserExists(string Username)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string qyery = @"SELECT FOUND=1 FROM Users
									where Username=@Username";

			SqlCommand cmd = new SqlCommand(qyery, connection);
			cmd.Parameters.AddWithValue("@Username", Username);

			try
			{
				connection.Open();
				SqlDataReader reader = cmd.ExecuteReader();

				isFound = reader.HasRows;
				reader.Close();
			}
			catch (Exception ex)
			{
				isFound = false;
			}
			finally
			{
				connection.Close();
			}

			return isFound;
		}

		public static bool isPersonanUser(int ID)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string qyery = @"SELECT FOUND=1 FROM Users
									where PersonID=@PersonID";

			SqlCommand cmd = new SqlCommand(qyery, connection);
			cmd.Parameters.AddWithValue("@PersonID", ID);

			try
			{
				connection.Open();
				SqlDataReader reader = cmd.ExecuteReader();

				isFound = reader.HasRows;
				reader.Close();
			}
			catch (Exception ex)
			{
				isFound = false;
			}
			finally
			{
				connection.Close();
			}

			return isFound;
		}

	}
}
