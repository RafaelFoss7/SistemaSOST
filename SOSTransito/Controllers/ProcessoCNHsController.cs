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
    public class ProcessoCNHsController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public ProcessoCNHsController(Context context)
        {
            _context = context;
        }

        // GET: ProcessoCNHs
        public async Task<IActionResult> Index()
        {
            var context = _context.ProcessoCNH.Include(p => p.CNH);
            return View(await context.ToListAsync());
        }

        // GET: ProcessoCNHs/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var processoCNH = _context.ProcessoCNH.Include(z => z.CNH).Include(c => c.CNH.Clientes).Include(l => l.CNH.Clientes.Localidades).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (processoCNH == null)
            {
                return NotFound();
            }

            return View(processoCNH);
        }

        public IActionResult NextProcess(string id)
        {
            var processoCNH = _context.ProcessoCNH.Include(x => x.CNH).Include(y => y.CNH.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.CNH = processoCNH.CNH.RegistroCNH;
            ViewBag.ProcessoCNHId = processoCNH.LocalizadorHash;
            return PartialView(processoCNH);
        }

        [HttpPost]
        public IActionResult ChangeProcess(string id, ProcessoCNH processoCNH)
        {
            var cnh = _context.CNH.Find(processoCNH.CNHId);
            if (id != processoCNH.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Muito bem! Alteração do processo da CNH " + cnh.RegistroCNH + " realizado com sucesso!";
                    _context.Update(processoCNH);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoCNHExists(processoCNH.CNHId))
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
            ViewBag.CNH = processoCNH.CNH.RegistroCNH;
            ViewBag.CNHId = processoCNH.LocalizadorHash;
            return PartialView(processoCNH);
        }

        public IActionResult FinalProcess(string id)
        {
            var processoCNH = _context.ProcessoCNH.Include(x => x.CNH).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.CNH = processoCNH.CNH.RegistroCNH;
            ViewBag.ProcessoCNHId = processoCNH.LocalizadorHash;
            return PartialView(processoCNH);
        }

        [HttpPost]
        public IActionResult FinalizarProcesso(string id, ProcessoCNH processoCNH)
        {
            var cnh = _context.CNH.Find(processoCNH.CNHId);
            if (id != processoCNH.LocalizadorHash)
            {
                return NotFound();
            }
            if (processoCNH.Processo == "Deferido")
            {
                processoCNH.StatusCNH = "Regularizado";
            }
            if (processoCNH.Processo == "Indeferido")
            {
                processoCNH.StatusCNH = "Irregular";
            }
            processoCNH.StatusSistema = "Finalizado";
            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Muito bem! Finalização do processo da CNH " + cnh.RegistroCNH + " realizado com sucesso!";
                    _context.Update(processoCNH);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoCNHExists(processoCNH.ProcessoCNHId))
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

            ViewBag.CNH = processoCNH.CNH.RegistroCNH;
            ViewBag.ProcessoCNHId = processoCNH.LocalizadorHash;
            return PartialView(processoCNH);
        }

        public IActionResult Periodo(string id)
        {
            var processoCNH = _context.ProcessoCNH.Include(h => h.CNH).Include(c => c.CNH.Clientes).Include(l => l.CNH.Clientes.Localidades).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.CNH = processoCNH.CNH.RegistroCNH;
            ViewBag.ProcessoCNHId = processoCNH.LocalizadorHash;
            return PartialView(processoCNH);
        }

        [HttpPost]
        public IActionResult DefinirPeriodo(string id, ProcessoCNH processoCNH)
        {
            var cnh = _context.CNH.Find(processoCNH.CNHId);
            if (id != processoCNH.LocalizadorHash)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Muito bem! Definição de periodo de suspensão da CNH " + cnh.RegistroCNH + " realizado com sucesso!";
                    _context.Update(processoCNH);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoCNHExists(processoCNH.ProcessoCNHId))
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

            ViewBag.CNH = cnh.RegistroCNH;
            ViewBag.ProcessoCNHId = processoCNH.LocalizadorHash;
            return PartialView(processoCNH);
        }


        // GET: ProcessoCNHs/Create
        public IActionResult Create(string id)
        {
            var cnh = _context.CNH.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewData["CNHId"] = new SelectList(_context.CNH.Where(x => x.LocalizadorHash == id), "CNHId", "RegistroCNH");
            ViewBag.CNH = cnh.RegistroCNH;
            return View();
        }

        // POST: ProcessoCNHs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProcessoCNHId,StatusCNH,Processo,Prazo,De,Ate,StatusSistema,LocalizadorHash,CNHId")] ProcessoCNH processoCNH)
        {
            var cnh = _context.CNH.Find(processoCNH.CNHId);
            processoCNH.StatusSistema = "Ativo";
            processoCNH.LocalizadorHash = Repositories.Md5Hash.CalculaHash(Convert.ToString(randNum.Next()) + System.DateTime.Now);

            if (ModelState.IsValid)
            {
                _context.Add(processoCNH);
                await _context.SaveChangesAsync();
                TempData["message"] = "Muito bem! Cadastro do processo da CNH " + cnh.RegistroCNH + " realizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            ViewData["CNHId"] = new SelectList(_context.CNH.Where(x => x.LocalizadorHash == processoCNH.CNH.LocalizadorHash), "CNHId", "RegistroCNH");
            ViewBag.CNH = processoCNH.CNH.RegistroCNH;
            return View(processoCNH);
        }

        // GET: ProcessoCNHs/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processoCNH = _context.ProcessoCNH.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (processoCNH == null)
            {
                return NotFound();
            }
            ViewData["CNHId"] = new SelectList(_context.CNH, "CNHId", "Categoria", processoCNH.CNHId);
            return View(processoCNH);
        }

        // POST: ProcessoCNHs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProcessoCNHId,StatusCNH,Processo,Prazo,De,Ate,StatusSistema,LocalizadorHash,CNHId")] ProcessoCNH processoCNH)
        {
            var cnh = _context.CNH.Find(processoCNH.CNHId);
            if (id != processoCNH.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(processoCNH);
                    TempData["message"] = "Muito bem! Edição do processo da CNH " + cnh.RegistroCNH + " realizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProcessoCNHExists(processoCNH.ProcessoCNHId))
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
            ViewData["CNHId"] = new SelectList(_context.CNH, "CNHId", "Categoria", processoCNH.CNHId);
            return View(processoCNH);
        }

        // GET: ProcessoCNHs/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var processoCNH = _context.ProcessoCNH.Include(h => h.CNH).Include(c => c.CNH.Clientes).Include(l => l.CNH.Clientes.Localidades).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (processoCNH == null)
            {
                return NotFound();
            }

            return View(processoCNH);
        }

        // POST: ProcessoCNHs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var processoCNH = _context.ProcessoCNH.Include(c => c.CNH).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            _context.ProcessoCNH.Remove(processoCNH);
            _context.SaveChanges();
            TempData["message"] = "Muito bem! Exclusão da CNH " + processoCNH.CNH.RegistroCNH + " realizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool ProcessoCNHExists(int id)
        {
            return _context.ProcessoCNH.Any(e => e.ProcessoCNHId == id);
        }
    }
}
