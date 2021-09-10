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
        public async Task<IActionResult> Create([Bind("ClienteId,Nome,CPF,DataNascimento,Endereco,Telefone,email,StatusSistema,LocalizadorHash,LocalidadeId")] Cliente cliente)
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
        public async Task<IActionResult> Edit(string id, [Bind("ClienteId,Nome,CPF,DataNascimento,Endereco,Telefone,email,StatusSistema,LocalizadorHash,LocalidadeId")] Cliente cliente)
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
