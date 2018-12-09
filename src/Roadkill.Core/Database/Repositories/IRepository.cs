using System;
using System.Linq;
using Roadkill.Core.Configuration;
using Roadkill.Core.Converters;
using Roadkill.Core.Database.Repositories;
using Roadkill.Core.Plugins;
using StructureMap.Attributes;

namespace Roadkill.Core.Database
{
	/// <summary>
	/// Defines a repository for storing and retrieving Roadkill domain objects in a data store.
	/// </summary>
	public interface IRepository : IPageRepository, IUserRepository, ICommentRepository, ISettingsRepository, IAlertRepository, IDisposable
	{
		void Startup(DataStoreType dataStoreType, string connectionString, bool enableCache);
		void TestConnection(DataStoreType dataStoreType, string connectionString);

        void AddPageRating(int pageId, int rating);
        void RemovePageRating(int pageId, int rating);
    }
}
