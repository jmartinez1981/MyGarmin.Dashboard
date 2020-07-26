using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyGarmin.Dashboard.Api.Models;
using MyGarmin.Dashboard.ApplicationServices.Interfaces;
using System.Threading.Tasks;

namespace MyGarmin.Dashboard.Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class SubscriptionsController : ControllerBase
    {
        private readonly ILogger<SubscriptionsController> logger;
        private readonly IStravaSubscriptionsService stravaSubscriptionService;
        private readonly IStravaActivitiesService stravaActivitiesService;

        public SubscriptionsController(
            ILogger<SubscriptionsController> logger,
            IStravaSubscriptionsService stravaSubscriptionService,
            IStravaActivitiesService stravaActivitiesService)
        {
            this.logger = logger;
            this.stravaSubscriptionService = stravaSubscriptionService;
            this.stravaActivitiesService = stravaActivitiesService;
        }

        [HttpGet()]
        [Produces("application/json")]
        public IActionResult ValidateSubscription(
            [FromQuery(Name = "hub.verify_token")]string verifyToken,
            [FromQuery(Name = "hub.challenge")]string challenge,
            [FromQuery(Name = "hub.mode")]string mode)
        {
            if (this.stravaSubscriptionService.IsSubscriptionCallbackValid(verifyToken, mode))
            {
                this.logger.LogInformation($"Validation subscription succesfully. Challenge: {challenge}");

                return this.Ok(new ValidationSubscriptionModel() { Challenge = challenge });
            }

            this.logger.LogError($"Cannot validate subscription. Challenge: {challenge}");

            return this.BadRequest();
        }

        [HttpPost()]
        public async Task<IActionResult> EventReceived([FromBody] StravaWebhookModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                this.logger.LogError("Webhook event received invalid.");
                return this.BadRequest();
            }

            this.HttpContext.Items.Add("subscriptionId", model.SubscriptionId);

            if (model.EventType == "create")
            {
                if (model.ObjectType == "activity")
                {
                    await this.stravaActivitiesService.CreateActivity(model.ObjectId, model.SubscriptionId).ConfigureAwait(false);
                }
                else if (model.ObjectType == "athlete")
                {
                    this.logger.LogWarning($"Webhook received event not implemented. ObjectType: {model.ObjectType}");
                }
                else
                {
                    this.logger.LogWarning($"Webhook event received unknown. ObjectType: {model.ObjectType}");
                }
            }
            else if (model.EventType == "delete")
            {
                if (model.ObjectType == "activity")
                {
                    await this.stravaActivitiesService.DeleteActivity(model.ObjectId, model.SubscriptionId).ConfigureAwait(false);
                }
                else if (model.ObjectType == "athlete")
                {
                    this.logger.LogWarning($"Webhook received event not implemented. ObjectType: {model.ObjectType}");
                }
                else
                {
                    this.logger.LogWarning($"Webhook event received unknown. ObjectType: {model.ObjectType}");
                }
            }
            else if (model.EventType == "update")
            {
                this.logger.LogWarning($"Webhook received event not implemented. EventType: {model.EventType}");
            }
            else
            {
                this.logger.LogWarning($"Webhook event received unknown. EventType: {model.EventType}");
            }

            return this.Ok();
        }
    }
}
