using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ApiToDo.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);
// var connectionString = builder.Configuration.GetConnectionString("ContextConnection");
var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ;

// Add services to the container.
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString)
);

builder.Services.AddDefaultIdentity<User>(options => 
    options.SignIn.RequireConfirmedAccount = false
).AddEntityFrameworkStores<Context>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Allow-Orgin
builder.Services.AddCors(c =>
{
	c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
    // .AllowCredentials()
);

app.MapControllers();

app.Run();
