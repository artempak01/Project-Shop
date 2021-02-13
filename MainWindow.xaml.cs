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
        private Пластинки CurientSingle = null;
        public MainWindow()
        {
            InitializeComponent();
            
            LogOut(new object(), new RoutedEventArgs());
            LoginBox.Text = "1";
            PassworBox.Password = "1";
            CurientSingle = new Пластинки();
        }

        private void Found_Singls(object sender, RoutedEventArgs e)
        {
            
            lbSearchResult.Items.Clear();

            using (Магазин_пластинок_ магазин = new Магазин_пластинок_())
            {
                магазин.Configuration.LazyLoadingEnabled = false;
                var result = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(tBoxSearch.Text));
                foreach(var pl in result)
                {
                    lbSearchResult.Items.Add(pl.Название);
                }
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
            using (Магазин_пластинок_ магазин = new Магазин_пластинок_())
            {
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
                        LiViSearhResult.Visibility = Visibility.Visible;
                        MainMenu.Visibility = Visibility.Visible;
                        LoginBox.Text = String.Empty;
                        PassworBox.Password = String.Empty;
                        //для теста
                        tBoxSearch.Text = "eminem";
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
            
            
        }
        private void LogOut(object sender, RoutedEventArgs e)
        {
            tabConMain.Visibility = Visibility.Collapsed;
            LogonGrid.Visibility = Visibility.Visible;
            tBoxSearch.Text = String.Empty;
            LiViSearhResult.ItemsSource = null;
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
            //this.Resources.Clear();

            if (lbSearchResult.SelectedIndex == -1) return;
            using (Магазин_пластинок_ магазин = new Магазин_пластинок_())
            {
                магазин.SaveChanges();
                магазин.Configuration.LazyLoadingEnabled = false;
                CurientSingle = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(lbSearchResult.SelectedItem.ToString())).FirstOrDefault();
                    магазин.Entry<Пластинки>(CurientSingle).Reference(s => s.Жанры).Load();
                    магазин.Entry<Пластинки>(CurientSingle).Reference(s => s.Издатели).Load();
                    магазин.Entry<Пластинки>(CurientSingle).Reference(s => s.Исполнители).Load();
               
                this.DataContext = CurientSingle;
            }
        }
    }
    
}
