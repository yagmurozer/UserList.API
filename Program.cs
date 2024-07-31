using Microsoft.EntityFrameworkCore;
using UserList.API.Data;
using FluentValidation;
using UserList.API.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

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
    var users = await db.Users
        .OrderBy(user => user.Id) // ID'ye göre artan sýralama
        .ToListAsync();

    return Results.Ok(users);
});

app.MapGet("/api/users/{name}", async (string name, ApplicationDbContext db) =>
{
    var users = await db.Users
        .Where(u => u.Name.ToLower() == name.ToLower())
        .ToListAsync();
    return users.Any() ? Results.Ok(users) : Results.NotFound();
});

app.MapDelete("/api/users/{id}", async (int id, ApplicationDbContext db) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    db.Users.Remove(user);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPost("/api/users", async (User newUser, ApplicationDbContext db, IValidator<User> validator) =>
{
    var validationResult = await validator.ValidateAsync(newUser);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    db.Users.Add(newUser);
    await db.SaveChangesAsync();
    return Results.Created($"/api/users/{newUser.Id}", newUser);
});

app.MapPut("/api/users/{id}", async (int id, User updatedUser, ApplicationDbContext db, IValidator<User> validator) =>
{
    var user = await db.Users.FindAsync(id);
    if (user is null) return Results.NotFound();

    var validationResult = await validator.ValidateAsync(updatedUser);
    if (!validationResult.IsValid)
    {
        return Results.ValidationProblem(validationResult.ToDictionary());
    }

    user.Name = string.IsNullOrEmpty(updatedUser.Name) ? user.Name : updatedUser.Name;
    user.Surname = string.IsNullOrEmpty(updatedUser.Surname) ? user.Surname : updatedUser.Surname;
    user.Email = string.IsNullOrEmpty(updatedUser.Email) ? user.Email : updatedUser.Email;

    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.Run();


