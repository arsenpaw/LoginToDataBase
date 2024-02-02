using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using Label = System.Windows.Controls.Label;
namespace LoginToDataBase
{
    public class AccesStatus
    {
        private const byte minAllowedCharacters = 4;
        private bool __accesStatus = false;
        public void showWrongInputStatus(Label lblMessage)

        {
            lblMessage.Foreground = new SolidColorBrush(Colors.Red);
            lblMessage.Content = $"Password or login is has undefine symbols \nor has less than {minAllowedCharacters} characters";
            __accesStatus = false;
        }
        public void showEmptyLineError(Label lblMessage)

        {
            lblMessage.Foreground = new SolidColorBrush(Colors.Red);
            lblMessage.Content = "Your line is empty.";
            __accesStatus = false;
        }

        public void showExistAccrountError(Label lblMessage)
        {


            lblMessage.Foreground = new SolidColorBrush(Colors.Red);
            lblMessage.Content = "This account alredy created.";
            __accesStatus = false;

        }
        public void showNoAccountExist(Label lblMessage)
        {


            lblMessage.Foreground = new SolidColorBrush(Colors.Red);
            lblMessage.Content = "No kind of account, please register";
            __accesStatus = false;

        }

        public void showPasswordUnmathcedError(Label lblMessage)
        {


            lblMessage.Foreground = new SolidColorBrush(Colors.Red);
            lblMessage.Content = "Password unmatched.";
            __accesStatus = false;

        }


        public void show_status(Label lblMessage)
        {
            if (__accesStatus)
            {

                lblMessage.Foreground = new SolidColorBrush(Colors.Blue);
                lblMessage.Content = "Sucess";
            }
            else
            {
                lblMessage.Foreground = new SolidColorBrush(Colors.Red);
                lblMessage.Content = "Access denied.";
            }
        }
        public bool accesStatus
        {
            get { return __accesStatus; }

        }

        public bool defineAccesStatus(MySqlDataAdapter adapter, DataTable table = null, MySqlCommand command = null)
        {
            /// <summary>
            /// Defines the access status using a MySQL data adapter and optional parameters for a DataTable or a MySqlCommand.
            /// </summary>

            if (table != null)
                if (table.Rows.Count > 0)
                    __accesStatus = true;
                else
                    __accesStatus = false;
            else
               if (command.ExecuteNonQuery() == 1)
                __accesStatus = true;
            else
                __accesStatus = false;
            return __accesStatus;
        }
        public bool isPasswordValid(string password)
        {
            if (password.Length < minAllowedCharacters)
                return false;
            string allowedSymbols = "!@#$%^&*()_=<>?";
            string regexPattern = $@"^[a-zA-Z0-9{Regex.Escape(allowedSymbols)}]+$";
            bool isValidPassword = Regex.IsMatch(password, regexPattern);
            return isValidPassword;
        }
        public bool isLoginValid(string login)
        {
            if (login.Length < minAllowedCharacters)
                return false;
            string allowedSymbols = "!#$%^&*()_=<>?";
            string regexPattern = $@"^[a-zA-Z0-9{Regex.Escape(allowedSymbols)}]+$";
            bool isValidLogin = Regex.IsMatch(login, regexPattern);
            return isValidLogin;
        }


    }
}
