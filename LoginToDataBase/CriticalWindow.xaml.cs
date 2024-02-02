using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    static class CriticalPageChose
    {
        static public bool userSelectedButton { get; set; }

    }


    public partial class CriticalWindow : Window
    {
        private static CriticalWindow single = null;
        public static CriticalWindow GetInstanseURL()
            {
            return single;
            }
        protected CriticalWindow()
        {
            InitializeComponent();
            this.Focus();
        }
        public static CriticalWindow Initialize()
        {
            if (single == null)       
                single = new CriticalWindow();
            return single;
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            CriticalPageChose.userSelectedButton = true;
            DB db = new DB();
            int rowsAffected = db.DeletUserByLoginID(SelectedUser.id, SelectedUser.login);
            if (SelectedUser.login == CurrentUser.login || SelectedUser.id == CurrentUser.id)
            {
                CurrentUser.id = 0;
                CurrentUser.login = null;
               
            }
            else
            {
                SelectedUser.login = null;
                SelectedUser.id = 0;
                SelectedUser.status = null;
            }      
            Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            CriticalPageChose.userSelectedButton = false;
            Close();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            single = null;
        }
    }
}
