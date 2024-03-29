using Quartz;
using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using GenZStyleApp.DAL.Models;
//using YourNamespace.Models;


namespace GenZStyleAPP.BAL.Repository.Implementations
{
    public class CheckAndDeleteJob : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public CheckAndDeleteJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task Execute(IJobExecutionContext context)
        {
            // Lấy đối tượng DbContext từ ServiceProvider
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GenZStyleDbContext>();

                // Lấy danh sách bài Post có ít nhất 2 báo cáo và IsStatusReport là 1
                var postsToDelete = dbContext.Posts
                    .Include(p => p.Reports)
                    .Where(p => p.Reports.Count(r => r.IsStatusReport == 1) >= 2 && p.Reports.All(r => r.IsStatusReport == 1))
                    .ToList();

                // Xóa bài Post
                foreach (var post in postsToDelete)
                {
                    dbContext.Posts.Remove(post);
                }

                // Lưu thay đổi vào cơ sở dữ liệu
                dbContext.SaveChanges();
            }

            return Task.CompletedTask;
        }
    }
    //public void CheckAndDeletePost(GenZStyleDbContext dbContext)
    //{
    //    // Lấy danh sách bài Post có ít nhất 2 báo cáo và IsReport là true
    //    var postsToDelete = dbContext.Posts
    //        .Include(p => p.Reports)
    //        .Where(p => p.Reports.Count(r => r.IsReport) >= 2 && p.Reports.All(r => r.IsReport))
    //        .ToList();

    //    // Xóa bài Post
    //    foreach (var post in postsToDelete)
    //    {
    //        dbContext.Posts.Remove(post);
    //    }

    //    // Lưu thay đổi vào cơ sở dữ liệu
    //    dbContext.SaveChanges();
    //}
}
