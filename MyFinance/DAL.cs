using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace MyFinance
{
    // Data Access Layer - Camada de Acesso á Dados
    public class DAL
    {
        private static string dataSource = @".\SQLExpress";
        private static string InitialCatalog = "financeiro";
        private static string UserID = "cadu";
        private static string Password = "cadusenha";

        //Interpolação de String
        private string connectionString = $"Data Source={dataSource}; Initial Catalog={InitialCatalog}; User ID={UserID};Password={Password}";

      

        private SqlConnection connection;

        //Construtor -  Atalho ctor
        public DAL()
        {
           
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        // metodo p/ execulta SELECTS
        public DataTable RetDataTable(string sql)
        {
            DataTable dataTable = new DataTable();
            SqlCommand command = new SqlCommand(sql, connection);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(dataTable);
            return dataTable;

        }

        // metodo p/ execulta INSERTS - UPDATS - DELETES 
        public void ExecutarCommandoSQL(string sql)
        {
            SqlCommand command = new SqlCommand(sql, connection);
            command.ExecuteNonQuery();
        }
    }
}
