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
        public string Ilk { get; set; }

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
            Id = new Guid();
            PageId = 0;
            CreatedBy = "";
            CreatedOn = DateTime.Now;
            CommentId = Guid.Empty;
            Ilk = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="createdBy"></param>
        public Alert(int pageId, string createdBy, string type)
        {
            Id = new Guid();
            PageId = pageId;
            CommentId = Guid.Empty;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            Ilk = type;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="createdBy"></param>
        public Alert(Guid commentId, string createdBy, string type)
        {
            Id = new Guid();
            PageId = 0;
            CommentId = commentId;
            CreatedBy = createdBy;
            CreatedOn = DateTime.Now;
            Ilk = type;
        }
    }
}
