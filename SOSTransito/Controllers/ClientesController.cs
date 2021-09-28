using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOSTransito.Data;
using SOSTransito.Models;

namespace SOSTransito.Controllers
{
    [Authorize(Roles = "ADM, SCT")]
    public class ClientesController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public ClientesController(Context context)
        {
            _context = context;
        }

        // GET: Clientes
        public IActionResult Index()
        {
            var userId = User.Identity.Name;
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

            return View(Clientes.ToList());
        }

        public IActionResult IndexAniversariantes()
        {
            //usuario logado...
            var user = User.Identity.Name;
            var usuario = _context.Usuario.Where(x => x.Nome == user).FirstOrDefault();
            //datas atuais...
            var AnoAtual = System.DateTime.Now.Year;
            var MesAtual = System.DateTime.Now.Month;
            var DiaAtual = System.DateTime.Now.Day;
            //alertas de aniversário...
            var userId = User.Identity.Name;
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

            List<Cliente> Aniversariantes = new List<Cliente>();
            foreach (var objCli in Clientes)
            {
                var UserMes = objCli.DataNascimento.Month;
                var UserDia = objCli.DataNascimento.Day;

                if (UserMes == MesAtual && UserDia == DiaAtual)
                {
                    Aniversariantes.Add(objCli);
                }
            }

            return View(Aniversariantes);
        }

        public IActionResult WppTexto(string id)
        {
            var cliente = _context.Cliente.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.ClienteNome = cliente.Nome;
            ViewBag.ClienteId = cliente.LocalizadorHash;
            return PartialView();
        }

        [HttpPost]
        public IActionResult EnviarWhatsapp(string id, string texto)
        {
            var cliente = _context.Cliente.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            var result = Repositories.whatsapp.sendWpp(cliente.Telefone, texto);

            if (result == false)
            {
                TempData["error"] = result;
                TempData["msg"] = "Erro ao realizar o envio de mensagem pelo whatsapp.";
                return RedirectToAction("IndexAniversariantes");
            }

            TempData["msg"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("IndexAniversariantes");
        }

        public IActionResult MailTexto(string id)
        {
            var cliente = _context.Cliente.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.ClienteNome = cliente.Nome;
            ViewBag.ClienteId = cliente.LocalizadorHash;
            return PartialView();
        }

        [HttpPost]
        public IActionResult EnviarEmail(string id, string titulo, string texto, bool result)
        {
            var cliente = _context.Cliente.Where(x => x.LocalizadorHash == id).FirstOrDefault();

            try
            {
                cliente.NotificationYear = Convert.ToString(System.DateTime.Now.Year);
                _context.Update(cliente);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            finally
            {
                result = Repositories.MailService.sendMail(cliente.email, titulo, texto);
            }

            if (result == false)
            {
                TempData["error"] = result;
                TempData["msg"] = "Erro ao realizar o envio de mensagem pelo whatsapp.";
                return RedirectToAction("IndexAniversariantes");
            }

            TempData["msg"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("IndexAniversariantes");
        }


        // GET: Clientes/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _context.Cliente
                .Include(c => c.Localidades).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao");
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,Nome,CPF,RG,DataNascimento,Endereco,Telefone,email,StatusSistema,LocalizadorHash,NotificationYear,whatsapp,LocalidadeId")] Cliente cliente)
        {
            var ClienteUser = _context.Cliente.Any(x => x.CPF == cliente.CPF);
            if (ClienteUser == false)
            {
                cliente.StatusSistema = "Ativo";
                cliente.LocalizadorHash = Repositories.Md5Hash.CalculaHash(Convert.ToString(randNum.Next()) + System.DateTime.Now);
                if (ModelState.IsValid)
                {
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Muito bem! Cadastro do cliente " + cliente.Nome + " realizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["message"] = "Atenção! Esse CPF já encontra-se registrado no sistema.";
                ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao", cliente.LocalidadeId);
                return View(cliente);
            }
            ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao", cliente.LocalidadeId);
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _context.Cliente.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (cliente == null)
            {
                return NotFound();
            }
            ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao", cliente.LocalidadeId);
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ClienteId,Nome,CPF,RG,DataNascimento,Endereco,Telefone,email,StatusSistema,LocalizadorHash,NotificationYear,whatsapp,LocalidadeId")] Cliente cliente)
        {
            if (id != cliente.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cliente);
                    TempData["message"] = "Muito bem! Edição do cliente " + cliente.Nome + " realizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClienteExists(cliente.ClienteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao", cliente.LocalidadeId);
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = _context.Cliente
                .Include(c => c.Localidades).Where(x => x.LocalizadorHash == id)
                .FirstOrDefault();
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var cliente = _context.Cliente.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            _context.Cliente.Remove(cliente);
            _context.SaveChanges();
            TempData["message"] = "Muito bem! Exclusão do cliente " + cliente.Nome + " realizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Cliente.Any(e => e.ClienteId == id);
        }
    }
}
