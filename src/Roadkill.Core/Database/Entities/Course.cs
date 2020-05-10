using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Database
{
    public class Course
    {
        private Guid _objectId;

        public int Id { get; set; }

        public string Title { get; set; }

        public string CreatedBy{ get; set; }

        public Course(string title, string createdBy)
        {
            Title = title;
            CreatedBy = createdBy;
        }

        public Course()
        {
            Title = "";
            CreatedBy = "";
        }

    }
}
