using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Services;


public class CommentService
    {
        private readonly FootballForumDbContext _context;

        public CommentService(FootballForumDbContext context)
        {
            _context = context;
        }

        public void Create(CreateCommentDto dto, int postId, int userId)
        {
            var comment = new Comment
            {
                Content = dto.Content,
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public bool Update(int commentId, int userId, CreateCommentDto dto)
{
    var comment = _context.Comments.Find(commentId);

    if (comment == null || comment.UserId != userId)
    {
        return false; 
    }

    comment.Content = dto.Content;
    

    _context.SaveChanges();
    return true;
}


        public bool Delete(int commentId, int userId)
    {
    var comment = _context.Comments.Find(commentId);

    
    if (comment == null || comment.UserId != userId)
    {
        return false; 
    }

    _context.Comments.Remove(comment);
    _context.SaveChanges();
    return true;
    }


 }