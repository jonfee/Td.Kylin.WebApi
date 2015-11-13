using Microsoft.Framework.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Td.Kylin.WebApi.Website
{
    public class KylinWebApiOptions
    {
        public IConfigurationRoot Configuration { get; set; }
    }
}
