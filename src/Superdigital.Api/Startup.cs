using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using Superdigital.Application.AppServices;
using Superdigital.Application.Interfaces;
using Superdigital.Domain.Interfaces.Repositories;
using Superdigital.Domain.Interfaces.Services;
using Superdigital.Domain.Services;
using Superdigital.Infra.CrossCutting.Configuration;
using Superdigital.Infra.Data.Context;
using Superdigital.Infra.Data.Repositories;
using Swashbuckle.Swagger;
using System.IO;
using System.Text;

namespace Superdigital.Api
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
            services.AddScoped<IContaRepository, ContaRepository>()
                    .AddScoped<IContaService, ContaService>()
                    .AddScoped<IContaAppService, ContaAppService>()
                    .AddScoped<ILancamentoRepository, LancamentoRepository>()
                    .AddScoped<ILancamentoService, LancamentoService>()
                    .AddScoped<ILancamentoAppService, LancamentoAppService>()
                    .AddScoped<IMongoContext, MongoContext>()
                    .AddScoped<ITokenService, TokenService>();
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "SuperDigital",
                        Version = "v1",
                        Description = "API responsável por realizar operações bancárias.",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Name = "Renan Ranciaro",
                            Url = new System.Uri("https://www.linkedin.com/in/renan-ranciaro-544993133/")
                        }
                    });

                string caminhoAplicacao =
                    PlatformServices.Default.Application.ApplicationBasePath;
                string nomeAplicacao =
                    PlatformServices.Default.Application.ApplicationName;
                string caminhoXmlDoc =
                    Path.Combine(caminhoAplicacao, $"{nomeAplicacao}.xml");

                c.IncludeXmlComments(caminhoXmlDoc);
            });

            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json",
                    "SuperDigital");
            });
            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

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
