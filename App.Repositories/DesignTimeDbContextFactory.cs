using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace App.Repositories
{
    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    //{
    //    public AppDbContext CreateDbContext(string[] args)
    //    {
    //        var currentDirectory = Directory.GetCurrentDirectory();
    //        var projectDirectory = Directory.GetParent(currentDirectory).Parent.Parent.FullName; // Projenin kök dizinine git

    //        IConfigurationRoot configuration = new ConfigurationBuilder()
    //            .SetBasePath(projectDirectory) // Projenin kök dizinini ayarla
    //            .AddJsonFile("App.Api/appsettings/appsettings.Development.json", optional: false, reloadOnChange: true) // Doğru yol
    //            .Build();

    //        var builder = new DbContextOptionsBuilder<AppDbContext>();
    //        var connectionString = configuration.GetConnectionString("SqlServer");

    //        builder.UseSqlServer(connectionString);

    //        return new AppDbContext(builder.Options);
    //    }
    //}


}
