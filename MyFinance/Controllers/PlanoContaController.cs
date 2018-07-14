using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Models;

namespace MyFinance.Controllers
{
    public class PlanoContaController : Controller
    {


        IHttpContextAccessor HttpContextAccessor;


        //Recebe o contexto p/ variaveis de Sessao
        public PlanoContaController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        //Metodo Listar Plano de Contas
        public IActionResult Index()
        {
            //CRIA UM OBJETO CONTA
            PlanoContasModel objPlanoContas = new PlanoContasModel(HttpContextAccessor);
            // LISTA O OBJETO CONTA E PASSA PAA VIEW - ATRAVEZ DO (ViewBag)
            ViewBag.ListaPlanoContas = objPlanoContas.ListaPlanoContas();
            return View();
        }





        // VALIDA CAMPOS VAZIOS DO FORMULARIO 
        [HttpGet]
        public IActionResult CriarPlanoConta(int? id)
        {

            if (id != null)
            {
                //CRIA UM OBJETO CONTA
                PlanoContasModel objPlanoContas = new PlanoContasModel(HttpContextAccessor);
                // LISTA O OBJETO CONTA E PASSA PAA VIEW - ATRAVEZ DO (ViewBag)
                ViewBag.Registro = objPlanoContas.CarregarRegistro(id);
            }
            return View();
        }


        // Insert Conta
        [HttpPost]
        public IActionResult CriarPlanoConta(PlanoContasModel planoContasModel)
        {

            if (ModelState.IsValid)
            {
                //PEGA USUARIO DA SESSÃO
                planoContasModel.HttpContextAccessor = HttpContextAccessor;

                planoContasModel.Insert();
                return RedirectToAction("Index");

            }
            return View();
        }







        [HttpGet]
        public IActionResult ExcluirPlanoConta(int id)
        {

            PlanoContasModel objPlanoContas = new PlanoContasModel(HttpContextAccessor);
            objPlanoContas.Excluir(id);
            return RedirectToAction("Index");
        }




    }

}
