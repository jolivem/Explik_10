using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Roadkill.Core;
using Roadkill.Core.Database;

namespace Roadkill.Tests.Unit
{
	[TestFixture]
	[Category("Unit")]
	public class DataStoreTypeTests
	{
		[Test]
		public void AllTypes_Should_Contain_Types_With_Name_And_Description()
		{
			// Arrange
			IEnumerable<DataStoreType> dataStoreTypes = DataStoreType.AllTypes;

			// Act + Assert
			foreach (DataStoreType type in dataStoreTypes)
			{
				Assert.That(type.Name, Is.Not.Empty);
				Assert.That(type.Description, Is.Not.Empty);
				Console.WriteLine("Found {0}", type);
			}
		}

		[Test]
		[ExpectedException(typeof(DatabaseException))]
		public void ByName_Should_Throw_DatabaseException_If_DataStoreType_Not_Found()
		{
			// Arrange
			IEnumerable<DataStoreType> dataStoreTypes = DataStoreType.AllTypes;

			// Act + Assert
			DataStoreType datastoreType = DataStoreType.ByName("thunderbird");
		}

	}
}
