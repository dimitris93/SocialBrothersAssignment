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
}
