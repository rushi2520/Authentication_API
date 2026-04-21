using ConsumingJWT_WebApi_Authentication_.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Newtonsoft.Json;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace ConsumingJWT_WebApi_Authentication_.Controllers
{
    public class AccountController : Controller
    {
        private readonly HttpClient client;

        // ✅ Use IHttpClientFactory (Best Practice)
        public AccountController(IHttpClientFactory factory)
        {
            client = factory.CreateClient();
        }

        // 🔹 GET Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 🔹 POST Register
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModels model)
        {
            if (model == null)
            {
                ViewBag.Message = "Model is NULL ❌";
                return View();
            }

            if (ModelState.IsValid)
            {
                // 🔥 Convert model to JSON
                var json = JsonConvert.SerializeObject(model);

                // 🔥 Send JSON to API
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:44302/api/Auth/register", data);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Message"] = "Registration Successful...";
                    return RedirectToAction("Login");
                }
            }

            ViewBag.Message = "Unable To Register User.";
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModels log)
        {
            if (ModelState.IsValid)
            {
                // 🔹 Convert the LoginViewModels object into JSON string
                // Example:
                // { "userName": "admin", "password": "1234" }
                var json = JsonConvert.SerializeObject(log);

                // 🔹 Create HTTP content with JSON data
                // Encoding.UTF8 → ensures proper character encoding
                // "application/json" → tells API that we are sending JSON
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                // 🔹 Send POST request to API Login endpoint
                // API URL must match exactly with your backend route
                var response = await client.PostAsync("https://localhost:44302/api/Auth/Login", data);

                //This code reads the JWT token from API response, converts it into an object, stores it in session, and redirects the user.
                if (response.IsSuccessStatusCode)
                {
                    // 🔹 Read the response body returned by API as a string
                    // Example API response:
                    // { "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." }
                    // it return token
                    var result = await response.Content.ReadAsStringAsync();

                    // 🔹 Convert (deserialize) JSON string into a dynamic object
                    // Now we can access properties like tokenObj.token
                    dynamic tokenObj = JsonConvert.DeserializeObject(result);

                    // 🔹 Extract the token from the object and store it in Session
                    // "JWtoken" is the key used to store the value
                    // Later we can retrieve it using: HttpContext.Session.GetString("JWtoken")
                    HttpContext.Session.SetString("JWtoken", (string)tokenObj.token);
                    return RedirectToAction("Index","Home");
                }
            }
            ViewBag.Message = "Unable to Login...";
            return View(log);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}