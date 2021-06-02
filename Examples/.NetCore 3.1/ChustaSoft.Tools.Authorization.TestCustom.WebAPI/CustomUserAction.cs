using System;

namespace ChustaSoft.Tools.Authorization.TestCustom.WebAPI
{
    public class CustomUserAction : IUserCreated
    {
        
        public void DoAfter(object sender, UserEventArgs e)
        {
            Console.WriteLine("User custom action performed");
        }

    }
}
