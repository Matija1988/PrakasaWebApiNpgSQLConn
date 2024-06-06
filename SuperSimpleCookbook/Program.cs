using Autofac;
using Autofac.Extensions.DependencyInjection;
using Npgsql;
using SuperSimpleCookbook;
using SuperSimpleCookbook.Model;
using SuperSimpleCookbook.Model.Model;
using SuperSimpleCookbook.Repository.Common;
using SuperSimpleCookbook.Service.Common;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new AutofacModule()));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddScoped((provider) => new NpgsqlConnection(connectionString));

//builder.Services.AddScoped<IAuthorService<Author, AuthorRecipe>, AuthorService>();
//builder.Services.AddScoped<IRepositoryAuthor<Author, AuthorRecipe>, AuthorRepository>();

//builder.Services.AddScoped<IRecipeService<Recipe>, RecipeService>(); 
//builder.Services.AddScoped<IRepositoryRecipe<Recipe>, RecipeRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
