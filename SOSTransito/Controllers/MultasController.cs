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
    public class MultasController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public MultasController(Context context)
        {
            _context = context;
        }

        // GET: Multas
        public async Task<IActionResult> Index()
        {
            var context = _context.Multa.Include(m => m.CNH).Include(c => c.CNH.Clientes);
            return View(await context.ToListAsync());
        }

        public IActionResult NextProcess(string id)
        {
            var multas = _context.Multa.Include(x => x.CNH).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.Multa = multas.NAIT;
            ViewBag.MultaId = multas.LocalizadorHash;
            return PartialView(multas);
        }

        [HttpPost]
        public IActionResult ChangeProcess(string id, Multa multa)
        {
            if (id != multa.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Muito bem! Alteração do processo da multa " + multa.NAIT + " realizado com sucesso!";
                    _context.Update(multa);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultaExists(multa.CNHId))
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
            ViewBag.Multa = multa.NAIT;
            ViewBag.MultaId = multa.LocalizadorHash;
            return PartialView(multa);
        }

        // GET: Multas/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var multa = _context.Multa.Include(m => m.CNH).Include(c => c.CNH.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (multa == null)
            {
                return NotFound();
            }

            return View(multa);
        }

        // GET: Multas/Create
        public IActionResult Create(string id)
        {
            ViewData["CNHId"] = new SelectList(_context.CNH.Where(x => x.LocalizadorHash == id), "CNHId", "RegistroCNH");
            ViewData["Veiculo"] = new SelectList(_context.Veiculo, "VeiculoId", "Placa");
            return View();
        }

        // POST: Multas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MultaId,OrgAtuador,NAIT,DataInfracao,Veiculo,Pontuacao,Processo,StatusSistema,LocalizadorHash,Arquivo,CNHId")] Multa multa)
        {
            var MultUser = _context.Multa.Any(x => x.NAIT == multa.NAIT);
            if (MultUser == false)
            {
                multa.StatusSistema = "Ativo";
                multa.LocalizadorHash = Repositories.Md5Hash.CalculaHash(Convert.ToString(randNum.Next()) + System.DateTime.Now);
                if (ModelState.IsValid)
                {
                    _context.Add(multa);
                    await _context.SaveChangesAsync();
                    TempData["message"] = "Muito bem! Cadastro da multa " + multa.NAIT + " realizado com sucesso!";
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                TempData["message"] = "Atenção! Essa multa já encontra-se registrado no sistema.";
                ViewData["CNHId"] = new SelectList(_context.CNH, "CNHId", "RegistroCNH", multa.CNHId);
                ViewData["Veiculo"] = new SelectList(_context.Veiculo, "VeiculoId", "Placa");
                return View(multa);
            }
            ViewData["CNHId"] = new SelectList(_context.CNH, "CNHId", "RegistroCNH", multa.CNHId);
            ViewData["Veiculo"] = new SelectList(_context.Veiculo, "VeiculoId", "Placa");
            return View(multa);
        }

        // GET: Multas/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var multa = _context.Multa.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (multa == null)
            {
                return NotFound();
            }
            ViewData["CNHId"] = new SelectList(_context.CNH, "CNHId", "Categoria", multa.CNHId);
            ViewData["Veiculo"] = new SelectList(_context.Veiculo, "VeiculoId", "Placa");
            return View(multa);
        }

        // POST: Multas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("MultaId,OrgAtuador,NAIT,DataInfracao,Veiculo,Pontuacao,Processo,StatusSistema,LocalizadorHash,Arquivo,CNHId")] Multa multa)
        {
            if (id != multa.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(multa);
                    TempData["message"] = "Muito bem! Edição da multa " + multa.NAIT + " realizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MultaExists(multa.MultaId))
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
            ViewData["CNHId"] = new SelectList(_context.CNH, "CNHId", "Categoria", multa.CNHId);
            ViewData["Veiculo"] = new SelectList(_context.Veiculo, "VeiculoId", "Placa");
            return View(multa);
        }

        // GET: Multas/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var multa = _context.Multa.Include(m => m.CNH).Include(c => c.CNH.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (multa == null)
            {
                return NotFound();
            }

            return View(multa);
        }

        // POST: Multas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var multa = _context.Multa.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            _context.Multa.Remove(multa);
            _context.SaveChanges();
            TempData["message"] = "Muito bem! Exclusão da multa " + multa.NAIT + " realizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool MultaExists(int id)
        {
            return _context.Multa.Any(e => e.MultaId == id);
        }
    }
}
