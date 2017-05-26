using System;
using System.Collections.Generic;
using System.Linq;
namespace Sport.Mobile.Shared.Services
{
	public static class LocalDatabase
	{
		static SQLite.SQLiteConnection database;
		static SQLite.SQLiteConnection Database => database ?? (database = new SQLite.SQLiteConnection (DatabasePath));
		public static string RootPath { get; set; }
		public static string DatabasePath => System.IO.Path.Combine(RootPath,"test.db");

		public static List<T> GetItems<T> () where T : new ()
		{
			return Database.Table<T> ().ToList ();
		}

		public static T GetItem<T> (string id) where T : BaseModel, new()
		{
			return Database.Table<T>().FirstOrDefault(x => x.Id == id);
		}

		public static int Insert (object item)
		{
			return 1;
			//return Database.Insert (item);
		}

		public static int Update (object item)
		{
			return 1;
			//return Database.Update (item);
		}
		public static int Delete (object item)
		{
			return 1;
			//return Database.Delete (item);
		}
	}
}
