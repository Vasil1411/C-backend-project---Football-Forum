using System.ComponentModel.DataAnnotations;
namespace WebApi.DTOs;

public class CreateCommentDto
{
    [Required]
    [MinLength(5)]
    public string Content { get; set; }
}