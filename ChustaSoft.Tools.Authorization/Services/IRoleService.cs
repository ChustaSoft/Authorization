using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{
    public interface IRoleService<TRole>
         where TRole : Role
    {

        Task<TRole> Get(Guid roleId);

    }

    public interface IRoleService : IRoleService<Role> { }

}
