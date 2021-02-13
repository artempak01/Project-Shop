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
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        string Login;
        string Password;
        public bool Aborted = false;
        public Authorization()
        {
            InitializeComponent();
        }

        private void Logon(object sender, RoutedEventArgs e)
        {
            Login = LoginBox.Text;
            Password = PassworBox.Password;
            while (true)
            {
                if (Login == String.Empty) 
                {
                    MessageBox.Show("Не указан логин!");
                    break;
                }
                using (Магазин_пластинок_ магазин = new Магазин_пластинок_())
                {
                    var User = магазин.Пользователи.FirstOrDefault<Пользователи>(s => s.Логин == Login);
                    if (User != null)
                    {
                        if (User.Пароль == Password)
                        {
                            MainWindow.Access = "true";
                            Close();
                            break;
                        }
                        else
                        {
                            Login = String.Empty;
                            Password = String.Empty;
                            MessageBox.Show("Пара логин/пароль некорректная!");
                            LoginBox.Text = String.Empty;
                            PassworBox.Password = String.Empty;
                            break;
                        }
                    }
                    else
                    {
                        Login = String.Empty;
                        Password = String.Empty;
                        MessageBox.Show("Пара логин/пароль некорректная!");
                        LoginBox.Text = String.Empty;
                        PassworBox.Password = String.Empty;
                        break;
                    }
                }

            }
        }

        private void Window_Close(object sender, EventArgs e)
        {
            Aborted = true;
        }
    }
}
