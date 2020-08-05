using ChustaSoft.Tools.Authorization.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.TestCustom.WebAPI
{
    public class CustomUserAction : ICustomUserCreationAction
    {
        public Task AfterCreationAction(Guid userId, IDictionary<string, string> parameters)
        {
            throw new NotImplementedException();
        }
    }
}
