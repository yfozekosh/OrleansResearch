using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using PoC.Blog.GrainContracts;

namespace PoC.Blog.WebClient.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IClusterClient _client;

        public UserController(IClusterClient client)
        {
            _client = client;
        }

        public string Hello()
        {
            return "hello world";
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegistrationModel model)
        {
            IUserRegistry userRegistry = _client.GetGrain<IUserRegistry>((long) UserRegistryId.ActiveUsers);

            bool isRegistered = await userRegistry.RegisterUser(model.Email,model.Password);

            if (!isRegistered)
            {
                return BadRequest("Cannot register user. Maybe this email is already taken.");
            }

            return Ok("registered");
        }
    }
}
