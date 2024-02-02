using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace LoginToDataBase
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string login, password, confirmPassword;
            login = txtUsernameReg.Text;
            SecureString securePassword = txtPasswordReg.SecurePassword;
            SecureString securePasswordConfirm = txtConfirmPasswordReg.SecurePassword;
            password = new System.Net.NetworkCredential(string.Empty, securePassword).Password;
            confirmPassword = new System.Net.NetworkCredential(string.Empty, securePasswordConfirm).Password;
            AccesStatus acces = new AccesStatus();

            if (acces.isLoginValid(login) && acces.isPasswordValid(password) && password == confirmPassword)
            {
                DB db = new DB();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command_check = new MySqlCommand("SELECT * FROM `auntefication` WHERE `login` = @ul", db.GetConnection());
                command_check.Parameters.Add("@ul", MySqlDbType.VarChar).Value = login;
                adapter.SelectCommand = command_check;
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                    acces.showExistAccrountError(lblMessageReg);
                else
                {
                    table.Clear();
                    MySqlCommand command = new MySqlCommand("INSERT INTO auntefication (login,password) VALUES (@ul, @up)", db.GetConnection());
                    command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = login;
                    command.Parameters.Add("@up", MySqlDbType.VarChar).Value = password;
                    db.openConnection();
                    acces.defineAccesStatus(adapter, command: command);             
                    acces.show_status(lblMessageReg);

                }
            }
            else if (password != confirmPassword)
            {
                acces.showPasswordUnmathcedError(lblMessageReg);
            }
            else
            {
                acces.showWrongInputStatus(lblMessageReg);
            }
        }

        private void btnBackToLogin_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
