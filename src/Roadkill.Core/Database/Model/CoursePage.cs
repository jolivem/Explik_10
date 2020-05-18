using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Database
{
    public class CoursePage
    {
        private Guid _objectId;

        public int Id { get; set; }

        public int CourseId { get; set; }

        public int PageId { get; set; }

        public int Order { get; set; }
    }
}
