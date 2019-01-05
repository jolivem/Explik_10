using Roadkill.Core.Database;
using System;

namespace Roadkill.Core.Mvc.ViewModels
{
    public class CommentViewModel
    {
        public string Id;
        public string CreatedBy;
        public DateTime CreatedOn;
        public string Text;

        /// <summary>
        /// 
        /// </summary>
        public CommentViewModel()
        {
            Id = "";
            CreatedBy = "";
            CreatedOn = DateTime.MinValue;
            Text = "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comment"></param>
        public CommentViewModel(Comment comment)
        {
            Id = comment.Id.ToString();
            CreatedBy = comment.CreatedBy;
            CreatedOn = comment.CreatedOn;
            Text = comment.Text;
        }
    }
}
