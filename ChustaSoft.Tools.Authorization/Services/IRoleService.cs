using System;
using System.Threading.Tasks;


namespace ChustaSoft.Tools.Authorization
{
    public interface IRoleService<TRole>
         where TRole : Role
    {

        Task<TRole> Get(Guid roleId);

        Task<bool> ExistAsync(string roleName);

        Task<bool> SaveAsync(string roleName);

    }



    public interface IRoleService : IRoleService<Role> { }

}
