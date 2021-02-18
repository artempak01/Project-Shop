using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

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
                this.DataContext = newSingle;
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
                if (cbArtists.Text == String.Empty) { return; }
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
                EditingSingleGrid.DataContext = newSingle;
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
                if (cbGanres.Text == String.Empty) return; 
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
                if (cbPublichers.Text == String.Empty) { return; }

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
            try { if (NewSingleName.Text == string.Empty || cbArtists.Text == string.Empty)
                {
                    StatusBar.Text = "Сначала нужно заполнить название пластинки и исполнителя";
                    return;
                }
                var open = new OpenFileDialog();
                open.ShowDialog();
                string puth = open.FileName;
                newSingle.Обложка = $"{cbArtists.Text} - {NewSingleName.Text}.jpg";
                open.Reset();
                File.Copy(puth, $"{cbArtists.Text} - {NewSingleName.Text}.jpg");
            }
            catch (Exception ex)
            {
                StatusBar.Text = ex.Message;
            }
        }
    }
}
