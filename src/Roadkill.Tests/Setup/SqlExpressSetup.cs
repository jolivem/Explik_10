using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Tests
{
	public class SqlExpressSetup
	{
        // This should match connectionStrings.dev.config
        //		public static string ConnectionString { get { return @"Server=.\SQLEXPRESS;Integrated Security=true;Connect Timeout=5;database=Roadkill"; } }
        //        public static string ConnectionString { get { return @"Server=(LocalDb)\\v11.0;Initial Catalog=Roadkill;Integrated Security=SSPI;Trusted_Connection=yes;"; } }
        //        public static string ConnectionString { get { return @"Server=(localdb)\\Roadkill;Integrated Security=true;"; } }
        //public static string ConnectionString { get { return @"Server=(localdb)\\roadkill;Integrated Security=true;"; } }
        //public static string ConnectionString { get { return @"Server=(localdb)\\Roadkill;Integrated Security=true;AttachDbFileName= myDbFile;"; } }
        //public static string ConnectionString { get { return @"Server=(localdb)\\Roadkill;Integrated Security=true;AttachDbFileName=|DataDirectory|\Roadkill.mdf;"; } }
        //public static string ConnectionString { get { return @"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=Roadkill;"; } }
        //public static string ConnectionString { get { return @"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;Initial Catalog=MSSQLLocalDB;"; } }
        //public static string ConnectionString { get { return @"Server=(localdb)\\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\jolivem\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\MSSQLLocalDB\model.mdf;"; } }
        public static string ConnectionString { get { return @"Server=(localdb)\v11.0;Integrated Security=true;"; } }


        public static void RecreateLocalDbData()
		{
			using (SqlConnection connection = new SqlConnection(ConnectionString))
			{
				connection.Open();

				SqlCommand command = connection.CreateCommand();
				command.CommandText = ReadSqlServerScript();

				command.ExecuteNonQuery();
			}
		}

		private static string ReadSqlServerScript()
		{
			string path = Path.Combine(Settings.LIB_FOLDER, "Test-databases", "roadkill-sqlserver.sql");
			return File.ReadAllText(path);
		}
	}
}
