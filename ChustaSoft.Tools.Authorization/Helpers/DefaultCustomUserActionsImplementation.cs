using ChustaSoft.Tools.Authorization.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.Helpers
{
    public class DefaultCustomUserActionsImplementation : ICustomUserCreationAction
    {
        /// <summary>
        /// Do nothing behaviour (Null Object Pattern)
        /// </summary>
        public Task AfterCreationAction(Guid userId, IDictionary<string, string> parameters)
        {
            return null;
        }
    }
}
