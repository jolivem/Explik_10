using System;

namespace Roadkill.Core.Database
{
    /// <summary>
    /// A comment object for use with the data store, whatever that might be (e.g. an RDMS or MongoDB)
    /// </summary>
    public class Comment : IDataStoreEntity
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
        public int Rating { get; set; }
        public string Text { get; set; }

        /// <summary>
        /// The unique id for this object, this is the same as the <see cref="Id"/> property.
        /// </summary>
        public Guid ObjectId
        {
            get { return Id; }
            set { Id = value; }
        }

        public Comment()
        {
            Id = new Guid();
            PageId = 0;
            CreatedBy = "";
            Rating = 0;
            Text = "";
            CreatedOn = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="createdBy"></param>
        /// <param name="rating"></param>
        /// <param name="text"></param>
        public Comment( int pageId, string createdBy, int rating, string text)
        {
            Id = new Guid();
            PageId = pageId;
            CreatedBy = createdBy;
            Rating = rating;
            Text = text;
            CreatedOn = DateTime.Now;
        }

    }
}
