using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{

    [Table("explik_alerts")]
    public class AlertEntity : Entity<Guid>
    {
        [Column("pageid")]
        private int _pageid;

        [Column("commentid")]
        private Guid _commentid;

        [Column("createdby")]
        private string _createdby;

        [Column("createdon")]
        private DateTime _createdon;

        public int PageId
        {
            get
            {
                return _pageid;
            }
            set
            {
                Set<int>(ref _pageid, value);
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

        public DateTime CreatedOn
        {
            get
            {
                return _createdon;
            }
            set
            {
                Set<DateTime>(ref _createdon, value);
            }
        }

        public Guid CommentId
        {
            get
            {
                return _commentid;
            }
            set
            {
                Set<Guid>(ref _commentid, value);
            }
        }
    }
}
