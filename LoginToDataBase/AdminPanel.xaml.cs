using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;




namespace LoginToDataBase
{
  
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        private static AdminPanel single = null;
        public static AdminPanel GetInstanseURL()
        {
            return single;
        }
        public static AdminPanel Initialize()
        {
            if (single == null)
                single = new AdminPanel();
            return single;
        }
        protected AdminPanel()
        {
            InitializeComponent();
            LayoutUpdated += AdminPanel_LayoutUpdated;
        }

        private void AdminPanel_LayoutUpdated(object? sender, EventArgs e)
        {
            MakeAdminButton.Click -= BackToUserButton_Click; // Unsubscribe previous handler
            MakeAdminButton.Click -= MakeAdminButton_Click;
            BanButton.Click -= BanButton_Click;
            BanButton.Click -= UnbanButton_Click;
            if (CurrentUser.GetUserStatus() != DbConstatnts.adminStatus)
            {
                Close();
            }
            MakeAdminButton.Visibility = Visibility.Visible;
            if (SelectedUser.status != DbConstatnts.bannedStatus)
            {               
                BanButton.Content = "Ban";
                BanButton.Click += BanButton_Click;
            }
            else 
            {
                MakeAdminButton.Visibility = Visibility.Collapsed;
                BanButton.Content = "Unban";
                BanButton.Click += UnbanButton_Click;
                
            }
            if (SelectedUser.status == DbConstatnts.adminStatus)
            {             
                MakeAdminButton.Content = "Downgrade to user";
                MakeAdminButton.Click += BackToUserButton_Click;
            }
            else
            {              
                MakeAdminButton.Content = "Upgrade to admin";
                MakeAdminButton.Click += MakeAdminButton_Click;
            }

        }

       

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            single = null;
        }

        private void BackToUserButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccesStatus acces = new AccesStatus();
                DB db = new DB();
                DataTable table = new DataTable();
                table.Clear();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand("UPDATE auntefication SET status = 'User' WHERE login = @ul AND id = @id", db.GetConnection());
                command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = SelectedUser.login;
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = SelectedUser.id;
                db.openConnection();
                acces.defineAccesStatus(adapter, command: command);
                acces.show_status(labelAdminPanel);
                db.closeConnection();
            }
            catch (Exception ex) 
            { 
                Debug.WriteLine(ex.Message);
                labelAdminPanel.Content = "Reselect the person";
            }
        }
        private void MakeAdminButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccesStatus acces = new AccesStatus();
                DB db = new DB();
                DataTable table = new DataTable();
                table.Clear();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand("UPDATE auntefication SET status = 'Admin' WHERE login = @ul AND id = @id", db.GetConnection());
                command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = SelectedUser.login;
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = SelectedUser.id;
                db.openConnection();
                acces.defineAccesStatus(adapter, command: command);
                acces.show_status(labelAdminPanel);
                db.closeConnection();
            }
            catch(Exception ex) 
            {
                Debug.WriteLine(ex.Message);
                labelAdminPanel.Content = "Reselect the person";
            }
        }

        private void BanButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccesStatus acces = new AccesStatus();
                DB db = new DB();
                DataTable table = new DataTable();
                table.Clear();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand("UPDATE auntefication SET status = @st WHERE login = @ul AND id = @id", db.GetConnection());
                command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = SelectedUser.login;
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = SelectedUser.id;
                command.Parameters.Add("@st", MySqlDbType.VarChar).Value = DbConstatnts.bannedStatus;
                db.openConnection();
                acces.defineAccesStatus(adapter, command: command);
                acces.show_status(labelAdminPanel);
                db.closeConnection();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                labelAdminPanel.Content = "Reselect the person";
            }
        }
        private void UnbanButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AccesStatus acces = new AccesStatus();
                DB db = new DB();
                DataTable table = new DataTable();
                table.Clear();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand("UPDATE auntefication SET status = @st WHERE login = @ul AND id = @id", db.GetConnection());
                command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = SelectedUser.login;
                command.Parameters.Add("@id", MySqlDbType.VarChar).Value = SelectedUser.id;
                command.Parameters.Add("@st", MySqlDbType.VarChar).Value = DbConstatnts.userStatus;
                db.openConnection();
                acces.defineAccesStatus(adapter, command: command);
                acces.show_status(labelAdminPanel);
                db.closeConnection();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                labelAdminPanel.Content = "Reselect the person";
            }
        }
    }
}
