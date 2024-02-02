using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginToDataBase
{
    internal class DB
    {
        MySqlConnection connection = new MySqlConnection("server = localhost; port = 3306; username = root; password = root; database = customers");

        public void openConnection()
        { 
            if (connection.State ==System.Data.ConnectionState.Closed) 
                connection.Open();
        }

        public void closeConnection()
        { 
            if (connection.State ==System.Data.ConnectionState.Closed) 
                connection.Close();
        }

        public MySqlConnection GetConnection()  
        {
            return connection;
        }
        public int DeletUserByLoginID(int id,string login)
        {
            int rowsAffected;
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command_remove = new MySqlCommand("DELETE FROM `auntefication` WHERE id = @id AND login = @log ", db.GetConnection());
            command_remove.Parameters.Add("@id", MySqlDbType.VarChar).Value = id;
            command_remove.Parameters.Add("@log", MySqlDbType.VarChar).Value = login;
            db.openConnection();
            rowsAffected = command_remove.ExecuteNonQuery();
            return rowsAffected;
        }

    }
}
