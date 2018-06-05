using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Api.Hubs;
using Api.Mappings;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.AlgorithmService;
using Services.GameResourcesService;
using Services.MapGenerationService;
using Services.MatchesManagerService;
using Services.MatrixService;
using Services.PlayerResourcesService;

namespace Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);
            services.AddCors(options => {
                options.AddPolicy("AllowAny", x => {
                    x.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin()
                        .AllowCredentials();
                });
            });
            services.AddSignalR();
            services.AddAutoMapper();

            services.AddSingleton<IGameResourcesService, GameResourcesService>();
            services.AddSingleton<IPlayerResourcesService, PlayerResourcesService>();
            services.AddSingleton<IAlgorithmService, AlgorithmService>();
            services.AddSingleton<IMatrixService, MatrixService>();
            services.AddSingleton<IMapGenerationService, MapGenerationService>();
            services.AddSingleton<IMatchesManagerService, MatchesManagerService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseHsts();
            }

            app.UseCors("AllowAny");

            app.UseMvc();

            app.UseSignalR (routes => {
                routes.MapHub<MatchHub>("/match");
            });
        }
    }
}