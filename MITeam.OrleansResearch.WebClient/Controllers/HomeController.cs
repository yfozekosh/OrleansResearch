using System.Threading.Tasks;
using GrainContracts;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using MITeam.OrleansResearch.WebClient.Model;

namespace MITeam.OrleansResearch.WebClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly IClusterClient _client;

        public HomeController(IClusterClient client)
        {
            this._client = client;
        }

        public IActionResult Index()
        {
            var model = new CountModel()
            {
                GrainId = 0,
                Count = 0
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CallGrain(string submitButton, CountModel model)
        {
            switch (submitButton)
            {
                case "Refresh":
                    model.Count = await this._client.GetGrain<ICounterGrain>(model.GrainId).GetCount(); 
                    break;
                case "Increment":
                    await this._client.GetGrain<ICounterGrain>(model.GrainId).Add();
                    model.Count = await this._client.GetGrain<ICounterGrain>(model.GrainId).GetCount();
                    break;
            }
            
            return this.View("Index", model);
        }
    }
}