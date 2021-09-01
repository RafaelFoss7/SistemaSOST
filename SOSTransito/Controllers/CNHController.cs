using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SOSTransito.Data;
using SOSTransito.Models;

namespace SOSTransito.Controllers
{
    public class CNHController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public CNHController(Context context)
        {
            _context = context;
        }

        // GET: CNH
        public async Task<IActionResult> Index()
        {
            var context = _context.CNH.Include(c => c.Clientes);
            return View(await context.ToListAsync());
        }

        // GET: CNH/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cNH = await _context.CNH
                .Include(c => c.Clientes)
                .FirstOrDefaultAsync(m => m.CNHId == id);
            if (cNH == null)
            {
                return NotFound();
            }

            return View(cNH);
        }

        // GET: CNH/Create
        public IActionResult Create(string id)
        {
            ViewData["ClienteId"] = new SelectList(_context.Cliente.Where(x => x.LocalizadorHash == id), "ClienteId", "Nome");
            ViewBag.Cliente = _context.Cliente.Where(x => x.LocalizadorHash == id).FirstOrDefault().Nome;
            return View();
        }

        // POST: CNH/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CNHId,RegistroCNH,Categoria,ValidadeCNH,StatusCNH,Processo,StatusSistema,LocalizadorHash,ClienteId")] CNH cnh)
        {
            var cliente = _context.Cliente.Find(cnh.ClienteId);
            var CNHUser = _context.CNH.Any(x => x.ClienteId == cnh.ClienteId);
            if (CNHUser == false)
            {
                cnh.StatusSistema = "Ativo";
                cnh.LocalizadorHash = Repositories.Md5Hash.CalculaHash(Convert.ToString(randNum.Next()) + System.DateTime.Now);
                cnh.Processo = "Aguardando Processo";
                if (ModelState.IsValid)
                {
                    _context.Add(cnh);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Muito bem! Cadastro da CNH " + cnh.RegistroCNH + " do cliente " + cliente.Nome + " realizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["message"] = "Atenção! A CNH " + cnh.RegistroCNH + " já possui registro no sistema.";
                ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "CPF", cnh.ClienteId);
                return View(cnh);
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "CPF", cnh.ClienteId);
            return View(cnh);
        }

        // GET: CNH/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cNH = await _context.CNH.FindAsync(id);
            if (cNH == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "CPF", cNH.ClienteId);
            return View(cNH);
        }

        // POST: CNH/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CNHId,RegistroCNH,Categoria,ValidadeCNH,StatusCNH,Processo,StatusSistema,LocalizadorHash,ClienteId")] CNH cNH)
        {
            if (id != cNH.CNHId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cNH);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CNHExists(cNH.CNHId))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "CPF", cNH.ClienteId);
            return View(cNH);
        }

        // GET: CNH/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cNH = await _context.CNH
                .Include(c => c.Clientes)
                .FirstOrDefaultAsync(m => m.CNHId == id);
            if (cNH == null)
            {
                return NotFound();
            }

            return View(cNH);
        }

        // POST: CNH/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cNH = await _context.CNH.FindAsync(id);
            _context.CNH.Remove(cNH);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CNHExists(int id)
        {
            return _context.CNH.Any(e => e.CNHId == id);
        }
    }
}
