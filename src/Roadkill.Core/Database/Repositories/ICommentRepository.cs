using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Converters;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Database
{
    public interface ICommentRepository
    {
        void DeleteComment(Guid commentId);
        IEnumerable<Comment> FindAllCommentByPage(int pageId);
        void AddComment(Comment comment);
        Comment FindCommentByPageAndUser(int pageId, string username);
    }
}
