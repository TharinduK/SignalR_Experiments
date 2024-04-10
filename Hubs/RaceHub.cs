using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class RaceHub : Hub
	{
		public static int CloakCount { get; set; } = 0;
		public static int StoneCount { get; set; } = 0;
		public static int WandCount { get; set; } = 0;

		public Dictionary<string, int> GetRaceStatus()
		{
			var status = new Dictionary<string, int>();
			status.Add("cloak", CloakCount);
			status.Add("stone", StoneCount);
			status.Add("wand", WandCount);

			return status;
		}

		public override Task OnConnectedAsync()
		{
			Clients.All.SendAsync("updateRaceVote", CloakCount, StoneCount, WandCount);
			return base.OnConnectedAsync();
		}
	}
}
