using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization.TestCustom.WebAPI
{
    public class CustomUserAction : IAfterUserCreationAction
    {
        public async Task<bool> DoAfter(Guid userId, IDictionary<string, string> parameters)
        {
            return await Task.Factory.StartNew(() => 
            {
                Console.WriteLine("User custom action performed");

                return true; 
            });
        }
    }
}
