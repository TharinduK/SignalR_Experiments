using Microsoft.AspNetCore.SignalR;

namespace SignalRSample.Hubs
{
	public class HarryHousesHub:Hub
	{
		public static List<string> JoinedHousList { get; set; } = new List<string>();

		public async Task JoinHouse(string houseName)
		{
			if (!JoinedHousList.Contains(GroupConnectionName(houseName)))
			{
				JoinedHousList.Add(GroupConnectionName(houseName));
				await Groups.AddToGroupAsync(Context.ConnectionId, houseName);

				await Clients.Caller.SendAsync("subscriptionStatus", string.Join(" ", GetHouseListForConnection()), houseName, true);
				await Clients.Others.SendAsync("otherSubscriptionNotification", houseName, true);
				
			}
		}

		private string GetHouseListForConnection()
		{
			var houseList = "";
			foreach (var idAndHouseName in JoinedHousList)
			{
				if (idAndHouseName.Contains(Context.ConnectionId))
				{
					houseList = string.Concat(houseList, " ", GetHouseNameFromConnection(idAndHouseName));
				}
			}

			return houseList;
		}

		public async Task LeaveHouse(string houseName)
		{
			if (JoinedHousList.Contains(GroupConnectionName(houseName)))
			{
				JoinedHousList.Remove(GroupConnectionName(houseName));

				await Groups.RemoveFromGroupAsync(Context.ConnectionId, houseName);
				//notify
				await Clients.Caller.SendAsync("subscriptionStatus", GetHouseListForConnection(), houseName, false);
				await Clients.Others.SendAsync("otherSubscriptionNotification", houseName, false);
			}
		}

		public async Task TriggerNotification(string houseName)
		{
			await Clients.Group(houseName).SendAsync("triggerNotification", houseName);
		}

		private string GroupConnectionName(string houseName)
		{
			return $"{Context.ConnectionId}:{houseName}";
		}
		private string GetHouseNameFromConnection(string connection)
		{
			if (string.IsNullOrEmpty(connection) || !connection.Contains(":")) return "";
			return connection.Split(":")[1];
			
		}
	}
}
