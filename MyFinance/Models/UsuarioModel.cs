using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace MyFinance.Models
{
    public class UsuarioModel
    {
        // Campo Auto Increment
        public int Id { get; set; }

        //Anotações para validações de campos do Fomulario 
        [Required (ErrorMessage ="Informe seu Nome!")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Informe seu E-mail!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="O E-mail informado é Invalido!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Informe sua Senha!")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Informe sua Data de Nascimento!")]
        public string Data_Nascimento { get; set; }

       
        public string IMG { get; set; }


        public bool ValidarLogin()
        {
            string sql = $"SELECT ID, NOME, DATA_NASCIMENTO, IMG FROM USUARIO WHERE EMAIL = '{Email}' AND SENHA = '{Senha}'";
            DAL objDAL = new DAL();

            DataTable dt = objDAL.RetDataTable(sql);

            // verifica se a DataTable foi preenchida
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    // capetura dados do usuario e quarda nas variaveis - 
                    //                     C    L
                    Id = int.Parse(dt.Rows[0]["ID"].ToString());
                    Nome = dt.Rows[0]["NOME"].ToString();
                    Data_Nascimento = dt.Rows[0]["DATA_NASCIMENTO"].ToString();
                    IMG = dt.Rows[0]["IMG"].ToString();

                    //retorna true
                    return true;
                }
            }
            return false;

        }

        public void RegistrarUsuario()
        {
            string DataNascimento = DateTime.Parse(Data_Nascimento).ToString("yyyy/MM/dd");
            string sql = $"INSERT INTO USUARIO(NOME, EMAIL, SENHA, DATA_NASCIMENTO) VALUES('{Nome}','{Email}','{Senha}','{DataNascimento}')";
            DAL objDAL = new DAL();
            objDAL.ExecutarCommandoSQL(sql);
        }
    }
}
