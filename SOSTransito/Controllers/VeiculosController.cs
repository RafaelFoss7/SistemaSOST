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
    public class VeiculosController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public VeiculosController(Context context)
        {
            _context = context;
        }

        // GET: Veiculos
        public async Task<IActionResult> Index()
        {
            var context = _context.Veiculo.Include(v => v.Clientes);
            return View(await context.ToListAsync());
        }

        // GET: Veiculos/Create
        public IActionResult Create(string id)
        {
            ViewBag.Cliente = _context.Cliente.Where(x => x.LocalizadorHash == id).FirstOrDefault().Nome;
            ViewData["ClienteId"] = new SelectList(_context.Cliente.Where(x => x.LocalizadorHash == id), "ClienteId", "CPF");
            return View();
        }

        // POST: Veiculos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("VeiculoId,Placa,RENAVAN,StatusSistema,LocalizadorHash,ClienteId")] Veiculo veiculo)
        {
            var cliente = _context.Cliente.Find(veiculo.ClienteId);
            var VeiculoUser = _context.Veiculo.Any(x => x.ClienteId == veiculo.ClienteId && x.Placa == veiculo.Placa);
            if (VeiculoUser == false)
            {
                veiculo.StatusSistema = "Ativo";
                veiculo.LocalizadorHash = Repositories.Md5Hash.CalculaHash(Convert.ToString(randNum.Next()) + System.DateTime.Now);
                if (ModelState.IsValid)
                {
                    _context.Add(veiculo);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Muito bem! Cadastro do veículo " + veiculo.Placa + " do cliente " + cliente.Nome + " realizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["message"] = "Atenção! O veículo " + veiculo.Placa + " já possui registro no sistema.";
                ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "CPF", veiculo.ClienteId);
                return View(veiculo);
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "CPF", veiculo.ClienteId);
            return View(veiculo);
        }

        // GET: Veiculos/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var veiculo = _context.Veiculo.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (veiculo == null)
            {
                return NotFound();
            }
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "CPF", veiculo.ClienteId);
            return View(veiculo);
        }

        // POST: Veiculos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("VeiculoId,Placa,RENAVAN,StatusSistema,LocalizadorHash,ClienteId")] Veiculo veiculo)
        {
            var cliente = _context.Cliente.Find(veiculo.ClienteId);
            if (id != veiculo.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(veiculo);
                    TempData["message"] = "Muito bem! Edição do veículo " + veiculo.Placa + " do cliente " + cliente.Nome + " realizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VeiculoExists(veiculo.VeiculoId))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "CPF", veiculo.ClienteId);
            return View(veiculo);
        }

        // GET: Veiculos/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var veiculo = _context.Veiculo.Include(v => v.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (veiculo == null)
            {
                return NotFound();
            }

            return View(veiculo);
        }

        // POST: Veiculos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var veiculo = _context.Veiculo.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            _context.Veiculo.Remove(veiculo);
            _context.SaveChanges();
            TempData["message"] = "Muito bem! Exclusão do veículo " + veiculo.Placa + " realizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool VeiculoExists(int id)
        {
            return _context.Veiculo.Any(e => e.VeiculoId == id);
        }
    }
}
