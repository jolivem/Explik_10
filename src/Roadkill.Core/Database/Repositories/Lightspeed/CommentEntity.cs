using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{

    [Table("explik_comments")]
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

        [Column("controlledby")]
        private string _controlledby;

        [Column("iscontrolled")]
        private bool _isControlled;

        [Column("isrejected")]
        private bool _isRejected;

        [Column("text")]
        private string _text;

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

        public string ControlledBy
        {
            get
            {
                return _controlledby;
            }
            set
            {
                Set<string>(ref _controlledby, value);
            }
        }

        public bool IsControlled
        {
            get
            {
                return _isControlled;
            }
            set
            {
                Set<bool>(ref _isControlled, value);
            }
        }

        public bool IsRejected
        {
            get
            {
                return _isRejected;
            }
            set
            {
                Set<bool>(ref _isRejected, value);
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
    }
}
