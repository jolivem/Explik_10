using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{

    /// <summary>
    /// Descritpion of a pedagogoc competition
    /// </summary>
    [Table("explik_course", IdentityMethod = IdentityMethod.IdentityColumn)]
    public class CourseEntity : Entity<int>
    {
        [Column("title")]
        private string _title;

        [Column("createdby")]
        private string _createdby;


        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set<string>(ref _title, value);
            }
        }

        public string CreatedBy
        {
            get
            {
                return _createdby;
            }
            set
            {
                Set<string>(ref _createdby, value);
            }
        }
    }
}