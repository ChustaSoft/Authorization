﻿using Microsoft.AspNetCore.Identity;
using System;

namespace ChustaSoft.Tools.Authorization.Models
{
    public class User : IdentityUser<Guid>
    {
        public string Culture { get; set; }
    }
}
