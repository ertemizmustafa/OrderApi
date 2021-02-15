using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Order.Data.Configuration;
using Order.Logic.Configuration;
using Order.Logic.Model;
using Order.Logic.Validations;
using OrderApi.Middleware;
using System.Linq;

namespace OrderApi
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
            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = c =>
                {
                    var errors = string.Join('\n', c.ModelState.Values.Where(i => i.Errors.Count > 0)
                        .SelectMany(i => i.Errors)
                        .Select(i => i.ErrorMessage));
                    return new OkObjectResult(ResponseModel.Error(errors)); // custom validasyon modifiye
                };
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<OrderInfoModelValidation>())
            .Services.AddTransient<IValidatorInterceptor, ValidationHandler>();

            var connection = Configuration.GetSection("ConnectionString").Value;

            services
                .AddDatabase<SqliteConnection>(x => { x.ConnectionString = connection; x.CreateOrderDatabase(); })
                .AddOrdersLogic()
                .AddMediatR(typeof(LogicConfigurationExtension).Assembly)
                .AddSwaggerGen(x =>
                {
                    x.SwaggerDoc("v1", new OpenApiInfo { Title = "Order Api", Version = "v1" });
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "Order Api");
            });

            //Exception kontrol ve loglama
            app.UseMiddleware<ExceptionHandler>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
