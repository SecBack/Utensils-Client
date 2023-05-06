using Microsoft.AspNetCore.Components;
using Radzen.Blazor.Rendering;
using Radzen.Blazor;
using Shared.Dto.Models;

namespace Utensils_Client.Pages
{
    public class CalenderPageLogic : ComponentBase
    {
        protected RadzenScheduler<EventDto> Scheduler;
        protected IList<EventDto> Events = new List<EventDto>();

        protected override async Task OnInitializedAsync()
        {
            
        }

        protected void OnSlotRender()
        {

        }

        protected void OnSlotSelect()
        {

        }

        protected void OnEventSelect()
        {

        }

        protected void OnEventRender()
        {

        }
    }
}
