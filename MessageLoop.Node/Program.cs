using MessageLoop.Node;

using Serilog;
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
try
{
    Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, builder) =>
    {
        builder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
        if (args?.Length > 0)
        {
            builder.AddCommandLine(args);
        }
        builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.AddEnvironmentVariables();
    }).UseSerilog().ConfigureWebHostDefaults(builder =>
    {
        builder.UseStartup<Startup>();
        builder.UseKestrel();
    }).Build().Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Critical error.");
}
finally
{
    Log.CloseAndFlush();
}