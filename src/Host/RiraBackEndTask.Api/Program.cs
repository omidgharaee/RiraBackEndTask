using RiraBackEndTask.Api.Extensions;
using RiraBackEndTask.Modules.Persons;
using RiraBackEndTask.Modules.Persons.Presentation.Grpc;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File("logs/Rira-.log", rollingInterval: RollingInterval.Day)
        .CreateLogger();

    builder.Host.UseSerilog();

    builder.Services.AddGrpc();

    builder.Services.AddPersonsModule(builder.Configuration);

    var app = builder.Build();

    app.Services.GetRequiredService<IHost>().ApplyAllMigrations();

    app.UseHttpsRedirection();

    app.MapGrpcService<PersonGrpcService>();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}