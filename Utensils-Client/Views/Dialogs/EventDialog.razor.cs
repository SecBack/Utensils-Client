using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Dto.Models;
using Shared.Requests;
using System.Runtime.CompilerServices;
using Utensils_Client.Shared.Services;

namespace Utensils_Client.Views.Dialogs
{
    public class EventDialogLogic : ComponentBase
    {
        [Inject] private DialogService _dialogService { get; set; }
        [Inject ] private EventService _eventService { get; set; }

        [Parameter] public EventDto InputEvent { get; set; }
        protected EventDto Event { get; set; } = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Event = InputEvent;
        }

        protected async Task OnSubmit()
        {
            // convert the EventDto to a CreateEventRequest
            CreateEventRequest request = new()
            {
                Title = Event.Title,
                Description = Event.Description,
                StartDate = Event.StartDate,
                EndDate = Event.EndDate,
                Group = Event.GroupId
            };

            //TODO: When a user creates an event, they should be able to choose which group it belongs to

            EventDto createdEvent = await _eventService.CreateEvent(Event);

            _dialogService.Close();
        }
    }
}
