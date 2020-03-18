using Microsoft.AspNetCore.Components;

namespace WebApp.Services
{
    public class UiMessaginReadyComponent : ComponentBase
    {
        [Inject]
        protected UiMessagingPipeline UiMessagingPipeline { get; set; }
    }
}