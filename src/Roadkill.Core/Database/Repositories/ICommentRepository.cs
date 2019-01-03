﻿using System;
using System.Collections.Generic;
using System.Linq;
using Roadkill.Core.Converters;
using Roadkill.Core.Mvc.ViewModels;

namespace Roadkill.Core.Database
{
    public interface ICommentRepository
    {
        void DeleteComment(Guid commentId);
        IEnumerable<Comment> FindCommentsByPage(int pageId);
        void AddComment(Comment comment);
        void UpdateRating(Guid commentId, int rating);
        void ValidateComment(Guid commentId);
        void RejectComment(Guid commentId);
        void UpdateComment(Guid commentId, string text);
        Comment FindCommentByPageAndUser(int pageId, string username);
        void DeleteComments(int pageId);
        IEnumerable<Comment> FindCommentsToControl();
    }
}
