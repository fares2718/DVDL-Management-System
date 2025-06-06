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
	public class clsCountryData
	{
		public static bool GetCountryInfoByID(int ID, ref string CountryName)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = "SELECT * FROM Countries WHERE CountryID = @CountryID";

			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@CountryID", ID);

			try
			{
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{

					// The record was found
					isFound = true;

					CountryName = (string)reader["CountryName"];
				}
				else
				{
					// The record was not found
					isFound = false;
				}

				reader.Close();


			}
			catch (Exception ex)
			{
				//Console.WriteLine("Error: " + ex.Message);
				isFound = false;
			}
			finally
			{
				connection.Close();
			}

			return isFound;
		}


		public static bool GetCountryInfoByName(string CountryName, ref int ID)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = "SELECT * FROM Countries WHERE CountryName = @CountryName";

			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@CountryName", CountryName);

			try
			{
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				if (reader.Read())
				{

					// The record was found
					isFound = true;

					ID = (int)reader["CountryID"];

				}
				else
				{
					// The record was not found
					isFound = false;
				}

				reader.Close();


			}
			catch (Exception ex)
			{
				//Console.WriteLine("Error: " + ex.Message);
				isFound = false;
			}
			finally
			{
				connection.Close();
			}

			return isFound;
		}

		public static DataTable GetAllCountries()
		{

			DataTable dt = new DataTable();
			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = "SELECT * FROM Countries order by CountryName";

			SqlCommand command = new SqlCommand(query, connection);

			try
			{
				connection.Open();

				SqlDataReader reader = command.ExecuteReader();

				if (reader.HasRows)

				{
					dt.Load(reader);
				}

				reader.Close();


			}

			catch (Exception ex)
			{
				// Console.WriteLine("Error: " + ex.Message);
			}
			finally
			{
				connection.Close();
			}

			return dt;

		}



		public static bool IsCountryExist(int ID)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = "SELECT Found=1 FROM Countries WHERE CountryID = @CountryID";

			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@CountryID", ID);

			try
			{
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				isFound = reader.HasRows;

				reader.Close();
			}
			catch (Exception ex)
			{
				//Console.WriteLine("Error: " + ex.Message);
				isFound = false;
			}
			finally
			{
				connection.Close();
			}

			return isFound;
		}


		public static bool IsCountryExist(string CountryName)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = "SELECT Found=1 FROM Countries WHERE CountryName = @CountryName";

			SqlCommand command = new SqlCommand(query, connection);

			command.Parameters.AddWithValue("@CountryName", CountryName);

			try
			{
				connection.Open();
				SqlDataReader reader = command.ExecuteReader();

				isFound = reader.HasRows;

				reader.Close();
			}
			catch (Exception ex)
			{
				//Console.WriteLine("Error: " + ex.Message);
				isFound = false;
			}
			finally
			{
				connection.Close();
			}

			return isFound;
		}

		public static int GetCountryID(string CountryName)
		{
			int ID = -1;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"SELECT CountryID FROM Countries WHERE CountryName=@CountryName;";

			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@CountryName",CountryName);

			try
			{
				connection.Open();
				object result = cmd.ExecuteScalar();

				if (result != null && int.TryParse(result.ToString(), out int PreID))
				{
					ID = PreID;
				}
			}
			catch (Exception ex)
			{
				
			}
			finally
			{
				connection.Close() ;
			}
			return ID;
		}

	}
}
