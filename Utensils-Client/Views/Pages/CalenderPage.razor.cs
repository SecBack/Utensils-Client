using Microsoft.AspNetCore.Components;
using Radzen.Blazor.Rendering;
using Radzen.Blazor;
using Shared.Dto.Models;
using Utensils_Client.Shared.Services;
using Radzen;
using Utensils_Client.Views.Dialogs;

namespace Utensils_Client.Pages
{
    public class CalenderPageLogic : ComponentBase
    {
        [Inject] protected UserService UserService { get; set; }
        [Inject] protected DialogService DialogService { get; set; }

        protected RadzenScheduler<EventDto> Scheduler;
        protected UserDto UserDetails = new UserDto();
        protected EventDto SelectedEvent = new EventDto();

        protected override async Task OnInitializedAsync()
        {
            UserDetails = await UserService.GetUserDetails();
        }

        protected async Task OnSlotSelect(SchedulerSlotSelectEventArgs args)
        {
            // wait for the user to fill in the event details
            await DialogService.OpenAsync<EventDialog>(
                "Add Event",
                new Dictionary<string, object>
                {
                    { "InputEvent", SelectedEvent }
                },
                options: new DialogOptions
                {
                    CloseDialogOnOverlayClick = true,
                }
            );

            // reload the scheduler to show the new event
            await OnInitializedAsync();

            await Scheduler.Reload();
        }

        protected async Task OnEventSelect(SchedulerAppointmentSelectEventArgs<EventDto> args)
        {
            EventDto returnedEvent = await DialogService.OpenAsync<EventDialog>(
                $"Edit {args.Data.Title}",
                new Dictionary<string, object>
                {
                    { "InputEvent", SelectedEvent }
                },
                options: new DialogOptions
                {
                    CloseDialogOnOverlayClick = true,
                }
            );

            await Scheduler.Reload();
        }

        protected void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            // Highlight today in month view
            if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
            {
                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }

            // Highlight working hours (9-18)
            if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour > 8 && args.Start.Hour < 19)
            {
                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }
        }

        protected void OnEventRender(SchedulerAppointmentRenderEventArgs<EventDto> args)
        {
            if (args.Data.Title == "Commen dinner")
            {
                args.Attributes["style"] = "background: red";
            }
        }
    }
}
