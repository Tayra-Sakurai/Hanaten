using Microsoft.EntityFrameworkCore;
using Shigino.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Storage;

namespace Shigino.Contexts
{
    public class ShiginoContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }

        public string DbPath { get; } = null!;

        public ShiginoContext()
            : base()
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            DbPath = Path.Combine(localFolder.Path, "Shigino.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(
                $"Data Source={DbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .Property(e => e.Vector)
                .HasConversion(
                e => JsonSerializer.Serialize(e, JsonSerializerOptions.Default),
                e => JsonSerializer.Deserialize<List<float>>(e, JsonSerializerOptions.Default));
            modelBuilder.Entity<Item>()
                .Property(e => e.Vector)
                .HasConversion(
                e => JsonSerializer.Serialize(e, JsonSerializerOptions.Default),
                e => JsonSerializer.Deserialize<List<float>>(e, JsonSerializerOptions.Default));
            modelBuilder.Entity<PaymentMethod>();
        }
    }
}
