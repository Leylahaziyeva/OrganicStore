using Microsoft.EntityFrameworkCore;

namespace OrganicStore.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ContactInfo> ContactInfos { get; set; }
        public DbSet<Logo> Logos { get; set; }
        public DbSet<SocialLink> SocialLinks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<ProductTag> ProductTags { get; set; }
      
    }
}
