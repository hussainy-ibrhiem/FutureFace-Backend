using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SingularizeTableNames(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
        public void SingularizeTableNames(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                modelBuilder.Entity(entityType.ClrType).ToTable(entityType.ClrType.Name);
            EntityModelConfiguration(modelBuilder);
        }
        private void EntityModelConfiguration(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedOnAdd().HasColumnType("int");
                entity.Property(t => t.Name).IsRequired().HasColumnType("nvarchar(50)");
                entity.Property(t => t.CategoryId).IsRequired().HasColumnType("int");
                entity.Property(t => t.SubCategoryId).HasColumnType("int");
                entity.Property(t => t.Photo).HasColumnType("nvarchar(max)");
                entity.Property(t => t.CreationDate).IsRequired();
                entity.HasOne(t => t.Category).WithMany(t => t.Products).HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.Cascade);
                entity.HasQueryFilter(x => x.Deleted != true);
            });
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Id).ValueGeneratedOnAdd().HasColumnType("int");
                entity.Property(t => t.Name).IsRequired().HasColumnType("nvarchar(50)");
                entity.Property(t => t.ParentId).HasColumnType("int");
                entity.HasMany(t => t.Products).WithOne(t => t.Category).HasForeignKey(t => t.CategoryId).OnDelete(DeleteBehavior.Restrict);
            });
        }
        public DbSet<Product> Product { get; set; }
        public DbSet<Category> Categorie { get; set; }
    }
}
