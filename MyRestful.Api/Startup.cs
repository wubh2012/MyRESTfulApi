using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyRestful.Api.Config;
using MyRestful.Api.Models;
using MyRestful.Api.Validator;
using MyRestful.Api.ViewModel;
using MyRestful.Core.Interface;
using MyRestful.Infrastructure;
using MyRestful.Infrastructure.Repository;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.Swagger;

namespace MyRestful.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            _logger = logger;
        }

        private readonly Microsoft.Extensions.Logging.ILogger _logger;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // 配置 Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(Path.Combine("Logs", "log-.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();

            #region 读取配置文件

            // 单个读取
            Log.Information($"GetSection.Value key1 = {Configuration.GetSection("key1").Value}");
            Log.Information($"等价于 GetValue key1 = {Configuration.GetValue<string>("key1")}");
            Log.Information($"key2 = {Configuration.GetSection("key2").Value}");
            Log.Information($"childkey1 = {Configuration.GetSection("key3:childkey1").Value}");


            #endregion

            #region 依赖注入
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            // 添加 AutoMapper 支持
            services.AddAutoMapper(typeof(Startup)); // newer automapper version uses this signature
            // 添加 DbContext
            var sqliteConnectionStr = Configuration.GetSection("ConnectionString:sqlite").Value;
            Log.Debug($"当前 sqlite 连接字符串 = {sqliteConnectionStr}");
            services.AddDbContext<MyContext>(opt =>
            {
                // 使用 sqlite 数据库
                opt.UseSqlite(sqliteConnectionStr);
            });

            // 添加 Swagger 文档支持
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "MyRestful API",
                    Version = "v1",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name = "Shayne Boyer",
                        Email = string.Empty,
                        Url = "https://twitter.com/spboyer"
                    },
                    License = new License
                    {
                        Name = "Use under LICX",
                        Url = "https://example.com/license"
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddFluentValidation(fv =>
                {
                    //方式一：程序集引入方式，这样就不用一个个的去手动添加了
                    //fv.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            // 方式二：手动添加 FluentValidation 验证
            services.AddTransient<IValidator<CityAddVM>, CityAddOrUpdateValidator<CityAddVM>>();
            services.AddTransient<IValidator<CityUpdateVM>, CityAddOrUpdateValidator<CityUpdateVM>>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // 自定义异常页面
            //app.UseStatusCodePages(async context =>
            //{
            //    context.HttpContext.Response.ContentType = "text/plain";
            //    await context.HttpContext.Response.WriteAsync($"My Status page, status code is {context.HttpContext.Response.StatusCode}");
            //});

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = 500;
                        await context.Response.WriteAsync("An Error Occurred");
                    });
                });

            }
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
                // 在根域名中启动 Swagger UI, 设置 RoutePrefix = ''
                c.RoutePrefix = string.Empty;
            });
            // 启动HTTPS，将HTTP请求重定向到HTTPS
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
