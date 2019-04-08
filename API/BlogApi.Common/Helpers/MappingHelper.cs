using AutoMapper;
using CityNavigator.Services.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace CityNavigator.Services.Helpers
{
    public static class MappingsHelper
    {
        public static void Configure(params Assembly[] assemblies)
        {
            var profiles = assemblies.SelectMany(a => a.GetTypes())
                                     .Where(t => typeof(Profile).IsAssignableFrom(t))
                                     .Select(Activator.CreateInstance)
                                     .Cast<Profile>()
                                     .ToList();
            
            Mapper.Initialize(cfg =>
            {
                profiles.ForEach(cfg.AddProfile);
            });
        }
    }
}
