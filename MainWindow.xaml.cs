﻿using System;
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
using System.Windows.Markup;
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
        Button Submit;
        
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

                //ChekboxPanel.Margin = new Thickness(0, 0, 500, 0);
                flagFullScrean = true;
            }

            else
            {
                this.WindowState = WindowState.Normal;

                //ChekboxPanel.Margin = new Thickness(0, 0, 200, 0);
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

            textInCheckBox.Text += "Wtrite Somesthing";

            textInCheckBox.KeyDown += textInCheckBox_KeyDown;

            Submit = new Button();
            ControlTemplate template1 = new ControlTemplate(typeof(Button));
            FrameworkElementFactory border1 = new FrameworkElementFactory(typeof(Border));
            border1.SetValue(Border.CornerRadiusProperty, new CornerRadius(3));
            border1.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            border1.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter))
            {
                Name = "PART_ContentHost"
            });
            template1.VisualTree = border1;

            Submit.Template = template1;

            Submit.Click += Submit_CLick;
            TextPanel.Children.Add(Submit);

            Submit.Content = "  Submit";

            TextPanel.Children.Add(textInCheckBox);

            // Start Customization
            Canvas.SetBottom(Submit, 0);
            Canvas.SetLeft(Submit, 20);

            Submit.Height = 20;
            Submit.Width = 50;

            Submit.Background = Brushes.Green;
            Submit.Foreground = Brushes.White;
           


            Canvas.SetLeft(textInCheckBox, 174);
            Canvas.SetTop(textInCheckBox, 0);
            textInCheckBox.Height = 67;
            textInCheckBox.Width = 428;

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

        private void Is_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.IsChecked == true )
            {
                Thread.Sleep(100);
                ChekboxPanel.Children.Remove(checkBox);
            }
        }

        private void textInCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && TextPanel.Focusable == true)
                endText();
        }

        private void Submit_CLick(object sender, RoutedEventArgs e)
            => endText();

        private void endText()
        {

            checkBox.Content = textInCheckBox.Text;
            TextPanel.Children.Remove(textInCheckBox);

            checkBox.Checked += Is_Checked;

            ChekboxPanel.Children.Add(checkBox);

            TextPanel.Children.Remove(Submit);
            NewTask.Visibility = Visibility.Visible;
        }

        private void ChekboxPanel_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
        //
        //End of check boxes and their logic
        //

    }
}
