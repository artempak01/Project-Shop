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
                StatusLable.Content = "Не указан логин!";
                return;
            }

            var User = магазин.Пользователи.FirstOrDefault<Пользователи>(s => s.Логин == LoginBox.Text);
            if (User != null)
            {
                if (User.Пароль == PassworBox.Password)
                {
                    switch (User.Роль)
                    {
                        case "admin":
                            MainTabControl.Visibility = Visibility.Visible;
                            CatalogTabItem.Visibility = Visibility.Visible;
                            SalesTabItem.Visibility = Visibility.Visible;
                            SaleTabItem.Visibility = Visibility.Visible;
                            SalesResultsTabItem.Visibility = Visibility.Visible;
                            MainMenu.Visibility = Visibility.Visible;
                            LogonGrid.Visibility = Visibility.Hidden;
                            LoginBox.Text = String.Empty;
                            PassworBox.Password = String.Empty;
                            return;
                        case "seller":
                            MainTabControl.Visibility = Visibility.Visible;
                            CatalogTabItem.Visibility = Visibility.Collapsed;
                            LogonGrid.Visibility = Visibility.Hidden;
                            SalesTabItem.Visibility = Visibility.Collapsed;
                            SalesResultsTabItem.Visibility = Visibility.Collapsed;
                            SaleTabItem.Visibility = Visibility.Visible;
                            SaleTabItem.IsSelected = true;                            
                            MainMenu.Visibility = Visibility.Visible;
                            LoginBox.Text = String.Empty;
                            PassworBox.Password = String.Empty;
                            return;
                    }
                    
                }
                else
                {
                    LoginBox.Text = String.Empty;
                    PassworBox.Password = String.Empty;
                    StatusLable.Content = "Пара логин/пароль некорректная!";
                    LoginBox.Text = String.Empty;
                    PassworBox.Password = String.Empty;
                }
            }
            else
            {
                StatusLable.Content = "Пара логин/пароль некорректная!";
                LoginBox.Text = String.Empty;
                PassworBox.Password = String.Empty;
            }
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            ///очищаем заполненные поля
            CurientSingle = new Пластинки();
            SalingSingle = new Пластинки();
            SellingCover.Source = null;
            CurientCover.Source = null;
            CurientSingleGrid.DataContext = CurientSingle;
            SingleToSaleGrid.DataContext = SalingSingle;
            CustomersList.Items.Clear();
            lbSearchArtistResult.Items.Clear();
            lbSearchSinglesResult.Items.Clear();
            lbSearchResultToSale.Items.Clear();
            tBoxSearchToSale.Text = String.Empty;
            tBoxSearchCatalog.Text = String.Empty;
            
            ///скрываем интерфейс
            MainTabControl.Visibility = Visibility.Collapsed;
            MainMenu.Visibility = Visibility.Collapsed;
            
            ///показываем интерфейс авторизации
            LogonGrid.Visibility = Visibility.Visible;
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