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
    public class VeiculosController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public VeiculosController(Context context)
        {
            _context = context;
        }

        // GET: Veiculos
        public IActionResult Index()
        {
            var userId = User.Identity.Name;
            var listLocalidades = _context.Atribuicao_Localidade.Where(X => X.Usuario.Nome == userId).ToList();
            var listVeiculos = _context.Veiculo.Include(x => x.Clientes).Include(y => y.Clientes.Localidades).ToList();
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

            return View(Veiculos.ToList());
        }

        public IActionResult IndexLicenciamentos()
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
            var listVeiculos = _context.Veiculo.Include(x => x.Clientes).Include(y => y.Clientes.Localidades).ToList();
            List<Veiculo> veiculos = new List<Veiculo>();

            List<Veiculo> licenciamentos = new List<Veiculo>();
            foreach (var objVeic in listVeiculos)
            {
                var placa = objVeic.Placa.Substring(objVeic.Placa.Length - 1, 1);

                if (Convert.ToInt32(placa) == 1 || Convert.ToInt32(placa) == 2 || Convert.ToInt32(placa) == 3)
                {
                    if (MesAtual == 3)
                    {
                        licenciamentos.Add(objVeic);
                    }
                }
                if (Convert.ToInt32(placa) == 4)
                {
                    if (MesAtual == 4)
                    {
                        licenciamentos.Add(objVeic);
                    }
                }
                if (Convert.ToInt32(placa) == 5)
                {
                    if (MesAtual == 5)
                    {
                        licenciamentos.Add(objVeic);
                    }
                }
                if (Convert.ToInt32(placa) == 6)
                {
                    if (MesAtual == 6)
                    {
                        licenciamentos.Add(objVeic);
                    }
                }
                if (Convert.ToInt32(placa) == 7)
                {
                    if (MesAtual == 7)
                    {
                        licenciamentos.Add(objVeic);
                    }
                }
                if (Convert.ToInt32(placa) == 8)
                {
                    if (MesAtual == 8)
                    {
                        licenciamentos.Add(objVeic);
                    }
                }
                if (Convert.ToInt32(placa) == 9)
                {
                    if (MesAtual == 9)
                    {
                        licenciamentos.Add(objVeic);
                    }
                }
                if (Convert.ToInt32(placa) == 0)
                {
                    if (MesAtual == 10)
                    {
                        licenciamentos.Add(objVeic);
                    }
                }
            }

            return View(licenciamentos.ToList());
        }

        public IActionResult WppTexto(string id)
        {
            var veic = _context.Veiculo.Include(x => x.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.ClienteNome = veic.Clientes.Nome;
            ViewBag.VeiculoId = veic.LocalizadorHash;
            return PartialView();
        }

        [HttpPost]
        public IActionResult EnviarWhatsapp(string id, string texto)
        {
            var veic = _context.Veiculo.Include(x => x.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            var result = Repositories.whatsapp.sendWpp(veic.Clientes.Telefone, texto);

            if (result == false)
            {
                TempData["error"] = result;
                TempData["msg"] = "Erro ao realizar o envio de mensagem pelo whatsapp.";
                return RedirectToAction("IndexLicenciamentos");
            }

            TempData["msg"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("IndexLicenciamentos");
        }

        public IActionResult MailTexto(string id)
        {
            var veic = _context.Veiculo.Include(x => x.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();
            ViewBag.ClienteNome = veic.Clientes.Nome;
            ViewBag.VeiculoId = veic.LocalizadorHash;
            return PartialView();
        }

        [HttpPost]
        public IActionResult EnviarEmail(string id, string titulo, string texto, bool result)
        {
            var veic = _context.Veiculo.Include(x => x.Clientes).Where(x => x.LocalizadorHash == id).FirstOrDefault();

            try
            {
                veic.NotificationYear = Convert.ToString(System.DateTime.Now.Year);
                _context.Update(veic);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            finally
            {
                result = Repositories.MailService.sendMail(veic.Clientes.email, titulo, texto);
            }
         
            if (result == false)
            {
                TempData["error"] = result;
                TempData["msg"] = "Erro ao realizar o envio de mensagem pelo whatsapp.";
                return RedirectToAction("IndexLicenciamentos");
            }

            TempData["msg"] = "Mensagem enviada com sucesso!";
            return RedirectToAction("IndexLicenciamentos");
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
        public async Task<IActionResult> Create([Bind("VeiculoId,Placa,RENAVAN,StatusSistema,LocalizadorHash,NotificationYear,ClienteId")] Veiculo veiculo)
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
        public async Task<IActionResult> Edit(string id, [Bind("VeiculoId,Placa,RENAVAN,StatusSistema,LocalizadorHash,NotificationYear,ClienteId")] Veiculo veiculo)
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
