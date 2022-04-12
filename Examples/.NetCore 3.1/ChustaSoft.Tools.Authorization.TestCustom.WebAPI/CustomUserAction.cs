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

    public class CustomUserActionA : IUserCreated
    {

        public void DoAfter(object sender, UserEventArgs e)
        {
            Console.WriteLine("User custom action A performed");
        }

    }

    public class CustomUserActionB : IUserCreated
    {

        public void DoAfter(object sender, UserEventArgs e)
        {
            Console.WriteLine("User custom action B performed");
        }

    }
}
