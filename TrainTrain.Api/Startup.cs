using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TrainTrain.Domain;
using TrainTrain.Infra;

namespace TrainTrain.Api
{
    public class Startup
    {
        private const string UriBookingReferenceService = "http://localhost:51691/";
        private const string UriTrainDataService = "http://localhost:50680";


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            var bookingReferenceServiceAdapter = new BookingReferenceServiceAdapter(UriBookingReferenceService);
            var trainDataServiceAdapter = new TrainDataServiceAdapter(UriTrainDataService);
            var ticketOffice = new TicketOffice(trainDataServiceAdapter, bookingReferenceServiceAdapter);
            var seatsReservationAdapter = new SeatsReservationAdapter(ticketOffice);
            services.AddSingleton(seatsReservationAdapter);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
