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
        [Parameter] public UserDto UserDetails { get; set; }
 
        protected EventDto Event { get; set; } = new();
        protected string GroupNameSearch;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Event = InputEvent;
        }

        protected async Task OnSubmit()
        {
            // find the group that the user selected
            GroupDto selectedGroup = UserDetails.Groups.FirstOrDefault(g => g.Name == GroupNameSearch);

            // convert the EventDto to a CreateEventRequest
            CreateEventRequest request = new()
            {
                Title = Event.Title,
                Description = Event.Description,
                StartDate = Event.StartDate,
                EndDate = Event.EndDate,
                EventType = Event.EventType,
                Group = selectedGroup,
                UserId = UserDetails.Id
            };

            EventDto createdEvent = await _eventService.CreateEvent(request);

            _dialogService.Close();
        }
    }
}
