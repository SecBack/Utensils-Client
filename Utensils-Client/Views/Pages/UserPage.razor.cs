using Microsoft.AspNetCore.Components;
using Radzen;
using Shared.Dto.Models;
using Utensils_Client.Shared.Services;
using Utensils_Client.Views.Dialogs;

namespace Utensils_Client.Pages
{

    public class UserPageLogic : ComponentBase
    {
        [Inject] private DialogService DialogService { get; set; }
        [Inject] private GroupService GroupService { get; set; }
        
        protected List<GroupDto> Groups { get; set; } = new List<GroupDto>();

        protected override async Task OnInitializedAsync()
        {
            Groups = await GroupService.GetGroups();
        }

        protected async Task OnCreateGroup()
        {
            await DialogService.OpenAsync<CreateGroupDialog>("Create Group", new Dictionary<string, object>() { { "Groups", Groups } });
        }
    }
}
