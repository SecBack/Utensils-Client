﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;
using Shared.Dto.Models;
using Shared.Requests;
using Utensils_Client.Shared;
using Utensils_Client.Shared.Services;
using Utensils_Client.Views.Dialogs;

namespace Utensils_Client.Pages
{

    public class UserPageLogic : ComponentBase
    {
        [Inject] protected DialogService DialogService { get; set; }
        [Inject] private GroupService GroupService { get; set; }
        [Inject] private AuthService AuthService { get; set; }
        protected IEnumerable<GroupDto> Groups { get; set; }
        protected GroupDto? SelectedGroup { get; set; } = null;
        protected bool IsMemberOfSelectedGroup { get; set; } = false;

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
            // determin if the user is a member of the selected group
            UserDto user = await AuthService.GetCurrentUser();
            IsMemberOfSelectedGroup = SelectedGroup.Users.Any(m => m.Id == user.Id);
        }

        protected async Task OnJoinGroupSwitch(GroupDto selectedGroup)
        {
            UserDto user = await AuthService.GetCurrentUser();
            UpdateGroupRequest request = new()
            {
                UserId = user.Id,
                GroupId = selectedGroup.Id
            };

            SelectedGroup = await GroupService.UpdateGroup(request);
            IsMemberOfSelectedGroup = !IsMemberOfSelectedGroup;
        }
    }
}
