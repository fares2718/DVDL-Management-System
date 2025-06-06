using DVDLDataAccess;
using DVDLPeopleBuissnesslayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVDLUsersBuissnesslayer
{
	public class clsUser
	{
		public enum enMode { AddNew = 0, Update = 1 };
		public enMode Mode = enMode.AddNew;

		public clsPerson Person;
		public int UserID { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public bool isActive { get; set; }

		public clsUser()
		{
			Person = new clsPerson();
			UserID = -1;
			Username = string.Empty;
			Password = string.Empty;
			isActive = false;

		}

		public clsUser(clsPerson person, int userID, string username, string password, bool isactive)
		{
			Person = person;
			UserID = userID;
			Username = username;
			Password = password;
			isActive = isactive;
			Mode = enMode.Update;
		}

		private bool _AddNewUser()
		{
			this.UserID = clsUsersDataAccess.AddUser(this.Person.ID, this.Username, this.Password, this.isActive);

			return (this.UserID != -1);
		}

		private bool _UpdateUser()
		{
			return clsUsersDataAccess.UpdateUser(this.UserID, this.Username, this.Password, this.isActive);
		}

		public static clsUser Find(int UserID)
		{
			string username = string.Empty;
			string password = string.Empty;
			bool isactive = false;
			int PersonID = -1;
			
			clsUsersDataAccess.GetUserByID(UserID,ref PersonID,ref username,ref password,ref isactive);

			clsPerson Person = clsPerson.Find(PersonID);

			return new clsUser(Person,UserID,username,password,isactive);

		}

		public static clsUser Find(string username)
		{
			int UserID = -1;
			string password = string.Empty;
			bool isactive = false;
			int PersonID = -1;

			clsUsersDataAccess.GetUserByUsername(ref UserID, ref PersonID,username, ref password, ref isactive);

			clsPerson Person = clsPerson.Find(PersonID);

			return new clsUser(Person, UserID, username, password, isactive);

		}

		public bool Save()
		{
			switch (Mode)
			{
				case enMode.AddNew:
					{
						if (_AddNewUser())
						{
							Mode = enMode.Update;
							return true;
						}
						else
						{
							return false;
						}
					}
				case enMode.Update:
					{
						return _UpdateUser();
					}

			}
			return false;
		}

		public static DataTable GetAllUsers()
		{
			return clsUsersDataAccess.GetAllUsers();
		}

		public static bool isUserExists(int UserID)
		{
			return clsUsersDataAccess.isUserExists(UserID);
		}

		public static bool isUserExists(string Username)
		{
			return clsUsersDataAccess.isUserExists(Username);
		}

		public static bool isPersonanUser(int ID)
		{
			return clsUsersDataAccess.isPersonanUser(ID);
		}

	}
}
