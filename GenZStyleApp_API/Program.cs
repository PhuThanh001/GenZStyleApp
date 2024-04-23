using BMOS.BAL.DTOs.JWT;
using BMOS.BAL.Validators.Accounts;
using FluentValidation;
using GenZStyleAPP.BAL.DTOs.Accounts;
using GenZStyleAPP.BAL.DTOs.Comments;
using GenZStyleAPP.BAL.DTOs.FireBase;
using GenZStyleAPP.BAL.DTOs.PostLike;
using GenZStyleAPP.BAL.DTOs.HashTags;
using GenZStyleAPP.BAL.DTOs.Notifications;
using GenZStyleAPP.BAL.DTOs.Posts;
using GenZStyleAPP.BAL.Profiles.Accounts;
using GenZStyleAPP.BAL.Profiles.HashTags;
using GenZStyleAPP.BAL.Profiles.Notifications;
using GenZStyleAPP.BAL.Profiles.Posts;
using GenZStyleAPP.BAL.Profiles.Users;
using GenZStyleAPP.BAL.Profiles.Comments;
using GenZStyleAPP.BAL.Profiles.PostLike;
using GenZStyleAPP.BAL.Repository.Implementations;
using GenZStyleAPP.BAL.Repository.Interfaces;
using GenZStyleAPP.BAL.Validators.Accounts;
using GenZStyleAPP.BAL.Validators.Posts;
using GenZStyleAPP.BAL.Validators.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using ProjectParticipantManagement.BAL.DTOs.Authentications;
using ProjectParticipantManagement.BAL.Repositories.Interfaces;
using ProjectParticipantManagement.DAL.Infrastructures;
using System.Reflection;
using System.Text;
using GenZStyleAPP.BAL.Validators.Hashtags;
using GenZStyleAPP.BAL.Validators.Comments;
using GenZStyleAPP.BAL.Validators.Authentication;
using BMOS.BAL.DTOs.Authentications;
using GenZStyleAPP.BAL.Profiles.UserRelations;
using GenZStyleAPP.BAL.DTOs.UserRelations;
using GenZStyleAPP.BAL.DTOs.Users;
using GenZStyleAPP.BAL.DTOs.Transactions.MoMo;
using GenZStyleAPP.BAL.DTOs.Transactions;
using GenZStyleAPP.BAL.Validators.Transactions;
using GenZStyleApp_API.SignalRHubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using GenZStyleAPP.BAL.Models;
using GenZStyleApp.DAL.Models;
using Microsoft.Extensions.Configuration;
using GenZStyleAPP.BAL.DTOs.Reports;
using GenZStyleAPP.BAL.Profiles.Reports;
using GenZStyleAPP.BAL.Validators.Reports;
using Quartz.Impl;
using Quartz;
using GenZStyleAPP.BAL.DTOs.HashPosts;
using GenZStyleAPP.BAL.Profiles.HashPosts;
using GenZStyleAPP.BAL.Profiles.Packages;
using GenZStyleAPP.BAL.DTOs.Transactions.ZaloPay;
using GenZStyleAPP.BAL.Profiles.Invoices;
using GenZStyleAPP.BAL.DTOs.Package;
using GenZStyleAPP.BAL.Validators.Packages;
using GenZStyleApp_API.Middlewares;
using GenZStyleApp_API.Extensions;
using GenZStyleAPP.BAL.Validators.Package;

