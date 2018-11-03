using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{

    [Table("roadkill_comments")]
    public class CommentEntity : Entity<Guid>
    {
        [Column("pageid")]
        private int _pageid;

        [Column("createdby")]
        private string _createdby;

        [Column("createdon")]
        private DateTime _createdon;

        [Column("rating")]
        private int _rating;

        [Column("text")]
        private string _text;

        [Column("nbalert")]
        private int _nbalert;

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

        public int Rating
        {
            get
            {
                return _rating;
            }
            set
            {
                Set<int>(ref _rating, value);
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                Set<string>(ref _text, value);
            }
        }

        public int NbAlert
        {
            get
            {
                return _nbalert;
            }
            set
            {
                Set<int>(ref _nbalert, value);
            }
        }
    }
}
