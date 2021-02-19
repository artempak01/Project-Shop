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
    /// Логика взаимодействия для DeleteSingleWindow.xaml
    /// </summary>
    public partial class DeleteSingleWindow : Window
    {
        Пластинки EditingSingle = new Пластинки();
        public DeleteSingleWindow(Пластинки Single)
        {
            InitializeComponent();
            EditingSingle = Single;
            EditingSingleGrid.DataContext = EditingSingle;
            CurientCover.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + "\\" + EditingSingle.Обложка));
        }
    }
}
