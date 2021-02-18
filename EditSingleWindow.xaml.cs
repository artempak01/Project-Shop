using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Музыкальный_магазин_пластинок
{
    /// <summary>
    /// Логика взаимодействия для EditSingleWindow.xaml
    /// </summary>
    public partial class EditSingleWindow : Window
    {
        Пластинки EditingSingle = new Пластинки();
        Магазин_пластинок_ магазин = new Магазин_пластинок_();

        public EditSingleWindow(Пластинки edit)
        {
            EditingSingle = edit;
            InitializeComponent();
            EditingSingleGrid.DataContext = EditingSingle;
            CurientCover.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + EditingSingle.Обложка));
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
                cbArtists.SelectedItem = EditingSingle.Исполнители.Имя;
                cbGanres.SelectedItem = EditingSingle.Жанры.Название;
                cbPublichers.SelectedItem = EditingSingle.Издатели.Название;
            }
            catch(Exception ex)
            {
                StatusBar.Text = ex.Message;
            }
        }

        private void EditArtist(object sender, SelectionChangedEventArgs e)
        {
            
            var Artist_ID = (from a in магазин.Исполнители
                            where a.Имя == cbArtists.Text
                            select a.Id).FirstOrDefault();
            if(Artist_ID == 0)
            {
                магазин.Исполнители.Add(new Исполнители { Имя = cbArtists.Text });
            }
            else EditingSingle.Испольнитель_ID = Artist_ID;
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
                    EditingSingle.Испольнитель_ID = (from a in магазин.Исполнители
                                     where a.Имя == cbArtists.Text
                                     select a.Id).FirstOrDefault();
                }
                else
                {
                    EditingSingle.Испольнитель_ID = Artist_ID;

                }
                StatusBar.Text = String.Empty;
            }
            catch(Exception ex)
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
                    EditingSingle.Жанр_ID = (from a in магазин.Жанры
                                             where a.Название == cbGanres.Text
                                             select a.Id).FirstOrDefault();
                }
                else
                {
                    EditingSingle.Жанр_ID = Ganre_ID;

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
                    магазин.Издатели.Add(new Издатели {  Название = cbPublichers.Text });
                    магазин.SaveChanges();
                    EditingSingle.Издатель_ID = (from a in магазин.Издатели
                                                 where a.Название == cbPublichers.Text
                                             select a.Id).FirstOrDefault();
                }
                else
                {
                    EditingSingle.Издатель_ID = Publisher_ID;

                }
                StatusBar.Text = String.Empty;
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;

            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                магазин.SaveChanges();
                StatusBar.Text = "Пластинка изменена";
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;

            }
        }

        private void CloseEditWindow(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
