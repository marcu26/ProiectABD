using Core.Database.Context;
using Core.Dtos.Users;
using Core.Email;
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

        private bool isResettingPasswordStep1 = false;

        private bool isResettingPasswordStep2 = false;

        private EmailService _emailService = new EmailService();

        private UserDto loggedUser;
        public LogInWindow()
        {
            InitializeComponent();
        }

        private async void LogInButton_Click(object sender, RoutedEventArgs e)
        {

            if (isResettingPasswordStep1)
            {
                string email = EmailTextBox.Text;
                try
                {
                    await _usersRepository.GetResetPasswordEmail(email);
                    TextBlockEmail.Text = "Enter code from email";
                    PasswordGrid.Visibility = Visibility.Visible;
                    ConfirmPasswordGrid.Visibility = Visibility.Visible;
                    LogInButton.Content = "Reset password";
                    isResettingPasswordStep2 = true;
                    isResettingPasswordStep1 = false;
                    EmailTextBox.Text = string.Empty;


                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            if (isResettingPasswordStep2)
            {
                string code = EmailTextBox.Text;
                string newPassw = PasswordTextBox.Password;
                string confirmNewPassw = ConfirmPasswordTextBox.Password;

                if (newPassw != confirmNewPassw)
                {
                    MessageBox.Show("Parolele nu coincid!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                try
                {
                    await _usersRepository.ResetPasswordAsync(code, newPassw);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                isResettingPasswordStep1 = false;
                isResettingPasswordStep2 = false;


                CancelButton_Click(sender, e);
                return;
            }

            if (isLogInMode)
            {
                string email = EmailTextBox.Text;
                string password = PasswordTextBox.Password;

                if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Completati toate campurile!", "Warning!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    loggedUser = await _usersRepository.LoginByEmailAndPassword(email, password);
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
                string password = PasswordTextBox.Password;
                string confirmPassword = ConfirmPasswordTextBox.Password;

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

            MainWindow mw = new MainWindow(loggedUser);
            mw.Show();
            this.Close();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            FullnameTextBox.Text = String.Empty;
            EmailTextBox.Text = String.Empty;
            PasswordTextBox.Password = String.Empty;
            ConfirmPasswordTextBox.Password = String.Empty;

            GuestLogInButton.Visibility = Visibility.Hidden;
            ForgotPasswordButton.Visibility = Visibility.Hidden;

            LogInButton.Content = "Sign Up";
            LogLabel.Content = "Register";
            PasswordGrid.Visibility = Visibility.Visible;
            ConfirmPasswordGrid.Visibility = Visibility.Visible;
            FullnameGrid.Visibility = Visibility.Visible;
            CancelButton.Visibility = Visibility.Visible;
            isLogInMode = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            FullnameTextBox.Text = String.Empty;
            EmailTextBox.Text = String.Empty;
            PasswordTextBox.Password = String.Empty;
            ConfirmPasswordTextBox.Password = String.Empty;

            isResettingPasswordStep1 = false;
            isResettingPasswordStep2 = false;
            GuestLogInButton.Visibility = Visibility.Visible;
            ForgotPasswordButton.Visibility = Visibility.Visible;

            LogInButton.Content = "Log In";
            LogLabel.Content = "Log In";
            PasswordGrid.Visibility = Visibility.Visible;
            FullnameGrid.Visibility = Visibility.Hidden;
            ConfirmPasswordGrid.Visibility = Visibility.Hidden;
            CancelButton.Visibility = Visibility.Hidden;
            isLogInMode = true;
        }

        private void ForgotPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            isResettingPasswordStep1 = true;
            CancelButton.Visibility = Visibility.Visible;
            GuestLogInButton.Visibility = Visibility.Hidden;
            ForgotPasswordButton.Visibility = Visibility.Hidden;
            PasswordGrid.Visibility = Visibility.Hidden;
            LogLabel.Content = "Email for reseting your password";
            LogInButton.Content = "Send email";
        }

        private async void GuestLogInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                loggedUser = await _usersRepository.LoginByEmailAndPassword("guest", "guest");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            MainWindow mw = new MainWindow(loggedUser);
            mw.Show();
            this.Close();
        }

        private void PasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBlockPassword.Visibility = Visibility.Hidden;
        }

        private void PasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(PasswordTextBox.Password.Length == 0)
            {
                TextBlockPassword.Visibility = Visibility.Visible;
            }
        }

        private void ConfirmPasswordTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBlockConfirmPassword.Visibility = Visibility.Hidden;
        }

        private void ConfirmPasswordTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ConfirmPasswordTextBox.Password.Length == 0)
            {
                TextBlockConfirmPassword.Visibility = Visibility.Visible;
            }
        }
    }
}
