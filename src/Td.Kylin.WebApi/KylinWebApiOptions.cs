﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Td.Kylin.WebApi
{
    public class KylinWebApiOptions
    {
        public IConfigurationRoot Configuration { get; set; }
    }
}
