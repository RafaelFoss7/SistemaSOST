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
    public class CNHController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public CNHController(Context context)
        {
            _context = context;
        }

        // GET: CNH
        public IActionResult Index()
        {
            var userId = User.Identity.Name;
            var listLocalidades = _context.Atribuicao_Localidade.Where(X => X.Usuario.Nome == userId).ToList();
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

            return View(cnhs.ToList());
        }

        public IActionResult IndexRenovacao()
        {
            //informações do usuário logado...
            var userId = User.Identity.Name;
            var usuario = _context.Usuario.Where(x => x.Nome == userId).FirstOrDefault();

            //datas atuais...
            var AnoAtual = System.DateTime.Now.Year;
            var MesAtual = System.DateTime.Now.Month;
            var DiaAtual = System.DateTime.Now.Day;

            //alertas de CNH...
            var listLocalidades = _context.Atribuicao_Localidade.Where(X => X.Usuario.Nome == userId).ToList();
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

            List<CNH> renovacoes = new List<CNH>();
            foreach (var objCNH in cnhs)
            {
                var UserAno = objCNH.ValidadeCNH.Year;
                var UserMes = objCNH.ValidadeCNH.Month - 1;
                var UserDia = objCNH.ValidadeCNH.Day;

                if (UserAno == AnoAtual && UserMes == MesAtual)
                {
                    renovacoes.Add(objCNH);
                }
            }

            return View(renovacoes.ToList());
        }

        public IActionResult WppTexto(string id)
        {
            var cnh = _context.CNH.Include(x => x.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.ClienteNome = cnh.Clientes.Nome;
            ViewBag.CNHId = cnh.LocalizadorHash;
            return PartialView();
        }

        [HttpPost]
        public IActionResult EnviarWhatsapp(string id, string texto)
        {
            var cnh = _context.CNH.Include(x => x.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            var result = Repositories.whatsapp.sendWpp(cnh.Clientes.Telefone, texto);

            if (result == false)
            {
                TempData["error"] = result;
                TempData["msg"] = "Erro ao realizar o envio de mensagem pelo whatsapp.";
                return RedirectToAction("IndexRenovacao");
            }

            TempData["msg"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("IndexRenovacao");
        }

        public IActionResult MailTexto(string id)
        {
            var cnh = _context.CNH.Include(x => x.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.ClienteNome = cnh.Clientes.Nome;
            ViewBag.CNHId = cnh.LocalizadorHash;
            return PartialView();
        }

        [HttpPost]
        public IActionResult EnviarEmail(string id, string titulo, string texto, bool result)
        {
            var cnh = _context.CNH.Include(x => x.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            try
            {
                cnh.NotificationYear = Convert.ToString(System.DateTime.Now.Year);
                _context.Update(cnh);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
                return RedirectToAction("IndexRenovacao");
            }
            finally
            {
                result = Repositories.MailService.sendMail(cnh.Clientes.email, titulo, texto);
            }

            if (result == false)
            {
                TempData["error"] = result;
                TempData["msg"] = "Erro ao realizar o envio de mensagem pelo whatsapp.";
                return RedirectToAction("IndexRenovacao");
            }

            TempData["msg"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("IndexRenovacao");
        }

        public IActionResult NextProcess(string id)
        {
            var cnh = _context.CNH.Include(x => x.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.CNH = cnh.RegistroCNH;
            ViewBag.CNHId = cnh.LocalizadorHash;
            return PartialView(cnh);
        }

        [HttpPost]
        public IActionResult ChangeProcess(string id, CNH cnh)
        {
            if (id != cnh.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Muito bem! Alteração do processo da CNH " + cnh.RegistroCNH + " realizado com sucesso!";
                    _context.Update(cnh);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CNHExists(cnh.CNHId))
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
            ViewBag.CNHId = cnh.LocalizadorHash;
            return PartialView(cnh);
        }

        // GET: CNH/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cnh = _context.CNH.Include(c => c.Clientes).Include(l => l.Clientes.Localidades).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (cnh == null)
            {
                return NotFound();
            }

            return View(cnh);
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
        public async Task<IActionResult> Create([Bind("CNHId,RegistroCNH,Categoria,ValidadeCNH,StatusCNH,Processo,StatusSistema,LocalizadorHash,NotificationYear,ClienteId")] CNH cnh)
        {
            var cliente = _context.Cliente.Find(cnh.ClienteId);
            var CNHUser = _context.CNH.Any(x => x.ClienteId == cnh.ClienteId);
            if (CNHUser == false)
            {
                cnh.StatusSistema = "Ativo";
                cnh.LocalizadorHash = Repositories.Md5Hash.CalculaHash(Convert.ToString(randNum.Next()) + System.DateTime.Now);
                if (cnh.Processo == null)
                    cnh.Processo = "Em Andamento...";
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
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cnh = _context.CNH.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (cnh == null)
            {
                return NotFound();
            }
            var cliente = _context.Cliente.Find(cnh.ClienteId);
            ViewBag.Cliente = cliente.Nome;
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "Nome", cnh.ClienteId);
            return View(cnh);
        }

        // POST: CNH/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CNHId,RegistroCNH,Categoria,ValidadeCNH,StatusCNH,Processo,StatusSistema,LocalizadorHash,NotificationYear,ClienteId")] CNH cnh)
        {
            var cliente = _context.Cliente.Find(cnh.ClienteId);
            if (id != cnh.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    TempData["message"] = "Muito bem! Edição da CNH " + cnh.RegistroCNH + " do cliente " + cliente.Nome + " realizado com sucesso!";
                    _context.Update(cnh);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CNHExists(cnh.CNHId))
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
            ViewData["ClienteId"] = new SelectList(_context.Cliente, "ClienteId", "CPF", cnh.ClienteId);
            return View(cnh);
        }

        // GET: CNH/Delete/5
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cnh = _context.CNH.Include(c => c.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (cnh == null)
            {
                return NotFound();
            }

            return View(cnh);
        }

        // POST: CNH/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var cnh = _context.CNH.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            _context.CNH.Remove(cnh);
            _context.SaveChanges();
            TempData["message"] = "Muito bem! Exclusão da CNH " + cnh.RegistroCNH + " realizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool CNHExists(int id)
        {
            return _context.CNH.Any(e => e.CNHId == id);
        }
    }
}
