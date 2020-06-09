using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{
    public interface IRoleService<TRole>
         where TRole : Role
    {

        Task<TRole> Get(Guid roleId);

        Task<bool> CreateAsync(string roleName);

    }



    public interface IRoleService : IRoleService<Role> { }

}
