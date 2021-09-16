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
using Microsoft.EntityFrameworkCore;
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

        [Authorize(Roles = "ADM, SCT")]
        public IActionResult Index()
        {
            //informações do usuário logado...
            var userId = User.Identity.Name;
            var usuario = _context.Usuario.Where(x => x.Nome == userId).FirstOrDefault();

            ViewBag.NOME = usuario.Nome;
            ViewBag.EMAIL = usuario.Email;
            ViewBag.STATUS = usuario.StatusSistema;
            ViewBag.TIPO = usuario.Tipo;
            ViewBag.HASH = usuario.LocalizadorHash;

            //datas atuais...
            var AnoAtual = System.DateTime.Now.Year;
            var MesAtual = System.DateTime.Now.Month;
            var DiaAtual = System.DateTime.Now.Day;

            //alertas de aniversário...
            var listLocalidades = _context.Atribuicao_Localidade.Where(X => X.Usuario.Nome == userId).ToList();
            var listClientes = _context.Cliente.Include(x => x.Localidades).ToList();
            List<Cliente> Clientes = new List<Cliente>();

            foreach (var objLoc in listLocalidades)
            {
                foreach (var objCli in listClientes)
                {
                    if (objLoc.LocalidadeId == objCli.LocalidadeId)
                    {
                        Clientes.Add(_context.Cliente.Find(objCli.ClienteId));
                    }
                }
            }

            int aniversariantes = 0;
            foreach (var objCli in Clientes)
            {
                var UserMes = objCli.DataNascimento.Month;
                var UserDia = objCli.DataNascimento.Day;

                if (UserMes == MesAtual && UserDia == DiaAtual)
                {
                    aniversariantes++;
                }
            }

            //alertas de CNH...
            var listCNH = _context.CNH.Include(x => x.Clientes).Include(y => y.Clientes.Localidades).ToList();
            List<CNH> cnhs = new List<CNH>();

            foreach (var objLoc in listLocalidades)
            {
                foreach (var objCNH in listCNH)
                {
                    if (objLoc.LocalidadeId == objCNH.Clientes.LocalidadeId)
                    {
                        cnhs.Add(_context.CNH.Find(objCNH.CNHId));
                    }
                }
            }
            int renovacoes = 0;
            foreach (var objCNH in cnhs)
            {
                var UserAno = objCNH.ValidadeCNH.Year;
                var UserMes = objCNH.ValidadeCNH.Month-1;
                var UserDia = objCNH.ValidadeCNH.Day;

                if (UserAno == AnoAtual && UserMes == MesAtual)
                {
                    renovacoes++;
                }
            }

            //alertas de licenciamentos...
            var listVeiculos = _context.Veiculo.Include(x => x.Clientes).Include(y => y.Clientes.Localidades).ToList();
            List<Veiculo> Veiculos = new List<Veiculo>();

            foreach (var objLoc in listLocalidades)
            {
                foreach (var objVeic in listVeiculos)
                {
                    if (objLoc.LocalidadeId == objVeic.Clientes.LocalidadeId)
                    {
                        Veiculos.Add(_context.Veiculo.Find(objVeic.VeiculoId));
                    }
                }
            }
            int licenciamentos = 0;
            foreach (var objVeic in Veiculos)
            {
                var placa = objVeic.Placa.Substring(objVeic.Placa.Length - 1, 1);

                if (Convert.ToInt32(placa) == 1 || Convert.ToInt32(placa) == 2 || Convert.ToInt32(placa) == 3)
                {
                    if (MesAtual == 3)
                    {
                        licenciamentos++;
                    }
                }
                if (Convert.ToInt32(placa) == 4)
                {
                    if (MesAtual == 4)
                    {
                        licenciamentos++;
                    }
                }
                if (Convert.ToInt32(placa) == 5)
                {
                    if (MesAtual == 5)
                    {
                        licenciamentos++;
                    }
                }
                if (Convert.ToInt32(placa) == 6)
                {
                    if (MesAtual == 6)
                    {
                        licenciamentos++;
                    }
                }
                if (Convert.ToInt32(placa) == 7)
                {
                    if (MesAtual == 7)
                    {
                        licenciamentos++;
                    }
                }
                if (Convert.ToInt32(placa) == 8)
                {
                    if (MesAtual == 8)
                    {
                        licenciamentos++;
                    }
                }
                if (Convert.ToInt32(placa) == 9)
                {
                    if (MesAtual == 9)
                    {
                        licenciamentos++;
                    }
                }
                if (Convert.ToInt32(placa) == 0)
                {
                    if (MesAtual == 10)
                    {
                        licenciamentos++;
                    }
                }
            }

            ViewBag.ANIVERSARIANTES = aniversariantes;
            ViewBag.LICENCIAMENTOS = licenciamentos;
            ViewBag.RENOVACOES = renovacoes;

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
