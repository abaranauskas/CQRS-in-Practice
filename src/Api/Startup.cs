using Api.Utils;
using Logic.Decorators;
using Logic.Dtos;
using Logic.Students;
using Logic.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var config = new Config(3); //from env vars in reality
            services.AddSingleton(config);

            var commandsConnectionString = new CoammandsConnectionString(Configuration["ConnectionString"]);
            services.AddSingleton(commandsConnectionString);

            var queriesConnectionString = new QueriesConnectionString(Configuration["QueriesConnectionString"]);
            services.AddSingleton(queriesConnectionString);

            services.AddSingleton<SessionFactory>();
            services.AddTransient<UnitOfWork>();
            //services.AddTransient<ICommandHandler<EditPersonalInfoCommand>>(provider =>
            //    new AuditLoggingDecorator<EditPersonalInfoCommand>(
            //        new DataBaseRetryDecorator<EditPersonalInfoCommand>(
            //            new EditPersonalInfoCommandHandler(provider.GetService<SessionFactory>()),
            //            provider.GetService<Config>()
            //            )));
            //services.AddTransient<ICommandHandler<RegisterCommand>, RegisterCommandHandler>();
            //services.AddTransient<ICommandHandler<UnregisterCommand>, UnregisterCommandHandler>();
            //services.AddTransient<ICommandHandler<DisenrollCommand>, DisenrollCommandHandler>();
            //services.AddTransient<ICommandHandler<TransferCommand>, TransferCommandHandler>();
            //services.AddTransient<ICommandHandler<EnrollCommand>, EnrollCommandHandler>();
            //services.AddTransient<IQueryHandler<GetListQuery, List<StudentDto>>, GetListQueryHandler>();
            services.AddSingleton<Messages>();
            services.AddHandlers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }
    }
}
