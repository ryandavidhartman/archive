﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagementService.Common
{
    interface IDateTime
    {
        DateTime UtcNow { get; }
    }
}
