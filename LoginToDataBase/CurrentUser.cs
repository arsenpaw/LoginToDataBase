using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginToDataBase
{
    static class CurrentUser
    {
        static public int id { get; set; }
        static public string login { get; set; }
        static public string password { get; set; }
        static public string GetUserStatus()
        {

            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT `status` FROM `auntefication` WHERE `login` = @ul AND `password` = @up", db.GetConnection());
            command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = login;
            command.Parameters.Add("@up", MySqlDbType.VarChar).Value = password;
            adapter.SelectCommand = command;
            adapter.Fill(table);
            if (table.Rows.Count == 0)
                return null;
            string status = table.Rows[0]["status"].ToString();
            return status;
        }

    }
}
