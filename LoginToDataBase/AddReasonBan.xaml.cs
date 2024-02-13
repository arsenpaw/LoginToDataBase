using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoginToDataBase
{
    /// <summary>
    /// Interaction logic for AddReasonBan.xaml
    /// </summary>
    public partial class AddReasonBan : Page
    {
        private readonly DB db;
        private readonly DataTable table;
        private readonly MySqlDataAdapter adapter;
        private readonly AccesStatus acces;
        AdminPanel adminPanel;
        public AddReasonBan(AdminPanel adminPanel)
        {
            InitializeComponent();
            db = new DB();
            table = new DataTable();
            adapter = new MySqlDataAdapter();
            acces = new AccesStatus();
            this.adminPanel = adminPanel;
            adminPanel.labelAdminPanel.Content = "What is the reasone?";
        }

        private void InsertIntoBanTable(string banReasone)
        {
            try
            {
                MySqlCommand command_ban = new MySqlCommand("INSERT INTO banned_users (`id`,`login`,`banned_by`,`reasone`) VALUES (@id,@ul,@bb,@re)", db.GetConnection());
                command_ban.Parameters.Add("@ul", MySqlDbType.VarChar).Value = SelectedUser.login;
                command_ban.Parameters.Add("@id", MySqlDbType.Int32).Value = SelectedUser.id;
                command_ban.Parameters.Add("@bb", MySqlDbType.VarChar).Value = CurrentUser.login;
                command_ban.Parameters.Add("@re", MySqlDbType.VarChar).Value = banReasone;   
                acces.defineAccesStatus(adapter, command: command_ban);
                acces.show_status(adminPanel.labelAdminPanel);
                int rowsAffected = command_ban.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    adminPanel.labelAdminPanel.Content = "User banned and inserted into the table";
                }
                else
                {
                    adminPanel.labelAdminPanel.Content = "User was not inserted into ban table";
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);           
            }

        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {       
            try
            {
                string banReasone = BanReasonTextBox.Text;
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand("UPDATE auntefication SET status = @st WHERE login = @ul AND id = @id", db.GetConnection());

                command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = SelectedUser.login;
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = SelectedUser.id;
                command.Parameters.Add("@st", MySqlDbType.VarChar).Value = DbConstatnts.bannedStatus;
                db.openConnection();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    InsertIntoBanTable(banReasone);
                }
                else
                {
                    adminPanel.labelAdminPanel.Content = "User status was not changed";
                }
               
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                
            }
            finally { db.closeConnection(); table.Clear(); NavigationService.GoBack(); }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BanReasonTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
