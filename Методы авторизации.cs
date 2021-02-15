using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Музыкальный_магазин_пластинок
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void Logon(object sender, RoutedEventArgs e)
        {
            LogOut(sender, e);
            if (LoginBox.Text == String.Empty)
            {
                statusLabl.Content = "Не указан логин!";
                return;
            }

            var User = магазин.Пользователи.FirstOrDefault<Пользователи>(s => s.Логин == LoginBox.Text);
            if (User != null)
            {
                if (User.Пароль == PassworBox.Password)
                {
                    tabConMain.Visibility = Visibility.Visible;
                    tabItCatalog.Visibility = Visibility.Visible;
                    tBoxSearch.Visibility = Visibility.Visible;
                    LogonGrid.Visibility = Visibility.Hidden;
                    btnSearch.Visibility = Visibility.Visible;
                    tbISales.Visibility = Visibility.Visible;
                    tbItmSale.Visibility = Visibility.Visible;
                    MainMenu.Visibility = Visibility.Visible;
                    LoginBox.Text = String.Empty;
                    PassworBox.Password = String.Empty;
                    return;
                }
                else
                {
                    LoginBox.Text = String.Empty;
                    PassworBox.Password = String.Empty;
                    statusLabl.Content = "Пара логин/пароль некорректная!";
                    LoginBox.Text = String.Empty;
                    PassworBox.Password = String.Empty;
                }
            }
            else
            {
                statusLabl.Content = "Пара логин/пароль некорректная!";
                LoginBox.Text = String.Empty;
                PassworBox.Password = String.Empty;
            }
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            tabConMain.Visibility = Visibility.Collapsed;
            LogonGrid.Visibility = Visibility.Visible;
            tBoxSearch.Text = String.Empty;
            MainMenu.Visibility = Visibility.Collapsed;
        }


        private void PasswordEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Logon(sender, new RoutedEventArgs());
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}