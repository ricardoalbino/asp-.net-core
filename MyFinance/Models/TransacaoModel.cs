using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MyFinance.Models
{
    public class TransacaoModel
    {

        [Required (ErrorMessage ="Informe a Data!")]
        public int Id { get; set; }
        public string Data { get; set; }
        public string DataFinal { get; set; }
        public string Tipo { get; set; }
        public double Valor { get; set; }

        [Required(ErrorMessage = "Informe a Descrição!")]
        public string Descricao { get; set; }
        public int Conta_Id { get; set; }
        public string NomeConta { get; set; }
        public string DescricaoPlanoConta { get; set; }
        public int Plano_Conta_Id { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public TransacaoModel()
        {

        }

        public TransacaoModel(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        //
        public List<TransacaoModel> ListaTransacao()
        {
            List<TransacaoModel> lista = new List<TransacaoModel>();
            TransacaoModel item;

            // Pega Id do usuario logado 
            string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("id");

            string filtro = "";

            if (Data != null && DataFinal != null)
            {
                filtro += $"and t.Data >= '{DateTime.Parse(Data).ToString("dd-MM-yyyy")}' and t.Data <= '{DateTime.Parse(Data).ToString("dd-MM-yyyy")}'";
            }

            if (Tipo != null)
            {
                if (Tipo !=  "A")
                {
                    filtro += $" and t.Tipo = '{Tipo}'";
                }
            }

            if (Conta_Id != 0)
            {
                filtro += $" and t.Conta_Id = '{Conta_Id}'";
            }

            //Fim

            string sql = "SELECT t.ID, t.Data, t.TIPO, t.Valor, t.Descricao as Historico, t.Conta_Id, c.Nome as Conta, t.Plano_Contas_Id, p.Descricao as Plano_Conta from Transacao as t inner join Conta c on t.Conta_Id = c.Id inner join Plano_Conta as p on t.PLano_Contas_Id = p.Id"+
                $" where t.Usuario_Id = {id_usuaio_logado} {filtro} order by  t.data desc ";

            DAL objDAL = new DAL();
                DataTable dt = objDAL.RetDataTable(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    item = new TransacaoModel();
                    item.Id = int.Parse(dt.Rows[i]["ID"].ToString());
                    item.Data = DateTime.Parse(dt.Rows[i]["DATA"].ToString()).ToString("dd/MM/yyyy");
                    item.Tipo = dt.Rows[i]["tipo"].ToString();
                    item.Descricao = dt.Rows[i]["Historico"].ToString();

                    item.Valor = double.Parse(dt.Rows[i]["valor"].ToString());

                    item.Conta_Id = int.Parse(dt.Rows[i]["Conta_Id"].ToString());
                    item.NomeConta = dt.Rows[i]["Conta"].ToString();

                    item.Plano_Conta_Id = int.Parse(dt.Rows[i]["Plano_Contas_Id"].ToString());
                    item.DescricaoPlanoConta = dt.Rows[i]["Plano_Conta"].ToString();
                
                    lista.Add(item);
                }

            return lista;

        }

        //
        public TransacaoModel carregarRegistro(int? id)
        {
            TransacaoModel item;

            // Pega Id do usuario logado 
            string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("id");


            string sql = "SELECT t.ID, t.Data, t.TIPO, t.Valor, t.Descricao as Historico, t.Conta_Id, c.Nome as Conta, t.Plano_Contas_Id, p.Descricao as Plano_Conta from Transacao as t inner join Conta c on t.Conta_Id = c.Id inner join Plano_Conta as p on t.PLano_Contas_Id = p.Id" +
               $" where t.Usuario_Id = '{id_usuaio_logado}' and t.id = '{id}'";

            DAL objDAL = new DAL();
            DataTable dt = objDAL.RetDataTable(sql);

              item = new TransacaoModel();
                item.Id = int.Parse(dt.Rows[0]["ID"].ToString());
                item.Data = DateTime.Parse(dt.Rows[0]["Data"].ToString()).ToString("dd-MM-yyyy");
                item.Tipo = dt.Rows[0]["tipo"].ToString();
                item.Descricao = dt.Rows[0]["Historico"].ToString();

                item.Valor = double.Parse(dt.Rows[0]["valor"].ToString());

                item.Conta_Id = int.Parse(dt.Rows[0]["Conta_Id"].ToString());
                item.NomeConta = dt.Rows[0]["Conta"].ToString();

                item.Plano_Conta_Id = int.Parse(dt.Rows[0]["Plano_Contas_Id"].ToString());
                item.DescricaoPlanoConta = dt.Rows[0]["Plano_Conta"].ToString();

            return item;

        }

        //
        public void Insert()
        {

            string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("id");
            string sql = "";
            if (Id == 0)
            {
                sql = $"INSERT INTO TRANSACAO (DATA,TIPO,VALOR, DESCRICAO, CONTA_ID,Plano_Contas_Id, USUARIO_ID)VALUES('{Convert.ToDateTime(Data)}','{Tipo}', '{Valor}', '{Descricao}', '{Conta_Id}', '{Plano_Conta_Id}','{id_usuaio_logado}') ";
            }
            else
            {
                sql = $"UPDATE TRANSACAO SET DATA='{Convert.ToDateTime(Data)}', TIPO ='{Tipo}', VALOR = '{Valor}', DESCRICAO = '{Descricao}', CONTA_ID = '{Conta_Id}', Plano_Contas_Id = '{Plano_Conta_Id}' WHERE USUARIO_ID='{id_usuaio_logado}' AND ID = '{Id}'";
             
            }


            DAL objDAL = new DAL();
            objDAL.ExecutarCommandoSQL(sql);
        }



        public void Excluir(int id_conta)
        {

            string id_usuaio_logado = HttpContextAccessor.HttpContext.Session.GetString("id");
            string sql = $"DELETE FROM TRANSACAO WHERE ID = " + id_conta;

            DAL objDAL = new DAL();
            objDAL.ExecutarCommandoSQL(sql);
        }






    }


    public class Dashboard
    {
        public double Total { get; set; }
        public string PlanoConta { get; set; }

        public List<Dashboard> RetornarDadosGraficoPie()
        {
            string sql = "select sum(t.valor) as total, p.Descricao from transacao as t inner join plano_conta as p on t.plano_contas_id = p.Id where t.tipo = 'D' group by p.Descricao";


            List<Dashboard> lista = new List<Dashboard>();
            Dashboard item;



            DAL dAL = new DAL();
            DataTable dt = new DataTable();
            dt = dAL.RetDataTable(sql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                item = new Dashboard();
                item.Total = double.Parse(dt.Rows[i]["Total"].ToString());
                item.PlanoConta = dt.Rows[i]["Descricao"].ToString();
                lista.Add(item);


            }

            return lista;
        }




    }
}
