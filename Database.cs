using System.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityCRUD
{
    internal class Database
    {
        SqlConnection connection = new SqlConnection(@"Data Source=DESKTOP-AO7O24K;Initial Catalog=books_db_klychev;Integrated Security=True");

        public void openConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed) //Открываем соединение
                connection.Open();
        }

        public void closeConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open) //Закрываем соединение
                connection.Close();
        }

        public SqlConnection getConnection()
        {
            return connection;
        }
    }
}
