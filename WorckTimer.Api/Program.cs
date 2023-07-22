using Microsoft.EntityFrameworkCore;
using QuickActions.Api.Identity;
using QuickActions.Api.Identity.IdentityCheck;
using WorkTimer.Api;
using WorkTimer.Api.Repository;
using WorkTimer.Common.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Filters.Add<IdentityFilter<User>>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))),
    contextLifetime: ServiceLifetime.Transient);

builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<User>("session-key", rolesChecker: (s, r) => r.Contains(s.Data.Role.ToString()));

builder.Services
    .AddTransient<UsersRepository>()
    .AddTransient<WorkPeriodRepository>();

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true)
    .AllowCredentials());

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
