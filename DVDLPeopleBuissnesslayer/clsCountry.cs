using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVDLDataAccess;

namespace DVDLPeopleBuissnesslayer
{
	public class clsCountry
	{



		public int ID { set; get; }
		public string CountryName { set; get; }



		public clsCountry()

		{
			this.ID = -1;
			this.CountryName = "";


		}

		private clsCountry(int ID, string CountryName)

		{
			this.ID = ID;
			this.CountryName = CountryName;



		}


		public static clsCountry Find(int ID)
		{

			string CountryName = "";

			int CountryID = -1;

			if (clsCountryData.GetCountryInfoByID(ID, ref CountryName))

				return new clsCountry(ID, CountryName);
			else
				return null;

		}

		public static clsCountry Find(string CountryName)
		{

			int ID = -1;

			if (clsCountryData.GetCountryInfoByName(CountryName, ref ID))

				return new clsCountry(ID, CountryName);
			else
				return null;

		}


		public static DataTable GetAllCountries()
		{
			return clsCountryData.GetAllCountries();

		}


		public static bool isCountryExist(int ID)
		{
			return clsCountryData.IsCountryExist(ID);
		}

		public static bool isCountryExist(string CountryName)
		{
			return clsCountryData.IsCountryExist(CountryName);
		}

		public static int GetCountryID(string CountryName)
		{
			return clsCountryData.GetCountryID(CountryName);
		}

	}
}
