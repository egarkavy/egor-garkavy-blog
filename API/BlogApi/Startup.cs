using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityNavigator.Services;
using CityNavigator.Services.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AutoMapper;
using CityNavigator.Services.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CityNavigatorApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            //Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("conf.json");

            Configuration = builder.Build();
            ConfigurationManager.Configuration = Configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = "{yourAuthorizationServerAddress}";
                    options.Audience = "{yourAudience}";
                });

            MappingsHelper.Configure();

            RegisterTypes(services);
        }

        private void RegisterTypes(IServiceCollection services)
        {
            services.AddTransient<IMongoRepository, Repository>();
            services.AddTransient<IMongoContext, MongoContext>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder.AllowAnyOrigin());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
