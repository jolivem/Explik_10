﻿using Roadkill.Core.Database.Repositories;
using System;
using System.Linq;


namespace Roadkill.Core.Database
{
	/// <summary>
	/// Defines a repository for storing and retrieving Roadkill domain objects in a data store.
	/// </summary>
	public interface IRepository : IPageRepository, IUserRepository, ICommentRepository, ISettingsRepository, IAlertRepository, ICompetitionRepository, ICourseRepository, IDisposable
	{
		//void Startup(DataStoreType dataStoreType, string connectionString, bool enableCache);
		//void TestConnection(DataStoreType dataStoreType, string connectionString);
    }
}
