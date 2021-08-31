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
    public class LocalidadesController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public LocalidadesController(Context context)
        {
            _context = context;
        }

        // GET: Localidades
        public async Task<IActionResult> Index()
        {
            return View(await _context.Localidade.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Localidades/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LocalidadeId,Regiao,StatusSistema,LocalizadorHash")] Localidade localidade)
        {
            var LocalidadeUser = _context.Localidade.Any(x => x.Regiao == localidade.Regiao);
            if (LocalidadeUser == false)
            {
                localidade.StatusSistema = "Ativo";
                localidade.LocalizadorHash = Repositories.Md5Hash.CalculaHash(Convert.ToString(randNum.Next()) + System.DateTime.Now);
                if (ModelState.IsValid)
                {
                    _context.Add(localidade);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Muito bem! Cadastro da localidade " + localidade.Regiao + " realizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["message"] = "Atenção! Localidade já cadastrada no sistema.";
                return View(localidade);
            }
            return View(localidade);
        }

        // GET: Localidades/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localidade = _context.Localidade.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (localidade == null)
            {
                return NotFound();
            }
            return View(localidade);
        }

        // POST: Localidades/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("LocalidadeId,Regiao,StatusSistema,LocalizadorHash")] Localidade localidade)
        {
            if (id != localidade.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(localidade);
                    TempData["message"] = "Muito bem! Alteração das informações da localidade " + localidade.Regiao + " realizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocalidadeExists(localidade.LocalidadeId))
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
            return View(localidade);
        }

        // GET: Localidades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var localidade = await _context.Localidade
                .FirstOrDefaultAsync(m => m.LocalidadeId == id);
            if (localidade == null)
            {
                return NotFound();
            }

            return View(localidade);
        }

        // POST: Localidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var localidade = await _context.Localidade.FindAsync(id);
            _context.Localidade.Remove(localidade);
            TempData["message"] = "Muito bem! Exclusão da localidade " + localidade.Regiao + " realizado com sucesso!";
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocalidadeExists(int id)
        {
            return _context.Localidade.Any(e => e.LocalidadeId == id);
        }
    }
}
