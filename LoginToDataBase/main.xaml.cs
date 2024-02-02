using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for main.xaml
    /// </summary>
    /// 

   


    public partial class main : Page
    {
      
        public main()
        {
            InitializeComponent();
            showUpdatedTable(updateTable());
            dataGrid.SelectionChanged += DataGrid_SelectionChanged;
           
        }

       

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            showUpdatedTable(updateTable());

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private DataTable updateTable()
        {
            if (CurrentUser.GetUserStatus() == null || CurrentUser.login == null)
                NavigationService.GoBack();
            else if (CurrentUser.GetUserStatus() == DbConstatnts.adminStatus)
            {
                btnAdminPanel.Visibility = Visibility.Visible;
            }
            DB db = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command_check = new MySqlCommand("SELECT * FROM `auntefication`", db.GetConnection());
            adapter.SelectCommand = command_check;
            adapter.Fill(table);
            return table;
        }
        private void showUpdatedTable(DataTable table)
        {

            dataGrid.ItemsSource = table.DefaultView;
            showCurrentTime();
        }
        private void showCurrentTime()
        {
            DateTime time = DateTime.Now;
            captionMain.Text = $"Update time is: {time.ToString()}";
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string login, status;
            int id;
            DataRowView rowView;
           

            if (dataGrid.SelectedItems.Count > 1)
            {
                captionMain.Foreground = new SolidColorBrush(Colors.Red);
                captionMain.Text = "You can not delete multiple people";
                return;
            }
            try
            {
                rowView = (DataRowView)dataGrid.SelectedItem;

            }
            catch (System.InvalidCastException ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                captionMain.Text = $"Empty line selected .";
                return;
            }
            if (rowView == null)
                return;
            try
            {

                object[] selectedItems = rowView.Row.ItemArray.ToArray();
                id = Convert.ToInt32(selectedItems[0]);
                login = Convert.ToString(selectedItems[1]);
                status = Convert.ToString(selectedItems[3]);
                SelectedUser.status = status;
                SelectedUser.id = id;
                SelectedUser.login = login;
                
            }
            catch (Exception exParse)
            {
                SelectedUser.status = null;
                SelectedUser.id = 0;
                SelectedUser.login = null;
                Debug.WriteLine(exParse.Message);
            }

        }
        private void showDeletedAcount(int rowsAffected, int id, string status, string login)
        {
            if (rowsAffected > 0)
            {
                showUpdatedTable(updateTable());
                captionMain.Foreground = new SolidColorBrush(Colors.Blue);
                captionMain.Text = $"{status}: {login} ID: {id}, was successfully deleted";
            }
            else
            {
                captionMain.Foreground = new SolidColorBrush(Colors.Red);
                captionMain.Text = $"No rows were deleted. Check your conditions.";
            }
        }
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            updateTable();
            try
            {
                string logedUserLogin = CurrentUser.login;
                string logedUserPassword = CurrentUser.password;
                string logedUserStatus = CurrentUser.GetUserStatus();
                DB db = new DB();
                if (logedUserLogin != SelectedUser.login && SelectedUser.status != DbConstatnts.adminStatus && logedUserStatus == DbConstatnts.adminStatus)
                {
                    int rowsAffected = db.DeletUserByLoginID(SelectedUser.id, SelectedUser.login);
                    showDeletedAcount(rowsAffected, SelectedUser.id, SelectedUser.status, SelectedUser.login);
                }
                else if (SelectedUser.login == null || SelectedUser.status == null)
                {
                    captionMain.Foreground = new SolidColorBrush(Colors.Red);
                    captionMain.Text = $"You havent selected an account";
                }
                else if (SelectedUser.status == DbConstatnts.adminStatus)
                {
                    captionMain.Foreground = new SolidColorBrush(Colors.Red);
                    captionMain.Text = $"Admins can not be deleted.";
                }
                else if (SelectedUser.login == logedUserLogin)
                {
                    CriticalWindow criticalWindow = CriticalWindow.Initialize();
                    criticalWindow.Show();
                }

                else
                {
                    captionMain.Foreground = new SolidColorBrush(Colors.Red);
                    captionMain.Text = $"Only admins can delete other accounts.";
                }
            }

            catch (Exception ex)
            {

                Debug.WriteLine($"Error: {ex.Message}");
                Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                captionMain.Text = $"An error occurred. Please check the console .";
            }
        }

        private void btnAdminPanel_Click(object sender, RoutedEventArgs e)
        {
            AdminPanel adminPanel =  AdminPanel.Initialize();
            adminPanel.Show();
        }
    }

}


