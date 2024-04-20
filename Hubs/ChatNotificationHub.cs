using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class ChatNotificationHub: Hub
	{
		public static List<string> chatMessageList = new List<string>();
		public static int NotificationCounter;

		public async Task recordChatEntry(string message)
		{
			chatMessageList.Add(message);
			NotificationCounter++;
			//returning all messages, however you can return the latst as well
			
			await GetAllMessages();
		}

		public async Task GetAllMessages()
		{
			Clients.All.SendAsync("populateChat", chatMessageList.ToArray(), NotificationCounter);
		}

		public override Task OnConnectedAsync()
		{
			GetAllMessages().GetAwaiter().GetResult();
			return base.OnConnectedAsync();
		}
	}
}
