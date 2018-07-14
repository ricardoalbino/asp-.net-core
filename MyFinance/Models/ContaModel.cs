using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Models
{
    public class ContaModel
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="Informe o Nome da Conta!")]
        public string Nome { get; set; }
        [Required (ErrorMessage ="Informe o Saldo da Conta!")]
        public double Saldo { get; set; }
        public int Usuaio_Id { get; set; }

        // PEGA USUARIO DA SESSAO
        public IHttpContextAccessor HttpContextAccessor { get; set; }


        public ContaModel()
        {

        }

        //Recebe o contexto p/ variaveis de Sessao
        public ContaModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }


        //Read -> Carregar Tabela
        public List<ContaModel> ListaConta()
        {
            List<ContaModel> lista = new List<ContaModel>();
            ContaModel item;

            // Pega Id do usuario logado 
            string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("id");


            string sql = $"SELECT ID, NOME,SALDO,USUARIO_ID FROM CONTA WHERE USUARIO_ID = {id_usuaio_logado}";

            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new ContaModel();
                item.Id = int.Parse(dt.Rows[i]["ID"].ToString());
                item.Nome = dt.Rows[i]["Nome"].ToString();
                item.Saldo =  double.Parse(dt.Rows[i]["SALDO"].ToString());
                item.Usuaio_Id = int.Parse(dt.Rows[i]["USUARIO_ID"].ToString());
                lista.Add(item);
            }
            return lista;

        }

        public void Insert()
        {

            string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("id");
            string sql = $"INSERT INTO CONTA (NOME,SALDO,USUARIO_ID) VALUES('{Nome}','{Saldo}','{id_usuaio_logado}') ";

            DAL objDAL = new DAL();
            objDAL.ExecutarCommandoSQL(sql);
        }

        public void Excluir(int id_conta)
        {

            string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("IDUsuarioLogado");
            string sql = $"DELETE FROM CONTA WHERE ID = " + id_conta;

            DAL objDAL = new DAL();
            objDAL.ExecutarCommandoSQL(sql);
        }
    }
}
