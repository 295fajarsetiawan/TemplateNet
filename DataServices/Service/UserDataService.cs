using System.Net;
using Core;
using Data.Base;
using Data.Entities;
using Data.Repository.Interfaces;
using DataServices.Base;
using DataServices.Service.Interfaces;

namespace DataServices.Service;

public class UserDataService: BaseServices<User>, IUserDataService
{   
    
    private readonly IUserRepository _repository;

    public UserDataService(IUserRepository repository) : base(repository)
    {
        _repository = repository;
    }

    public async Task<BooleanErrorResult<User>> FindUsername(string username)
    {
        var query = _repository.Query().Where(x => x.Username == username).FirstOrDefault();
        if (query == null)
        {
            return new BooleanErrorResult<User>(false, "Username not found", null, (int)HttpStatusCode.NotFound);
        }
        return new BooleanErrorResult<User>(true, "success", query, (int)HttpStatusCode.OK);
    }
}
