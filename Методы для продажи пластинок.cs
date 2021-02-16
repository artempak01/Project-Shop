using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Музыкальный_магазин_пластинок
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Поиск всех пластинок для продажи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SearchSinglToSale(object sender, RoutedEventArgs e)
        {
            lbSearchResultToSale.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result = магазин.Пластинки.Where<Пластинки>(s => (s.Название.Contains(tBoxSearchToSale.Text) && s.Количество > 0));
            foreach (var pl in result)
            {
                lbSearchResultToSale.Items.Add(pl.Название);
            }
        }

        /// <summary>
        /// Поиск выбранной пластинки для продажи и отображение информации по ней, предпродажная подготовка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ChooseSingleToSale(object sender, SelectionChangedEventArgs e)
        {
            if (lbSearchResultToSale.SelectedIndex == -1) return;
            if (SalingSingle == null) SalingSingle = new Пластинки();
            магазин.SaveChanges();
            магазин.Configuration.LazyLoadingEnabled = false;
            SalingSingle = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(lbSearchResultToSale.SelectedItem.ToString())).FirstOrDefault();
            магазин.Entry<Пластинки>(SalingSingle).Reference(s => s.Жанры).Load();
            магазин.Entry<Пластинки>(SalingSingle).Reference(s => s.Издатели).Load();
            магазин.Entry<Пластинки>(SalingSingle).Reference(s => s.Исполнители).Load();
            SingleToSaleGrid.DataContext = SalingSingle;
            CustomersList.Items.Clear();
            CustomersList.Items.Add("--клиент не зарегистрирован--");
            CustomersList.SelectedIndex = 0;
            foreach (var customer in магазин.Покупатели) CustomersList.Items.Add(customer.Фамилия + " " + customer.Имя);
            SellingCover.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + SalingSingle.Обложка));
        }

        /// <summary>
        /// Продажа пластинки: списание в базе, занесение в таблицу продажи и Покупатели данных о сделке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SaleSingle(object sender, RoutedEventArgs e)
        {
            if (SalingSingle == null || amount.Text == String.Empty)
            {
                StatusBar.Text = "Выберите пластинку для продажи и укажите количество для продажи";
                return;
            }
            int q = 0;
            int g = 0;
            Int32.TryParse(amount.Text, out q);
            Int32.TryParse(inStock.Text, out g);
            int t = g - q;
            if (t < 0 && q !=0)
            {
                StatusBar.Text = "Укажите корректное количество для продажи";
                return;
            }
            else
            {
                int sellingAmount = 0;
                Int32.TryParse(amount.Text, out sellingAmount);
                Продажи newSale = new Продажи();
                newSale.ID_пластинки = SalingSingle.Id;
                //newSale.ID_покупателя  = добавить как будет готова логика выбора покупателя 
                newSale.Дата_продажи = DateTime.Now;
                newSale.Цена = SalingSingle.Цена;
                newSale.Количество = sellingAmount;
                SalingSingle.Количество = SalingSingle.Количество - sellingAmount;
                магазин.Продажи.Add(newSale);
                магазин.SaveChanges();
                SalingSingle = new Пластинки();
                SingleToSaleGrid.DataContext = SalingSingle;
                SellingCover.Source = null;
                amount.Text = String.Empty;
                StatusBar.Text = "Успешно";
                tBoxSearchToSale.Text = String.Empty;
                lbSearchResultToSale.Items.Clear();
            }

        }
    }
}