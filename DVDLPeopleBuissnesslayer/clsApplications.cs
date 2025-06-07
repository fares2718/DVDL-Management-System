using DVDLPeopleDataAccess;
using DVDLUsersBuissnesslayer;
using DVDLPeopleBuissnesslayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLApplicationsBuissnesslayer
{
	
	public class clsApplications
	{
		public enum enMode { Add,Update}
		#region Properties
		public struct stApplicationType
		{
			public int ApplicationTypeID { get; set; }
			public string ApplicationTypeTitle { get; set; }
			public decimal ApplicationFees { get; set; }

		};
		public stApplicationType ApplicationTypeInfo;
		public int ApplicationID { get; set; }
		public string ApplicantNationalNo { get; set; }
		public string ApplicationsTitle { get; set; }
		public Byte ApplicationStatus	{ get; set; }
		public DateTime ApplicationDate { get; set; }
		public DateTime LastStatusDate	{ get; set; }
		public string CreatorUsername { get; set; }
		public decimal PaidFees { get; set; }
		public clsPerson Person;
		public clsUser User;
		enMode _Mode;
		#endregion

		#region Constructors
		public clsApplications()
		{
			ApplicationID = -1;
			Person = new clsPerson();
			
			ApplicationsTitle=string.Empty;
			ApplicationStatus=0;
			ApplicationDate=DateTime.Now;
			LastStatusDate=DateTime.Now;
			User = new clsUser();
			ApplicationTypeInfo.ApplicationFees = 0;
			PaidFees=0;
			_Mode = enMode.Add;
		}

		clsApplications(int applicationID,string nationalNo,string applicationsTitle,
			Byte applicationStatus,DateTime applicationDate,DateTime lastStatusDate,string creatorUsername,
			decimal paidFees)
		{
			ApplicationID= applicationID;
			Person = clsPerson.Find(nationalNo);
			ApplicantNationalNo=nationalNo;
			ApplicationsTitle=applicationsTitle;
			ApplicationStatus=applicationStatus;
			ApplicationDate=applicationDate;
			LastStatusDate=lastStatusDate;
			User = clsUser.Find(creatorUsername);
			CreatorUsername=creatorUsername;
			PaidFees = paidFees;
			int ApplicationTypeID=-1;
			decimal ApplicationFees=0;
			clsApplicationsDataAccess.GetApplicationTypeInfo(applicationsTitle,
				ref ApplicationTypeID, ref ApplicationFees);
			ApplicationTypeInfo.ApplicationTypeTitle=applicationsTitle;
			ApplicationTypeInfo.ApplicationTypeID=ApplicationTypeID;
			ApplicationTypeInfo.ApplicationFees=ApplicationFees;
			_Mode=enMode.Update;
		}

		#endregion

		#region FindMethods
		public static clsApplications Find(int applicationID)
		{
			string applicantName=string.Empty;
			string applicantNationalNo=string.Empty;
			string applicationsTitle=string.Empty;
			Byte applicationStatus=0;
			DateTime applicationDate=DateTime.Now;
			DateTime lastStatusDate=DateTime.Now;
			string creatorUsername=string.Empty;
			decimal applicationFees=0;
			decimal paidFees=0;
			if (clsApplicationsDataAccess.GetApplicationByApplicationID(applicationID, ref applicantNationalNo,
				ref applicationDate, ref applicationsTitle, ref applicationFees, ref applicationStatus, ref lastStatusDate,
				ref paidFees, ref creatorUsername))
			{
				return new clsApplications(applicationID,applicantNationalNo, applicationsTitle,
					applicationStatus, applicationDate, lastStatusDate, creatorUsername,
					paidFees);
			}
			else
				return new clsApplications();
		}

		public static clsApplications Find(string applicantNationalNo)
		{
			string applicantName = string.Empty;
			int applicationID = -1;
			string applicationsTitle = string.Empty;
			Byte applicationStatus = 0;
			DateTime applicationDate = DateTime.Now;
			DateTime lastStatusDate = DateTime.Now;
			string creatorUsername = string.Empty;
			decimal applicationFees = 0;
			decimal paidFees = 0;
			if (clsApplicationsDataAccess.GetApplicationByNationalNo(applicantNationalNo,ref applicationID,
				ref applicationDate, ref applicationsTitle, ref applicationFees, ref applicationStatus, ref lastStatusDate,
				ref paidFees, ref creatorUsername))
			{
				return new clsApplications(applicationID,applicantNationalNo, applicationsTitle,
					applicationStatus, applicationDate, lastStatusDate, creatorUsername,
					paidFees);
			}
			else
				return new clsApplications();
		}

		#endregion

		#region GetData
		public static DataTable GetApplications()
		{
			return clsApplicationsDataAccess.GetAllApplications();
		}

		public static DataTable GetApplicationTypes()
		{
			return clsApplicationsDataAccess.GetApplicationTypes();
		}

		public static bool GetTypeInfo(string Title, int ApplicationTypeID, decimal ApplicationFees)
		{
			return clsApplicationsDataAccess.GetApplicationTypeInfo(Title,
				ref ApplicationTypeID, ref ApplicationFees);
		}

		#endregion

		#region AddAndUpdateMethods

		private bool _AddApplication()
		{	
			int applicationTypeID = -1;
			decimal applicationFees = 0;
			if(clsApplicationsDataAccess.GetApplicationTypeInfo(ApplicationTypeInfo.ApplicationTypeTitle,ref applicationTypeID,ref applicationFees))
			{
				ApplicationTypeInfo.ApplicationTypeID=applicationTypeID;
				ApplicationTypeInfo.ApplicationFees=applicationFees;
				this.ApplicationID = clsApplicationsDataAccess.AddApplication(Person.ID, ApplicationTypeInfo.ApplicationTypeID,
					User.UserID, ApplicationDate, LastStatusDate, PaidFees);
			}
			else 
				this.ApplicationID=-1;

			return ApplicationID != 0;
		}

		private bool _UpdatePaidFees()
		{
			return clsApplicationsDataAccess.UpdatePaidFees(this.ApplicationID,this.PaidFees);
		}

		public bool Save()
		{
			switch(_Mode)
			{
				case enMode.Add:
					{
						if(_AddApplication())
						{
							_Mode = enMode.Update;
							return true;
						}
						else
							return false;
					}
				case enMode.Update:
					{
						return _UpdatePaidFees();
					}

			}
			return false;
		}

		public static bool CanceleApplication(int applicationID)
		{
			return clsApplicationsDataAccess.Cancele(applicationID);
		}

		#endregion


	}
}
