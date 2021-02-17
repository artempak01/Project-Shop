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
            var d = new DateTime(2020,12,1);
            StartDate.SelectedDate = d;
            lbReportTypes.SelectedIndex = 1;
            EndDate.SelectedDate = DateTime.Now;

        }

        private void ClearStatusBar(object sender, RoutedEventArgs e)
        {
            StatusBar.Text = String.Empty;
        }

        
    }
}
