using Data.Base;
using Data.DbContexts;
using Data.Entities;
using Data.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(ConnectionDbContexts context) : base(context) 
    {
    }
    
}
