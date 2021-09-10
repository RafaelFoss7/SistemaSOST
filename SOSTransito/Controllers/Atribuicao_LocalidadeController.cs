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
    [Authorize(Roles = "ADM")]
    public class Atribuicao_LocalidadeController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public Atribuicao_LocalidadeController(Context context)
        {
            _context = context;
        }

        // GET: Atribuicao_Localidade
        public async Task<IActionResult> Index()
        {
            var context = _context.Atribuicao_Localidade.Include(a => a.Localidades).Include(a => a.Usuario);
            return View(await context.ToListAsync());
        }

        // GET: Atribuicao_Localidade/Create
        public IActionResult Create()
        {
            ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao");
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioID", "Nome");
            return View();
        }

        // POST: Atribuicao_Localidade/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ATRLOCId,StatusSistema,LocalizadorHash,LocalidadeId,UsuarioId")] Atribuicao_Localidade atribuicao_Localidade)
        {
            var AtrUser = _context.Atribuicao_Localidade.Any(x => x.UsuarioId == atribuicao_Localidade.UsuarioId && x.LocalidadeId == atribuicao_Localidade.LocalidadeId);

            var loc = _context.Localidade.Find(atribuicao_Localidade.LocalidadeId);
            var user = _context.Usuario.Find(atribuicao_Localidade.UsuarioId);

            if (AtrUser == false)
            {
                atribuicao_Localidade.StatusSistema = "Ativo";
                atribuicao_Localidade.LocalizadorHash = Repositories.Md5Hash.CalculaHash(Convert.ToString(randNum.Next()) + System.DateTime.Now);
                if (ModelState.IsValid)
                {
                    _context.Add(atribuicao_Localidade);
                    TempData["message"] = "Muito bem! Cadastro de atribuição da localidade " + loc.Regiao + " para o usuário " + user.Nome + " realizado com sucesso!";
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["message"] = "Atenção! Usuário e localidade já possuem atribuição no sistema.";
                ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao", atribuicao_Localidade.LocalidadeId);
                ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioID", "Nome", atribuicao_Localidade.UsuarioId);
                return View(atribuicao_Localidade);
            }
            ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao", atribuicao_Localidade.LocalidadeId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioID", "Nome", atribuicao_Localidade.UsuarioId);
            return View(atribuicao_Localidade);
        }

        // GET: Atribuicao_Localidade/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atribuicao_Localidade = _context.Atribuicao_Localidade.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (atribuicao_Localidade == null)
            {
                return NotFound();
            }
            ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao", atribuicao_Localidade.LocalidadeId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioID", "Email", atribuicao_Localidade.UsuarioId);
            return View(atribuicao_Localidade);
        }

        // POST: Atribuicao_Localidade/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ATRLOCId,StatusSistema,LocalizadorHash,LocalidadeId,UsuarioId")] Atribuicao_Localidade atribuicao_Localidade)
        {
            if (id != atribuicao_Localidade.LocalizadorHash)
            {
                return NotFound();
            }

            var loc = _context.Localidade.Find(atribuicao_Localidade.LocalidadeId);
            var user = _context.Usuario.Find(atribuicao_Localidade.UsuarioId);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(atribuicao_Localidade);
                    TempData["message"] = "Muito bem! Alteração de atribuição da localidade " + loc.Regiao + " para o usuário " + user.Nome + " realizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Atribuicao_LocalidadeExists(atribuicao_Localidade.ATRLOCId))
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
            ViewData["LocalidadeId"] = new SelectList(_context.Localidade, "LocalidadeId", "Regiao", atribuicao_Localidade.LocalidadeId);
            ViewData["UsuarioId"] = new SelectList(_context.Usuario, "UsuarioID", "Email", atribuicao_Localidade.UsuarioId);
            return View(atribuicao_Localidade);
        }

        // GET: Atribuicao_Localidade/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var atribuicao_Localidade = _context.Atribuicao_Localidade
                .Include(a => a.Localidades)
                .Include(a => a.Usuario).Where(x => x.LocalizadorHash == id)
                .FirstOrDefault();
            if (atribuicao_Localidade == null)
            {
                return NotFound();
            }

            return View(atribuicao_Localidade);
        }

        // POST: Atribuicao_Localidade/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var atribuicao_Localidade = _context.Atribuicao_Localidade.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            var loc = _context.Localidade.Find(atribuicao_Localidade.LocalidadeId);
            var user = _context.Usuario.Find(atribuicao_Localidade.UsuarioId);
            _context.Atribuicao_Localidade.Remove(atribuicao_Localidade);
            _context.SaveChanges();
            TempData["message"] = "Muito bem! Exclusão de atribuição da localidade " + loc.Regiao + " para o usuário " + user.Nome + " realizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool Atribuicao_LocalidadeExists(int id)
        {
            return _context.Atribuicao_Localidade.Any(e => e.ATRLOCId == id);
        }
    }
}
