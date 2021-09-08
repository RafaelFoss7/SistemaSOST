using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SOSTransito.Data;
using SOSTransito.Models;
using SOSTransito.Repositories;

namespace SOSTransito.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Context _context;

        public HomeController(ILogger<HomeController> logger, Context context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string email, string senha, string returnUrl, string manterLogado)
        {
            var usuario = _context.Usuario.Where(x => x.Email == email).FirstOrDefault();
            var password = Md5Hash.CalculaHash(Convert.ToString(senha));

            if (email == usuario.Email && password == usuario.Senha)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("E-mail", email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, usuario.Nome));
                claims.Add(new Claim(ClaimTypes.Name, usuario.Nome));
                claims.Add(new Claim(ClaimTypes.Role, usuario.Tipo));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);

                bool LogVerify = false;
                if (manterLogado == "on")
                    LogVerify = true;

                await HttpContext.SignInAsync(claimsPrincipal,
                    new AuthenticationProperties
                    {
                        IsPersistent = LogVerify,
                        ExpiresUtc = DateTime.Now.AddHours(1)
                    });

                return Redirect(returnUrl);
            }
            TempData["Error"] = "E-mail ou senha invalidos, por favor, tente novamente!";
            return View("login");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [HttpGet("denied")]
        public IActionResult Denied()
        {
            return View();
        }

        [Authorize(Roles = "ADM")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "ADM")]
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
