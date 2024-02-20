using Microsoft.EntityFrameworkCore;
using QuickActions.Api;
using QuickActions.Api.Identity;
using QuickActions.Api.Identity.IdentityCheck;
using WorkTimer.Api;
using WorkTimer.Api.Repository;
using WorkTimer.Api.Services;
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

#if DEBUG
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase(databaseName: "TestDatabase"));
#else
builder.Services.AddDbContext<AppDbContext>(
    options => options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))),
    contextLifetime: ServiceLifetime.Transient);
#endif

builder.Services.AddHttpContextAccessor();
builder.Services.AddIdentity<User>("session-key", rolesChecker: (s, r) => r.Contains(s.Data.Role.ToString()));

builder.Services
    .AddTransient<UsersRepository>()
    .AddTransient<WorkPeriodRepository>()

    .AddTransient<WorkPeriodsService>()
    ;

var app = builder.Build();

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
#if DEBUG
    context.Database.EnsureCreated();
#else
    context.Database.Migrate();
#endif
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
app.UseExceptionHandler(exception => exception.AddExceptionsHandling());

app.MapControllers();

app.Run();
