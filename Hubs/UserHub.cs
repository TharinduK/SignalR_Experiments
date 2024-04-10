using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class UserHub : Hub
	{
		public static int TotalViews { get; set; }
		public static int TotalUsers { get; set; }

		public async Task NewWindowsLoaded()
		{
			TotalViews++;
			//inform client method
			await Clients.All.SendAsync("updateTotalViews", TotalViews);
		}

		public async Task<string> GetTotalViewsOnNewWindowLoad()
		{
			TotalViews++;
			//inform client method
			await Clients.All.SendAsync("updateTotalViews", TotalViews);
			return $"total views - {TotalViews}";
		}

		public override Task OnConnectedAsync()
		{
			TotalUsers++;
			Clients.All.SendAsync("updateTotalSession", TotalUsers).GetAwaiter().GetResult();
			return base.OnConnectedAsync();
		}

		public override Task OnDisconnectedAsync(Exception? exception)
		{
			TotalUsers--;
			Clients.All.SendAsync("updateTotalSession", TotalUsers).GetAwaiter().GetResult();
			return base.OnDisconnectedAsync(exception);
		}
	}
}
