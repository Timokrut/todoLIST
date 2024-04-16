using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace todoLIST
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        // Variables
        TextBox textInCheckBox;
        CheckBox checkBox;
        bool flagFullScrean = false;

        // Main
        public MainWindow()
        {
            InitializeComponent();
        }

        //
        //Start of standart button
        //
        private void CloseButtonClick(object sender, RoutedEventArgs e)
            => Close();

        private void HideButtonClick(object sender, RoutedEventArgs e)
            => this.WindowState = WindowState.Minimized;

        private void FullScreanButtonClick(object sender, RoutedEventArgs e)
        {
            if (flagFullScrean == false)
            {
                this.WindowState = WindowState.Maximized;
                flagFullScrean = true;
            }

            else
            {
                this.WindowState = WindowState.Normal;
                flagFullScrean = false;
            }
        }
        //
        //End of standart button
        //




        //
        //Start of check boxes and their logic
        //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // creating entities
            checkBox = new CheckBox();
            textInCheckBox = new TextBox();

            // enter TextBox in Check Box
            checkBox.Content = textInCheckBox;

            // Handler for delition
            checkBox.Checked += Is_Checked;

            // Handler for writing tex
            textInCheckBox.KeyDown += textInCheckBox_KeyDown;

            // For Creating new CheckBox
            ChekboxPanel.Children.Add(checkBox);

            // Start Customization
            textInCheckBox.Background = new SolidColorBrush(Color.FromArgb(255, 38, 38, 38));
            
            checkBox.Margin = new Thickness(5);
            
            textInCheckBox.FontSize = 14;
            checkBox.FontSize = 14;
            
            textInCheckBox.Foreground = Brushes.White;
            checkBox.Foreground = Brushes.White;

            textInCheckBox.FontFamily = new FontFamily("Lobster");
            textInCheckBox.FontFamily = new FontFamily("Aerial");

            textInCheckBox.BorderThickness = new Thickness(0, 0, 0, 0);
            // End Customization
        }

        private void textInCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                checkBox.Content = textInCheckBox.Text;
        }

        private void Is_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.IsChecked == true)
            {
                Thread.Sleep(100);
                ChekboxPanel.Children.Remove(checkBox);
            }
        }
        //
        //End of check boxes and their logic
        //
    }
}
