using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVDLDataAccess;

namespace DVDLPeopleBuissnesslayer
{
    public class clsPerson
    {
		public enum enMode { AddNew = 0, Update = 1 };
		public enMode Mode = enMode.AddNew;

		public int ID { get; set; }
		public string NationalNo { get; set; }
		public string FirstName { get; set; }
		public string SecondName { get; set; }
		public string ThirdName { get; set; }
		public string LastName { get; set; }
		public DateTime DateOfBirth { get; set; }
		public Byte Gendor { get; set; }
		public string Address { get; set; }
		public string Email { get; set; }
		public string Phone { get; set; }
		public int NationalityCountryID { get; set; }
		public string ImagePath { get; set; }

		public clsPerson()
		{
			ID = -1;
			NationalNo = string.Empty;
			FirstName = string.Empty;
			SecondName = string.Empty;
			ThirdName = string.Empty;
			LastName = string.Empty;
			DateOfBirth = DateTime.Now;
			Gendor = 0;
			Address = string.Empty;
			Email = string.Empty;
			Phone = string.Empty;
			NationalityCountryID = -1;
			ImagePath = string.Empty;
		}

		private clsPerson(int PersonID, string NationalNo,
	string FirstName, string SecondName, string ThirdName, string LastName,
	DateTime BirthDate, Byte Gendor, string Address, string Phone, string Email,
	int NationalityCountryID, string ImagePath)
		{
			this.ID = PersonID;
			this.NationalNo = NationalNo;
			this.FirstName = FirstName;
			this.SecondName = SecondName;
			this.ThirdName = ThirdName;
			this.LastName = LastName;
			this.DateOfBirth = BirthDate;
			this.Gendor = Gendor;
			this.Address = Address;
			this.Email = Email;
			this.Phone = Phone;
			this.NationalityCountryID = NationalityCountryID;
			this.ImagePath = ImagePath;

			Mode = enMode.Update;
		}

		private bool _AddNewPerson()
		{
			this.ID = clsPeopleDataAccess.AddPerson(this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
				this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);

			return (this.ID != -1);
		}

		private bool _UpdatePerson()
		{
			return clsPeopleDataAccess.UpdatePerson(this.ID, this.NationalNo, this.FirstName, this.SecondName, this.ThirdName, this.LastName,
				this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email, this.NationalityCountryID, this.ImagePath);
		}

		public static clsPerson Find(string NationalNo)
		{
			int ID = -1;
			string FirstName = string.Empty;
			string SecondName = string.Empty;
			string ThirdName = string.Empty;
			string LastName = string.Empty;
			DateTime BirthDate = DateTime.Now;
			Byte Gendor = 0;
			string Address = string.Empty;
			string Email = string.Empty;
			string Phone = string.Empty;
			int NationalityCountryID = -1;
			string ImagePath = string.Empty;

			if (clsPeopleDataAccess.GetPersonByNatinalNo(ref ID,NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
				ref BirthDate, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
				return new clsPerson(ID, NationalNo, FirstName, SecondName, ThirdName, LastName,
								BirthDate, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
			else
				return null;
		}

		public static clsPerson Find(int ID)
		{
			string NationalNo = string.Empty;
			string FirstName = string.Empty;
			string SecondName = string.Empty;
			string ThirdName = string.Empty;
			string LastName = string.Empty;
			DateTime BirthDate = DateTime.Now;
			Byte Gendor = 0;
			string Address = string.Empty;
			string Email = string.Empty;
			string Phone = string.Empty;
			int NationalityCountryID = -1;
			string ImagePath = string.Empty;

			if (clsPeopleDataAccess.GetPersonByID(ID, ref NationalNo, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
				ref BirthDate, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
				return new clsPerson(ID, NationalNo, FirstName, SecondName, ThirdName, LastName,
								BirthDate, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
			else
				return null;
		}

		public bool Save()
		{
			switch (Mode)
			{
				case enMode.AddNew:
					{
						if (_AddNewPerson())
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
						return _UpdatePerson();
					}

			}
			return false;
		}

		public static DataTable GetAllPeople()
		{
			return clsPeopleDataAccess.GetAllPeople();
		}

		public static bool DeletePerson(int ID)
		{
			return clsPeopleDataAccess.DeletePerson(ID);
		}

		public static bool isPersonExists(int ID)
		{
			return clsPeopleDataAccess.isPersonExists(ID);
		}

		public static bool isPersonExists(string NationalNo)
		{
			return clsPeopleDataAccess.isPersonExists(NationalNo);
		}

	}
}

