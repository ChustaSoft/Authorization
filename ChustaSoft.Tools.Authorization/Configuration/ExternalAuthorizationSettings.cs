using System;
using System.Collections.Generic;
using System.Text;

namespace ChustaSoft.Tools.Authorization.Configuration
{
    public class ExternalAuthorizationOptions
    {
        public Tuple<string, string> GoogleOptions { get; set; }
        public Tuple<string, string> MicrosoftOptions { get; set; }


    }

   
}
