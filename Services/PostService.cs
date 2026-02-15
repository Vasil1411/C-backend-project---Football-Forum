using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTOs;
using WebApi.Entities;

namespace WebApi.Services;

public class PostService
    {
        private readonly FootballForumDbContext _context;

        public PostService(FootballForumDbContext context)
        {
            _context = context;
        }

        public void Create(CreatePostDto dto, int teamId, int userId)
        {
            var post = new Post
            {
                Content = dto.Content,
                TeamId = teamId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.Posts.Add(post);
            _context.SaveChanges();
        }

         public List<PostResponseDto> GetAllPosts()
        {
                    return _context.Posts
                    .OrderByDescending(p => p.CreatedAt) 
                    .Select(p => new PostResponseDto
                {
                     PostId = p.PostId,
                     Content = p.Content,
                     CreatedAt = p.CreatedAt,
                     AuthorName = p.User.Username,
                     AuthorId = p.UserId,
            
           
                    Comments = p.Comments.Select(c => new CommentResponseDto 
                 {
                            CommentId = c.Id,
                            Content = c.Content,
                            CreatedAt = c.CreatedAt,
                            AuthorName = c.User.Username,
                            AuthorId = c.UserId
                }).ToList()
                })
                  .ToList();
        }


        public List<PostResponseDto> GetPostsByTeam(int teamId)
        {
            return _context.Posts
                .Where(p => p.TeamId == teamId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new PostResponseDto
                {
                    PostId = p.PostId,
                    Content = p.Content,
                    CreatedAt = p.CreatedAt,
                    AuthorName = p.User.Username,
                    
                    Comments = p.Comments.Select(c => new CommentResponseDto 
                    {
                        CommentId = c.Id,
                        Content = c.Content,
                        CreatedAt = c.CreatedAt,
                        AuthorName = c.User.Username,
                        AuthorId = c.UserId
                    }).ToList()
                })
                .ToList();
        }
        
        public void Delete(int postId, int userId)
        {
            var post = _context.Posts.Find(postId);
            if (post != null && post.UserId == userId)
            {
                _context.Posts.Remove(post);
                _context.SaveChanges();
            }
        }

    public bool Update(int postId, int userId, CreatePostDto dto)
    {
    
    var post = _context.Posts.Find(postId);

    
    if (post == null || post.UserId != userId)
    {
        return false; 
    }

    
    post.Content = dto.Content;
    
    _context.SaveChanges();
    return true; 
    }

    public PostResponseDto GetPostById(int postId)
    {
    var post = _context.Posts
        .Include(p => p.User)       
        .Include(p => p.Comments)   
        .ThenInclude(c => c.User)   
        .FirstOrDefault(p => p.PostId == postId);

    if (post == null) return null;

    return new PostResponseDto
    {
        PostId = post.PostId,
        Content = post.Content,
        CreatedAt = post.CreatedAt,
        AuthorName = post.User.Username,
        AuthorId = post.UserId,
        Comments = post.Comments.Select(c => new CommentResponseDto
        {
            CommentId = c.Id,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            AuthorName = c.User.Username,
            AuthorId = c.UserId
        }).ToList()
    };
    }


    }