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

        private void ClearStatusBar(object sender, RoutedEventArgs e)
        {
            StatusBar.Text = String.Empty;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //     < ComboBoxItem Content = "Новинки" />

            //                     < ComboBoxItem Content = "Самые продаваемые пластинки" />

            //                      < ComboBoxItem Content = "Самые популярные авторы" />

            //                       < ComboBoxItem Content = "Самые популярные жанры" />

                    var Sales = магазин.Продажи.Where<Продажи>(S => S.Дата_продажи.Value.Year > StartDate.SelectedDate.Value.Year && S.Дата_продажи.Value.Year <= EndDate.SelectedDate.Value.Year);
            switch (lbReportTypes.Text)
            {
                case "Новинки":
                    var Report = магазин.Пластинки.Where<Пластинки>(s => s.Год_издания > StartDate.SelectedDate.Value.Year && s.Год_издания < EndDate.SelectedDate.Value.Year);
                    ReportView.ItemsSource= Report.ToList<Пластинки>();
                    break;
                case "Самые продаваемые пластинки":
                    List <Пластинки> Report_1 = new List<Пластинки> ();
                    foreach (Продажи pl in Sales)
                    {
                        Пластинки res = магазин.Пластинки.Where<Пластинки>(s => s.Id == pl.ID_пластинки).FirstOrDefault();
                        Report_1.Add(res);
                    }
                    //Report_1 = Report_1.GroupBy<Пластинки,int>((s=>return s.Id;), )
                    ReportView.ItemsSource = Report_1.ToList<Пластинки>();
                    break;
                case "Самые популярные авторы":
                    List<Исполнители> Report_4 = new List<Исполнители>();
                    List<Пластинки> Report_5 = new List<Пластинки>();
                    foreach (Продажи pl in Sales)
                    {
                        Пластинки res = магазин.Пластинки.Where<Пластинки>(s => s.Id == pl.ID_пластинки).FirstOrDefault();
                        Report_5.Add(res);
                    }
                    foreach (Пластинки pl in Report_5)
                    {
                        Исполнители res = магазин.Исполнители.Where<Исполнители>(s => s.Id == pl.Испольнитель_ID).FirstOrDefault();
                        Report_4.Add(res);
                    }
                    ReportView.ItemsSource = Report_4.ToList<Исполнители>();
                    break;
                case "Самые популярные жанры":
                    List<Жанры> TopGanres= new List<Жанры>();
                    List<Пластинки> Report_7 = new List<Пластинки>();
                    foreach (Продажи pl in Sales)
                    {
                        Пластинки res = магазин.Пластинки.Where<Пластинки>(s => s.Id == pl.ID_пластинки).FirstOrDefault();
                        Report_7.Add(res);
                    }
                    foreach (Пластинки pl in Report_7)
                    {
                        Жанры res = магазин.Жанры.Where<Жанры>(s => s.Id == pl.Жанр_ID).FirstOrDefault();
                        TopGanres.Add(res);
                    }
                    ReportView.ItemsSource = TopGanres.ToList<Жанры>();
                    break;
            }
        }
    }

}
