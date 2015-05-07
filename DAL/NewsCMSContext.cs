using System;
using NewsCMS.Areas.Admin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace NewsCMS.DAL
{
    public class NewsCMSContext : DbContext
    {
            public NewsCMSContext() : base("NewsCMSContext")
            {

            }

            public DbSet<Author> Authors { get; set; }
            public DbSet<AuthorAvatar> AuthorAvatars { get; set; }    
        
            public DbSet<Post> Posts { get; set; }
            public DbSet<PostImage> PostImages { get; set; }
        
            public DbSet<Category> Categories { get; set; }
            public DbSet<CategoryImage> CategoryImages { get; set; }
            
            public DbSet<SubCategory> SubCategories { get; set; }
            public DbSet<SubCategoryImage> SubCategoryImagies { get; set; }
            
            protected override void OnModelCreating(DbModelBuilder modelBuilder)
            {
                modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            }
    }
}