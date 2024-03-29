using GenZStyleApp.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenZStyleApp.DAL.DAO
{
    public class CommentDAO
    {
        private GenZStyleDbContext _dbContext;
        public CommentDAO(GenZStyleDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task AddCommentAsync(Comment newComment)
        {
            try
            {
                await this._dbContext.Comments.AddAsync(newComment);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Comment>> GetCommentByPostIdAsync(int postid)
        {
            try
            {
                return await _dbContext.Comments/*Include(c => c.Account)*/
                                                 .Include(c => c.Post)
                                                 .Where(co => co.PostId == postid)
                                                 .ToListAsync();
                                                 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task DeleteComment (List<Comment> comments)
        {
            try {
                foreach (Comment comment in comments) 
                {
                    this._dbContext.Comments.Remove(comment);
                } 
            
            }catch(Exception ex)
            {
                throw new Exception (ex.Message);
            }
            
        }
        public async Task DeleteCommentById(Comment Comment)
        {
            try
            {   
                this._dbContext.Comments.Remove(Comment);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<List<Comment>> GetCommentByPostIdAss(int postid)
        {
            try
            {
                return await _dbContext.Comments.Where(co => co.PostId == postid)
                                                 .ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Comment> GetCommentByIdAsync(int commentId)
        {
            try
            {
                return await _dbContext.Comments.SingleOrDefaultAsync(co => co.CommentId == commentId);
                                                 

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}



