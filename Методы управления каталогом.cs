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
       
        private void Found_Singls(object sender, RoutedEventArgs e)
        {

            lbSearchSinglesResult.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(tBoxSearchCatalog.Text));
            foreach (var pl in result)
            {
                lbSearchSinglesResult.Items.Add(pl.Название);
            }
            магазин.SaveChanges();
            lbSearchArtistResult.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result_1 = магазин.Исполнители.Where<Исполнители>(s => s.Имя.Contains(tBoxSearchCatalog.Text));
            foreach (var Art in result_1)
            {
                lbSearchArtistResult.Items.Add(Art.Имя);
            }
            lbSearchGenreResult.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result_2 = магазин.Жанры.Where<Жанры>(s => s.Название.Contains(tBoxSearchCatalog.Text));
            foreach (var Art in result_2)
            {
                lbSearchGenreResult.Items.Add(Art.Название);
            }

        }

        private void Key_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (tBoxSearchCatalog.Text.Length >= 0) Found_Singls(sender, new RoutedEventArgs());
                else MessageBox.Show("Введите текст для поиска", "Внимание", MessageBoxButton.OK);
            }
        }

        private void ChooseSingle(object sender, SelectionChangedEventArgs e)
        {
            if (lbSearchSinglesResult.SelectedIndex == -1) return;
            магазин.SaveChanges();
            магазин.Configuration.LazyLoadingEnabled = false;
            CurientSingle = магазин.Пластинки.Where<Пластинки>(s => s.Название.Contains(lbSearchSinglesResult.SelectedItem.ToString())).FirstOrDefault();
            магазин.Entry<Пластинки>(CurientSingle).Reference(s => s.Жанры).Load();
            магазин.Entry<Пластинки>(CurientSingle).Reference(s => s.Издатели).Load();
            магазин.Entry<Пластинки>(CurientSingle).Reference(s => s.Исполнители).Load();
            CurientSingleGrid.DataContext = CurientSingle;

            CurientCover.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + CurientSingle.Обложка));
        }

        private void ChooseSingles(object sender, SelectionChangedEventArgs e)
        {
            магазин.SaveChanges();
            if (lbSearchArtistResult.SelectedIndex == -1) return;
            lbSearchSinglesResult.Items.Clear();
            lbSearchGenreResult.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result = магазин.Пластинки.Where<Пластинки>(s => s.Исполнители.Имя.Contains(lbSearchArtistResult.SelectedItem.ToString()));
            foreach (var pl in result)
            {
                lbSearchSinglesResult.Items.Add(pl.Название);
                lbSearchGenreResult.Items.Add(pl.Жанры.Название);
            }

        }
        
        private void FindAllContentByGanre(object sender, SelectionChangedEventArgs e)
        {
            магазин.SaveChanges();
            if (lbSearchGenreResult.SelectedIndex == -1) return;
            lbSearchSinglesResult.Items.Clear();
            lbSearchArtistResult.Items.Clear();
            магазин.Configuration.LazyLoadingEnabled = false;
            var result = магазин.Пластинки.Where<Пластинки>(s => s.Жанры.Название.Contains(lbSearchGenreResult.SelectedItem.ToString()));
            foreach (var pl in result)
            {
                lbSearchSinglesResult.Items.Add(pl.Название);
            }
            lbSearchArtistResult.Items.Clear();
            foreach (var pl in result)
            {
                var result_1 = магазин.Исполнители.Where<Исполнители>(s => s.Имя.Contains(pl.Исполнители.Имя)).FirstOrDefault<Исполнители>();
                lbSearchArtistResult.Items.Add(result_1.Имя);
                
            }
            
        }

    }

}
