using AutoMapper;
using Data;
using Data.DbContexts;
using DataServices;
using Microsoft.EntityFrameworkCore;
using Template.Config;
using Template.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ConnectionDbContexts>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// add sevices from other layers
builder.Services.RegisterRepositoryService();
builder.Services.RegisterDataService();
builder.Services.RegisterValidatorMapping();
builder.Services.RegisterJwt(builder.Configuration); 
builder.Services.RegisterCors(builder.Configuration);
builder.Services.RegisterSwagger();
builder.Services.AddCustomAuthorizationPolicies();
builder.Services.AddScoped<IdentityConfig>();

// Configure AutoMapper with logging
var loggerFactory = LoggerFactory.Create(builder =>
{
    builder.AddConsole();  // Adds console logging
});
var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new AutoMapperProfile());
}, loggerFactory);
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/V1/swagger.json", "Swagger API V1");
    c.DocumentTitle = "API DOC Swagger";
    c.RoutePrefix = "api/swagger";
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCors("AllowSpecificOrigin");

app.UseRouting();

app.UseAuthentication(); // Mengaktifkan autentikasi JWT
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
