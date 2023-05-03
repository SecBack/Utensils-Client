using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Dto.Models;
using Utensils_Client.Shared.Services;
using Utensils_Client.Views.Dialogs;

namespace Utensils_Client.Pages
{

    public class UserPageLogic : ComponentBase
    {
        [Inject] protected DialogService DialogService { get; set; }
        [Inject] private GroupService GroupService { get; set; }

        protected IEnumerable<GroupDto> Groups { get; set; }
        protected GroupDto? SelectedGroup { get; set; } = null;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            Groups = await GroupService.GetGroups();
        }

        protected async Task OnCreateGroup()
        {
            await DialogService.OpenAsync<CreateGroupDialog>(
                "Create a new Group",
                new Dictionary<string, object>() { }
            );

            Groups = await GroupService.GetGroups();
        }

        protected async Task OnGroupSelected(DataGridCellMouseEventArgs<GroupDto> args)
        {
            SelectedGroup = await GroupService.GetGroupMembers(args.Data.Id);
        }
    }
}
