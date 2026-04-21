using ConsumingJWT_WebApi_Authentication_.Models;
using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsumingJWT_WebApi_Authentication_.Controllers
{
    public class HomeController : Controller
    {
        private readonly HttpClient client = new HttpClient();
        public IActionResult Index()
        {
            return View();
        }


        //This method takes JWT token from session, sends it in request header, calls a protected API, and shows the returned user data.

        public async Task<IActionResult> GetUserData()
        {
            // 🔹 Get JWT token stored earlier during login from Session
            // Example value: "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
            var token = HttpContext.Session.GetString("JWtoken");

            // 🔹 Attach token to HTTP request header
            // "Bearer <token>" is the standard format for JWT authentication
            // This tells the API: "This request is from an authenticated user"
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer",token);

            // 🔹 Call protected API endpoint
            // This API requires a valid JWT token to allow access
            var response = await client.GetAsync("https://localhost:44302/api/Secure/user");
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                TempData["message"] = result;
            }
            return View();
        }

        public IActionResult Privacy()
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
