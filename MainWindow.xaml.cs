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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

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
        DockPanel dockPanelAboveNewTask;

        bool wrtieNotEnd = true;
        
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

        //private void AnimateButton(Button button)
        //{
        //    DoubleAnimation animation = new DoubleAnimation();
        //    animation.From = button.Margin.Top; 
        //    animation.To = 200; 
        //    animation.Duration = TimeSpan.FromSeconds(1); 

        //    button.BeginAnimation(Button.MarginProperty, animation);
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NewTask.Visibility = Visibility.Collapsed;
            

            // creating entities
            checkBox = new CheckBox();
            textInCheckBox = new TextBox();

            ControlTemplate template = new ControlTemplate(typeof(TextBox));
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(5));
            border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Control.BorderThicknessProperty));
            border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Control.BorderBrushProperty));
            border.AppendChild(new FrameworkElementFactory(typeof(ScrollViewer))
            {
                Name = "PART_ContentHost"
            });


            template.VisualTree = border;

            textInCheckBox.Template = template;


            //dockPanelAboveNewTask = new DockPanel();
            //dockPanelAboveNewTask.LastChildFill = true;

            textInCheckBox.Text += "Wtrite Somesthing";

            textInCheckBox.KeyDown += textInCheckBox_KeyDown;

            //dockPanelAboveNewTask.Children.Add(textInCheckBox);

            //DockPanel.SetDock(textInCheckBox, Dock.Left);

            

            TextPanel.Children.Add(textInCheckBox);
            Canvas.SetLeft(textInCheckBox, 174);
            Canvas.SetTop(textInCheckBox, 0);
            textInCheckBox.Height = 67;
            textInCheckBox.Width = 428;

            // enter TextBox in Check Box
            //checkBox.Content = test;

            // Handler for delition
            checkBox.Checked += Is_Checked;

            // Handler for writing tex
            //textInCheckBox.KeyDown += textInCheckBox_KeyDown;

            // For Creating new CheckBox
            ChekboxPanel.Children.Add(checkBox);
            
            //ChekboxPanel.VerticalAlignment = VerticalAlignment.Top;

            //ChekboxPanel.Children.Add(dockPanelAboveNewTask);

            Button temp = new Button();



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
            if (e.Key == Key.Enter && TextPanel.Focusable == true)
            {
                wrtieNotEnd = false;


                checkBox.Content = textInCheckBox.Text;
                TextPanel.Children.Remove(textInCheckBox);

                NewTask.Visibility = Visibility.Visible;

                //Button clickedButton = NewTask;

                // Применяем анимацию к этой кнопке
                //AnimateButton(clickedButton);
            }
        }

        private void Is_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.IsChecked == true && wrtieNotEnd == false)
            {
                Thread.Sleep(100);
                ChekboxPanel.Children.Remove(checkBox);
                wrtieNotEnd = true;
            }
        }
        //
        //End of check boxes and their logic
        //
    }
}
