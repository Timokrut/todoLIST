using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace todoLIST
{
    /// <summary>
    /// Логика взаимодействия для CompletedSqueezed.xaml
    /// </summary>
    public partial class CompletedSqueezed : Window
    {

        string filePathC = "uielementC.json";
        public CompletedSqueezed()
        {
            InitializeComponent();
        }


        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();


            main.LoadCanvasData(filePathC);


            main.Show();


            Close();



            
            
            //this.Visibility = Visibility.Visible;

        }
    }
}
