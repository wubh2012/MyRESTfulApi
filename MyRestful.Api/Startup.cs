using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
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
using MyRestful.Core.Interface;
using MyRestful.Infrastructure;
using MyRestful.Infrastructure.Repository;
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
        private readonly ILogger _logger;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            #region 读取配置文件

            // 单个读取
            _logger.LogInformation($"GetSection.Value key1 = {Configuration.GetSection("key1").Value}");
            _logger.LogInformation($"等价于 GetValue key1 = {Configuration.GetValue<string>("key1")}");
            _logger.LogInformation($"key2 = {Configuration.GetSection("key2").Value}");
            _logger.LogInformation($"childkey1 = {Configuration.GetSection("key3:childkey1").Value}");

            // 绑定至类 方式一
            var secondConfig = new SecondConfig();
            Configuration.GetSection("second").Bind(secondConfig);
            _logger.LogInformation($"绑定至类 方式一 second.name = {secondConfig.name}");
            _logger.LogInformation($"绑定至类 方式一 second.email = {secondConfig.email}");
            // 遇到了获取中文乱码问题，默认使用 VS 创建的 json 文件编码方式不是 UTF-8 导致乱码
            _logger.LogInformation($"绑定至类 方式一 second.address = {secondConfig.address}");

            // 绑定至类 方式二
            services.Configure<FirstConfig>(Configuration);
            #endregion

            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddAutoMapper(typeof(Startup)); // newer automapper version uses this signature
            // 添加 DbContext
            services.AddDbContext<MyContext>(opt =>
            {
                // 使用内存数据库
                opt.UseInMemoryDatabase("MyDatabase");
            });

            // 添加 Swagger 文档支持
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Todo API",
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // 自定义异常页面
            app.UseStatusCodePages(async context =>
            {
                context.HttpContext.Response.ContentType = "text/plain";
                await context.HttpContext.Response.WriteAsync($"My Status page, status code is {context.HttpContext.Response.StatusCode}");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();


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
