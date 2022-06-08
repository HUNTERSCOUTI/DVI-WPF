using System;
using System.Collections.Generic;
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

namespace DVI
{
    public partial class UserLogin : Window
    {
        public UserLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var username = UserName.Text;
            var password = PassWord.Text;

            using UserDataContext context = new UserDataContext();

            var userExists = context.Users.Any(user => user.Username == username && user.Password == password);

            if (userExists)
            {
                GrantAccess();
                Close();
            } else
            {
                MessageBox.Show("User not found");
            }
        }

        public void GrantAccess()
        {
            var main = new MainPanel();
            main.Show();
        }
    }
}
