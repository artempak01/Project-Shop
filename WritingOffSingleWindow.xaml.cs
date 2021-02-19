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
    /// Логика взаимодействия для WritingOffSingleWindow.xaml
    /// </summary>
    public partial class WritingOffSingleWindow : Window
    {
        Пластинки EditingSingle = new Пластинки();
        public WritingOffSingleWindow(Пластинки Single)
        {
            InitializeComponent();
            EditingSingle = Single;
            EditingSingleGrid.DataContext = EditingSingle;
            CurientCover.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + EditingSingle.Обложка));
        }

        private void WriteOffSingle(object sender, RoutedEventArgs e)
        {
            int i = 0;
            int j = 0;
            Int32.TryParse(tbCount.Text, out i);
            Int32.TryParse(tbAmount.Text, out j);
            if (i - j < 0)
            {
                StatusBar.Text = "Укажите корректное количество";
            }
            else
            {
                using (Магазин_пластинок_ магазин = new Магазин_пластинок_())
                {
                    EditingSingle.Количество -= j;
                    магазин.SaveChanges();
                }
            }
        }
    }
}