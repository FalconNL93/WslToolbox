using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.EntityFrameworkCore;
using WslToolbox.Gui.Configurations;

namespace WslToolbox.Gui
{
    public class WslToolboxDbContext : DbContext
    {
        public DbSet<QuickAction> QuickActions { get; set; }
        public DbSet<Distribution> Distributions { get; set; }
        public string DbPath { get; }

        public WslToolboxDbContext()
        {
            var dbFolder = $"{AppConfiguration.AppExecutableDirectory}\\data";

            if (Directory.Exists(dbFolder) == false)
                Directory.CreateDirectory(dbFolder);

            DbPath = $"{dbFolder}\\wsltoolbox.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }

    public class Distribution
    {
        public int DistributionId { get; set; }
        public string DistributionGuid { get; set; }
        public List<QuickAction> QuickActions { get; } = new();
    }

    public class QuickAction
    {
        public int QuickActionId { get; set; }
        public string QuickActionCommand { get; set; }

        public int DistributionId { get; set; }
        public Distribution Distribution { get; set; }
    }
}