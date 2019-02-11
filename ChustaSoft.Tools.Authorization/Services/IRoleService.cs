using ChustaSoft.Tools.Authorization.Models;
using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization.Services
{
    public interface IRoleService
    {

        Task<Role> Get(Guid roleId);

    }
}
