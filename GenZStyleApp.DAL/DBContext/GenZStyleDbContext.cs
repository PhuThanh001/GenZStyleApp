using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace GenZStyleApp.DAL.Models
{
    public class GenZStyleDbContext : DbContext
    {
        public GenZStyleDbContext()
        {

        }
        public GenZStyleDbContext(DbContextOptions<GenZStyleDbContext> opts) : base(opts)
        {

        }


        public DbSet<Account> Accounts { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Inbox> Inboxes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<InboxPaticipant> InboxPaticipants { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserRelation> UserRelations { get; set; }
        public DbSet<Style> Styles { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Blogger> Bloggers { get; set; }
        public DbSet<Hashtag> Hashtags { get; set; }
        public DbSet<HashPost> HashPosts { get; set; }
        public DbSet<PackageRegistration> packageRegistrations { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Report> Reports { get; set; }









        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var builder = new ConfigurationBuilder()
                                  .SetBasePath(Directory.GetCurrentDirectory())
                                  .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                IConfigurationRoot configuration = builder.Build();
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnectionStringDB"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(e =>
            {
                e.ToTable("Account");
                e.Property(e => e.AccountId)
                .ValueGeneratedOnAdd();
                e.Property(acc => acc.Username).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.PasswordHash).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.Firstname).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.Email).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.Lastname).IsUnicode(true).HasMaxLength(50);
                e.Property(acc => acc.IsVip).IsRequired();
                e.Property(acc => acc.IsActive).IsRequired();


                e.HasOne(e => e.User)
                .WithOne(e => e.Account)
                .HasForeignKey<Account>(e => e.UserId);

                e.HasOne(e => e.Inbox)
                .WithOne(e => e.Account)
                .HasForeignKey<Inbox>(e => e.InboxId);

                


            }
            );            
            modelBuilder.Entity<PackageRegistration>(Entity =>
            {
                Entity.ToTable("PackageRegistration");
                Entity.HasKey(p => new { p.AccountId, p.PackageId });


                Entity.HasOne(L => L.Account)
                .WithMany(L => L.PackageRegistrations)
                .HasForeignKey(po => po.AccountId);

                Entity.HasOne(L => L.Package)
                .WithMany(L => L.PackageRegistrations)
                .HasForeignKey(po => po.PackageId);

            });
            modelBuilder.Entity<Role>(Entity =>
            {
                Entity.ToTable("Role");
                Entity.Property(r => r.Id).IsUnicode(true).HasMaxLength(50);
                Entity.Property(r => r.RoleName).IsUnicode(true).HasMaxLength(50);

            });


            /*modelBuilder.Entity<PackageRegistration>(Entity =>
            {
                Entity.HasNoKey();

            });*/


            modelBuilder.Entity<Hashtag>(Entity =>
            {
                Entity.ToTable("Hashtag");
                Entity.Property(r => r.Name).IsUnicode(true).HasMaxLength(50);
                Entity.Property(r => r.Name).IsUnicode(true).HasMaxLength(50);
                Entity.Property(r => r.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Token>(Entity =>
            {
                Entity.ToTable("Token");
                Entity.Property(r => r.JwtID).IsUnicode(true).HasMaxLength(258);
                Entity.Property(r => r.RefreshToken).IsUnicode(true).HasMaxLength(258);
                Entity.Property(r => r.CreatedDate).HasColumnType("datetime");
                Entity.Property(r => r.ExpiredDate).HasColumnType("datetime");
                modelBuilder.Entity<Token>()
                .HasOne(t => t.Account)
                .WithMany(a => a.Tokens) // Một Account có nhiều Tokens
                .HasForeignKey(t => t.ID) // Khóa ngoại của Token là AccountId
                .OnDelete(DeleteBehavior.Cascade); // Xóa Token khi Account liên quan bị xóa
            });

            modelBuilder.Entity<Blogger>(Entity =>
            {
                Entity.ToTable("Blogger");
                Entity.Property(r => r.City).IsUnicode(true).HasMaxLength(50);
                Entity.Property(r => r.Address).IsUnicode(true).HasMaxLength(50);
                Entity.Property(r => r.Phone).IsUnicode(true).HasMaxLength(50);
                Entity.Property(r => r.AvatarURL).IsUnicode(true).HasMaxLength(50);
                Entity.Property(r => r.Dob).HasColumnType("datetime");


                modelBuilder.Entity<Blogger>()
                .HasOne(t => t.Account)
                .WithOne()
                .HasForeignKey<Blogger>(t => t.AccountID)
                .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<User>(e =>
            {
                e.ToTable("User");
                e.Property(us => us.UserId)
                .ValueGeneratedOnAdd();
                e.Property(us => us.City).IsUnicode(true).HasMaxLength(50);
                e.Property(us => us.Height).HasColumnType("decimal(18,2)");
                e.Property(us => us.Address).IsUnicode(true).HasMaxLength(50);
                e.Property(us => us.Phone).IsUnicode(true).HasMaxLength(50);
                e.Property(us => us.Dob).HasColumnType("datetime");
                e.Property(us => us.AvatarUrl).IsUnicode(true).HasMaxLength(int.MaxValue);

                e.HasOne(us => us.Role)
                .WithMany(us => us.Users)
                .HasForeignKey(us => us.RoleId);

            }
            );
            modelBuilder.Entity<Invoice>(e =>
            {
                e.ToTable("Invoice");
                e.Property(In => In.InvoiceId)
                .ValueGeneratedOnAdd();
                e.Property(In => In.Total).HasColumnType("decimal(18, 2)").IsRequired();
                e.Property(In => In.PaymentType).IsUnicode(true).HasMaxLength(50);
                e.Property(In => In.Status).IsUnicode(true).HasMaxLength(50);
                e.Property(In => In.Date).HasColumnType("datetime");

                e.HasOne(In => In.Account)
                .WithMany(In => In.Invoices)
                .HasForeignKey(In => In.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                e.HasOne(In => In.Package)
                .WithMany(In => In.Invoices)
                .HasForeignKey(In => In.PackageId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            }
            );
            modelBuilder.Entity<Package>(Entity =>
            {
                Entity.ToTable("Package");
                Entity.Property(p => p.PackageId)
                .ValueGeneratedOnAdd();
                Entity.Property(p => p.PackageName).IsUnicode(true).HasMaxLength(50);
                Entity.Property(p => p.Cost).HasColumnType("decimal(18, 2)").IsRequired();
                Entity.Property(p => p.Image).IsUnicode(true).HasMaxLength(50);

            });



            

            modelBuilder.Entity<Post>(e =>
            {
                e.ToTable("Post");
                e.Property(po => po.PostId)
                .ValueGeneratedOnAdd();
                e.Property(po => po.Content).IsUnicode(true).HasMaxLength(50);
                e.Property(po => po.Image).IsUnicode(true).HasMaxLength(int.MaxValue);
                e.Property(po => po.CreateTime).HasColumnType("datetime");
                e.Property(po => po.UpdateTime).HasColumnType("datetime");



                e.HasOne(po => po.Account)
                .WithMany(po => po.Posts)
                .HasForeignKey(po => po.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                


            }
            );
            modelBuilder.Entity<Collection>(e =>
            {
                e.ToTable("Collection");
                e.Property(po => po.Id)
                .ValueGeneratedOnAdd();

                e.HasOne(e => e.Post)
               .WithOne(e => e.Collection)
               .HasForeignKey<Collection>(e => e.PostId)
               .OnDelete(DeleteBehavior.ClientSetNull);
            }
            );
            modelBuilder.Entity<Notification>(e =>
            {
                e.ToTable("Notification");
                e.Property(no => no.NotificationId)
                .ValueGeneratedOnAdd();
                e.Property(no => no.Message).IsUnicode(true).HasMaxLength(50);
                e.Property(no => no.CreateAt).HasColumnType("datetime");
                e.Property(no => no.UpdateAt).HasColumnType("datetime");



                e.HasOne(no => no.Account)
                .WithMany(no => no.Notifications)
                .HasForeignKey(no => no.AccountId);


            }
            );

            modelBuilder.Entity<Like>(Entity =>
            {
                Entity.ToTable("Like");
                Entity.HasKey(e => new { e.LikeBy, e.PostId });


                Entity.HasOne(L => L.Post)
                .WithMany(L => L.Likes)
                .HasForeignKey(po => po.PostId);

                Entity.HasOne(L => L.Account)
                .WithMany(L => L.Likes)
                .HasForeignKey(po => po.LikeBy);

            });

            modelBuilder.Entity<Message>(Entity =>
            {
                Entity.ToTable("Message");

                Entity.HasKey(e => new { e.AccountId, e.InboxId });
                Entity.Property(m => m.Content).IsUnicode(true).HasMaxLength(50);
                Entity.Property(m => m.File).IsUnicode(true).HasMaxLength(50);
                Entity.Property(m => m.CreateAt).HasColumnType("datetime");
                Entity.Property(m => m.DeleteAt).HasColumnType("datetime");

                Entity.HasOne(m => m.AccountSender)
                .WithMany(m => m.Messages)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.ClientSetNull);
               
                Entity.HasOne(m => m.Inbox)
                .WithMany(m => m.Messages)
                .HasForeignKey(m => m.InboxId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            });

            modelBuilder.Entity<Inbox>(Entity =>
            {
                Entity.ToTable("Inbox");
                Entity.Property(m => m.InboxId)
                .ValueGeneratedOnAdd();



                Entity.HasOne(m => m.Account)
                .WithOne(m => m.Inbox)
                .HasForeignKey<Inbox>(m => m.AccountId);


            });

            modelBuilder.Entity<InboxPaticipant>(Entity =>
            {
                Entity.ToTable("InboxPaticipant");
                Entity.Property(pa => pa.InboxPaticipantId)
                .ValueGeneratedOnAdd();


                Entity.HasOne(pa => pa.Account)
                .WithMany(pa => pa.InboxPaticipants)
                .HasForeignKey(pa => pa.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull);


                Entity.HasOne(pa => pa.Inbox)
                .WithMany(pa => pa.Paticipants)
                .HasForeignKey(pa => pa.InboxId)
                .OnDelete(DeleteBehavior.ClientSetNull);



            });

            modelBuilder.Entity<UserRelation>(Entity =>
            {
                Entity.ToTable("UserRelation");
                Entity.HasKey(e => new { e.Id });

                Entity.HasOne(u => u.Account)
                .WithMany(u => u.UserRelations)
                .HasForeignKey(u => u.FollowerId);


            });           
            modelBuilder.Entity<Category>(Entity =>
            {
                Entity.ToTable("Category");
                Entity.Property(fa => fa.CategoryId)
                .ValueGeneratedOnAdd();
                Entity.Property(fa => fa.CategoryName).IsUnicode(true).HasMaxLength(50);
                Entity.Property(fa => fa.CategoryDescription).IsUnicode(true).HasMaxLength(50);

                Entity.HasOne(fa => fa.Style)
                .WithOne(st => st.Category)
                .HasForeignKey<Category>(st => st.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull);
              

                modelBuilder.Entity<Category>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Categories)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            modelBuilder.Entity<Comment>(Entity =>
            {
                Entity.ToTable("Comment");
                Entity.Property(cm => cm.CommentId)
                .ValueGeneratedOnAdd();
                Entity.Property(cm => cm.Content).IsUnicode(true).HasMaxLength(50);
                Entity.Property(cm => cm.CreateAt).HasColumnType("datetime");


                Entity.HasOne(cm => cm.ParentComment)
                .WithMany(cm => cm.SubComments)
                .HasForeignKey(cm => cm.ParentCommentId)
                .OnDelete(DeleteBehavior.ClientSetNull);


                Entity.HasOne(cm => cm.Post)
                .WithMany(cm => cm.Comments)
                .HasForeignKey(cm => cm.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

                Entity.HasOne(cm => cm.Account)
                .WithMany(cm => cm.Comments)
                .HasForeignKey(cm => cm.CommentBy)
                .OnDelete(DeleteBehavior.ClientSetNull);



            });

            modelBuilder.Entity<Style>(Entity =>
            {
                Entity.ToTable("Style");
                Entity.Property(st => st.StyleId)
                .ValueGeneratedOnAdd();
                Entity.Property(st => st.StyleName).IsUnicode(true).HasMaxLength(50);
                Entity.Property(st => st.Description).IsUnicode(true).HasMaxLength(50);
                Entity.Property(st => st.CreateAt).HasColumnType("datetime");
                Entity.Property(st => st.UpdateAt).HasColumnType("datetime");

                Entity.HasOne(st => st.Account)
                .WithMany(st => st.Styles)
                .HasForeignKey(st => st.AccountId);

                Entity.HasOne(st => st.Category)
                .WithOne(ca => ca.Style)
                .HasForeignKey<Category>(st => st.CategoryId);



            });

            modelBuilder.Entity<HashPost>(Entity =>
            {
                Entity.ToTable("HashPost");
                Entity.HasKey(sf => new { sf.PostId, sf.HashTageId });

                Entity.Property(sf => sf.CreateAt).HasColumnType("datetime");
                Entity.Property(sf => sf.UpdateAt).HasColumnType("datetime");

                modelBuilder.Entity<HashPost>()
                .HasOne(it => it.Post)
                .WithMany(a => a.HashPosts)
                .HasForeignKey(it => it.PostId);

                modelBuilder.Entity<HashPost>()
                    .HasOne(it => it.Hashtag)
                    .WithMany(b => b.HashPosts)
                    .HasForeignKey(it => it.HashTageId);
            });



        }

    }
}
