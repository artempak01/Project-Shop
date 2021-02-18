using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Музыкальный_магазин_пластинок
{
    /// <summary>
    /// Логика взаимодействия для AddSingleWindow.xaml
    /// </summary>
    public partial class AddSingleWindow : Window
    {
        Магазин_пластинок_ магазин = new Магазин_пластинок_();
        Пластинки newSingle = new Пластинки();
        public AddSingleWindow()
        {
            InitializeComponent();
            try
            {
                var Ganres = from a in магазин.Жанры
                             select a.Название;
                cbGanres.ItemsSource = Ganres.ToList();
                var Artists = from a in магазин.Исполнители
                              select a.Имя;
                cbArtists.ItemsSource = Artists.ToList();
                var Publishers = from a in магазин.Издатели
                                 select a.Название;
                cbPublichers.ItemsSource = Publishers.ToList();
                EditingSingleGrid.DataContext = newSingle;
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;
            }
            ///для теста
        }

        private void EditArtist(object sender, RoutedEventArgs e)
        {
            try
            {
                var Artist_ID = (from a in магазин.Исполнители
                                 where a.Имя == cbArtists.Text
                                 select a.Id).FirstOrDefault();
                if (Artist_ID == 0)
                {
                    магазин.Исполнители.Add(new Исполнители { Имя = cbArtists.Text });
                    магазин.SaveChanges();
                    newSingle.Испольнитель_ID = (from a in магазин.Исполнители
                                                     where a.Имя == cbArtists.Text
                                                     select a.Id).FirstOrDefault();
                }
                else
                {
                    newSingle.Испольнитель_ID = Artist_ID;

                }
                StatusBar.Text = String.Empty;
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;
            }
        }

        private void EditGanre(object sender, RoutedEventArgs e)
        {
            try
            {
                var Ganre_ID = (from a in магазин.Жанры
                                where a.Название == cbGanres.Text
                                select a.Id).FirstOrDefault();
                if (Ganre_ID == 0)
                {
                    магазин.Жанры.Add(new Жанры { Название = cbGanres.Text });
                    магазин.SaveChanges();
                    newSingle.Жанр_ID = (from a in магазин.Жанры
                                             where a.Название == cbGanres.Text
                                             select a.Id).FirstOrDefault();
                }
                else
                {
                    newSingle.Жанр_ID = Ganre_ID;

                }
                StatusBar.Text = String.Empty;
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;

            }
        }

        private void EditPublisher(object sender, RoutedEventArgs e)
        {
            try
            {
                var Publisher_ID = (from a in магазин.Издатели
                                    where a.Название == cbPublichers.Text
                                    select a.Id).FirstOrDefault();
                if (Publisher_ID == 0)
                {
                    магазин.Издатели.Add(new Издатели { Название = cbPublichers.Text });
                    магазин.SaveChanges();
                    newSingle.Издатель_ID = (from a in магазин.Издатели
                                                 where a.Название == cbPublichers.Text
                                                 select a.Id).FirstOrDefault();
                }
                else
                {
                    newSingle.Издатель_ID = Publisher_ID;

                }
                StatusBar.Text = String.Empty;
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;

            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                EditingSingleGrid.DataContext = newSingle;
                магазин.Пластинки.Add(newSingle);
                магазин.SaveChanges();
                StatusBar.Text = "Пластинка сохранена";
            }
            catch(Exception ex)
            {
                StatusBar.Text = ex.Message;
            }
        }

        private void AddCover(object sender, MouseButtonEventArgs e)
        {
            var open = new OpenFileDialog();
            open.ShowDialog();
            newSingle.Обложка = open.SafeFileName.ToString();
        }
    }
}
