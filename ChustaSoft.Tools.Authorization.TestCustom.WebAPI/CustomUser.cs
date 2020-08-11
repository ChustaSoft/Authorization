using ChustaSoft.Tools.Authorization.Models;

namespace ChustaSoft.Tools.Authorization.TestCustom.WebAPI
{
    public class CustomUser : User
    {
        public string CustomStringProperty { get; set; }

        public int CustomIntProperty { get; set; }
    }
}
