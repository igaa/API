using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer; 

namespace API
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
            services.AddCors(); 
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0); 
            var  key = Encoding.ASCII.GetBytes("This is My Secret Key Do You Like it"); 

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });

            services.AddAuthentication(x =>  {x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; 
            }).AddJwtBearer(x => { x.RequireHttpsMetadata = false; 
            x.SaveToken = true; 
            x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters{
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false, 
                ValidateAudience = false 
            }; 

            }); 

            services.AddSingleton<IManager, Manager>(); 
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); 

            app.UseAuthentication(); 

            // app.UseMvc();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
