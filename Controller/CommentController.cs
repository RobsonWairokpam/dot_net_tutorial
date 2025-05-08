using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly ICommentRepository _commentRepo;
        private readonly IStockRepository _stockRepo;

        public CommentController(
            ApplicationDBContext context,
            ICommentRepository commentRepo,
            IStockRepository stockRepo
        )
        {
            _context = context;
            _commentRepo = commentRepo;
            _stockRepo = stockRepo;
        }

        [HttpGet]
        // public IActionResult GetAll()
        public async Task<IActionResult> GetAll()
        {
            // var stocks = _context.Stock.ToList()
            // .Select(s => s.ToStockDtos());
            // var stocks = await _context.Stock.ToListAsync();
            var comments = await _commentRepo.GetAllAsync();
            var commentDtos = comments.Select(s => s.ToCommentDto());

            return Ok(commentDtos);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var comment = await _commentRepo.GetByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment.ToCommentDto());
        }

        // [HttpGet]
        // [Route("{stockId}")]
        // public async Task<IActionResult> GetByStockId([FromRoute] int stockId)
        // {

        // }

        [HttpPost]
        [Route("{stockId}")]
        public async Task<IActionResult> CreateComment(
            [FromRoute] int stockId,
            CreateCommentDto commentDto
        )
        {
            var stock = await _stockRepo.IsExistStock(stockId);
            if (!stock)
            {
                return BadRequest("Stock Does not Exit");
            }
            var comment = commentDto.ToCommentCreateDto(stockId);
            await _commentRepo.CreateAsync(comment);
            return CreatedAtAction(nameof(GetById), new { id = comment }, comment.ToCommentDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateComment(
            [FromRoute] int id,
            [FromBody] UpdateCommentDto updateComment
        )
        {
            var comment = await _commentRepo.UpdateCommentAsync(
                id,
                updateComment.ToCommentUpdateDto()
            );
            if (comment == null)
            {
                return NotFound("Comment not Found");
            }
            return Ok(comment.ToCommentDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            // var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);
            var comment = await _commentRepo.DeleteCommentAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            // _context.Stock.Remove(stockModel);
            // await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
