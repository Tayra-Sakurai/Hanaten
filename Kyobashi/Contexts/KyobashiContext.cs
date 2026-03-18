// SPDX-FileCopyrightText: 2026 Tayra Sakurai
//
// SPDX-License-Identifier: AGPL-3.0-or-later

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kyobashi.Contexts
{
    public class KyobashiContext : DbContext
    {
        public DbSet<Models.Request> Requests { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseAzureSql(
                "Server=tcp:familybalancesheet.database.windows.net,1433;Initial Catalog=free-sql-db-4460118;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;Authentication=\"Active Directory Default\";");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.Request>(
                t =>
                {
                    t.HasKey(e => e.Id);

                    t.Property(e => e.Id)
                    .UseIdentityColumn(1, 1);

                    t.Property(e => e.UserName)
                    .HasColumnType("text")
                    .IsRequired();

                    t.Property(e => e.Message)
                    .HasColumnType("ntext");

                    t.Property(e => e.RequestedAmount)
                    .HasColumnType("money")
                    .IsRequired();

                    t.Property(e => e.Balance)
                    .HasColumnType("money")
                    .IsRequired();

                    t.Property(e => e.DateTime)
                    .HasColumnType("datetime")
                    .IsRequired();
                });
        }
    }
}
