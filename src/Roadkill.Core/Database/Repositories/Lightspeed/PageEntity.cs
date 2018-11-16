using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{
	[Table("roadkill_pages", IdentityMethod=IdentityMethod.IdentityColumn)]
	public class PageEntity : Entity<int>
	{
		[Column("title")]
		private string _title;

        [Column("createdby")]
        private string _createdBy;

        [Column("summary")]
        private string _summary;

        [Column("createdon")]
		private DateTime _createdOnColumn;

		[Column("modifiedby")]
		private string _modifiedBy;

		[Column("modifiedon")]
		private DateTime _modifiedOn;

		[Column("tags")]
		private string _tags;

        [Column("islocked")]
        private bool _isLocked;

        [Column("isvideo")]
        private bool _isVideo;

        [Column("issubmitted")]
        private bool _isSubmitted;

        [Column("iscontrolled")]
        private bool _isControlled;

        [Column("isrejected")]
        private bool _isRejected;

        [Column("nbrating")]
        private long _nbRating;

        [Column("nbview")]
        private long _nbView;

        [Column("nbalert")]
        private long _nbAlert;

        [Column("totalrating")]
        private long _totalRating;

        [ReverseAssociation("PageContents")]
		private readonly EntityCollection<PageContentEntity> _pageContents = new EntityCollection<PageContentEntity>();

		public EntityCollection<PageContentEntity> PageContents
		{
			get { return Get(_pageContents); }
		}

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
				return _createdBy;
			}
			set
			{
				Set<string>(ref _createdBy, value);
			}
		}

        public string Summary
        {
            get
            {
                return _summary;
            }
            set
            {
                Set<string>(ref _summary, value);
            }
        }

        public DateTime CreatedOn
		{
			get
			{
				return _createdOnColumn;
			}
			set
			{
				Set<DateTime>(ref _createdOnColumn, value);
			}
		}

		public string ModifiedBy
		{
			get
			{
				return _modifiedBy;
			}
			set
			{
				Set<string>(ref _modifiedBy, value);
			}
		}

		public DateTime ModifiedOn
		{
			get
			{
				return _modifiedOn;
			}
			set
			{
				Set<DateTime>(ref _modifiedOn, value);
			}
		}

		public string Tags
		{
			get
			{
				return _tags;
			}
			set
			{
				Set<string>(ref _tags, value);
			}
		}

        public bool IsLocked
        {
            get
            {
                return _isLocked;
            }
            set
            {
                Set<bool>(ref _isLocked, value);
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
        public bool IsSubmitted
        {
            get
            {
                return _isSubmitted;
            }
            set
            {
                Set<bool>(ref _isSubmitted, value);
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
        public bool IsVideo
        {
            get
            {
                return _isVideo;
            }
            set
            {
                Set<bool>(ref _isVideo, value);
            }
        }

        public long NbView
        {
            get
            {
                return _nbView;
            }
            set
            {
                Set<long>(ref _nbView, value);
            }
        }
        public long NbAlert
        {
            get
            {
                return _nbAlert;
            }
            set
            {
                Set<long>(ref _nbAlert, value);
            }
        }

        public long NbRating
        {
            get
            {
                return _nbRating;
            }
            set
            {
                Set<long>(ref _nbRating, value);
            }
        }

        public long TotalRating
        {
            get
            {
                return _totalRating;
            }
            set
            {
                Set<long>(ref _totalRating, value);
            }
        }
    }
}