namespace GenZStyleApp_API
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMemoryCache(options =>
            {
                options.SizeLimit = 1024; // Giới hạn kích thước cache là 1024 items
                options.ExpirationScanFrequency = TimeSpan.FromMinutes(1); // Tần suất quét hết hạn cache là mỗi 1 phút
            });
            // Add services to the container.

            builder.Services.AddControllers()
                        .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                            options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                        });
            ;
            /*builder.Services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder("Bearer").RequireAuthenticatedUser().Build(); // chinh cho nay
                opt.Filters.Add(new AuthorizeFilter(policy));
            });*/
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            /*builder.Services.AddSwaggerGen();*/
            builder.Services.AddDbContext<GenZStyleDbContext>(opt =>
            opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionStringDB")));


            /*builder.Services.AddSingleton<IScheduler>(provider =>
            {
                var schedulerFactory = new StdSchedulerFactory();
                return schedulerFactory.GetScheduler().Result;
            });
            builder.Services.AddSingleton<CheckAndDeleteJob>();
            builder.Services.AddHostedService<QuartzHostedService>();*/
            #region JWT 
            builder.Services.AddSwaggerGen(options =>
            {
                // using System.Reflection;
                /*var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));*////chú ý chỗ này

                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "GenZStyle Store Application API",
                    Description = "JWT Authentication API"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[]{}
                    }
                });
            });


            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                //options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; //chinh cho nay
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtAuth:Key"])),
                    ValidateLifetime = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            //ODATA
            var modelBuilder = new ODataConventionModelBuilder();
            //xoa 1 chu s la het loi 
            modelBuilder.EntitySet<GetUserResponse>("User");
            modelBuilder.EntitySet<GetAccountResponse>("Account");
            modelBuilder.EntitySet<GetLoginResponse>("Authentication");
            modelBuilder.EntitySet<GetHashTagResponse>("HashTag");
            modelBuilder.EntitySet<GetPostLikeResponse>("PostLike");
            modelBuilder.EntitySet<GetCommentResponse>("Comment");
            modelBuilder.EntitySet<GetPostLikeResponse>("Like");
            modelBuilder.EntitySet<GetPostResponse>("Posts");
            modelBuilder.EntitySet<GetNotificationResponse>("Notification");
            modelBuilder.EntitySet<GetReportResponse>("Reports");
            modelBuilder.EntitySet<GetHashPostsResponse>("HashPost");
            modelBuilder.EntitySet<GetInvoiceResponse>("Invoice");
            modelBuilder.EntitySet<GetPackageResponse>("Package");



            builder.Services.AddControllers().AddOData(options => options.Select()
                                                                         .Filter()
                                                                         .OrderBy()
                                                                         .Expand()
                                                                         .Count()
                                                                         .SetMaxTop(null)
                                                                         .AddRouteComponents("odata", modelBuilder.GetEdmModel()));



            //Dependency Injections
            builder.Services.Configure<JwtAuth>(builder.Configuration.GetSection("JwtAuth"));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAuthenticationRepository, AuthenticationRepository>();
            builder.Services.AddScoped<IHashTagRepository, HashTagRepository>();
            builder.Services.AddScoped<ILikeRepository, LikeRepository>();
            builder.Services.AddScoped<ICommentRepository, CommentRepository>();
            //builder.Services.AddScoped<IPostRepository, PostRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IReportRepository, ReportRepository>();
            builder.Services.AddScoped<IPackageRepository, PackageRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
            builder.Services.AddScoped<ICollectionRepository, CollectionRepository>();
            



            
            builder.Services.AddScoped<IEmailRepository, EmailRepository>();
            builder.Services.Configure<FireBaseImage>(builder.Configuration.GetSection("FireBaseImage"));
            //DI Validator
            builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterValidation>();
            builder.Services.AddScoped<IValidator<GetLoginRequest>, LoginValidation>();
            builder.Services.AddScoped<IValidator<ChangePasswordRequest>, ChangePasswordValidation>();
            builder.Services.AddScoped<IValidator<UpdateUserRequest>, UpdateUserValidation>();
            builder.Services.AddScoped<IValidator<GetHashTagRequest>, PostHashTagRequestValidation>();
            builder.Services.AddScoped<IValidator<GetCommentRequest>, GetCommentRequestValidator>();
            builder.Services.AddScoped<IValidator<PostRecreateTokenRequest>, PostRecreateTokenValidation>();
            builder.Services.AddScoped<IValidator<PostTransactionRequest>, PostTransactionValidation>();
            builder.Services.AddScoped<IValidator<AddPostRequest>, AddPostValidation>();
            builder.Services.AddScoped<IValidator<UpdatePostRequest>, UpdatePostValidation>();
            builder.Services.AddScoped<GenZStyleAPP.BAL.Models.EmailConfiguration>();
            builder.Services.AddScoped<IValidator<AddReportRequest>, AddReportValidation>();
            builder.Services.AddScoped<IValidator<AddReporterRequest>, AddReporterValidation>();
            builder.Services.AddScoped<IValidator<GetPackageRequest>, PackageValidation>();
            builder.Services.AddScoped<IValidator<UpdatePackageRequest>, UpdatePackageValidation>();

            builder.Services.AddScoped<GenZStyleAPP.BAL.Models.EmailConfiguration>();
            builder.Services.Configure<FireBaseImage>(builder.Configuration.GetSection("FireBaseImage"));

            // Momo config
            builder.Services.Configure<MomoConfigModel>(builder.Configuration.GetSection("MomoAPI"));
            // Zalo config
            builder.Services.Configure<ZaloConfigModel>(builder.Configuration.GetSection("ZaloAPI"));
            // Auto mapper config
            builder.Services.AddAutoMapper(typeof(AccountProfile),
                                           typeof(PostLikeProfile),
                                           typeof(CommentProfile),
                                            typeof(CustomerProfile),
                                            typeof(PostProfile),
                                            typeof(NotificationProfile),
                                            typeof(HashTagProfile),
                                            typeof(AccountProfile),
                                            typeof(CustomerProfile),
                                            typeof(UserRelationProfile),
                                            typeof(HashPostProfile) ,                                                                                                                                 
                                            typeof(ReportProfile),
                                            typeof(InvoiceProfile),
                                            typeof(PackageProfile)
                                            
                                            ); 
            //*/ For Entity Framework
            var configuration = builder.Configuration;
            /*builder.Services.AddDbContext<GenZStyleDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnectionStringDB")));*/ //chỉnh sửa lại chỗ này
            //builder.Services.AddSignalR();
            


            //Add middleware extentions
            builder.Services.AddTransient<ExceptionMiddleware>(); //comment chỗ này
            //Add Config for Required Email
            builder.Services.Configure<IdentityOptions>(
                opts => opts.SignIn.RequireConfirmedEmail = true
                );           
            //Add Email Configs
            var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            builder.Services.AddSingleton(emailConfig);
            // For Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<GenZStyleDbContext>()
                .AddDefaultTokenProviders();



            var app = builder.Build();
            
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");

                });
            
  
            
            //add CORS
            app.UseCors(
                x =>
                x.AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials()
                );
            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();
            //Add middleware extentions
            /*app.ConfigureExceptionMiddleware();*/ //comment chỗ này
            app.MapControllers();
            //app.MapHub<ChatHub>("/Chat");


            app.Run();
        }


    }
}


     




