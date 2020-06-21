
using System;
using System.Data.Entity;

namespace Roadkill.Core.Database.Repositories.Entities
{

    // Extensions added after migration from Mindscape to Entity framework
    // MindScape handled the GUID Id.
    // It is now handled in the constructor

    public partial class explik_users
    {
        public explik_users()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public partial class explik_pagecontent
    {
        public explik_pagecontent()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public partial class Entities : DbContext
    {
        public Entities(string cnxString) : base(cnxString)
        {
        }
    }
}