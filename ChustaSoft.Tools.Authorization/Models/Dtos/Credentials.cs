﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace ChustaSoft.Tools.Authorization
{
    [Serializable]
    public class Credentials
    {

        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Culture { get; set; }

        public IDictionary<string, string> Parameters { get; set; }


        public Credentials()
        {
            Parameters = new Dictionary<string, string>();
        }

    }
}
