using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Comment;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;

        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateAsync(Comment comment)
        {
            await _context.Comment.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteCommentAsync(int id)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return null;
            }
            _context.Comment.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.Comment.ToListAsync();
        }

        public async Task<Comment?> GetByIdAsync(int id)
        {
            var comment = await _context.Comment.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null)
            {
                return null;
            }
            return comment;
        }

        public async Task<Comment?> UpdateCommentAsync(int id, Comment comment)
        {
            var commentUpdate = await _context.Comment.FirstOrDefaultAsync(x => x.Id == id);
            if (commentUpdate == null)
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(comment.Title) && comment.Title != "string")
            {
                commentUpdate.Title = comment.Title;
            }
            if (!string.IsNullOrWhiteSpace(comment.Title) && comment.Title != "string")
            {
                commentUpdate.Content = comment.Content;
            }
            await _context.SaveChangesAsync();
            return commentUpdate;
        }
    }
}
