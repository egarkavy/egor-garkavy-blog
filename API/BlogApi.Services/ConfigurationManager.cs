using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace CityNavigator.Services
{
    public static class ConfigurationManager
    {
        public static IConfiguration Configuration { get; set; }
    }
}
