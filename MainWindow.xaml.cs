using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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

namespace Музыкальный_магазин_пластинок
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Пластинки CurientSingle = new Пластинки();
        Пластинки SalingSingle = new Пластинки();
        Магазин_пластинок_ магазин = new Магазин_пластинок_();
        public MainWindow()
        {
            InitializeComponent();
            LogOut(new object(), new RoutedEventArgs());
            
            ///для теста
            LoginBox.Text = "1";
            PassworBox.Password = "1";
            Logon(new object(), new RoutedEventArgs());
        }

        private void Found_Singls(object sender, RoutedEventArgs e)
        {

            lbSearchSinglesResult.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(tBoxSearch.Text));
            foreach (var pl in result)
            {
                lbSearchSinglesResult.Items.Add(pl.Название);
            }
            магазин.SaveChanges();
            lbSearchArtistResult.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result_1 = магазин.Исполнители.Where<Исполнители>(s => s.Имя.Contains(tBoxSearch.Text));
            foreach (var Art in result_1)
            {
                lbSearchArtistResult.Items.Add(Art.Имя);
            }
        }

        private void Key_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (tBoxSearch.Text.Length >= 0) Found_Singls(sender, new RoutedEventArgs());
                else MessageBox.Show("Введите текст для поиска", "Внимание", MessageBoxButton.OK);
            }
        }

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

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void PasswordEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) Logon(sender, new RoutedEventArgs());
        }

        private void ChooseSingle(object sender, SelectionChangedEventArgs e)
        {
            if (lbSearchSinglesResult.SelectedIndex == -1) return;
            магазин.SaveChanges();
            магазин.Configuration.LazyLoadingEnabled = false;
            CurientSingle = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(lbSearchSinglesResult.SelectedItem.ToString())).FirstOrDefault();
            магазин.Entry<Пластинки>(CurientSingle).Reference(s => s.Жанры).Load();
            магазин.Entry<Пластинки>(CurientSingle).Reference(s => s.Издатели).Load();
            магазин.Entry<Пластинки>(CurientSingle).Reference(s => s.Исполнители).Load();
            CurrSingle.DataContext = CurientSingle;
            
            Обложка.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + CurientSingle.Обложка));
        }

        

      

        private void ChooseSingles(object sender, SelectionChangedEventArgs e)
        {
            магазин.SaveChanges();
            if(lbSearchArtistResult.SelectedIndex == -1) return;
            lbSearchSinglesResult.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result = магазин.Пластинки.Where<Пластинки>(s => s.Исполнители.Имя.Contains(lbSearchArtistResult.SelectedItem.ToString()));
            foreach (var pl in result)
            {
                lbSearchSinglesResult.Items.Add(pl.Название);
            }
        }
       
    }

}
