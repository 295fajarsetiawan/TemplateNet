using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.DbContexts;

public class ConnectionDbContexts : DbContext
{
    public ConnectionDbContexts(DbContextOptions<ConnectionDbContexts> options) : base(options) { }

    public virtual DbSet<User> Users { get; set; }
}

