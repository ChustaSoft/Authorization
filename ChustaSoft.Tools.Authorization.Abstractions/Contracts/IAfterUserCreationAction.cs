﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChustaSoft.Tools.Authorization
{
    public interface IAfterUserCreationAction
    {

        Task<bool> DoAfter(Guid userId, IDictionary<string, string> parameters);

    }
}
