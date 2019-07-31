using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roadkill.Core.Database
{
    public class Competition : IDataStoreEntity
    {
        private Guid _objectId;

        public int Id { get; set; }


        public DateTime PublicationStart { get; set; }

        public DateTime PublicationStop { get; set; }

        public DateTime RatingStart { get; set; }

        public DateTime RatingStop { get; set; }

        /// <summary>
        /// 0 -> init
        /// 1 -> publication on going
        /// 2 -> rating on going
        /// 3 -> end
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// The page tag which describes the competition
        /// </summary>
        public string PageTag { get; set; }

        /// <summary>
        /// The page id which describes the competition
        /// </summary>
        public int PageId { get; set; }

        /// <summary>
        /// !!!!  Useless  !!!!
        /// </summary>
        //public Guid UserId { get; set; }

        public Guid ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }
    }
}
