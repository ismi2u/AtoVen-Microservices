using DataService.AccountControl.Models;
using DataService.DataContext;
using EmailSendService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//LocalSQLServerAndAPIRunningInContainer'
//SQLServerRunningAsContainer
//ContainerNetworkAtoVenMSSQLServer

//SchwarzLocalSQLServerAndAPIRunningInContainer
//SchwarzSQLServerRunningAsContainer
//ContainerNetworkSchwarzMSSQLServer

builder.Services.AddDbContextPool<AtoVenDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("ContainerNetworkAtoVenMSSQLServer")));
builder.Services.AddDbContextPool<SchwarzDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ContainerNetworkSchwarzMSSQLServer")));


//builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
//builder.Services.AddScoped<IBankRepository, BankRepository>();
//builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AtoVenDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);    // to provide default token for password reset

//services.AddHttpsRedirection(options => options.HttpsPort = 443);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // setting it to false, as we dont know the users connecting to this server
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,


        ValidIssuer = "https://localhost:5001",
        ValidAudience = "https://localhost:5001",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKey12323232"))
    };
});

builder.Services.AddCors(options =>
              options.AddPolicy("myCorsPolicy", builder => {
                  builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
              }
              ));

//email service
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);
builder.Services.AddScoped<IEmailSender, EmailSender>();

var app = builder.Build();

IWebHostEnvironment env = app.Environment;

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthentication(); //add before MVC
app.UseAuthorization();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, @"docdata")),
    RequestPath = "/app/docdata"
});

app.MapControllers();
app.UseCors("myCorsPolicy");

app.Run();
