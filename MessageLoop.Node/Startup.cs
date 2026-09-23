using Asp.Versioning;

using MessageLoop.Common.Models.LongRun;
using MessageLoop.Common.Services.LongRun;
using MessageLoop.Common.Services.LongRun.Implementation;
using MessageLoop.Node.Code.Providers;
using MessageLoop.Node.Models;
using MessageLoop.Node.Services.MessageLoop;
using MessageLoop.Node.Services.MessageLoop.Implementation;
using MessageLoop.Node.Services.Schedule;
using MessageLoop.Node.Services.Schedule.Implementation;
using MessageLoop.Service.Services.Message;
using MessageLoop.Service.Services.Message.Implementation;
using MessageLoop.Web.Common.Code.Enums;
using MessageLoop.Web.Common.Code.Filters;

using Serilog;

namespace MessageLoop.Node
{
    public class Startup(IConfiguration Configuration)
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(config =>
            {
                config.Filters.Add<LongRunAttribute>();
            }).ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(new GenericControllerProvider());
            });
         
            services.AddSwaggerGen();
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            }).AddApiExplorer(config =>
            {
                config.GroupNameFormat = "'v'VVV";
                config.SubstituteApiVersionInUrl = true;
            });
            
            services.AddSingleton<LongRunContext>();
            services.AddSingleton<ILongRunService<LongRunItem>, LongRunService<LongRunItem>>();
            services.AddSingleton<IMessageService<Messages>, MessageService<Messages>>();
            services.AddTransient<IMessageLoopService, MessageLoopService>();
            services.AddOptions<NodeOptions>().Bind(Configuration.GetSection(nameof(NodeOptions)));
            services.AddOptions<MessageLoopOptions>().Bind(Configuration.GetSection(nameof(MessageLoopOptions)));
            services.AddTransient<IScheduleService, ScheduleService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSerilogRequestLogging();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
