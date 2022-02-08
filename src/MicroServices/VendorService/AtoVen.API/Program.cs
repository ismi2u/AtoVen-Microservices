using DataService.AccountControl.Models;
using DataService.DataContext;
using EmailSendService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<AtoVenDbContext>(options => 
                options.UseSqlServer(builder.Configuration.GetConnectionString("SQLServerRunningAsContainer")));
builder.Services.AddDbContextPool<SchwarzDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SchwarzSQLServerRunningAsContainer")));


//builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
//builder.Services.AddScoped<IBankRepository, BankRepository>();
//builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AtoVenDbContext>()
                .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);    // to provide default token for password reset



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
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, @"documents")),
//    RequestPath = "/app/documents"
//});

app.MapControllers();
app.UseCors("myCorsPolicy");

app.Run();
