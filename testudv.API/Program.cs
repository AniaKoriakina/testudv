using FluentValidation;
using Serilog;
using testudv.Application.Commands;
using testudv.Application.Handlers;
using testudv.Application.Services;
using testudv.Application.Validators;
using testudv.Domain.Interfaces;
using testudv.Infrastructure.Data;
using testudv.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/postinfo.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

if (!Directory.Exists("logs"))
{
    Directory.CreateDirectory("logs");
}

builder.Host.UseSerilog();
builder.Services.AddScoped<IValidator<GetPostsCommand>, GetPostsValidator>();
builder.Services.AddScoped<IValidator<CreatePostsInfoCommand>, CreatePostsInfoValidator>();

builder.Services.AddScoped<IPostInfoRepository, PostInfoRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPostsHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePostInfoHandler).Assembly));

builder.Services.AddHttpClient("VkApi", client =>
{
    client.BaseAddress = new Uri("https://api.vk.com/");
});
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddSingleton(builder.Configuration["VkApi:AccessToken"]);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.WithOrigins("*")
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();