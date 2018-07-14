using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyFinance.Models;

namespace MyFinance.Controllers
{
    public class TransacaoController : Controller
    {

        IHttpContextAccessor HttpContextAccessor;

        //Recebe o contexto p/ variaveis de Sessao
        public TransacaoController(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        //Metodo Listar Plano de Contas
        public IActionResult Index()
        {
            //CRIA UM OBJETO CONTA
            TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor);
            // LISTA O OBJETO CONTA E PASSA PAA VIEW - ATRAVEZ DO (ViewBag)
            ViewBag.ListaTransacao = objTransacao.ListaTransacao();
            return View();
        }


        // VALIDA CAMPOS VAZIOS DO FORMULARIO 
        [HttpGet]
        public IActionResult Registrar(int? id)
        {

            if (id != null)
            {
                
                TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor);
                ViewBag.Registro = objTransacao.carregarRegistro(id);

            }
            ViewBag.ListaContas = new ContaModel(HttpContextAccessor).ListaConta();
            ViewBag.ListaPlanoConta = new PlanoContasModel(HttpContextAccessor).ListaPlanoContas();
            return View();
        }



        [HttpPost]
        public IActionResult Registrar(TransacaoModel transacaoModel)
        {
            if (ModelState.IsValid)
            {
                //PEGA USUARIO DA SESSÃO
                transacaoModel.HttpContextAccessor = HttpContextAccessor;

                transacaoModel.Insert();
                return RedirectToAction("Index");

            }
            return View();
        }





        [HttpGet]
        public IActionResult ExcluirTransacao(int id)
        {

            TransacaoModel objPlanoContas = new TransacaoModel(HttpContextAccessor);
            objPlanoContas.Excluir(id);
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult DetalhesTransacao(int id)
        {

           
            TransacaoModel objTransacao = new TransacaoModel(HttpContextAccessor);
            ViewBag.Registro = objTransacao.carregarRegistro(id);
            return View();

        }




        [HttpGet]
        [HttpPost]
        public IActionResult Extrato(TransacaoModel transacaoModel)
        {
            transacaoModel.HttpContextAccessor = HttpContextAccessor;
            ViewBag.ListaTransacao = transacaoModel.ListaTransacao();

            ViewBag.ListaContas = new ContaModel(HttpContextAccessor).ListaConta();

            return View();
        }


        public IActionResult Grafico()
        {

            List<Dashboard> lista = new Dashboard().RetornarDadosGraficoPie();

            string valores = "";
            string labels = "";
            string cores = "";

            var random = new Random();

            for (int i = 0; i < lista.Count; i++)
            {
                valores += lista[i].Total.ToString() + ",";
                labels += "'" + lista[i].PlanoConta.ToString() + "',";
                cores += "'" + String.Format("#{0:x6}", random.Next(0x1000000)) + "',";

            }
            //ViewBag.Cores  = "'#cecece', '#465789', '#fff125', '#784586', '#504030'";

            ViewBag.Cores = cores;
            ViewBag.Labels = labels;
            ViewBag.Valores = valores;
            return View();
        }
    }
}