using Serilog;
using testudv.Application.Handlers;
using testudv.Application.Services;
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

builder.Services.AddScoped<IPostInfoRepository, PostInfoRepository>();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(GetPostsHandler).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreatePostInfoHandler).Assembly));


builder.Services.AddHttpClient<PostService>((provider, client) =>
{
    client.BaseAddress = new Uri("https://api.vk.com/method/");
});

builder.Services.AddSingleton(provider =>
{
    var token = builder.Configuration["VkApi:AccessToken"];
    var httpClient = provider.GetRequiredService<HttpClient>();
    return new PostService(httpClient, token);
});

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