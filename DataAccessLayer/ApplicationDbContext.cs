using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace DataAccessLayer
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
            
        }

        public DbSet<Idea> Ideas { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<relIdeaTag> relIdeaTags { get; set; }
        public DbSet<relIdeaFavorite> relIdeaFavorites { get; set; }
        public DbSet<relIdeaLike> relIdeaLikes { get; set; }
        public DbSet<relCommentLike> relCommentLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Idea>()
                .HasOne(x => x.CreatorUser)
                .WithMany(x => x.Ideas)
                .HasForeignKey(x => x.CreatorId);
            
            builder.Entity<Comment>()
                .HasOne(x => x.CreatorUser)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.CreatorId);

            builder.Entity<Comment>()
                .HasOne(x => x.Idea)
                .WithMany(x => x.Comments)
                .HasForeignKey(x => x.IdeaId);

            builder.Entity<Tag>()
                .HasOne(x => x.CreatorUser)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.CreatorId);

            builder.Entity<relIdeaFavorite>()
                .HasOne(x => x.CreatorUser)
                .WithMany(x => x.relIdeaFavorites)
                .HasForeignKey(x => x.CreatorId);

            builder.Entity<relIdeaFavorite>()
                .HasOne(x => x.Idea)
                .WithMany(x => x.Favorites)
                .HasForeignKey(x => x.IdeaId);

            builder.Entity<relIdeaTag>()
                .HasOne(x => x.CreatorUser)
                .WithMany(x => x.relIdeaTags)
                .HasForeignKey(x => x.CreatorId);

            builder.Entity<relIdeaTag>()
                .HasOne(x => x.Tag)
                .WithMany(x => x.IdeaTags)
                .HasForeignKey(x => x.TagId);

            builder.Entity<relIdeaTag>()
                .HasOne(x => x.Idea)
                .WithMany(x => x.Tags)
                .HasForeignKey(x => x.IdeaId);
            
            builder.Entity<relIdeaLike>()
                .HasOne(x => x.CreatorUser)
                .WithMany(x => x.relIdeaLikes)
                .HasForeignKey(x => x.CreatorId);

            builder.Entity<relIdeaLike>()
                .HasOne(x => x.Idea)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.IdeaId);

            builder.Entity<relCommentLike>()
                .HasOne(x => x.CreatorUser)
                .WithMany(x => x.relCommentLikes)
                .HasForeignKey(x => x.CreatorId);

            builder.Entity<relCommentLike>()
                .HasOne(x => x.Comment)
                .WithMany(x => x.Likes)
                .HasForeignKey(x => x.CommentId);

        }
    }
}