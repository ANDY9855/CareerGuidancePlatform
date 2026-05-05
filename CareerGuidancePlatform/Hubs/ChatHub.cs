using Microsoft.AspNetCore.SignalR;

namespace CareerGuidancePlatform.Hubs
{
    public class ChatHub : Hub
    {
        // ─── Chat Groups ───────────────────────────────────────────────────
        public async Task JoinChat(string chatGroupId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, chatGroupId);

        public async Task LeaveChat(string chatGroupId)
            => await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatGroupId);

        // ─── Mentor/User Presence ──────────────────────────────────────────
        // Called on page load to receive incoming call alerts
        public async Task RegisterMentor(string mentorId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, $"Mentor_{mentorId}");
            
        public async Task RegisterUser(string userId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, $"User_{userId}");

        // ─── Video Call Signaling ──────────────────────────────────────────
        // User initiates a call to mentor
        public async Task CallMentor(string mentorId, string callerName, string callRoomId)
        {
            await Clients.Group($"Mentor_{mentorId}").SendAsync("IncomingCall", callerName, callRoomId, Context.ConnectionId);
        }

        // Mentor initiates a call to user
        public async Task CallUser(string userId, string callerName, string callRoomId)
        {
            await Clients.Group($"User_{userId}").SendAsync("IncomingCall", callerName, callRoomId, Context.ConnectionId);
        }

        // Mentor accepts and joins the video room
        public async Task AcceptCall(string callerConnectionId, string callRoomId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, callRoomId);
            await Clients.Client(callerConnectionId).SendAsync("CallAccepted", Context.ConnectionId);
        }

        // Either side declines/ends
        public async Task DeclineCall(string callerConnectionId)
            => await Clients.Client(callerConnectionId).SendAsync("CallDeclined");

        public async Task EndCallInRoom(string callRoomId)
            => await Clients.Group(callRoomId).SendAsync("CallEnded");

        // Join video room (after accept)
        public async Task JoinVideoRoom(string callRoomId)
            => await Groups.AddToGroupAsync(Context.ConnectionId, callRoomId);

        // ─── WebRTC Signaling (room-based) ────────────────────────────────
        public async Task SendOffer(string callRoomId, string sdpOffer)
            => await Clients.OthersInGroup(callRoomId).SendAsync("ReceiveOffer", Context.ConnectionId, sdpOffer);

        public async Task SendAnswer(string callerConnectionId, string sdpAnswer)
            => await Clients.Client(callerConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, sdpAnswer);

        public async Task SendIceCandidate(string callRoomId, string candidate)
            => await Clients.OthersInGroup(callRoomId).SendAsync("ReceiveIceCandidate", candidate);

        public string GetConnectionId() => Context.ConnectionId;
    }
}
