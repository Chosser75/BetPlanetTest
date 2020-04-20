using BetPlanetTest.Data;
using BetPlanetTest.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;

namespace BetPlanetTest
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
            services.AddControllers();
            services.AddSingleton<IDatabaseDispatcher, PostgresDispatcher>();
            services.AddHttpContextAccessor();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            // ��������� � ������������ �������� � appsettings �������� endpoint
            // �������� ����������: "endpoint - �������� �� config-�����".
            // (�� ���������� ������, ��� �� ���-���� ������� ��� ����������, � ENV ��� config-�����)
            var options = new RewriteOptions().AddRewrite(
                "^(" + Environment.GetEnvironmentVariable("endpoint") + @")(/\d/.*)", "/secretapi$2", true);
                //"^(" + Configuration.GetValue<string>("RestApi:endpoint") + @")(/\d/.*)", "/secretapi$2", true);
            app.UseRewriter(options);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
