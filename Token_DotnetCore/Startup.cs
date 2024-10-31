using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Token_Ifinsys.Interfaces;
using Token_Ifinsys.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;

namespace Token_Ifinsys
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public HttpContext HttpContext { get; private set; }
        public TokenValidationParameters ValidationParameters { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddRazorPages().AddMvcOptions(options => options.EnableEndpointRouting = false);

            // read LDAP Configuration
            services.Configure<LdapConfig>(Configuration.GetSection("Ldap"));

            // for angular 
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    //.AllowCredentials()
                    .WithExposedHeaders("Content-Disposition")); // untuk get filename download
            });
            // end for angular cors

            services.AddHttpClient();

            //token
            services.Configure<TokenManagement>(Configuration.GetSection("tokenManagement"));
            //ini untuk testing token dan secret nya debug untuk tau hasilnya
            var token = Configuration.GetSection("tokenManagement").Get<TokenManagement>();
            var secret = Encoding.ASCII.GetBytes(token.Secret);
            //end token and secret

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });


            services.AddScoped<IAuthenticateService, TokenAuthenticationService>();
            services.AddScoped<IUserManagementService, UserManagementService>();
            //end token

            services.AddControllers().AddNewtonsoftJson(options =>
               options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseSwagger();

            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("v1/swagger.json", "My API V1");
            //});

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy"); //for angular
            app.UseAuthentication();//for jwt
            app.UseMvc();

        }
        public IActionResult StatusCode(int? code)
        {
            // Call the static method
            return Utilities.StatusCode(code: code.Value, this.HttpContext);
        }
    }
}
