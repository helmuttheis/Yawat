using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Okta.AspNetCore;
using WebApiContrib.Core.Formatter.Bson;
using WebApiContrib.Core.Formatter.Csv;
using WebApiContrib.Core.Formatter.PlainText;
using WebApiContrib.Core.Formatter.Protobuf;
using Yawat;
using YawatServer.Models.TodoApi.Models;
using YawatServer.SeedData;

namespace YawatServer
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
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });

            services.AddDbContext<TodoContext>(opt =>
                opt.UseInMemoryDatabase("TodoList"));

            services.AddScoped<IDbInitializer, DbInitializer>();

            services.AddCors(options =>
            {
                // The CORS policy is open for testing purposes. In a production application, you should restrict it to known origins.
                options.AddPolicy(
                    "AllowAll",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            services.AddAuthentication("BasicAuthentication")
                .AddScheme<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions, BasicAuthentication.BasicAuthenticationHandler>("BasicAuthentication", null);

            // configure DI for application services
            services.AddScoped<BasicAuthentication.IUserService, BasicAuthentication.UserService>();

            services.AddAuthentication("IdentityServer4")
                .AddJwtBearer("IdentityServer4", options =>
                {
                    options.Authority = YawatSettings.Get("IdentityServer.IdentityServerUrl");
                    options.RequireHttpsMetadata = options.Authority.StartsWith("https://");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
                    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
                    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
                })
                .AddOktaWebApi(new OktaWebApiOptions()
                {
                    OktaDomain = YawatSettings.Get("Okta.Domain")
                });


            services.AddControllers()
                .AddPlainTextFormatters()
                .AddXmlSerializerFormatters()
                .AddCsvSerializerFormatters()
                .AddProtobufFormatters()
                .AddBsonSerializerFormatters()
                ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            var scopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var scope = scopeFactory.CreateScope())
            {
                var dbInitializer = scope.ServiceProvider.GetService<IDbInitializer>();
                dbInitializer.SeedData();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
