var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapHealthChecks("/");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.Use(async (context, next) =>
{
    context.Response.OnStarting(() =>
    {
        context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
        return Task.FromResult(0);
    });
    await next();
});
app.MapControllers();

app.Run();