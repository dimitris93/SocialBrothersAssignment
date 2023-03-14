using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace SocialBrothersAssignment.Data;

public class DataContext : DbContext
{
    public DbSet<Address> Addresses {get; set;}

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    //// The following configures EF to create a Sqlite database file in the specified folder
    //protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    => options.UseSqlite($"Data Source={DbPath}");
}
