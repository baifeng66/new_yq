using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Autofac;
using AspectCore.Extensions.Autofac;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using WS.API;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace WS.API
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
            //��ConfigureServices������������NewtonsoftJson��������
            services.AddControllers().AddNewtonsoftJson(option => {
                //����ѭ������
                option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                option.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            });


            services.AddMvc(options =>{
                options.Filters.Add<HttpGlobalExceptionFilter>();//���ȫ�ִ��������
                /*options.Filters.Add<LogFilterAttribute>();*///��־��¼�����ظ���¼����
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddCors(option => option.AddPolicy("any", builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); })
          );

            services.AddControllersWithViews();//���MVC����

            var basePath = AppContext.BaseDirectory;//����swaggerע��
            

            //swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MyApi", Version = "v1" });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);//�ѽӿ��ĵ���·�����ý�ȥ

                //var xmlCPath = System.IO.Path.Combine(basePath, "WS.API.xml");
                //c.IncludeXmlComments(xmlCPath);

                var xmlModelPath = System.IO.Path.Combine(basePath, "WS.Db.Model.xml");
                c.IncludeXmlComments(xmlModelPath);

                //������Ϊwebapi��Ŀ���һ��ȫ�ֵ�token,�������
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "�����token,�Է���api����.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            new List<string>()
                        }
                    });

            });

            services.AddMemoryCache();
            // services.AddTransient<ITokenTool, TokenToolImp>();//��ӹ��߽ӿ���
            // services.Scoped<AuthorizeFilterAttribute>();//���Ȩ����֤����
            // services.AddSingleton<ICache, MemoryCacheImp>();//��ӻ���ӿ���

            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("hsj1234567890abcdefhsj1234567890abcdef"));

            ////���JWT��ǩ���ˣ������ˣ��Լ���Կ������ʱ���
            services.AddAuthentication("Bearer").AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    //3+2
                    //��֤��Կ
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,

                    //��֤������
                    ValidateIssuer = false,
                    ValidIssuer = "Issuer",

                    //��֤������
                    ValidateAudience = false,
                    ValidAudience = "Audience",

                    //�������ʱ��
                    RequireExpirationTime = true,

                    //��֤����ʱ��
                    ValidateLifetime = true,
                };


                o.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    // û��token����token��������
                    OnChallenge = context =>
                    {
                        //��ֹĬ�ϵķ��ؽ��(������)
                        context.HandleResponse();
                        var result = JsonConvert.SerializeObject(new Core.ApiResponse(401, "δ��¼���¼�ѹ��ڣ������µ�¼", 0, 0));
                        context.Response.ContentType = "application/json";
                        //��֤ʧ�ܷ���401
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        context.Response.WriteAsync(result);
                        return Task.FromResult(0);
                    },

                    // ��token������tokenû������ӿڵ�Ȩ��
                    OnForbidden = context =>
                    {
                        //��ֹĬ�ϵķ��ؽ��(������)
                        //context.HandleResponse(); // ���ƹ��������б���˵û������������Ǿ�ע�͵���
                        var result = JsonConvert.SerializeObject(new Core.ApiResponse(403, "��ǰ�˺Ų����иýӿ�Ȩ��", 0, 0));
                        context.Response.ContentType = "application/json";
                        //��֤ʧ�ܷ���403
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        context.Response.WriteAsync(result);
                        return Task.FromResult(0);
                    }
                };

            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            //�����ļ��ϴ��Ĵ�С����
            services.Configure<FormOptions>(o =>
            {
                o.BufferBodyLengthLimit = long.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = long.MaxValue;
                o.MultipartBoundaryLengthLimit = int.MaxValue;
                o.MultipartHeadersCountLimit = int.MaxValue;
                o.MultipartHeadersLengthLimit = int.MaxValue;
            });
        }


        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(System.Reflection.Assembly.Load("WS.Db.BLL")).
                Where(x => x.Name.EndsWith("BLL", StringComparison.OrdinalIgnoreCase))
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .SingleInstance();

            builder.RegisterDynamicProxy();          
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
            //  app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors("any");
            //�ȿ�����֤
            app.UseAuthentication();
            //Ȼ������Ȩ
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "";//���ø��ڵ����
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapAreaControllerRoute(
                name: "areas",
                "Api",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

            app.UseStaticFiles(new StaticFileOptions
            {
                //FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()),
                //���ò�����content-type �����ÿ��������������͵��ļ������ǲ�������ô���ã���Ϊ����ȫ
                //�������ÿ�������apk��nupkg���͵��ļ�
                ContentTypeProvider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider(new Dictionary<string, string>{
                    { ".apk", "application/vnd.android.package-archive" }
              })
            });
        }
    }
}
