using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{

    /// <summary>
    /// Descritpion of a pedagogoc competition
    /// </summary>
    [Table("explik_competition", IdentityMethod = IdentityMethod.IdentityColumn)]
    public class CompetitionEntity : Entity<int>
    {
        [Column("publicationstart")]
        private DateTime _publicationstart;

        [Column("publicationstop")]
        private DateTime _publicationstop;

        [Column("ratingstart")]
        private DateTime _ratingstart;

        [Column("ratingstop")]
        private DateTime _ratingstop;

        [Column("status")]
        private int _status;

        [Column("pagetag")]
        private string _pagetag;

        [Column("pageid")]
        private int _pageid;

        public DateTime PublicationStart
        {
            get
            {
                return _publicationstart;
            }
            set
            {
                Set<DateTime>(ref _publicationstart, value);
            }
        }

        public DateTime PublicationStop
        {
            get
            {
                return _publicationstop;
            }
            set
            {
                Set<DateTime>(ref _publicationstop, value);
            }
        }

        public DateTime RatingStart
        {
            get
            {
                return _ratingstart;
            }
            set
            {
                Set<DateTime>(ref _ratingstart, value);
            }
        }

        public DateTime RatingStop
        {
            get
            {
                return _ratingstop;
            }
            set
            {
                Set<DateTime>(ref _ratingstop, value);
            }
        }

        public int Status
        {
            get
            {
                return _status;
            }
            set
            {
                Set<int>(ref _status, value);
            }
        }

        public string PageTag
        {
            get
            {
                return _pagetag;
            }
            set
            {
                Set<string>(ref _pagetag, value);
            }
        }

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
    }
}