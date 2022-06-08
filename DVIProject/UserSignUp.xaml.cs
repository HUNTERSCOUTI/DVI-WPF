using System;
using System.Collections.Generic;
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

namespace DVI
{
    public partial class UserSignUp : Window
    {
        public UserSignUp()
        {
            InitializeComponent();
        }

        private void btnSignup_Click(object sender, RoutedEventArgs e)
        {
            var username = UserName.Text;
            var password = PassWord.Text;

            using var context = new UserDataContext();

            var user = new User(username, password);

            var userExists = context.Users.Any(user => user.Username == username);

            if (!userExists) 
            {
                context.Add(user);
                context.SaveChanges();
                Debug.WriteLine($"New user has id {user.Id}");

                GrantAccess();
                Close();
            } else
            {
                MessageBox.Show("Username already exists");
            }
        }

        public void GrantAccess()
        {
            var entry = new EntryWindow();
            entry.Show();
        }
    }
}
