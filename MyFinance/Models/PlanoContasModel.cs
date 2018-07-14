using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Models
{
    public class PlanoContasModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public string Tipo { get; set; }
        public int Usuario_Id { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }


        private string IdUsuarioLogado()
        {
            return HttpContextAccessor.HttpContext.Session.GetString("id");

        }

        public PlanoContasModel()
        {

        }

        public PlanoContasModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        //
        public List<PlanoContasModel> ListaPlanoContas()
        {
            List<PlanoContasModel> lista = new List<PlanoContasModel>();
            PlanoContasModel item;

            // Pega Id do usuario logado 
            //string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("id");


            string sql = $"SELECT ID, DESCRICAO,TIPO,USUARIO_ID FROM PLANO_CONTA WHERE USUARIO_ID = {IdUsuarioLogado()}";

            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item                = new PlanoContasModel();
                item.Id             = int.Parse(dt.Rows[i]["ID"].ToString());
                item.Descricao       = dt.Rows[i]["DESCRICAO"].ToString();
                item.Tipo           =     dt.Rows[i]["TIPO"].ToString();
                item.Usuario_Id     = int.Parse(dt.Rows[i]["USUARIO_ID"].ToString());
                lista.Add(item);
            }
            return lista;

        }


        public PlanoContasModel CarregarRegistro(int? id)
        {
            PlanoContasModel item = new PlanoContasModel();

            // Pega Id do usuario logado 
            string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("id");

            string sql = $"SELECT ID, DESCRICAO,TIPO,USUARIO_ID FROM PLANO_CONTA WHERE USUARIO_ID = {IdUsuarioLogado()} AND ID = {id}";

            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

           
            item.Id = int.Parse(dt.Rows[0]["ID"].ToString());
            item.Descricao = dt.Rows[0]["DESCRICAO"].ToString();
            item.Tipo = dt.Rows[0]["TIPO"].ToString();
            item.Usuario_Id = int.Parse(dt.Rows[0]["USUARIO_ID"].ToString());

            return item;

        }

        public void Insert()
        {

            string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("id");
            string sql = "";
            if (Id == 0)
            {
                 sql = $"INSERT INTO PLANO_CONTA (DESCRICAO,TIPO,USUARIO_ID) VALUES('{Descricao}','{Tipo}','{IdUsuarioLogado()}') ";
            }
            else{
                 sql = $"UPDATE PLANO_CONTA SET DESCRICAO='{Descricao}',TIPO ='{Tipo}' WHERE USUARIO_ID='{IdUsuarioLogado()}' AND ID = '{Id}'";
            }
            

            DAL objDAL = new DAL();
            objDAL.ExecutarCommandoSQL(sql);
        }



        public void Excluir(int id_conta)
        {

            string id_usuaio_logado = IdUsuarioLogado();
            string sql = $"DELETE FROM PLANO_CONTA WHERE ID = " + id_conta;

            DAL objDAL = new DAL();
            objDAL.ExecutarCommandoSQL(sql);
        }
    }


}

