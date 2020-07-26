using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.ApplicationServices.DataAccess;
using MyGarmin.Dashboard.ApplicationServices.Entities.Strava;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using MyGarmin.Dashboard.Connectivity.StravaClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.ApplicationServices
{
    internal class StravaActivitiesService : IStravaActivitiesService
    {
        private readonly IStravaConnectionsService stravaConnectionsService;
        private readonly IStravaActivitiesRepository stravaActivitiesRepository;
        private readonly IStravaAuthClient stravaAuthClient;
        private readonly IStravaWebhookClient stravaWebhookClient;
        private readonly ILogger<StravaActivitiesService> logger;

        public StravaActivitiesService(
            IStravaConnectionsService stravaConnectionsService,
            IStravaActivitiesRepository stravaActivitiesRepository,
            IStravaAuthClient stravaAuthClient,
            IStravaWebhookClient stravaWebhookClient,
            ILogger<StravaActivitiesService> logger)
        {
            this.stravaConnectionsService = stravaConnectionsService;
            this.stravaActivitiesRepository = stravaActivitiesRepository;
            this.stravaAuthClient = stravaAuthClient;
            this.stravaWebhookClient = stravaWebhookClient;
            this.logger = logger;
        }

        public async Task<int> CreateActivities(long athleteId, string clientId)
        {
            var activities = await this.GetActivities(athleteId, clientId).ConfigureAwait(false);

            if (!activities.Any())
            {
                this.logger.LogWarning($"No exists any activity for Athlete: {athleteId} and ClientId: {clientId}");
                return -1;
            }

            await this.stravaActivitiesRepository.CreateActivities(activities).ConfigureAwait(false);

            this.logger.LogInformation($"{activities.Count} Activities created successfully. AthleteId: {athleteId} and ClientId: {clientId}.");

            return activities.Count;
        }

        public async Task CreateActivity(long activityId, long subscriptionId)
        {
            var connection = await this.stravaConnectionsService.GetConnectionBySubscriptionId(subscriptionId).ConfigureAwait(false);

            var activity = await this.GetActivity(activityId, connection.ClientId).ConfigureAwait(false);

            if (activity == null)
            {
                this.logger.LogWarning($"No exists activity. ActivityId: {activityId} SubscriptionId: {subscriptionId} ClientId: {connection.ClientId}");
            }
            else
            {
                await this.stravaActivitiesRepository.CreateActivity(activity).ConfigureAwait(false);
                this.logger.LogInformation($"Activity with id: {activity.Id} and ClientId: {activity.ClientId} created successfully.");
            }
        }

        public async Task DeleteActivity(long activityId, long subscriptionId)
        {
            var connection = await this.stravaConnectionsService.GetConnectionBySubscriptionId(subscriptionId).ConfigureAwait(false);

            var activity = await this.GetActivity(activityId, connection.ClientId).ConfigureAwait(false);

            if (activity == null)
            {
                this.logger.LogWarning($"No exists activity. ActivityId: {activityId} SubscriptionId: {subscriptionId} ClientId: {connection.ClientId}");
            }
            else
            {
                await this.stravaActivitiesRepository.DeleteActivity(activityId).ConfigureAwait(false);
            }
        }

        private async Task<Activity> GetActivity(long activityId, string clientId)
        {
            var activityInfo = await this.stravaWebhookClient.GetActivity(activityId).ConfigureAwait(false);

            if (activityInfo == null)
            {
                return null;
            }
            else
            {
                var activity = new Activity(activityInfo.Id, clientId, activityInfo.Name, activityInfo.ActivityType);
                activity.SetGearData(activityInfo.GearId, activityInfo.Distance);
                activity.SetElevationData(activityInfo.ElevationGain, activityInfo.ElevantionHigh, activityInfo.ElevationLow);
                activity.SetActivityTimeData(activityInfo.StartDate, activityInfo.MovingTime, activityInfo.ElapsedTime);

                return activity;
            }
        }

        private async Task<List<Activity>> GetActivities(long athleteId, string clientId)
        {
            var allActivities = new List<Activity>();

            var activities = await this.stravaAuthClient.GetActivities(athleteId).ConfigureAwait(false);

            foreach (var activity in activities)
            {
                var act = new Activity(activity.Id, clientId, activity.Name, activity.ActivityType);
                act.SetGearData(activity.GearId, activity.Distance);
                act.SetElevationData(activity.ElevationGain, activity.ElevantionHigh, activity.ElevationLow);
                act.SetActivityTimeData(activity.StartDate, activity.MovingTime, activity.ElapsedTime);

                allActivities.Add(act);
            }

            return allActivities;
        }
    }
}
