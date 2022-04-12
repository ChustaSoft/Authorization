using ChustaSoft.Common.Utilities;
using System;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    public class AuthConfigurationException : Exception
    {

        public IEnumerable<ErrorMessage> Errors { get; private set; }


        public AuthConfigurationException(string message, IEnumerable<ErrorMessage> errors)
            : base(message)
        {
            Errors = errors;
        }

    }
}
