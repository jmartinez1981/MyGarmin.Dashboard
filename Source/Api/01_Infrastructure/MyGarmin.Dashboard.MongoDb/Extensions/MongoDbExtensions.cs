using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using MyGarmin.Dashboard.ApplicationServices;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities;

namespace MyGarmin.Dashboard.MongoDb.Extensions
{
    public static class MongoDbExtensions
    {
        /// <summary>
        /// AddToSession Mongo dependencies.
        /// </summary>
        /// <param name="services">Collection of service descriptors.</param>
        /// <param name="hostname">Hostname.</param>
        /// <param name="port">Port.</param>
        /// <param name="databaseName">Database name.</param>
        /// <param name="user">User.</param>
        /// <param name="password">Password.</param>
        /// <param name="replicaSetName">Name of the replica set.</param>
        /// <param name="scheme">Connection string scheme.</param>
        /// <returns>Service descriptors with new dependencies.</returns>
        public static IServiceCollection AddMongoDb(this IServiceCollection services, string hostname, int port, string databaseName, string user, string password, string replicaSetName, int scheme = 0)
        {
            var uri = GetMongoUrl(hostname, port, user, password, replicaSetName, scheme);
            var client = new MongoClient(uri.ToMongoUrl());

            var db = client.GetDatabase(databaseName);
            services.AddScoped(serviceProvider => db);

            RegisterMongoDbMappings();

            return services;
        }

        public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStravaConnectionRepository, StravaConnectionRepository>();
            services.AddScoped<IGarminConnectionRepository, GarminConnectionRepository>();

            return services;
        }

        private static MongoUrlBuilder GetMongoUrl(string hostname, int port, string user, string password, string replicaSetName, int scheme = 0)
        {
            return new MongoUrlBuilder
            {
                Server = new MongoServerAddress(hostname, port),
                ReplicaSetName = string.IsNullOrEmpty(replicaSetName) ? null : replicaSetName,
                Username = string.IsNullOrEmpty(user) ? null : user,
                Password = string.IsNullOrEmpty(password) ? null : password,
                Scheme = (ConnectionStringScheme)scheme,
                UseTls = string.IsNullOrEmpty(replicaSetName),
            };
        }

        private static void RegisterMongoDbMappings()
        {
            BsonClassMap.RegisterClassMap<User>().MapIdProperty(x => x.Username);
            BsonClassMap.RegisterClassMap<StravaConnection>().MapIdProperty(x => x.ClientId);
            BsonClassMap.RegisterClassMap<GarminConnection>().MapIdProperty(x => x.Username);
        }
    }
}
