using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVDLDataAccessLayer;

namespace DVDLDataAccess
{
    public class clsPeopleDataAccess
    {
		public static bool GetPersonByID(int PersonID, ref string NationalNo,
		   ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
		   ref DateTime BirthDate, ref Byte Gendor, ref string Address, ref string Phone, ref string Email,
		   ref int NationalityCountryID, ref string ImagePath)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = @"select * from People where PersonID = @PersonID";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@PersonID", PersonID);

			try
			{
				connection.Open();

				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.Read())
				{
					isFound = true;
					NationalNo = reader["NationalNo"].ToString();
					FirstName = reader["FirstName"].ToString();
					SecondName = reader["SecondName"].ToString();
					if (reader["ThirdName"] != DBNull.Value)
					{
						ThirdName = reader["ThirdName"].ToString();
					}
					else
					{
						ThirdName = "";
					}
					LastName = reader["LastName"].ToString();
					DateTime.TryParse(reader["DateOfBirth"].ToString().Trim(), out BirthDate);
					Byte.TryParse(reader["Gendor"].ToString().Trim(), out Gendor);
					Phone = reader["Phone"].ToString();
					Address = reader["Address"].ToString();
					int.TryParse(reader["NationalityCountryID"].ToString().Trim(), out NationalityCountryID);
					if (reader["Email"] != DBNull.Value)
					{
						Email = reader["Email"].ToString();
					}
					else
					{
						Email = "";
					}
					if (reader["ImagePath"] != DBNull.Value)
					{
						ImagePath = reader["ImagePath"].ToString();
					}
					else
					{
						ImagePath = "";
					}

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

		public static bool GetPersonByNatinalNo(ref int PersonID, string NationalNo,
		ref string FirstName, ref string SecondName, ref string ThirdName, ref string LastName,
		ref DateTime BirthDate, ref Byte Gendor, ref string Address, ref string Phone, ref string Email,
		ref int NationalityCountryID, ref string ImagePath)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = @"select * from People where NationalNo = @NationalNo";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@NationalNo", NationalNo);

			try
			{
				connection.Open();

				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.Read())
				{
					isFound = true;
					int.TryParse(reader["PersonID"].ToString().Trim(), out PersonID);
					FirstName = reader["FirstName"].ToString();
					SecondName = reader["SecondName"].ToString();
					if (reader["ThirdName"] != DBNull.Value)
					{
						ThirdName = reader["ThirdName"].ToString();
					}
					else
					{
						ThirdName = "";
					}
					LastName = reader["LastName"].ToString();
					DateTime.TryParse(reader["DateOfBirth"].ToString().Trim(), out BirthDate);
					Byte.TryParse(reader["Gendor"].ToString().Trim(), out Gendor);
					Phone = reader["Phone"].ToString();
					Address = reader["Address"].ToString();
					int.TryParse(reader["NationalityCountryID"].ToString().Trim(), out NationalityCountryID);
					if (reader["Email"] != DBNull.Value)
					{
						Email = reader["Email"].ToString();
					}
					else
					{
						Email = "";
					}
					if (reader["ImagePath"] != DBNull.Value)
					{
						ImagePath = reader["ImagePath"].ToString();
					}
					else
					{
						ImagePath = "";
					}

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

		public static int AddPerson(string NationalNo,
	string FirstName, string SecondName, string ThirdName, string LastName,
	DateTime BirthDate, Byte Gendor, string Address, string Phone, string Email,
	int NationalityCountryID, string ImagePath)
		{
			if(isPersonExists(NationalNo))
			{
				return -1;
			}

			int PersonID = -1;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = @"INSERT INTO People (NationalNo,FirstName,SecondName,ThirdName,LastName,DateOfBirth,Gendor,Address,Phone,
							Email,NationalityCountryID,ImagePath)
							VALUES (@NationalNo,@FirstName,@SecondName,@ThirdName,@LastName,@DateOfBirth,@Gendor,@Address,@Phone,
							@Email,@NationalityCountryID,@ImagePath);
							SELECT SCOPE_IDENTITY();";

			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@NationalNo", NationalNo);
			cmd.Parameters.AddWithValue("@FirstName", FirstName);
			cmd.Parameters.AddWithValue("@SecondName", SecondName);
			cmd.Parameters.AddWithValue("@LastName", FirstName);
			cmd.Parameters.AddWithValue("@DateOfBirth", BirthDate);
			cmd.Parameters.AddWithValue("@Gendor", Gendor);
			cmd.Parameters.AddWithValue("@Phone", Phone);
			cmd.Parameters.AddWithValue("@Address", Address);
			cmd.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
			if (Email != "")
				cmd.Parameters.AddWithValue("@Email", Email);
			else
				cmd.Parameters.AddWithValue("@Email", System.DBNull.Value);
			if (ThirdName != "")
				cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
			else
				cmd.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

			if (ImagePath != "")
				cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
			else
				cmd.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
			try
			{
				connection.Open();
				object result = cmd.ExecuteScalar();

				if (result != null && int.TryParse(result.ToString(), out int insertedID))
				{
					PersonID = insertedID;
				}
			}
			catch (Exception ex)
			{ }
			finally { connection.Close(); }


			return PersonID;
		}

		public static bool UpdatePerson(int PersonID, string NationalNo,
	string FirstName, string SecondName, string ThirdName, string LastName,
	DateTime BirthDate, Byte Gendor, string Address, string Phone, string Email,
	int NationalityCountryID, string ImagePath)
		{
			int rowAffected = 0;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"UPDATE People
							set NationalNo=@NationalNo,
								FirstName = @FirstName, 
								SecondName=@SecondName,
								ThirdName=@ThirdName,
								LastName=@LastName,
								Gendor=@Gendor,
								Email = @Email, 
                                Phone = @Phone, 
                                Address = @Address, 
                                DateOfBirth = @DateOfBirth,
								NationalityCountryID=@NationalityCountryID,
								ImagePath=@ImagePath
								WHERE PersonID=@PersonID";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@PersonID", PersonID);
			cmd.Parameters.AddWithValue("@NatinalNo", NationalNo);
			cmd.Parameters.AddWithValue("@FirstName", FirstName);
			cmd.Parameters.AddWithValue("@SecondName", SecondName);
			cmd.Parameters.AddWithValue("@LastName", FirstName);
			cmd.Parameters.AddWithValue("@DateOfBith", BirthDate);
			cmd.Parameters.AddWithValue("@Gendor", Gendor);
			cmd.Parameters.AddWithValue("@Phone", Phone);
			cmd.Parameters.AddWithValue("@Address", Address);
			cmd.Parameters.AddWithValue("@NatinalCountryID", NationalityCountryID);
			if (Email != "")
				cmd.Parameters.AddWithValue("@Email", Email);
			else
				cmd.Parameters.AddWithValue("@Email", System.DBNull.Value);
			if (ThirdName != "")
				cmd.Parameters.AddWithValue("@ThirdName", ThirdName);
			else
				cmd.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

			if (ImagePath != "")
				cmd.Parameters.AddWithValue("@ImagePath", ImagePath);
			else
				cmd.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
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

		public static bool DeletePerson(int PersonID)
		{
			int rowAffected = 0;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"DELETE People 
									where PersonID=@PersonID";

			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@PersonID", PersonID);

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

		public static DataTable GetAllPeople()
		{
			DataTable dt = new DataTable();
			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"SELECT * FROM People";
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

		public static bool isPersonExists(int PersonID)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string qyery = @"SELECT FOUND=1 FROM People
									where PersonID=@PersonID";

			SqlCommand cmd = new SqlCommand(qyery, connection);
			cmd.Parameters.AddWithValue("@PersonID", PersonID);

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

		public static bool isPersonExists(string NationalNo)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string qyery = @"SELECT FOUND=1 FROM People
									where NationalNo=@NationalNo";

			SqlCommand cmd = new SqlCommand(qyery, connection);
			cmd.Parameters.AddWithValue("@NationalNo", NationalNo);

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
