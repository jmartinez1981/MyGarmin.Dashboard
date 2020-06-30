using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyGarmin.Dashboard.MongoDb.Extensions;

namespace MyGarmin.Dashboard.Api.Hosting.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection ConfigureDataAccess(this IServiceCollection services, IConfiguration configuration)
        {
            var hostname = configuration.GetValue<string>("MongodbSettings:Host:Address");
            var port = configuration.GetValue<int>("MongodbSettings:Host:Port");
            var scheme = configuration.GetValue<int>("MongodbSettings:Host:Schema");
            var databaseName = configuration.GetValue<string>("MongodbSettings:Database");
            var replicaSetName = configuration.GetValue<string>("MongodbSettings:ReplicaSetName");
            var user = configuration.GetValue<string>("MongodbSettings:Credentials:User");
            var password = configuration.GetValue<string>("MongodbSettings:Credentials:Password");

            services.AddMongoDb(hostname, port, databaseName, user, password, replicaSetName, scheme);
            services.AddDataAccessServices();

            return services;
        }
    }
}
