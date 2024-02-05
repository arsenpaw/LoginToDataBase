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
using MySql.Data.MySqlClient;


namespace LoginToDataBase
{
    /// <summary>
    /// Interaction logic for BannedPage.xaml
    /// </summary>
    public partial class BannedPage : Page
    {
        public BannedPage()
        {
            InitializeComponent();
            try
            {
                DB db = new DB();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                DataTable table = new DataTable();
                MySqlCommand command = new MySqlCommand("SELECT * FROM `banned_users` WHERE `id` = @id AND `login` = @ul", db.GetConnection());
                command.Parameters.Add("@id", MySqlDbType.Int32).Value = CurrentUser.id;
                command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = CurrentUser.login;

                adapter.SelectCommand = command;
                adapter.Fill(table);

                if (table.Rows.Count > 0)
                {
                    string bannedBy = table.Rows[0]["banned_by"].ToString();
                    string reason = table.Rows[0]["reasone"].ToString();
                    LabelAdminName.Text = $"Admin: {bannedBy}";
                    labelReason.Text = $"Reason: {reason}";
                }
                else
                {
                    LabelAdminName.Text = "N/A";
                    labelReason.Text = "N/A";
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                LabelBannedSmall.Text = "ERRORE";
            }

        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
