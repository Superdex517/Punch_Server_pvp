﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GameServer.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameServer
{
    public class GameDbContext : DbContext
    {
        public DbSet<HeroDb> Heroes { get; set; }
        public DbSet<ItemDb> Items { get; set; }

        static readonly ILoggerFactory _logger = LoggerFactory.Create(builder => { builder.AddConsole(); });

        public GameDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            ConfigManager.LoadConfig();

            options
                .UseLoggerFactory(_logger)
                .UseSqlServer(ConfigManager.Config.connectionString);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // AccountDbId에 인덱스 걸어준다
            builder.Entity<HeroDb>()
                .HasIndex(t => t.AccountDbId);

            builder.Entity<HeroDb>()
                .Property(nameof(HeroDb.CreateDate))
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Entity<ItemDb>()
                .HasIndex(i => i.AccountDbId);

            builder.Entity<ItemDb>()
                .HasIndex(t => t.OwnerDbId);
        }
    }
}
