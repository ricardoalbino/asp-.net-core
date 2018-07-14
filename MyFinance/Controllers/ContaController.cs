using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Models;

using Rotativa.AspNetCore;

using PagedList;


namespace MyFinance.Controllers
{
    public class ContaController : Controller
    {
        IHttpContextAccessor HttpContextAccessor;

        //Recebe o contexto p/ variaveis de Sessao
        public ContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index()
        {
            //CRIA UM OBJETO CONTA
            ContaModel obConta = new ContaModel(HttpContextAccessor);
            // LISTA O OBJETO CONTA E PASSA PAA VIEW - ATRAVEZ DO (ViewBag)
            ViewBag.listaConta = obConta.ListaConta();

            //var listaConta = obConta.ListaConta();


           
                //Definindo a paginação              
            

                return View();

           
        

    }   

        // VALIDA CAMPOS VAZIOS DO FORMULARIO 
        [HttpGet]
        public IActionResult CriarConta()
        {

            return View();
        }


        // Insert Conta
        [HttpPost]
        public IActionResult CriarConta(ContaModel contaModel)
        {

            if (ModelState.IsValid)
            {
                //PEGA USUARIO DA SESSÃO
                contaModel.HttpContextAccessor = HttpContextAccessor;

                contaModel.Insert();
                return RedirectToAction("Index");

            }
            return View();
        }

        [HttpGet]
        public IActionResult ExcluirConta(int id)
        {

            ContaModel obConta = new ContaModel(HttpContextAccessor);
            obConta.Excluir(id);
            return RedirectToAction("Index");
        }




    }
}