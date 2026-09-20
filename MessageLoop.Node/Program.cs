using MessageLoop.Node;

using Serilog;
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateBootstrapLogger();
try
{
    Host.CreateDefaultBuilder(args).ConfigureAppConfiguration((context, builder) =>
    {
        builder.SetBasePath(context.HostingEnvironment.ContentRootPath);
        if (args?.Length > 0)
        {
            builder.AddCommandLine(args);
        }
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