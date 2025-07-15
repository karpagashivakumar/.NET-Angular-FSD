using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetDAL.Models
{
    public class AssetManagementDbContextFactory : IDesignTimeDbContextFactory<AssetManagementDbContext>
    {
        public AssetManagementDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AssetManagementDbContext>();

            // Use the same connection string as in your appsettings.json or hardcoded for now
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=AssetMS;Trusted_Connection=True;TrustServerCertificate=True;");

            return new AssetManagementDbContext(optionsBuilder.Options);
        }
    }
}
