using DVDLDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLPeopleDataAccess
{
	public class clsApplicationsDataAccess
	{


		#region GetData
		public static DataTable GetAllApplications()
		{
			DataTable dt = new DataTable();
			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"SELECT Applications.ApplicationID, (People.FirstName+' ' +People.LastName) as FullName, People.NationalNo, ApplicationTypes.ApplicationTypeTitle, Applications.ApplicationStatus, ApplicationTypes.ApplicationFees, Applications.PaidFees, 
                         Applications.ApplicationDate, Applications.LastStatusDate, Users.Username
						 FROM Applications INNER JOIN
                         ApplicationTypes ON Applications.ApplicationTypeID = ApplicationTypes.ApplicationTypeID INNER JOIN
                         People ON Applications.ApplicantPersonID = People.PersonID INNER JOIN
                         Users ON Applications.CreatedByUserID = Users.UserID ";
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

		public static bool GetApplicationByNationalNo(string NationalNo, ref int AppID, ref DateTime AppDate,
			ref string AppTitle, ref decimal AppFees, ref Byte AppStatus, ref DateTime LastStatusDate, ref decimal PaidFees, ref string Username)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = @"SELECT Applications.ApplicationID,  People.NationalNo, ApplicationTypes.ApplicationTypeTitle, Applications.ApplicationStatus, ApplicationTypes.ApplicationFees, Applications.PaidFees, 
                         Applications.ApplicationDate, Applications.LastStatusDate, Users.Username
						 FROM Applications INNER JOIN
                         ApplicationTypes ON Applications.ApplicationTypeID = ApplicationTypes.ApplicationTypeID INNER JOIN
                         People ON Applications.ApplicantPersonID = People.PersonID INNER JOIN
                         Users ON Applications.CreatedByUserID = Users.UserID
						 WHERE NationalNo = @NationalNo;";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@NationalNo", NationalNo);

			try
			{
				connection.Open();

				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.Read())
				{
					isFound = true;
					int.TryParse(reader["ApplicationID"].ToString(), out AppID);
					Byte.TryParse(reader["ApplicationStatus"].ToString(),out AppStatus);
					DateTime.TryParse(reader["ApplicationDate"].ToString(), out AppDate);
					DateTime.TryParse(reader["LastStatusDate"].ToString(), out LastStatusDate);
					AppTitle = reader["ApplicationTypeTitle"].ToString();
					Username = reader["Username"].ToString() ;
					decimal.TryParse(reader["ApplicationFees"].ToString() , out AppFees);
					decimal.TryParse(reader["PaidFees"].ToString(), out PaidFees);
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

		public static bool GetApplicationByApplicationID(int AppID, ref string NationalNo, ref DateTime AppDate,
	ref string AppTitle, ref decimal AppFees, ref Byte AppStatus, ref DateTime LastStatusDate, ref decimal PaidFees, ref string Username)
		{
			bool isFound = false;

			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = @"SELECT Applications.ApplicationID, People.NationalNo,(People.FirstName + ' '+People.LastName) AS FullName ,ApplicationTypes.ApplicationTypeTitle, Applications.ApplicationStatus, ApplicationTypes.ApplicationFees, Applications.PaidFees, 
                         Applications.ApplicationDate, Applications.LastStatusDate, Users.Username
						 FROM Applications INNER JOIN
                         ApplicationTypes ON Applications.ApplicationTypeID = ApplicationTypes.ApplicationTypeID INNER JOIN
                         People ON Applications.ApplicantPersonID = People.PersonID INNER JOIN
                         Users ON Applications.CreatedByUserID = Users.UserID AND People.PersonID = Users.PersonID
						 WHERE ApplicationID = @ApplicationID;";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@ApplicationID", AppID);

			try
			{
				connection.Open();

				SqlDataReader reader = cmd.ExecuteReader();

				if (reader.Read())
				{
					isFound = true;
					Byte.TryParse(reader["ApplicationStatus"].ToString(), out AppStatus);
					DateTime.TryParse(reader["ApplicationDate"].ToString(), out AppDate);
					DateTime.TryParse(reader["LastStatusDate"].ToString(), out LastStatusDate);
					AppTitle = reader["ApplicationTypeTitle"].ToString();
					Username = reader["Username"].ToString();
					NationalNo = reader["NationalNo"].ToString();
					decimal.TryParse(reader["ApplicationFees"].ToString(), out AppFees);
					decimal.TryParse(reader["PaidFees"].ToString(), out PaidFees);
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

		public struct stApplicationType
		{
			public int ApplicationTypeID;
			public string ApplicationTypeTitle;
			public decimal ApplicationFees;

		};

		public static bool GetApplicationTypeInfo(string ApplicationTitle, ref int ApplicationTypeID,ref decimal ApplicationFees)
		{
			bool isFound=false;
			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"SELECT * FROM ApplicationTypes
							 WHERE ApplicationTypeTitle = @ApplicationTypeTitle";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTitle);
			try
			{
				isFound=true;
				connection.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if (reader.Read())
				{
					int.TryParse(reader["ApplicationTypeID"].ToString(), out ApplicationTypeID);
					decimal.TryParse(reader["ApplicationFees"].ToString(), out ApplicationFees);
					ApplicationTitle = reader["ApplicationTypeTitle"].ToString();
				}
				reader.Close();
			}
			catch (Exception ex) {
				isFound = false;
			}
			finally
			{
				connection.Close();
			}
			return isFound;
		}

		public static DataTable GetApplicationTypes()
		{
			DataTable dt = new DataTable();
			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query=@"SELECT * From ApplicationTypes order by ApplicationTypeTitle";
			SqlCommand cmd = new SqlCommand(query, connection);
			try
			{
				connection.Open();
				SqlDataReader reader = cmd.ExecuteReader();
				if(reader.HasRows)
				{
					dt.Load(reader);
				}
				reader.Close();
			}
			catch (Exception ex) { }
			finally
			{
				connection.Close();
			}
			return dt;
		}


		#endregion

		#region UpdateData
		public static int AddApplication(int ApplicantID,int applicationTypeID,int UserID,DateTime applicationDate,
			DateTime lastStatusDate,decimal paidFees)
		{
			int applicationID = -1;
			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);

			string query = @"INSERT INTO Applications 
							 (ApplicantPersonID,ApplicationDate,ApplicationTypeID,ApplicationStatus,
							  LastStatusDate,PaidFees,CreatedByUserID)
							  VALUES (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,@ApplicationStatus,
							  @LastStatusDate,@PaidFees,@CreatedByUserID);
							  SELECT SCOPE_IDENTITY();";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@ApplicantPersonID", ApplicantID);
			cmd.Parameters.AddWithValue("@ApplicationTypeID", applicationTypeID);
			cmd.Parameters.AddWithValue("@ApplicationDate", applicationDate);
			cmd.Parameters.AddWithValue("@ApplicationStatus", 1);
			cmd.Parameters.AddWithValue("@LastStatusDate", lastStatusDate);
			cmd.Parameters.AddWithValue("@PaidFees", paidFees);
			cmd.Parameters.AddWithValue("@CreatedByUserID", UserID);
			try
			{
				connection.Open();
				object result = cmd.ExecuteScalar();
				if(result != null&&int.TryParse(result.ToString(),out int ID))
				{
					applicationID = ID;
				}
			}catch (Exception ex)
			{

			}
			finally
			{
				connection.Close();
			}
			return applicationID;
		}

		public static bool Cancele(int applicationID)
		{
			int rowAffected = 0;
			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"UPDATE Applications
							 SET ApplicationStatus = @ApplicationStatus,
								 LastStatusDate = @LastStatusDate
								 WHERE ApplicationID = @ApplicationID";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@ApplicationStatus", 3);
			cmd.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);
			cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
			try
			{
				connection.Open();
				rowAffected = cmd.ExecuteNonQuery();
			}
			catch (Exception ex) { }
			finally
			{
				connection.Close();
			}
			return (rowAffected > 0);
		}

		public static bool UpdatePaidFees(int applicationID, decimal PaidFees)
		{
			int rowAffected = 0;
			SqlConnection connection = new SqlConnection(clsDataAccessSetteing.ConnectionString);
			string query = @"UPDATE Applications
							 SET PaidFees = @PaidFees
								 WHERE ApplicationID = @ApplicationID";
			SqlCommand cmd = new SqlCommand(query, connection);
			cmd.Parameters.AddWithValue("@PaidFees", PaidFees);
			cmd.Parameters.AddWithValue("@ApplicationID", applicationID);
			try
			{
				connection.Open();
				rowAffected = cmd.ExecuteNonQuery();
			}
			catch (Exception ex) { }
			finally
			{
				connection.Close();
			}
			return (rowAffected > 0);
		}

		#endregion

	}
}



