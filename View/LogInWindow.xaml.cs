using Core.Database.Context;
using Core.Repositories;
using DevExpress.Xpf.Core;
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

namespace View
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : ThemedWindow
    {
        private UsersRepository _usersRepository = new UsersRepository(new ProjectDbContext());

        private bool isLogInMode = true;
        public LogInWindow()
        {
            InitializeComponent();
        }

        private async void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (isLogInMode)
            {
                string email = EmailTextBox.Text;
                string password = PasswordTextBox.Text;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Completati toate campurile!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    var user = await _usersRepository.LoginByEmailAndPassword(email, password);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            else
            {
                string fullname = FullnameTextBox.Text;
                string email = EmailTextBox.Text;
                string password = PasswordTextBox.Text;
                string confirmPassword = ConfirmPasswordTextBox.Text;

                if(password != confirmPassword)
                {
                    MessageBox.Show("Parolele nu coincid!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if(string.IsNullOrWhiteSpace(fullname) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                {
                    MessageBox.Show("Completati toate campurile!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    await _usersRepository.CreateUserAsync(email, fullname, password, 1);
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message, "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }


            MainWindow mw = new MainWindow();
            mw.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            FullnameTextBox.Text = String.Empty;
            EmailTextBox.Text = String.Empty;
            PasswordTextBox.Text = String.Empty;
            ConfirmPasswordTextBox.Text = String.Empty;

            LogInButton.Content = "Sign Up";
            LogLabel.Content = "Register";
            ConfirmPasswordGrid.Visibility = Visibility.Visible;
            FullnameGrid.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;
            isLogInMode = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            FullnameTextBox.Text = String.Empty;
            EmailTextBox.Text = String.Empty;
            PasswordTextBox.Text = String.Empty;
            ConfirmPasswordTextBox.Text = String.Empty;

            LogLabel.Content = "Log In";
            FullnameGrid.Visibility = Visibility.Hidden;
            ConfirmPasswordGrid.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;
            isLogInMode = true;
        }
    }
}
