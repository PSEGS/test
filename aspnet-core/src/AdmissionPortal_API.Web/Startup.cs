using AdmissionPortal_API.Data.Repository;
using AdmissionPortal_API.Data.RepositoryInterface;
using AdmissionPortal_API.Domain.ViewModel;
using AdmissionPortal_API.Service.Service;
using AdmissionPortal_API.Service.ServiceInterface;
using AdmissionPortal_API.Utility.AutoMapper;
using AdmissionPortal_API.Utility.ErrorLog;
using AdmissionPortal_API.Web.Attributes;
using AdmissionPortal_API.Web.Middlewares.Extensions;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace AdmissionPortal_API.Web
{
    public class Startup
    {       
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
           // Configuration = configuration;

            Environment = environment;

            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
   
            services.AddResponseCaching((options) =>
            {
                // restrict on what max size of response 
                // can be cached in bytes               
                options.MaximumBodySize = 1024;
                options.UseCaseSensitivePaths = true;
            });
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "AdmissionPortal_API.Web", Version = "v2" });
                //c.OperationFilter<VersionHeaderSwaggerAttribute>();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
            });
            });


            // Auto Mapper Configurations
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            services.AddCors();

            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
              options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
           );

            //services.AddApiVersioning(x =>
            //{
            //    x.DefaultApiVersion = new ApiVersion(1, 0);
            //    x.AssumeDefaultVersionWhenUnspecified = true;
            //    x.ReportApiVersions = true;
            //    x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            //});

            //Add Service user Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
           .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = true,
                   ValidateAudience = true,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = Configuration["Jwt:Issuer"],
                   ValidAudience = Configuration["Jwt:Issuer"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
               };

               options.Events = new JwtBearerEvents();
               options.Events.OnChallenge = context =>
               {
                   // Skip the default logic.
                   context.HandleResponse();

                   var payload = new JObject
                   {
                       ["statusCode"] = 401,
                       ["status"] = true,
                       ["message"] = "Unauthorized user",
                       ["resultData"] = null,
                       ["resourceType"] = ""
                   };
                   context.Response.StatusCode = 401;
                   return context.Response.WriteAsync(payload.ToString());
               };
           });

            ResolveDependency(services);

            //services.ConfigureWritable<AppVersion>(Configuration.GetSection("AppVersion"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, ILoggerFactory _loggerFactory)
        {

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("App-Version", "1.0.3");
                context.Response.Headers.Add("Access-Control-Expose-Headers", "App-Version");

                await next();
            });
            if (!Environment.IsProduction())
            {
                app.UseDeveloperExceptionPage();
               
            }
            
            if (!Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v2/swagger.json", "AdmissionPortal_API.Web v2"));
            }
            var logger = _loggerFactory.CreateLogger("Startup");
            logger.LogInformation("Got Here in Startup");

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseCors(builder =>
            //{
            //    builder.WithOrigins(Configuration["CORS:AllowedOriginsList"])
            //           .WithHeaders(Configuration["CORS:AllowedHeadersList"])
            //           .WithMethods(Configuration["CORS:AllowedMethodsList"]);
            //});
            app.UseCors(builder => builder
                                    .AllowAnyOrigin()
                                    .AllowAnyMethod()
                                    .SetIsOriginAllowed((host) => true)
                                    .AllowAnyHeader());

            app.UseAuthorization();

            //app.UseAddMiddlewareExtensions();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseResponseCaching();
            
            // response caching should be done only for GET requests
            app.Use(async (ctx, next) =>
            {
                if (ctx.Request.Method.Equals(HttpMethod.Get))
                {
                    ctx.Response.GetTypedHeaders().CacheControl =
                    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
                    {
                        NoCache=true,
                        NoStore=true
                        //Public = true,
                        //MaxAge = TimeSpan.FromSeconds(0)
                    };
                    ctx.Response.Headers[HeaderNames.Vary] =
                        new string[] { "Accept-Encoding" };
                }

                await next();
            });

        }

        /// <summary>
        /// Add services in container
        /// </summary>
        /// <param name="services"></param>
        public void ResolveDependency(IServiceCollection services)
        {
            services.AddTransient<ILogError, LogError>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IUniversityRepository, UniversityRepository>();
            services.AddTransient<IUniversityService, UniversityService>();
            services.AddTransient<ICollegeRepository, CollegeRepository>();
            services.AddTransient<ICollegeService, CollegeService>();
            services.AddTransient<IGeoRepository, GeoRepository>();
            services.AddTransient<IGeoService, GeoService>();

            services.AddTransient<ICitizenRepository, CitizenRepository>();
            services.AddTransient<ICitizenService, CitizenService>();
            services.AddTransient<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<IEmployeeService, EmployeeService>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<IStateService, StateService>();
            services.AddTransient<IDistrictRepository, DistrictRepository>();
            services.AddTransient<IDistrictService, DistrictService>();

            services.AddTransient<IWorkflowProcessService, WorkflowProcessService>();
            services.AddTransient<IWorkflowProcessRepository, WorkflowProcessRepository>();
            services.AddTransient<IWorkflowStageActionRepository, WorkflowStageActionRepository>();
            services.AddTransient<IWorkflowStageActionService, WorkflowStageActionService>();
            services.AddTransient<IWorkflowStageRepository, WorkflowStageRepository>();
            services.AddTransient<IWorkflowStageService, WorkflowStageService>();

            services.AddTransient<IDepartmentRepository, DepartmentRepository>();
            services.AddTransient<IDepartmentService, DepartmentService>();

            services.AddTransient<IAdminLoginRepository, AdminLoginRepository>();
            services.AddTransient<IAdminLoginService, AdminLoginService>();
            services.AddTransient<INavigationService, NavigationService>();
            services.AddTransient<INavigationRepository, NavigationRepository>();
            services.AddTransient<IRoleNavigationRepository, RoleNavigationRepository>();
            services.AddTransient<IRoleNavigationService, RoleNavigationService>();
            services.AddTransient<IEmployeeRoleMappingRepository, EmployeeRoleMappingRepository>();
            services.AddTransient<IEmployeeRoleMappingService, EmployeeRoleMappingService>();

            services.AddTransient<IProfileRepository, ProfileRepository>();
            services.AddTransient<IProfileService, ProfileService>();

            services.AddTransient<IBlobService, BlobService>();

            services.AddTransient<ILogService, LogService>();
            services.AddTransient<ILogRepository, LogRepository>();

            services.AddTransient<ILookUpAPIRepository, LookUpAPIRepository>();
            services.AddTransient<ILookUpApi, LookupAPIService>();
            services.AddTransient<IStackholderRepository, StackholderRepository>();
            services.AddTransient<IStackholder, StackholderService>();


            services.AddTransient<ICollegeCourseMappingRepository, CollegeCourseMappingRepository>();
            services.AddTransient<ICollegeCourseMappingService, CollegeCourseMappingService>();


            services.AddTransient<ICourse, CourseRepository>();
            services.AddTransient<ICourseService, CourseService>();


            services.AddTransient<ISectionRepository, SectionRepository>();
            services.AddTransient<ISectionService, SectionService>();



            services.AddTransient<ISubjectRepository, CourseSubjectRepository>();
            services.AddTransient<ISubjectService, SubjectService>();


            services.AddTransient<ICollegeSubjectMappingRepository, CollegeSubjectMappingRepository>();
            services.AddTransient<ICollegeCourseSubjectMappingService, CollegeCourseSubjectMappingService>();

            services.AddTransient<ICollegeCourseSeatRepository, CollegeCourseSeatRepository>();
            services.AddTransient<ICollegeCourseSeatService, CollegeCourseSeatService>();

            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddTransient<IStudentService, StudentService>();

            services.AddTransient<IMasterApiRepository, MasterApiRepository>();
            services.AddTransient<IMasterApiService, MasterApiService>();


            services.AddTransient<IUniversityCourseRepository, UniversityCourseRepository>();
            services.AddTransient<IUniversityCourseService, UniversityCourseService>();


            services.AddTransient<IFeeHead, FeeHeadRepository>();
            services.AddTransient<IFeeHeadService, FeeHeadService>();

            services.AddTransient<IExternalAPI, ExternalAPIRepository>();
            services.AddTransient<IExternalAPIService, ExternalAPIService>();
            services.AddTransient<ICourseFeeHead, CourseFeeHead>();
            services.AddTransient<ICourseFeeHeadService, CourseFeeHeadService>();
            services.AddTransient<ICombinationSeatService, CombinationSeatService>();
            services.AddTransient<ICombinationWiseSeat, CombinationSeatRepository>();

            services.AddTransient<IDashboardUgPgService, DashboardUgPgService>();
            services.AddTransient<IDashboardUgPgRepository, DashboardUgPgRepository>();

            services.AddTransient<IStudentRepositoryPG, StudentRepositoryPG>();
            services.AddTransient<IStudentServicePG, StudentServicePG>();

            services.AddTransient<IPgCourseRepository, PgCourseRepository>();
            services.AddTransient<IPgCourseService, PgCourseService>();

            services.AddTransient<IPgFeeHeadRepository, PgFeeHeadRepository>();
            services.AddTransient<IPgFeeHeadService, PgFeeHeadService>();

            services.AddTransient<IPgCourseFeeHeadRepository, PgCourseFeeHeadRepository>();
            services.AddTransient<IPgCourseFeeHeadService, PgCourseFeeHeadService>();

            services.AddTransient<IPgCollegeCourseMappingRepository, PgCollegeCourseMappingRepository>();
            services.AddTransient<IPgCollegeCourseMappingService, PgCollegeCourseMappingService>();

            services.AddTransient<IUniversityPgCourseRepository, UniversityPgCourseRepository>();
            services.AddTransient<IUniversityPgCourseService, UniversityPgCourseService>();

            services.AddTransient<IObjectionRepository, ObjectionRepository>();
            services.AddTransient<IObjectionService, ObjectionService>();

            services.AddTransient<IPGObjectionRepository, PGObjectionRepository>();
            services.AddTransient<IPGObjectionService, PGObjectionService>();

            services.AddTransient<IPgCollegeCourseSeatRepository, PgCollegeCourseSeatRepository>();
            services.AddTransient<IPgCollegeCourseSeatService, PgCollegeCourseSeatService>();

            services.AddTransient<IVerificationRepository, VerificationRepository>();
            services.AddTransient<IVerificationService, VerificationService>();

            services.AddTransient<IPgVerificationRepository, PgVerificationRepository>();
            services.AddTransient<IPgVerificationService, PgVerificationService>();
            services.AddTransient<ICollegeGroup, CollegeGroupRepository>();
            services.AddTransient<ICollegeGroupService, CollegeGroupService>();
            services.AddTransient<ICourseSubjectCombinationCheck, CourseSubjectRepository>();

            services.AddTransient<IRazorPayRepository, RazorPayReporsitory>();
            services.AddTransient<IRazorPayService, RazorPayService>();

            services.AddTransient<IHdfcService, HdfcService>();
            services.AddTransient<IHdfcPaymentRepository, HdfcPaymentRepository>();

            services.AddTransient<IMeritModulePGService, MeritModulePGService>();
            services.AddTransient<IMeritModulePGRepository, MeritModulePGRepository>();

            services.AddTransient<IMeritModuleUGService, MeritModuleUGService>();
            services.AddTransient<IMeritModuleUGRepository, MeritModuleUGRepository>();
            services.AddTransient<Data.RepositoryInterface.IOfflineAdmission, OfflineAdmissionRepository>();
            services.AddTransient<Service.ServiceInterface.IOfflineAdmission, OfflineAdmissionService>();

            services.AddTransient<IPaygovService, PaygovService>();
            services.AddTransient<IPayGovPaymentRepository, PayGovPaymentRepository>();

            var serviceProvider = services.BuildServiceProvider();
        }
    }
}



