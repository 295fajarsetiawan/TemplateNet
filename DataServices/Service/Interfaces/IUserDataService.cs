using Core;
using Data.Entities;
using DataServices.Base;

namespace DataServices.Service.Interfaces;
public interface IUserDataService : IBaseServices<User>
{
    Task<BooleanErrorResult<User>> FindUsername(string username);
}


