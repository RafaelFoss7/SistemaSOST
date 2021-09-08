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
    public class UsuariosController : Controller
    {
        private readonly Context _context;
        Random randNum = new Random();

        public UsuariosController(Context context)
        {
            _context = context;
        }

        // GET: Usuarios
        [Authorize(Roles = "ADM")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.ToListAsync());
        }


        // GET: Usuarios/Create
        [Authorize(Roles = "ADM")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "ADM")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UsuarioID,Nome,Tipo,Email,Senha,StatusSistema,LocalizadorHash")] Usuario usuario, string ConfirmSenha)
        {
            var NomeUser = _context.Usuario.Any(x => x.Nome == usuario.Nome);
            var EmailUser = _context.Usuario.Any(x => x.Email == usuario.Email);
            if (NomeUser == false && EmailUser == false)
            {
                if (ConfirmSenha != null)
                {
                    if (usuario.Senha == ConfirmSenha)
                    {
                        usuario.Senha = Repositories.Md5Hash.CalculaHash(usuario.Senha);
                        usuario.StatusSistema = "Ativo";
                        usuario.LocalizadorHash = Repositories.Md5Hash.CalculaHash(Convert.ToString(randNum.Next()) + System.DateTime.Now);
                        if (ModelState.IsValid)
                        {
                            _context.Add(usuario);
                            await _context.SaveChangesAsync();
                            TempData["message"] = "Muito bem! Cadastro do usuário " + usuario.Nome + " realizado com sucesso!";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        ViewBag.ConfirmSenha = "Confirmação de senha incorreta.";
                        return View(usuario);
                    }
                }
            }
            else
            {
                TempData["message"] = "Atenção! Nome ou e-mail de usuário já cadastrados no sistema.";
                return View(usuario);
            }
            return View(usuario);
        }

        // Change Password Usuários...
        [Authorize(Roles = "ADM")]
        public IActionResult ChangePassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuario.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }
            usuario.Senha = "";
            return View(usuario);
        }

        [HttpPost]
        [Authorize(Roles = "ADM")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, [Bind("UsuarioID,Nome,Tipo,Email,Senha,StatusSistema,LocalizadorHash")] Usuario usuario, string ConfirmSenha)
        {
            if (id != usuario.LocalizadorHash)
            {
                return NotFound();
            }

            if (usuario.Senha == ConfirmSenha)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        usuario.Senha = Repositories.Md5Hash.CalculaHash(usuario.Senha);
                        _context.Update(usuario);
                        TempData["message"] = "Muito bem! Alteração de senha do usuário " + usuario.Nome + " realizado com sucesso!";
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UsuarioExists(usuario.UsuarioID))
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
            }
            else
            {
                ViewBag.ConfirmSenha = "Senha de confirmação incorreta.";
                return View(usuario);
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        [Authorize(Roles = "ADM")]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = _context.Usuario.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "ADM")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UsuarioID,Nome,Tipo,Email,Senha,StatusSistema,LocalizadorHash")] Usuario usuario)
        {
            if (id != usuario.LocalizadorHash)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usuario);
                    TempData["message"] = "Muito bem! Alteração das informações do usuário " + usuario.Nome + " realizado com sucesso!";
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioID))
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        [Authorize(Roles = "ADM")]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var usuario = _context.Usuario.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "ADM")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var usuario = _context.Usuario.Where(x => x.LocalizadorHash == id).FirstOrDefault();
            _context.Usuario.Remove(usuario);
            _context.SaveChanges();
            TempData["message"] = "Muito bem! Exclusão do usuário " + usuario.Nome + " realizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.UsuarioID == id);
        }
    }
}
