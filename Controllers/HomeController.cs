using Microsoft.AspNetCore.Mvc;
using SignalRSample.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using SignalRSample.Data;
using SignalRSample.Hubs;

namespace SignalRSample.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IHubContext<RaceHub> _hub;
		private readonly ApplicationDbContext _context;
		private readonly IHubContext<OrderHub> _orderHub;

		public HomeController(ILogger<HomeController> logger, IHubContext<RaceHub> hub, ApplicationDbContext context, IHubContext<OrderHub> orderHub)
		{
			_logger = logger;
			_hub = hub;
			_context = context;
			_orderHub = orderHub;
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

		//order controller
		[ActionName("Order")]
		public async Task<IActionResult> Order()
		{
			string[] name = { "Bhrugen", "Ben", "Jess", "Laura", "Ron" };
			string[] itemName = { "Food1", "Food2", "Food3", "Food4", "Food5" };

			var rand = new Random();
			// Generate a random index less than the size of the array.  
			var index = rand.Next(name.Length);

			var order = new Order
			{
				Name = name[index],
				ItemName = itemName[index],
				Count = index
			};

			return View(order);
		}

		[ActionName("Order")]
		[HttpPost]
		public async Task<IActionResult> OrderPost(Order order)
		{

			_context.Orders.Add(order);
			_context.SaveChanges();
			await _orderHub.Clients.All.SendAsync("NewOrderPlaced");
			return RedirectToAction(nameof(Order));
		}
		[ActionName("OrderList")]
		public async Task<IActionResult> OrderList()
		{
			return View();
		}
		[HttpGet]
		public IActionResult GetAllOrder()
		{
			var productList = _context.Orders.ToList();
			return Json(new { data = productList });
		}
	}
}
