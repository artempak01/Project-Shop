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
       /// <summary>
       /// Авторизация пользователя.
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void Logon(object sender, RoutedEventArgs e)
        {
            ///
            ///Скрываем весь интерфейс, кроме интерфейса для авторизации.
            ///
            LogOut(sender, e);

            ///
            ///Проверяем поля логин/пароль на пустоту
            ///
            if (LoginBox.Text == String.Empty)
            {
                StatusLable.Content = "Не указан логин!";
                return;
            }
            ///
            ///Ищем пользователя по логину
            ///
            var User = магазин.Пользователи.FirstOrDefault<Пользователи>(s => s.Логин == LoginBox.Text);
            if (User != null)
            {
                ///Проверяем пароль
                if (User.Пароль == PassworBox.Password)
                {
                    ///Проверяем роль
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

        /// <summary>
        /// Скрытие всего интерфейса кроме интерфейса авторизации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// Авторизация по нажатию Enter.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PasswordEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Logon(sender, new RoutedEventArgs());
        }

        /// <summary>
        /// Выход из программы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}