using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() { }

    // Database is persisting between tests need to fix
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    // Causing persisting issues with tests
    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        if (!options.IsConfigured)
        {
            options.UseInMemoryDatabase("UserManagement.Data.DataContext");
        }

    }

    protected override void OnModelCreating(ModelBuilder model)
        => model.Entity<User>().HasData(new[]
        {
            new User
            {
                Id = 1,
                Forename = "Peter",
                Surname = "Loew",
                Email = "ploew@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(1999, 11, 1)
            },
            new User
            {
                Id = 2,
                Forename = "Benjamin Franklin",
                Surname = "Gates",
                Email = "bfgates@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(1998, 11, 2)
            },
            new User
            {
                Id = 3,
                Forename = "Castor",
                Surname = "Troy",
                Email = "ctroy@example.com",
                IsActive = false,
                DateOfBirth = new DateTime(1997, 11, 3)
            },
            new User
            {
                Id = 4,
                Forename = "Memphis",
                Surname = "Raines",
                Email = "mraines@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(1996, 11, 4)
            },
            new User
            {
                Id = 5,
                Forename = "Stanley",
                Surname = "Goodspeed",
                Email = "sgodspeed@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(1995, 11, 5)
            },
            new User
            {
                Id = 6,
                Forename = "H.I.",
                Surname = "McDunnough",
                Email = "himcdunnough@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(1994, 11, 6)
            },
            new User
            {
                Id = 7,
                Forename = "Cameron",
                Surname = "Poe",
                Email = "cpoe@example.com",
                IsActive = false,
                DateOfBirth = new DateTime(1993, 11, 7)
            },
            new User
            {
                Id = 8,
                Forename = "Edward",
                Surname = "Malus",
                Email = "emalus@example.com",
                IsActive = false,
                DateOfBirth = new DateTime(1992, 11, 8)
            },
            new User
            {
                Id = 9,
                Forename = "Damon",
                Surname = "Macready",
                Email = "dmacready@example.com",
                IsActive = false,
                DateOfBirth = new DateTime(1991, 11, 9)
            },
            new User
            {
                Id = 10,
                Forename = "Johnny",
                Surname = "Blaze",
                Email = "jblaze@example.com",
                IsActive = true,
                DateOfBirth = new DateTime(1990, 11, 10)
            },
            new User
            {
                Id = 11,
                Forename = "Robin",
                Surname = "Feld",
                Email = "rfeld@example.com",
                IsActive = true,
                DateOfBirth = null
            },
        });
    

    public DbSet<User> Users { get; set; }

    public DbSet<Log> Logs { get; set; }

    public async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class
        => await Set<TEntity>().ToListAsync();

    public async Task<TEntity> CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await base.AddAsync(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        base.Update(entity);
        await SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
        base.Remove(entity);
        await SaveChangesAsync();
    }
}
