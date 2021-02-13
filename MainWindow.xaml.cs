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
using System.Drawing;

namespace Музыкальный_магазин_пластинок
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string Access = "false";
        public MainWindow()
        {
            InitializeComponent();
            //CreateCopy();
            LogOut(new object(), new RoutedEventArgs());
        }

        private void Found_Singls(object sender, RoutedEventArgs e)
        {
            using (Магазин_пластинок_ магазин = new Магазин_пластинок_())
            {
                магазин.Configuration.LazyLoadingEnabled = false;
                var result = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(tBoxSearch.Text));
                foreach (var pl in result)
                {
                    магазин.Entry<Пластинки>(pl).Reference(s => s.Жанры).Load();
                    магазин.Entry<Пластинки>(pl).Reference(s => s.Издатели).Load();
                    магазин.Entry<Пластинки>(pl).Reference(s => s.Исполнители).Load();
                }

                foreach (var pl in result)
                {

                    BitmapImage imgsource = new BitmapImage();
                    imgsource.BeginInit();
                    imgsource.UriSource = new Uri(pl.Обложка, UriKind.Relative);
                    //imgsource.StreamSource = memorystream;
                    imgsource.EndInit();
                    System.Windows.Controls.Image Image = new System.Windows.Controls.Image();
                    Image.Source = imgsource;
                    CatalogGrid.Children.Add(Image); // реальный Image
                }
                LiViSearhResult.ItemsSource = result.ToList<Пластинки>();
                EditColumns();
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
        private void EditColumns()
        {
            LiViSearhResult.Columns.Remove(LiViSearhResult.Columns.Where(s => s.Header.Equals("Жанр_ID")).FirstOrDefault());
            LiViSearhResult.Columns.Remove(LiViSearhResult.Columns.Where(s => s.Header.Equals("Испольнитель_ID")).FirstOrDefault());
            LiViSearhResult.Columns.Remove(LiViSearhResult.Columns.Where(s => s.Header.Equals("Издатель_ID")).FirstOrDefault());
            LiViSearhResult.Columns.Remove(LiViSearhResult.Columns.Where(s => s.Header.Equals("Покупатели")).FirstOrDefault());
            LiViSearhResult.Columns.Remove(LiViSearhResult.Columns.Where(s => s.Header.Equals("Продажи")).FirstOrDefault());
            LiViSearhResult.Columns[0].Visibility = Visibility.Hidden;
            LiViSearhResult.Columns[2].Header = "Год издания";
            LiViSearhResult.Columns[3].Header = "Количество треков";
            LiViSearhResult.Columns[7].Header = "Кем зарезервирован";
            LiViSearhResult.Columns[8].Header = "Жанр";
            LiViSearhResult.Columns[9].Header = "Издатель";
            LiViSearhResult.Columns[10].Header = "Испольнитель";
        }
        //private byte[] CreateCopy()
        //{
        //    System.Drawing.Image img = System.Drawing.Image.FromFile(@"C:\Users\artem.pak01\Downloads\Eminem_-_the_eminem_show.jpg");
        //    int maxWidth = 300, maxHeight = 300;
        //    //размеры выбраны произвольно
        //    double ratioX = (double)maxWidth /
        //    img.Width;
        //    double ratioY = (double)maxHeight /
        //    img.Height;
        //    double ratio = Math.Min(ratioX, ratioY);
        //    int newWidth = (int)(img.Width * ratio);
        //    int newHeight = (int)(img.Height * ratio);
        //    System.Drawing.Image mi = new Bitmap(newWidth, newHeight);
        //    //рисунок в памяти
        //    Graphics g = Graphics.FromImage(mi);
        //    g.DrawImage(img, 0, 0, newWidth, newHeight);
        //    MemoryStream ms = new MemoryStream();
        //    //поток для ввода|вывода байт из памяти
        //    mi.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    ms.Flush();//выносим в поток все данные
        //               //из буфера
        //    ms.Seek(0, SeekOrigin.Begin);
        //    BinaryReader br = new BinaryReader(ms);
        //    byte[] buf = br.ReadBytes((int)ms.Length);
        //    using (Магазин_пластинок_ магазин = new Магазин_пластинок_())
        //    {
        //        магазин.Configuration.LazyLoadingEnabled = false;
        //        var result = магазин.Пластинки.Where<Пластинки>(s => s.Испольнитель_ID == 7).FirstOrDefault();
        //        result.Обложка = buf;
        //        магазин.SaveChanges();
        //    }
        //    return buf;

        //}
    }
}
