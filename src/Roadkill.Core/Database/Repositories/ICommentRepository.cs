using System;
using System.Collections.Generic;
using System.Linq;

namespace Roadkill.Core.Database
{
    public interface ICommentRepository
    {
        void DeleteComment(int commentId);
        IEnumerable<Comment> FindCommentsByPage(int pageId);
        int GetRatingByPageAndUser(int pageId, string username);
        void AddComment(Comment comment);
        void UpdateCommentRating(int commentId, int rating);
        void ValidateComment(int commentId);
        void RejectComment(int commentId);
        void UpdateComment(int commentId, string text);
        Comment FindCommentByPageAndUser(int pageId, string username);
        void DeleteComments(int pageId);
        IEnumerable<Comment> FindCommentsToControl();
    }
}
