using Microsoft.AspNetCore.Mvc;
using SignalRSample.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Hubs;

namespace SignalRSample.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IHubContext<RaceHub> _hub;

		public HomeController(ILogger<HomeController> logger, IHubContext<RaceHub> hub)
		{
			_logger = logger;
			_hub = hub;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult GroupNotification()
		{
			return View();
		}

		public IActionResult DeathlyHallows(string type)
		{
			switch (type.ToLower())
			{
				case "cloak": RaceHub.CloakCount++;
					break;
				case "stone": RaceHub.StoneCount++;
					break;
				case "wand":
					RaceHub.WandCount++;
					break;
			}
			_hub.Clients.All.SendAsync("updateRaceVote", RaceHub.CloakCount, RaceHub.StoneCount, RaceHub.WandCount);
			return Accepted();
		}

		public IActionResult Notification()
		{
			return View();
		}

		public IActionResult BasicChat()
		{
			return View();
		}
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
