using System;

namespace Roadkill.Core.Database
{
    /// <summary>
    /// A comment object for use with the data store, whatever that might be (e.g. an RDMS or MongoDB)
    /// </summary>
    public class Alert : IDataStoreEntity
    {
        /// <summary>
        /// Gets or sets the  uncomment unique ID.
        /// </summary>
        /// <value>
        /// The comment unique id.
        /// </value>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the activation key for the user.
        /// </summary>
        /// <value>
        /// The activation key.
        /// </value>
        public int PageId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid CommentId { get; set; }

        /// <summary>
        /// The unique id for this object, this is the same as the <see cref="Id"/> property.
        /// </summary>
        public Guid ObjectId
        {
            get { return Id; }
            set { Id = value; }
        }

        public Alert()
        {
            PageId = 0;
            CreatedBy = "";
            CreatedOn = DateTime.Now;
            //CommentId = null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="createdBy"></param>
        /// <param name="rating"></param>
        /// <param name="text"></param>
        public Alert(int pageId, Guid commentId, string createdBy)
        {
            PageId = pageId;
            CommentId = commentId;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
        }
    }
}
