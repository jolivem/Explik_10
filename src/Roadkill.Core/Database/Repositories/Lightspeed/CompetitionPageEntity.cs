using System;
using Mindscape.LightSpeed;

namespace Roadkill.Core.Database.LightSpeed
{

    /// <summary>
    /// 
    /// </summary>
    [Table("explik_competitionpage", IdentityMethod = IdentityMethod.IdentityColumn)]
    public class CompetitionPageEntity : Entity<int>
    {
        [Column("competitionid")]
        private int _competitionid;

        [Column("pageid")]
        private int _pageid;

        [Column("nbrating")]
        private long _nbRating;

        [Column("totalrating")]
        private long _totalRating;

        [Column("username")]
        private string _username;

        [Column("ranking")]
        private int _ranking;

        public int CompetitionId
        {
            get
            {
                return _competitionid;
            }
            set
            {
                Set<int>(ref _competitionid, value);
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

        public string UserName
        {
            get
            {
                return _username;
            }
            set
            {
                Set<string>(ref _username, value);
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
        public int Ranking
        {
            get
            {
                return _ranking;
            }
            set
            {
                Set<int>(ref _ranking, value);
            }
        }
    }
}