using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.Abstractions
{
    public interface ICustomUserCreationAction
    {

        Task AfterCreationAction(Guid userId, IDictionary<string, string> parameters);

    }
}
