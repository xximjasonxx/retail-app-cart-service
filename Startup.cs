using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CartApi.Caching;
using CartApi.Middleware;
using CartApi.Services;
using CartApi.Services.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CartApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string redisConnectionString = Configuration.GetConnectionString("RedisConnection");
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RedisConnection")))
            {
                redisConnectionString = Environment.GetEnvironmentVariable("RedisConnection");
            }
            
            var cacheClientInstance = new RedisCacheClient(redisConnectionString);
            services.AddSingleton<ICacheClient>(cacheClientInstance);

            // scoped services
            services.AddScoped<IUserDataContext, UserDataContext>();

            // transient services
            var productService = new DefaultProductService(Configuration["External:ProductService"]);
            services.AddSingleton<IProductService>(productService);
            services.AddTransient<ICartService, DefaultCartService>();

            services.AddCors(options => {
                options.AddPolicy("AllowAll",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<SetUserContextMiddleware>();
            app.UseCors("AllowAll");
            app.UseMvc();
        }
    }
}
