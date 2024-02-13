using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
using System.Security;
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
using System.Text.RegularExpressions;
using NLog;
using NLog.Web;

namespace LoginToDataBase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnLogin_Click(sender, e);

        }
      
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login, password;
            login = txtUsername.Text;
            SecureString securePassword = txtPassword.SecurePassword;

            password = new System.Net.NetworkCredential(string.Empty, securePassword).Password;

            AccesStatus acces = new AccesStatus();
            if (acces.isLoginValid(login) && acces.isPasswordValid(password))
            {
                DB db = new DB();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command_check = new MySqlCommand("SELECT * FROM `auntefication` WHERE `login` = @ul ", db.GetConnection());
                command_check.Parameters.Add("@ul", MySqlDbType.VarChar).Value = login;
                adapter.SelectCommand = command_check;
                adapter.Fill(table);

                if (table.Rows.Count == 0)
                {
                    acces.showNoAccountExist(lblMessage);
                }
                else
                {
                    table.Clear();
                    MySqlCommand command = new MySqlCommand("SELECT `id` FROM `auntefication` WHERE `login` = @ul AND `password` = @up", db.GetConnection());
                    command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = login;
                    command.Parameters.Add("@up", MySqlDbType.VarChar).Value = password;
                    adapter.SelectCommand = command;
                    adapter.Fill(table);
                    acces.defineAccesStatus(adapter, table);
                    acces.show_status(lblMessage);
                    if (acces.accesStatus)
                    {
                        CurrentUser.id = Convert.ToInt32(table.Rows[0]["id"]);
                        CurrentUser.login = login;
                        CurrentUser.password = password;
                        var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
                        if (CurrentUser.GetUserStatus() == DbConstatnts.bannedStatus)
                        {
                            logger.Trace($"User {CurrentUser.id} with {CurrentUser.login} loged in.");
                            mainFrame.Content = new BannedPage();
                        }
                        else
                        {
                            logger.Trace($"User {CurrentUser.id} with {CurrentUser.login} loged in.");
                            mainFrame.Content = new main();
                        }

                    }
                }
            }
            else if (login == "" || password == "")
            {
                acces.showEmptyLineError(lblMessage);
            }
            else
            {
                acces.showWrongInputStatus(lblMessage);
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Content = new Registration();

        }
    }
}
