using Microsoft.AspNetCore.SignalR;

namespace CareerGuidancePlatform.Hubs
{
    public class ChatHub : Hub
    {
        // Users and Mentors will join a specific group (e.g. "Chat_{MentorId}_{UserId}")
        public async Task JoinChat(string chatGroupId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatGroupId);
        }

        public async Task LeaveChat(string chatGroupId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatGroupId);
        }
    }
}
