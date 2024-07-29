using Microsoft.EntityFrameworkCore;
using UserList.API.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Users List API V1"));
}

app.UseHttpsRedirection();

// Define minimal API endpoints
app.MapGet("/api/users", async (ApplicationDbContext db) =>
{
    return await db.Users.ToListAsync();
});


app.Run();


