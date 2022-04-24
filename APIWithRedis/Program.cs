using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDistributedRedisCache(option =>
{
    option.Configuration = builder.Configuration["RedisConnectionString"];
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.MapGet("/", () => "Hello, World!");


// Connecting to Redis
var cache = ConnectionMultiplexer.Connect(builder.Configuration["RedisConnectionString"]).GetDatabase();

// Redis usage
var cacheKey = "ExecutionCount";
var executionCount = (int)cache.StringGet(cacheKey);
executionCount++;
cache.StringSet(cacheKey, executionCount);

app.MapGet("/Count", () => $"Execution count is: {executionCount}");

app.Run();