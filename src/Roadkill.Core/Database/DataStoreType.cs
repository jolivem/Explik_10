using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mindscape.LightSpeed;
using Roadkill.Core.Database.Schema;
using Roadkill.Core.Logging;

namespace Roadkill.Core.Database
{
	public class DataStoreType
	{
		public static readonly DataStoreType MySQL = new DataStoreType("MySQL", "A MySQL database.", DataProvider.MySql5, new MySqlSchema());
		public static readonly DataStoreType Postgres = new DataStoreType("Postgres", "A Postgres database.", DataProvider.PostgreSql9, new PostgresSchema());
		//public static readonly DataStoreType MongoDB = new DataStoreType("MongoDB", "A MongoDB server, using the official MongoDB driver.", typeof(MongoDBRepository).FullName);

		public static readonly DataStoreType Sqlite = new DataStoreType("Sqlite", "A Sqlite database.", DataProvider.SQLite3, new SqliteSchema());

		public string Name { get; private set; }
		public string Description { get; private set; }
		public bool RequiresCustomRepository { get; private set; }
		public string CustomRepositoryType { get; internal set; }
		public DataProvider LightSpeedDbType { get; private set; }
		public SchemaBase Schema { get; private set; }

		public static IEnumerable<DataStoreType> AllTypes { get; internal set; }
		public static IEnumerable<DataStoreType> AllMonoTypes { get; internal set; }

		static DataStoreType()
		{
			AllTypes = new List<DataStoreType>()
			{
				//MongoDB, 
				MySQL, 
				Postgres, 
				Sqlite, 
			};

			AllMonoTypes = new List<DataStoreType>()
			{
				//MongoDB, 
				MySQL, 
				Postgres
			};
		}

		public DataStoreType(string name, string description, string customRepositoryType) 
		{
			Name = name;
			Description = description;
			RequiresCustomRepository = true;
			CustomRepositoryType = customRepositoryType;
			LightSpeedDbType = DataProvider.Custom;
		}

		public DataStoreType(string name, string description, DataProvider lightSpeedDbType, SchemaBase schema)
		{
			Name = name;
			Description = description;
			RequiresCustomRepository = false;
			CustomRepositoryType = "";
			LightSpeedDbType = lightSpeedDbType;
			Schema = schema;
		}

		public static DataStoreType ByName(string name)
		{
			// Default to SQL Server, but warn
			DataStoreType dataStoreType = MySQL;

#if MONO
			dataStoreType = MongoDB; // default to MongoDB for Mono
#endif

			if (!string.IsNullOrEmpty(name))
			{
				dataStoreType = DataStoreType.AllTypes.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
				if (dataStoreType == null)
					throw new DatabaseException("Unable to find a data store provider for " + name, null);
			}
			else
			{
				Log.Warn("No name provided for DataStoreType.ByName - defaulting to SQLServer2005");
			}

			return dataStoreType;
		}

		public override string ToString()
		{
			return string.Format("{0} - {1} (custom repository: {2})", Name, Description, RequiresCustomRepository);
		}
	}
}
