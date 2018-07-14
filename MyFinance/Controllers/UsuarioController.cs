using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Models;

namespace MyFinance.Controllers
{
    public class UsuarioController : Controller
    {

       static Int32 sessao = 0; 

        [HttpGet]
        public IActionResult Login(int? usuario)
        {
            usuario = sessao;
            if (usuario != null)
            {
                if (usuario == 0)
                {
                    HttpContext.Session.SetString("nome", string.Empty);
                    HttpContext.Session.SetString("id", string.Empty);
                    HttpContext.Session.SetString("img", string.Empty);
                }
            }
            return View();
   
        }

        public IActionResult ValidarLogin(UsuarioModel usuarioModel)
        {

            bool login = usuarioModel.ValidarLogin();

                // Valida Usuario

            
            if (login || ModelState.IsValid)
            {
                //  pega nome do usuaio  da model e gera uma sessão p/ ele
                sessao = 1;
                HttpContext.Session.SetInt32("status", 1);
                HttpContext.Session.SetString("nome", usuarioModel.Nome);
                HttpContext.Session.SetString("id", usuarioModel.Id.ToString());
                HttpContext.Session.SetString("img", usuarioModel.IMG);

                return RedirectToAction("Dashboard", "Usuario");
            }
            else
            {
                TempData["MensagemLoginInvalido"] = "Dados de Login Invalidos!";
                return RedirectToAction("Login", "Usuario");
            }
               
        }



        public IActionResult DestroySession()
        {
            sessao = 0;
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Usuario");
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Registrar(UsuarioModel usuarioModel)
        {
            if (ModelState.IsValid)
            {
                usuarioModel.RegistrarUsuario();
                TempData["MensagemLoginSusesso"] = "Dados Registrado!";
            }
            else
            {
                return View();
            }

            return RedirectToAction("Registrar", "Usuario");
        }



        [HttpGet]
        public IActionResult Dashboard()
        {

            if(sessao == 1){
                return View();
            }

            return RedirectToAction("Login", "Usuario");
        }
        

    }
}