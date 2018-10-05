using System;
using DXC.EventStore.BaseDomain;
using DXC.EventStore.Domain.WorkItem;
using DXC.EventStore.Infrastructure;
using DXC.EventStore.Infrastructure.DataAccess;
using DXC.EventStore.PersistWorkItem.App.Application;
using DXC.EventStore.PersistWorkItem.App.DomainHandlers.WorkItem;
using DXC.EventStore.PersistWorkItem.App.Infrastructure;
using DXC.EventStore.ReadModel.WorkItem;
using EventStore.ClientAPI;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DXC.EventStore.PersistWorkItem
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddHttpClient();
            services.AddSingleton(x => EventStoreConnection.Create(new Uri("tcp://admin:changeit@localhost:1113")));
            services.AddTransient<IDomainEventPublisher, DomainEventMediator>();
            services.AddTransient<IDomainEventSubscriber, DomainEventMediator>();
            services.AddTransient<IEventStore, Infrastructure.DataAccess.EventStore>();
            services.AddTransient(typeof(IWriteRepository<,>), typeof(WriteRepository<,>));
            services.AddTransient<IReadRepository<WorkItemReadModel>, WorkItemReadModelRepository>();
            services.AddSingleton(x => new ReadDatabase());
            services.AddTransient<IHandleDomainEvent<WorkItemCreatedEvent, WorkItemId>, WorkItemCreatedHandler>();
            services.AddScoped<IUnitOfWork>(x => new UnitOfWork(x.GetService<ReadDatabase>()));
            services.AddTransient<ICreateWorkItem, CreateWorkItem>();
            services.AddTransient<IBus, Bus>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IEventStoreConnection conn)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            conn.ConnectAsync().Wait();
        }
    }
}
