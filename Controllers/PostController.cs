using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs;
using WebApi.Services;

namespace WebApi.Controllers;


[Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostService _postService;

        public PostsController(PostService postService)
        {
            _postService = postService;
        }

        
        [HttpGet("team/{teamId}")]
        public IActionResult GetByTeam(int teamId)
        {
            var posts = _postService.GetPostsByTeam(teamId);
            return Ok(posts);
        }

        [HttpGet]
        public IActionResult GetAll()
            {
                 var posts = _postService.GetAllPosts();
                 return Ok(posts);
    }
        
        [Authorize]
        [HttpPost("create/{teamId}")]
        public IActionResult Create(int teamId, [FromBody] CreatePostDto dto)
        {
            
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            
            _postService.Create(dto, teamId, userId);
            
            return Ok("Post created successfully.");
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreatePostDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            
            bool success = _postService.Update(id, userId, dto);

            if (!success)
            {
                return BadRequest("Post not found or you are not the owner.");
            }
            
            return Ok("Post updated.");
        }

        
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            
            _postService.Delete(id, userId); 
             
            return Ok("Post deleted (if existed and was yours).");
        }

        [HttpGet("{id}")]
        public IActionResult GetPostDetails(int id)
    {
        var post = _postService.GetPostById(id);

        if (post == null)
        {
        return NotFound("Post not found");
        }

        return Ok(post);
    }
        }