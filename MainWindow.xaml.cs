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
            var d = new DateTime(2020, 12, 1);
            StartDate.SelectedDate = d;
            lbReportTypes.SelectedIndex = 1;
            EndDate.SelectedDate = DateTime.Now;

        }

        //private void ClearStatusBar(object sender, RoutedEventArgs e)
        //{
        //    StatusBar.Text = String.Empty;
        //}

        private void Found_CategoryItems(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbCategoryTypes.SelectedIndex == -1)
                {
                    StatusBar.Text = "Выбирите категорию";
                    return;
                }
                cbCategoryList.ItemsSource = null;
                cbCategoryList.Items.Clear();
                switch (cbCategoryTypes.Text)
                {
                    case "Исполнители":
                        var result = магазин.Исполнители.Where<Исполнители>(s => s.Имя.Contains(tbSearchCategoryItems.Text));
                        List<string> ComboItems = new List<string>();
                        foreach (var res in result)
                        {
                            ComboItems.Add(res.Имя);
                        }
                        cbCategoryList.ItemsSource = ComboItems;
                        break;
                    case "Жанры":
                        var result_1 = магазин.Жанры.Where<Жанры>(s => s.Название.Contains(tbSearchCategoryItems.Text));
                        List<string> ComboItems_1 = new List<string>();
                        foreach (var res in result_1)
                        {
                            ComboItems_1.Add(res.Название);
                        }
                        cbCategoryList.ItemsSource = ComboItems_1;
                        break;
                }
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;

            }
        }

        private void Add_Sales(object sender, RoutedEventArgs e)
        {
            try {
                if (cbCategoryTypes.SelectedIndex == -1)
                {
                    StatusBar.Text = "Выбирите категорию";
                    return;
                }
                else if (cbCategoryList.SelectedIndex == -1)
                {
                    StatusBar.Text = "Выбирите наименование";
                    return;
                }
                else if (cbSalesAmount.SelectedIndex == -1)
                {
                    StatusBar.Text = "Выбирите размер скидки";
                    return;
                }
                switch (cbCategoryTypes.Text)
                {
                    case "Исполнители":
                        var result = магазин.Пластинки.Where<Пластинки>(s => s.Исполнители.Имя.Contains(cbCategoryList.Text));
                        foreach (var res in result)
                        {
                            int i = 0;
                            Int32.TryParse(cbSalesAmount.Text, out i);
                            res.Скидка = i;
                        }
                        магазин.SaveChanges();
                        break;
                    case "Жанры":
                        var result_1 = магазин.Пластинки.Where<Пластинки>(s => s.Жанры.Название.Contains(cbCategoryList.Text));
                        foreach (var res in result_1)
                        {
                            int i = 0;
                            Int32.TryParse(cbSalesAmount.Text, out i);
                            res.Скидка = i;
                        }
                        магазин.SaveChanges();
                        break;
                }
            }
            catch(Exception ex)
            {
                StatusBar.Text = ex.Message;
            }
            }

        private void SearchSinglesToAddSales(object sender, RoutedEventArgs e)
        {
            try
            {
                cbSearhingContentResult.ItemsSource = null;
                cbSearhingContentResult.Items.Clear();
                var result = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(tbSearchText.Text));
                List<string> ComboItems = new List<string>();
                foreach (var res in result)
                {
                    ComboItems.Add(res.Название);
                }
                cbSearhingContentResult.ItemsSource = ComboItems;
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;
            }
        }

        private void ChooseSingleToAddSale(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                var result = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(cbSearhingContentResult.SelectedItem.ToString()));
                gvSearchingResalt.ItemsSource = result.ToList();
                EditColumns(gvSearchingResalt);
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;
            }
        }

        private void EditSingle(object sender, RoutedEventArgs e)
        {
            if(CurientSingle.Название==null)
            {
                StatusBar.Text = "Выбирите пластинку для редактирования";
                return;
            }
            EditSingleWindow edit = new EditSingleWindow(CurientSingle);
            
            edit.ShowDialog();
        }
    }
}
