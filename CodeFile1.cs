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
       /// <summary>
       /// Поиск всех пластинок для продажи
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
             
        private void SearchSinglToSale(object sender, RoutedEventArgs e)
        {
            lbSearchResaltToSale.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result = магазин.Пластинки.Where<Пластинки>(s => (s.Название.Contains(tBoxSearchContent.Text) && s.Количество > 0));
            foreach (var pl in result)
            {
                lbSearchResaltToSale.Items.Add(pl.Название);
            }
        }

        /// <summary>
        /// Поиск выбранной пластинки для продажи и отображение информации по ней, предпродажная подготовка
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void ChooseSingleToSale(object sender, SelectionChangedEventArgs e)
        {
            if (lbSearchResaltToSale.SelectedIndex == -1) return;
            if (SalingSingle == null) SalingSingle = new Пластинки();
            магазин.SaveChanges();
            магазин.Configuration.LazyLoadingEnabled = false;
            SalingSingle = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(lbSearchResaltToSale.SelectedItem.ToString())).FirstOrDefault();
            магазин.Entry<Пластинки>(SalingSingle).Reference(s => s.Жанры).Load();
            магазин.Entry<Пластинки>(SalingSingle).Reference(s => s.Издатели).Load();
            магазин.Entry<Пластинки>(SalingSingle).Reference(s => s.Исполнители).Load();
            SingleToSale.DataContext = SalingSingle;
            CustomersList.Items.Clear();
            CustomersList.Items.Add("--клиент не зарегистрирован--");
            CustomersList.SelectedIndex = 0;
            foreach (var customer in магазин.Покупатели) CustomersList.Items.Add(customer.Фамилия + " " + customer.Имя);
            Обложка_.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + SalingSingle.Обложка));
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
            if (t < 0)
            {
                StatusBar.Text = "Укажите корректное количество для продажи";
                return;
            }
            else
            {
                //прописать логику обновления таблиц при продаже товара
                StatusBar.Text = "Успешно";
                SearchSinglToSale(new object(), new RoutedEventArgs());
                магазин.SaveChanges();
            }

        }
    }
}