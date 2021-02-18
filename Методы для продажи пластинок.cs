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

        private void SearchSingelToSale(object sender, RoutedEventArgs e)
        {
            lbSearchResultToSale.Items.Clear();
            CustomersList.Items.Clear();
            CustomersList.Items.Add("--клиент не зарегистрирован--");
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
            CustomersList.SelectedIndex = 0;
            if (SalingSingle.Скидка != 0)
            {
                PriceWithSales.Text = (SalingSingle.Цена -(SalingSingle.Цена / 100 * SalingSingle.Скидка )).ToString();
            }
            else PriceWithSales.Text = (SalingSingle.Цена).ToString();
            SellingCover.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + SalingSingle.Обложка));
        }

        /// <summary>
        /// Продажа пластинки: списание в базе, занесение в таблицу продажи и Покупатели данных о сделке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void SaleSingle(object sender, RoutedEventArgs e)
        {
            ///Проверяем выбрана ли пластинка для продажи и указанно ли количество для продажи
            ///
            try {
                if (SalingSingle == null || amount.Text == String.Empty)
                {
                    StatusBar.Text = "Выберите пластинку для продажи и укажите количество для продажи";
                    return;
                }
                int SellingAmount = 0;
                int inStockAmount = 0;
                Int32.TryParse(amount.Text, out SellingAmount);
                Int32.TryParse(inStock.Text, out inStockAmount);
                int t = inStockAmount - SellingAmount;
                ///
                ///Проверяем валидность указанного для продажи количества
                ///
                if (t < 0 && SellingAmount != 0)
                {
                    StatusBar.Text = "Укажите корректное количество для продажи";
                    return;
                }
                else
                {
                    ///
                    int sellingAmount = 0;
                    int ReserveAmount = 0;
                    Int32.TryParse(amount.Text, out sellingAmount);
                    Покупатели Customer;
                    Продажи newSale = new Продажи();
                    ///
                    ///Проверяем валидность выбранного покупателя
                    ///
                    if (!CustomersList.SelectedItem.ToString().Equals("--клиент не зарегистрирован--"))
                    {
                        Customer = магазин.Покупатели.Where<Покупатели>(S => (S.Фамилия + " " + S.Имя).Equals(CustomersList.SelectedItem.ToString())).FirstOrDefault<Покупатели>();
                        ///Проверяем резерв у покупателя и снимаем его.
                        if (Customer.Id == SalingSingle.ID_зарезервировавшего)
                        {
                            newSale.ID_покупателя = Customer.Id;
                            ReserveAmount = (int)SalingSingle.Количество_зарезервировано;
                            SalingSingle.Количество_зарезервировано = 0;
                            SalingSingle.ID_зарезервировавшего = null;
                            SalingSingle.Количество += ReserveAmount;
                        }
                    }

                    ///
                    ///Вносим информацию о продаже в таблицы
                    ///               
                    newSale.ID_пластинки = SalingSingle.Id;
                    newSale.Дата_продажи = DateTime.Now;
                    decimal i = 0;
                    decimal.TryParse(PriceWithSales.Text, out i);
                    newSale.Цена = i;
                    newSale.Количество = sellingAmount;
                    SalingSingle.Количество = SalingSingle.Количество - sellingAmount;
                    магазин.Продажи.Add(newSale);
                    магазин.SaveChanges();

                    ///
                    ///Очищаем форму, сообщаем о рузультате.
                    ///
                    SalingSingle = new Пластинки();
                    SingleToSaleGrid.DataContext = SalingSingle;
                    SellingCover.Source = null;
                    amount.Text = String.Empty;
                    tBoxSearchToSale.Text = String.Empty;
                    lbSearchResultToSale.Items.Clear();
                    CustomersList.Items.Clear();
                    tBoxSearchCustomer.Text = String.Empty;
                    PriceWithSales.Text = String.Empty;
                    StatusBar.Text = "Успешно";
                }

            }
            catch(Exception ex)
            {
                StatusBar.Text = ex.Message;
            }
            }

        /// <summary>
        /// Поиск покупателя.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FoundCustomers(object sender, RoutedEventArgs e)
        {
            CustomersList.Items.Clear();
            CustomersList.Items.Add("--клиент не зарегистрирован--");
            CustomersList.SelectedIndex = 0;
            foreach (var customer in магазин.Покупатели.Where<Покупатели>(s => s.Имя.Contains(tBoxSearchCustomer.Text) || s.Фамилия.Contains(tBoxSearchCustomer.Text)))
            {
                CustomersList.Items.Add(customer.Фамилия + " " + customer.Имя);
            }
        }

        /// <summary>
        /// Резервирование товара для выбранного покупателя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReserveSingle(object sender, RoutedEventArgs e)
        {
            if (SalingSingle == null || amount.Text == String.Empty)
            {
                StatusBar.Text = "Выберите пластинку для резерва и укажите количество";
                return;
            }
            int q = 0;
            int g = 0;
            Int32.TryParse(amount.Text, out q);
            Int32.TryParse(inStock.Text, out g);
            int t = g - q;
            if (t < 0 && q != 0)
            {
                StatusBar.Text = "Укажите корректное количество для резерва";
                return;
            }
            else
            {
                int ReserveAmount = 0;
                Int32.TryParse(amount.Text, out ReserveAmount);
                if (!CustomersList.SelectedItem.ToString().Equals("--клиент не зарегистрирован--"))
                {
                    var Customer = магазин.Покупатели.Where<Покупатели>(S => (S.Фамилия + " " + S.Имя).Equals(CustomersList.SelectedItem.ToString())).FirstOrDefault<Покупатели>();
                    SalingSingle.ID_зарезервировавшего = Customer.Id;
                    SalingSingle.Количество_зарезервировано = ReserveAmount;
                    SalingSingle.Количество -= ReserveAmount;
                }
               
                магазин.SaveChanges();
                SalingSingle = new Пластинки();
                SingleToSaleGrid.DataContext = SalingSingle;
                SellingCover.Source = null;
                amount.Text = String.Empty;
                StatusBar.Text = "Пластинка успешно зарезервирована";
                tBoxSearchToSale.Text = String.Empty;
                lbSearchResultToSale.Items.Clear();
                CustomersList.Items.Clear();
                tBoxSearchCustomer.Text = String.Empty;
            }
        }
       

        /// <summary>
        /// Регистрация нового покупателя.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterCustomer(object sender, RoutedEventArgs e)
        {
            магазин.Покупатели.Add(new Покупатели { Имя = tBoxCustomerName.Text, Фамилия = tBoxCustomerSurname.Text, Сумма_покупок = 0 });
            магазин.SaveChanges();
            StatusBar.Text = "Покупатель зарегистрирован";
            tBoxCustomerName.Text = String.Empty;
            tBoxCustomerSurname.Text = String.Empty;
        }

        /// <summary>
        /// Очистка поля для ввода.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void СlearText(object sender, RoutedEventArgs e)
        {
            ((TextBox)sender).Text = String.Empty;
        }
    }
    }