using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Reflection;
using szosztar.Data;
using szosztar.Data.Interfaces;
using szosztar.Logic;
using szosztar.Logic.Interfaces;

namespace szosztar
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
            services.AddControllersWithViews();

            services.AddScoped<IWordLogic, WordLogic>();
            services.AddScoped<IDataAccess, DataAccess>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new Microsoft.OpenApi.Models.OpenApiInfo
                    {
                        Title = "Szosztar API",
                        Description = "sz",
                        Version = "V1"
                    });

                var fileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var filePath = Path.Combine(AppContext.BaseDirectory, fileName);
                options.IncludeXmlComments(filePath);
            });

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(
            //       name: "AllowOrigin",
            //       builder =>
            //       {
            //           builder.AllowAnyOrigin()
            //          .AllowAnyMethod()
            //          .AllowAnyHeader();
            //       });
            //});

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAllOrigins",
            //     builder =>
            //     {
            //         builder.AllowAnyOrigin();
            //     });
            //});

            //var corsBuilder = new CorsPolicyBuilder();
            //corsBuilder.AllowAnyHeader();
            //corsBuilder.WithMethods("GET", "POST");
            //corsBuilder.AllowAnyOrigin();
            //services.AddCors(options => options.AddPolicy("AllowAll", corsBuilder.Build()));

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("CorsPolicy",
            //        builder => builder.AllowAnyOrigin()
            //            .AllowAnyMethod()
            //            .AllowAnyHeader());
            //});

            //services.AddCors();

            //services.AddCors(options =>
            //{
            //    options.AddPolicy(
            //      "CorsPolicy",
            //      builder => builder.WithOrigins("http://localhost:4200")
            //      .AllowAnyMethod()
            //      .AllowAnyHeader()
            //      .AllowCredentials());
            //});
            //services.AddAuthentication(IISDefaults.AuthenticationScheme);

            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowAll", p =>
            //    {
            //        p.AllowAnyOrigin()
            //        .AllowAnyHeader()
            //        .AllowAnyMethod();
            //    });
            //});
            //services.AddMvc(); // must be after AddCors

            //if (_env.IsDevelopment())
            //{
            //    services.AddCors(options =>
            //    {
            //        options.AddPolicy("AllowAll",
            //                  p => p.AllowAnyOrigin()
            //                        .AllowAnyHeader()
            //                        .AllowAnyMethod()
            //                        .AllowCredentials());
            //    });
            //}


            //services.AddCors(allowsites => {
            //    allowsites.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            //});

            services.AddCors(feature =>
                feature.AddPolicy(
                    "CorsPolicy",
                    apiPolicy => apiPolicy
                                    //.AllowAnyOrigin()
                                    //.WithOrigins("http://localhost:4200")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .SetIsOriginAllowed(host => true)
                                    .AllowCredentials()
                                ));
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(opts =>
            {
                opts.WithOrigins(new string[]
                {
                "http://localhost:4200"
                    // whatever domain/port u are using
                });

                opts.AllowAnyHeader();
                opts.AllowAnyMethod();
                opts.AllowCredentials();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "We Szosztarsz!");
                //options.RoutePrefix = "";
            });

            //app.UseCors(opts =>
            //{
            //    opts.AllowAnyOrigin();
            //});


            //app.UseCors("AllowAll");

            //if (_env.IsDevelopment())
            //{
            //    app.UseCors("AllowAll");
            //}

            //app.UseCors("AllowAll");
            //app.UseMvc();

            //app.UseCors("CorsPolicy");

            //app.UseCors(builder => { 
            //    builder
            //    .SetIsOriginAllowed(_ => true)
            //    .WithOrigins()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .AllowCredentials(); });

            //app.UseCors(
            //    options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
            //);

            //app.UseCors("CorsPolicy");

            //app.UseCors("AllowOrigin");
            //app.UseCors(x => x
            //    .AllowAnyOrigin()
            //    .AllowAnyMethod()
            //    .AllowAnyHeader()
            //    .SetIsOriginAllowed(origin => true) // allow any origin
            //    .AllowCredentials());
        }
    }
}
