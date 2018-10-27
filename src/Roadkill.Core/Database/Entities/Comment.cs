using System;

namespace Roadkill.Core.Database
{
    /// <summary>
    /// A comment object for use with the data store, whatever that might be (e.g. an RDMS or MongoDB)
    /// </summary>
    public class Comment : IDataStoreEntity
    {
        private Guid _objectId;

        /// <summary>
        /// Gets or sets the  uncomment unique ID.
        /// </summary>
        /// <value>
        /// The comment unique id.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the activation key for the user.
        /// </summary>
        /// <value>
        /// The activation key.
        /// </value>
        public long PageId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int Rating { get; set; }
        public string Text { get; set; }
        public int NbAlert{ get; set; }

        /// <summary>
        /// The unique id for this object - for use with document stores that require a unique id for storage.
        /// </summary>
        public Guid ObjectId
        {
            get { return _objectId; }
            set { _objectId = value; }
        }
    }
}
