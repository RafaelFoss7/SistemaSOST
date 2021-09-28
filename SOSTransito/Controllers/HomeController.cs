using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
            if (email != null || senha != null)
            {
                var usuario = _context.Usuario.Where(x => x.Email == email).FirstOrDefault();
                var password = Md5Hash.CalculaHash(Convert.ToString(senha));

                if (usuario != null)
                {

                    if (email == usuario.Email && password == usuario.Senha && usuario.StatusSistema == "Ativo")
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
                }
                else
                {
                    TempData["Error"] = "Registro do usuário não encontrado no sistema.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Error"] = "Digite o e-mail e a senha antes de confirmar!";
                return RedirectToAction("Index");
            }
            TempData["Error"] = "E-mail ou senha invalidos, por favor, tente novamente!";
            return RedirectToAction("Index");
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
            var listLocalidades = _context.Atribuicao_Localidade.Where(X => X.Usuario.Nome == userId && X.StatusSistema == "Ativo").ToList();
            var listClientes = _context.Cliente.Include(x => x.Localidades).Where(x => x.StatusSistema == "Ativo").ToList();
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
            var listCNH = _context.CNH.Include(x => x.Clientes).Include(y => y.Clientes.Localidades).Where(x => x.StatusSistema == "Ativo").ToList();
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
                var UserMes = objCNH.ValidadeCNH.Month - 1;
                var UserDia = objCNH.ValidadeCNH.Day;

                if (UserAno == AnoAtual && UserMes == MesAtual)
                {
                    renovacoes++;
                }
            }

            //alertas de licenciamentos...
            var listVeiculos = _context.Veiculo.Include(x => x.Clientes).Include(y => y.Clientes.Localidades).Where(x => x.StatusSistema == "Ativo").ToList();
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

                if (objVeic.Tipo == "Automóvel")
                {
                    if (Convert.ToInt32(placa) == 1)
                    {
                        if (MesAtual == 4)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 2)
                    {
                        if (MesAtual == 5)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 3)
                    {
                        if (MesAtual == 6)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 4)
                    {
                        if (MesAtual == 7)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 5 || Convert.ToInt32(placa) == 6)
                    {
                        if (MesAtual == 8)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 7)
                    {
                        if (MesAtual == 9)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 8)
                    {
                        if (MesAtual == 10)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 9)
                    {
                        if (MesAtual == 11)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 0)
                    {
                        if (MesAtual == 12)
                        {
                            licenciamentos++;
                        }
                    }
                }
                else
                {
                    if (Convert.ToInt32(placa) == 1 || Convert.ToInt32(placa) == 2)
                    {
                        if (MesAtual == 9)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 3 || Convert.ToInt32(placa) == 4 || Convert.ToInt32(placa) == 5)
                    {
                        if (MesAtual == 10)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 6 || Convert.ToInt32(placa) == 7 || Convert.ToInt32(placa) == 8)
                    {
                        if (MesAtual == 11)
                        {
                            licenciamentos++;
                        }
                    }
                    if (Convert.ToInt32(placa) == 9 || Convert.ToInt32(placa) == 0)
                    {
                        if (MesAtual == 12)
                        {
                            licenciamentos++;
                        }
                    }
                }
            }

            //Alertas prazo CNH...
            var listProcessoCNH = _context.ProcessoCNH.Include(x => x.CNH).Include(y => y.CNH.Clientes).Include(c => c.CNH.Clientes.Localidades).Where(x => x.StatusSistema == "Ativo").ToList();
            List<ProcessoCNH> processoCNH = new List<ProcessoCNH>();

            foreach (var objLoc in listLocalidades)
            {
                foreach (var objPCNH in listProcessoCNH)
                {
                    if (objLoc.LocalidadeId == objPCNH.CNH.Clientes.LocalidadeId)
                    {
                        processoCNH.Add(_context.ProcessoCNH.Find(objPCNH.ProcessoCNHId));
                    }
                }
            }

            int PrazoCNH = 0;
            foreach (var objPCNH in processoCNH)
            {
                DateTime prazo = objPCNH.Prazo.AddDays(-7);
                var dataAtual = DateTime.Now.Date;
                if (prazo <= dataAtual)
                {
                    PrazoCNH++;
                }
            }

            //Alertas prazo de multas...
            var listMultas = _context.Multa.Include(x => x.CNH).Include(y => y.CNH.Clientes).Include(c => c.CNH.Clientes.Localidades).Where(x => x.StatusSistema == "Ativo").ToList();
            List<Multa> multas = new List<Multa>();

            foreach (var objLoc in listLocalidades)
            {
                foreach (var objMulta in listMultas)
                {
                    if (objLoc.LocalidadeId == objMulta.CNH.Clientes.LocalidadeId)
                    {
                        multas.Add(_context.Multa.Find(objMulta.MultaId));
                    }
                }
            }

            int PrazoMulta = 0;
            foreach (var objMulta in multas)
            {
                DateTime prazo = objMulta.Prazo.AddDays(-7);
                var dataAtual = DateTime.Now.Date;
                if (prazo <= dataAtual)
                {
                    PrazoMulta++;
                }
            }


            ViewBag.ANIVERSARIANTES = aniversariantes;
            ViewBag.LICENCIAMENTOS = licenciamentos;
            ViewBag.RENOVACOES = renovacoes;
            ViewBag.PRAZOMULTA = PrazoMulta;
            ViewBag.PRAZOCNH = PrazoCNH;

            return View();
        }

        public IActionResult IndexRecuperarSenha()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IndexRecuperarSenha(string email)
        {
            if (email == null)
            {
                TempData["error"] = "true";
                TempData["message"] = "Digite o e-mail antes de confirmar.";
                return View();
            }

            try
            {
                if (_context.Usuario.Any(x => x.Email == email) == true)
                {
                    //Recuperando o cadastro do usuário pelo e-mail...
                    var usuario = _context.Usuario.Where(x => x.Email == email).FirstOrDefault();

                    //Definindo recursos para criptografar...
                    string source = email + DateTime.Now.ToShortTimeString() + usuario.UsuarioID + DateTime.Now.ToLocalTime();

                    //criptografar...
                    using (MD5 md5Hash = MD5.Create())
                    {
                        string hash = GetMd5Hash(md5Hash, source);

                        //salvar a criptografia no lugar da senha do usuario...
                        usuario.Senha = hash;
                        _context.Entry(usuario).State = EntityState.Modified;
                        _context.SaveChanges();

                        //gera uma url criptografada de acesso para enviar ao usuário...
                        var uri = new Uri("https://localhost:44382/Home/NovaSenha/?test=" + usuario.Senha);
                        //var authority = uri.GetLeftPart(UriPartial.Authority);

                        //definir informações do e-mail...
                        var titulo = "Definição de Nova Senha - " + usuario.Nome;
                        var texto = "Olá " + usuario.Nome + "!<br /><a href='" + uri + "'/>" + " Clique aqui para redefinir sua senha.";

                        //enviar e-mail...
                        Repositories.MailService.sendMail(email, titulo, texto);
                    }
                }
                else
                {
                    TempData["error"] = "true";
                    TempData["message"] = "O e-mail informado não foi encontrado, por favor tente novamente!";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["message"] = ex.Message;
                return View();
            }

            TempData["message"] = "E-mail enviado com sucesso!";
            return View();
        }

        public IActionResult NovaSenha()
        {
            string hash = Request.Query["test"];
            var usuario = _context.Usuario.SingleOrDefault(x => x.Senha == hash);
            ViewBag.Nome = usuario.Nome;
            ViewBag.UsuarioId = usuario.LocalizadorHash;

            return View(usuario);
        }

        [HttpPost]
        public IActionResult EfetuarNovaSenha(string ConfirmSenha, Usuario usuario)
        {
            if (usuario.Senha != null && ConfirmSenha != null)
            {
                if (usuario.Senha == ConfirmSenha)
                {
                    try
                    {
                        usuario.Senha = Repositories.Md5Hash.CalculaHash(usuario.Senha);
                        _context.Update(usuario);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        TempData["message"] = ex.Message;
                        return View(usuario);
                    }
                }
                else
                {
                    TempData["message"] = "Senha e confirmação de senha incorretas.";
                    return View(usuario);
                }
            }

            TempData["message"] = "Muito bem! Sua senha foi alterada com sucesso!";
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
