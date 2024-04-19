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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using todoLIST.Properties;
using System.IO;
using Newtonsoft.Json;
using System.Reflection;

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
        Rectangle lineUnderCheckBox;
        Button Submit;
        TextBlock textBlock;

        bool flagFullScrean = false;

        // Main
        public MainWindow()
        {
            InitializeComponent();

            string filePath = "uielements.json";

            LoadData(filePath);
            

            this.Closed += Window_Closed;

        }


        private void Window_Closed(object sender, EventArgs e)
        {
            string filePath = "uielements.json";
            SaveData(filePath);
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
            lineUnderCheckBox = new Rectangle();

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

            lineUnderCheckBox.Height = 1;
            lineUnderCheckBox.Width = 610;
            lineUnderCheckBox.Stroke = Brushes.Gray;

            

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
            textInCheckBox.FontSize = 14;    
            textInCheckBox.Foreground = Brushes.White;   
            textInCheckBox.FontFamily = new FontFamily("Aerial");
            textInCheckBox.BorderThickness = new Thickness(0, 0, 0, 0);

            customizeCheckBox();
            // End Customization
        }

        private void customizeCheckBox()
        {
            Grid.SetRow(checkBox, 2);
            Grid.SetColumn(checkBox, 1);
            checkBox.Margin = new Thickness(5);
            checkBox.FontSize = 14;
            checkBox.Foreground = Brushes.White;
        }

        private async void Is_CheckedAsync(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox checkBox && checkBox.IsChecked == true )
            {
                textBlock = new TextBlock();
                textBlock.Text = (string)checkBox.Content;
                textBlock.TextDecorations = TextDecorations.Strikethrough;
                textBlock.Foreground = Brushes.Black;
                textBlock.FontSize = 15;
                checkBox.Content = textBlock;

                await Task.Delay(1000);
                int index = ChekboxPanel.Children.IndexOf(checkBox);
                DoubleAnimation widthAnimation = new DoubleAnimation
                {
                    From = checkBox.ActualWidth,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.Stop
                };

                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    From = checkBox.ActualHeight,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.Stop
                };

                DoubleAnimation opacityAnimation = new DoubleAnimation
                {
                    From = 1,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.Stop
                };

                Storyboard storyboard = new Storyboard();
                storyboard.Children.Add(widthAnimation);
                storyboard.Children.Add(heightAnimation);
                storyboard.Children.Add(opacityAnimation);

                Storyboard.SetTarget(widthAnimation, checkBox);
                Storyboard.SetTarget(heightAnimation, checkBox);
                Storyboard.SetTarget(opacityAnimation, checkBox);

                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                storyboard.Begin(checkBox);
                

                await Task.Delay(490);
                ChekboxPanel.Children.Remove(checkBox);

                ChekboxPanel.Children.RemoveAt(index);
            }
        }

        private void textInCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                var textBox = sender as TextBox;
                if (textBox != null)
                {
                    int caretPosition = textBox.CaretIndex;
                    textBox.Text = textBox.Text.Insert(caretPosition, "\n");
                    textBox.CaretIndex = caretPosition + 1;
                }
                e.Handled = true;
            }

            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.None)
                if (TextPanel.Focusable)
                    endText();
        }

        private void Submit_CLick(object sender, RoutedEventArgs e)
            => endText();

        private void endText()
        {
            
            checkBox.Content = textInCheckBox.Text;
            TextPanel.Children.Remove(textInCheckBox);

            checkBox.Checked += Is_CheckedAsync;

            ChekboxPanel.Children.Add(checkBox);

            ChekboxPanel.Children.Add(lineUnderCheckBox);

            TextPanel.Children.Remove(Submit);
            NewTask.Visibility = Visibility.Visible;
        }

        private void ChekboxPanel_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }
        //
        //End of check boxes and their logic
        //

        public void SaveData(string filePath)
        {
            List<UIElementData> elementsData = new List<UIElementData>();

            foreach (UIElement element in ChekboxPanel.Children)
            {
                if (element is CheckBox checkBox)
                {
                    elementsData.Add(new UIElementData
                    {
                        Text = checkBox.Content?.ToString(),
                        IsChecked = checkBox.IsChecked
                    });
                }
            }

            string jsonString = JsonConvert.SerializeObject(elementsData);
            File.WriteAllText(filePath, jsonString);
        }

        public void LoadData(string filePath)
        {
            if (!File.Exists(filePath))
                return;

           

            string jsonString = File.ReadAllText(filePath);
            List<UIElementData> elementsData = JsonConvert.DeserializeObject<List<UIElementData>>(jsonString);

            foreach (UIElementData data in elementsData)
                if (!string.IsNullOrEmpty(data.Text))
                    if (data.IsChecked.HasValue)
                    {
                        CheckBox checkBox = new CheckBox
                        {
                            Content = data.Text,
                            IsChecked = data.IsChecked.Value,
                            Margin = new Thickness(5),
                            FontSize = 14,
                            Foreground = Brushes.White
                        };
                        checkBox.Checked += Is_CheckedAsync;
                        ChekboxPanel.Children.Add(checkBox);
                        lineUnderCheckBox = new Rectangle
                        {
                            Height = 1,
                            Width = 610,
                            Stroke = Brushes.Gray
                        };
                        ChekboxPanel.Children.Add(lineUnderCheckBox);
                    }
        }
    }
}
