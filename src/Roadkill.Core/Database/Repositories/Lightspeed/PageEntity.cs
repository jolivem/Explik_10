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

		[Column("controlledby")]
		private string _controlleddBy;

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

        [Column("totalrating")]
        private long _totalRating;

        [Column("filepath")]
        private string _filepath;

        [Column("videourl")]
        private string _videourl;

        [Column("controllerrating")]
        private long _controllerrating;

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

		public string ControlledBy
		{
			get
			{
				return _controlleddBy;
			}
			set
			{
				Set<string>(ref _controlleddBy, value);
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

        public string FilePath
        {
            get
            {
                return _filepath;
            }
            set
            {
                Set<string>(ref _filepath, value);
            }
        }
        public string VideoUrl
        {
            get
            {
                return _videourl;
            }
            set
            {
                Set<string>(ref _videourl, value);
            }
        }
        public long ControllerRating
        {
            get
            {
                return _controllerrating;
            }
            set
            {
                Set<long>(ref _controllerrating, value);
            }
        }
    }
}
