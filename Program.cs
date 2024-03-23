﻿using Microsoft.EntityFrameworkCore;
using movie_mart_api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context to dependency injection container
// Configure the database provider to use SQL Server
builder.Services.AddDbContext<MovieMartContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("MovieMartContext") ?? throw new InvalidOperationException("Connection string 'ApplicationContext' not found.")));

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

