using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public class DefaultCustomUserActionsImplementation : IAfterUserCreationAction
    {
        /// <summary>
        /// Do nothing behaviour (Null Object Pattern)
        /// </summary>
        public async Task<bool> DoAfter(Guid userId, IDictionary<string, string> parameters)
        {
            return await Task.Factory.StartNew(() => { return true; });
        }
    }
}
