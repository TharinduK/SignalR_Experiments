using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Data;

namespace SignalRSample.Hubs
{
	public class ChatHub :Hub
	{
		private readonly ApplicationDbContext _dbContext;

		public ChatHub(ApplicationDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task MessageSendToAllUsers(string message, string fromEmail="")
		{
			await Clients.All.SendAsync("messageReceived", fromEmail, message);
		}

		//restrict  sending messages to only authorized  users 
		[Authorize]
		public async Task MessageSendToUser(string message, string fromEmail, string toEmail)
		{
			var user = _dbContext.Users.FirstOrDefault(u => u.UserName.ToLower() == toEmail.ToLower());

			if (user != null && !string.IsNullOrEmpty(user.Id))
			{
				await Clients.User(user.Id).SendAsync("messageReceived", fromEmail, message);
			}
			
		}
	}
}
