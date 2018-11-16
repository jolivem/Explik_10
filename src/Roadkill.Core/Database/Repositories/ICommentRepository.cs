using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Converters;

namespace Roadkill.Core.Database
{
    public interface ICommentRepository
    {
        void DeleteComment(int commentId);
        IEnumerable<Comment> FindAllCommentByPage(int pageId);
        void AddComment(Comment comment);
    }
}
