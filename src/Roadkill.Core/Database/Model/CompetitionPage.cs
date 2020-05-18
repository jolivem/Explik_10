using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Database
{
    public class CompetitionPage
    {
        private Guid _objectId;

        public int Id { get; set; }

        public int CompetitionId { get; set; }

        public int PageId { get; set; }

        public long NbRating { get; set; }

        /// <summary>
        /// Gets or sets the sum of all rating values
        /// </summary>
        public long TotalRating { get; set; }

        public string UserName { get; set; }

        public int Ranking { get; set; }

        public Guid ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }
    }
}
