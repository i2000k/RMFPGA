using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Configurations;

namespace Persistence;

public class RfContext : DbContext
{
	public RfContext(DbContextOptions<RfContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SessionConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StandConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(StudentConfiguration).Assembly);
    }

    public DbSet<Student>? Students { get; set; }
    public DbSet<Administrator>? Administrators { get; set; }
    public DbSet<Stand>? Stands { get; set; }
    public DbSet<Session>? Sessions { get; set; }
    
}
