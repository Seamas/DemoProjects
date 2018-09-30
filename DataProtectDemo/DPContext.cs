using DataProtectDemo.Models;
using Microsoft.EntityFrameworkCore;

namespace DataProtectDemo
{
    public class DPContext : DbContext
    {
        
        #region  表集合
        public virtual DbSet<Product> Products { get; set; }
        #endregion
        
        public DPContext(DbContextOptions options): base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>(option =>
            {
                option.HasKey(item => item.Id);
                option.Property(item => item.Id).HasColumnName("id").ValueGeneratedOnAdd();
                option.Property(item => item.Name).HasColumnName("name");
            });
        }
    }
}