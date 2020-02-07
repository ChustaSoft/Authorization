using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{
    public interface IRoleService
    {

        Task<Role> Get(Guid roleId);

    }
}
