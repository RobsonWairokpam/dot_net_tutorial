using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class CreateCommentDto
    {
        [Required]
        [MaxLength(10, ErrorMessage = "title must be less than 10")]
        [MinLength(2, ErrorMessage = "title must be 3 charcaters")]
        public string Title { get; set; } = string.Empty;

        [Required]
        [MaxLength(10, ErrorMessage = "title must be less than 10")]
        [MinLength(2, ErrorMessage = "title must be 3 charcaters")]
        public string Content { get; set; } = string.Empty;
    }
}
