
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

    public partial class MainWindow : Window
    {

        private void CreateReport(object sender, RoutedEventArgs e)
        {
            try
            {
                var Sales = магазин.Продажи.Where<Продажи>(S => S.Дата_продажи.Value.Year > StartDate.SelectedDate.Value.Year && S.Дата_продажи.Value.Year <= EndDate.SelectedDate.Value.Year);
                switch (lbReportTypes.Text)
                {
                    case "Новинки":
                        var Report = магазин.Пластинки.Where<Пластинки>(s => s.Год_издания > StartDate.SelectedDate.Value.Year && s.Год_издания < EndDate.SelectedDate.Value.Year);
                        ReportView.ItemsSource = Report.ToList<Пластинки>();
                        EditColumns(ReportView);
                        break;
                    case "Самые продаваемые пластинки":
                        var sale = from Продажи in магазин.Set<Продажи>()
                                   join Пластинки in магазин.Set<Пластинки>()
                                   on Продажи.ID_пластинки equals Пластинки.Id
                                   join Исполнители in магазин.Set<Исполнители>()
                                   on Пластинки.Испольнитель_ID equals Исполнители.Id
                                   select new TopSinglesReport { Название = Пластинки.Название, Исполнитель = Исполнители.Имя, Всего_продано = (int)Продажи.Количество };
                        var s1 = from p in sale
                                 group p by p.Название into g
                                 select g;
                        List<TopSinglesReport> GroupingTopReport = new List<TopSinglesReport>();
                        foreach (var Group in s1)
                        {

                            foreach (var grouping in Group)
                                if (GroupingTopReport.Exists(ReportItem => ReportItem.Название == grouping.Название))
                                {
                                    GroupingTopReport.Find(Item => Item.Название == grouping.Название).Всего_продано += grouping.Всего_продано;
                                }
                                else
                                    GroupingTopReport.Add(grouping);
                        }
                        if (GroupingTopReport.Count > 10)
                        {
                            GroupingTopReport.RemoveRange(9, GroupingTopReport.Count - 10);
                        }
                        GroupingTopReport.Sort(SortReport);
                        ReportView.ItemsSource = GroupingTopReport;
                        ReportView.Columns[2].Header = "Всего продано";
                        break;
                    case "Самые популярные авторы":

                        var sales = from Продажи in магазин.Set<Продажи>()
                                    join Пластинки in магазин.Set<Пластинки>()
                                    on Продажи.ID_пластинки equals Пластинки.Id
                                    join Исполнители in магазин.Set<Исполнители>()
                                    on Пластинки.Испольнитель_ID equals Исполнители.Id
                                    select new TopSinglesReport { Название = Пластинки.Название, Исполнитель = Исполнители.Имя, Всего_продано = (int)Продажи.Количество };
                        var s2 = from p in sales
                                 group p by p.Исполнитель into g
                                 select g;
                        List<TopSinglesReport> GroupingTopReport_2 = new List<TopSinglesReport>();
                        foreach (var Group in s2)
                        {

                            foreach (var grouping in Group)
                                if (GroupingTopReport_2.Exists(ReportItem => ReportItem.Название == grouping.Название))
                                {
                                    GroupingTopReport_2.Find(Item => Item.Название == grouping.Название).Всего_продано += grouping.Всего_продано;
                                }
                                else
                                    GroupingTopReport_2.Add(grouping);
                        }
                        if (GroupingTopReport_2.Count > 10)
                        {
                            GroupingTopReport_2.RemoveRange(9, GroupingTopReport_2.Count - 10);
                        }
                        GroupingTopReport_2.Sort(SortReport);
                        ReportView.ItemsSource = GroupingTopReport_2;
                        ReportView.Columns[2].Header = "Всего продано";
                        ReportView.Columns.Remove(ReportView.Columns.Where(s => s.Header.Equals("Название")).FirstOrDefault());
                        break;
                    case "Самые популярные жанры":
                        var sales_1 = from Продажи in магазин.Set<Продажи>()
                                      join Пластинки in магазин.Set<Пластинки>()
                                      on Продажи.ID_пластинки equals Пластинки.Id
                                      join Жанры in магазин.Set<Жанры>()
                                      on Пластинки.Жанр_ID equals Жанры.Id
                                      join Исполнители in магазин.Set<Исполнители>()
                                     on Пластинки.Испольнитель_ID equals Исполнители.Id
                                      select new TopSinglesReport { Название = Жанры.Название, Исполнитель = Исполнители.Имя, Всего_продано = (int)Продажи.Количество };
                        var s3 = from p in sales_1
                                 group p by p.Название into g
                                 select g;
                        List<TopSinglesReport> GroupingTopReport_3 = new List<TopSinglesReport>();
                        foreach (var Group in s3)
                        {

                            foreach (var grouping in Group)
                                if (GroupingTopReport_3.Exists(ReportItem => ReportItem.Название == grouping.Название))
                                {
                                    GroupingTopReport_3.Find(Item => Item.Название == grouping.Название).Всего_продано += grouping.Всего_продано;
                                }
                                else
                                    GroupingTopReport_3.Add(grouping);
                        }
                        if (GroupingTopReport_3.Count > 10)
                        {
                            GroupingTopReport_3.RemoveRange(9, GroupingTopReport_3.Count - 10);
                        }
                        GroupingTopReport_3.Sort(SortReport);
                        ReportView.ItemsSource = GroupingTopReport_3;
                        ReportView.Columns[2].Header = "Всего продано";
                        ReportView.Columns[0].Header = "Жанр";
                        ReportView.Columns.Remove(ReportView.Columns.Where(s => s.Header.Equals("Исполнитель")).FirstOrDefault());
                        break;
                }
            }
            catch(Exception ex)
            {
                StatusBar.Text = "Ошибка" + ex.Message;
            }
        }

        private void EditColumns(DataGrid report)
        {
            report.Columns[0].Visibility = Visibility.Hidden;
            report.Columns.Where(s => s.Header.Equals("Количество_треков")).FirstOrDefault().Header = "Количество треков";
            report.Columns.Where(s => s.Header.Equals("Жанры")).FirstOrDefault().Header = "Жанр";
            report.Columns.Where(s => s.Header.Equals("Издатели")).FirstOrDefault().Header = "Издатель";
            report.Columns.Where(s => s.Header.Equals("Исполнители")).FirstOrDefault().Header = "Исполнитель";
            report.Columns.Where(s => s.Header.Equals("Год_издания")).FirstOrDefault().Header = "Год издания";
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Id")).FirstOrDefault());
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Себестоимость")).FirstOrDefault());
            //report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Скидка")).FirstOrDefault());
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("ID_зарезервировавшего")).FirstOrDefault());
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Испольнитель_ID")).FirstOrDefault());
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Издатель_ID")).FirstOrDefault());
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Жанр_ID")).FirstOrDefault());
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Обложка")).FirstOrDefault());
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Количество_зарезервировано")).FirstOrDefault());
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Покупатели")).FirstOrDefault());
            report.Columns.Remove(report.Columns.Where(s => s.Header.Equals("Продажи")).FirstOrDefault());

        }

        int SortReport(TopSinglesReport FirstReportItem, TopSinglesReport SecondReportItem)
        {

            if (FirstReportItem.Всего_продано > SecondReportItem.Всего_продано) return -1;
            else if (FirstReportItem.Всего_продано < SecondReportItem.Всего_продано) return 1;
            else return 0;

        }
    }
}