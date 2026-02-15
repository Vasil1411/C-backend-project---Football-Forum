using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApi.DTOs;
using WebApi.Services;

namespace WebApi.Controllers;


[Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly CommentService _commentService;

        public CommentsController(CommentService commentService)
        {
            _commentService = commentService;
        }

        
        [Authorize]
        [HttpPost("create/{postId}")]
        public IActionResult Create(int postId, [FromBody] CreateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            _commentService.Create(dto, postId, userId);
            return Ok("Comment added.");
        }

        
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] CreateCommentDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            bool success = _commentService.Update(id, userId, dto);
            
            if (!success) return BadRequest("Cannot update: Comment not found or not yours.");

            return Ok("Comment updated.");
        }

       
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            bool success = _commentService.Delete(id, userId);
            
            if (!success) return BadRequest("Cannot delete: Comment not found or not yours.");

            return Ok("Comment deleted.");
        }

   
     
    }