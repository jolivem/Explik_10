﻿using Roadkill.Core.Configuration;
using Roadkill.Core.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluginSettings = Roadkill.Core.Plugins.Settings;

namespace Roadkill.Core.Database.Repositories
{
	public interface ISettingsRepository
	{
		void SaveSiteSettings(SiteSettings siteSettings);
		SiteSettings GetSiteSettings();
		void SaveTextPluginSettings(TextPlugin plugin);
		PluginSettings GetTextPluginSettings(Guid databaseId);
	}
}
