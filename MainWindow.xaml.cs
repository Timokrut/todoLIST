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
using System.Runtime.InteropServices;
using System.Windows.Media.TextFormatting;
using System.CodeDom;

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
        Canvas canvas = new Canvas();
        string filePathC = "uielementC.json";
        bool flagFullScrean = false;

        string str;

        // Main
        public MainWindow()
        {
            InitializeComponent();

            string filePath = "uielements.json";
            ControlTemplate template1 = new ControlTemplate(typeof(Button));
            FrameworkElementFactory border1 = new FrameworkElementFactory(typeof(Border));
            border1.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
            border1.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            border1.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter))
            {
                Name = "PART_ContentHost"
            });
            template1.VisualTree = border1;

            Completed.Content = " Completed";
            Completed.Template = template1;
            Completed.Background = new SolidColorBrush(Color.FromArgb(200, 123, 124, 129));
            LoadData(filePath);

            //canvas.Loaded += Canvas_Loaded;

            this.Closed += Window_Closed;
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            string filePathC = "uielementC.json";
            LoadCanvasData(filePathC); // Вызываем LoadCanvasData после загрузки Canvas
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            string filePath = "uielements.json";
            string filePathC = "uielementC.json";
            SaveData(filePath, filePathC);
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
            textInCheckBox.GotFocus += Focus;
            
            

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

            DoubleAnimation animationTextInCheckBox = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
                AutoReverse = false,
                FillBehavior = FillBehavior.HoldEnd
            };

            Storyboard myStoryboard = new Storyboard();
            myStoryboard.Children.Add(animationTextInCheckBox);
            Storyboard.SetTarget(animationTextInCheckBox, textInCheckBox);
            Storyboard.SetTargetProperty(animationTextInCheckBox, new PropertyPath(Rectangle.OpacityProperty));

            myStoryboard.Begin(textInCheckBox);

            DoubleAnimation animationButton = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
                AutoReverse = false,
                FillBehavior = FillBehavior.HoldEnd
            };

            Storyboard buttonStoryboard = new Storyboard();
            buttonStoryboard.Children.Add(animationButton);
            Storyboard.SetTarget(animationButton, Submit);
            Storyboard.SetTargetProperty(animationButton, new PropertyPath(UIElement.OpacityProperty));

            buttonStoryboard.Begin(Submit);


            lineUnderCheckBox.Height = 1;
            lineUnderCheckBox.Width = 610;
            lineUnderCheckBox.Stroke = Brushes.Gray;
            lineUnderCheckBox.HorizontalAlignment = HorizontalAlignment.Center;
            lineUnderCheckBox.VerticalAlignment = VerticalAlignment.Center;

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
            textInCheckBox.Foreground = new SolidColorBrush(Color.FromRgb(94, 94, 94));
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
                checkBox.IsEnabled = false;

                str = (string)checkBox.Content;


                newTextBlock = AddNewTextBlock(str);
                if (newTextBlock != null)
                    canvas.Children.Add(newTextBlock);

                autoSave(filePathC);

                textBlock = new TextBlock();
                textBlock.Text = (string)checkBox.Content;
                textBlock.TextDecorations = TextDecorations.Strikethrough;
                textBlock.Foreground = Brushes.Black;
                textBlock.FontSize = 15;
                checkBox.Content = textBlock;

                await Task.Delay(1000);
                int index = ChekboxPanel.Children.IndexOf(checkBox);
                ChekboxPanel.Children.RemoveAt(index+1);
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

                

                
            }
        }

        private void Focus(object sender, RoutedEventArgs e)
        {
            textInCheckBox.SpellCheck.IsEnabled = true;

            textInCheckBox.Text = "";

            textInCheckBox.Foreground = Brushes.White;
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
            if (textInCheckBox.Text.Length == 0)
            {
                textInCheckBox.Text = "Looks like you forget to write something smart. Try again!";
                return;
            }

            string str = textInCheckBox.Text.Trim();

            textInCheckBox.Text = str;

            checkBox.Content = textInCheckBox.Text;
            TextPanel.Children.Remove(textInCheckBox);

            checkBox.Checked += Is_CheckedAsync;

            ChekboxPanel.Children.Add(checkBox);

            ChekboxPanel.Children.Add(lineUnderCheckBox);

            TextPanel.Children.Remove(Submit);
            NewTask.Visibility = Visibility.Visible;
        }

        //
        //End of check boxes and their logic
        //



        public void SaveData(string filePath, string filePathC)
        {
                    List<UIElementData> elementsData = new List<UIElementData>();

                    foreach (UIElement element in ChekboxPanel.Children)
                    {
                        if (element is CheckBox checkBox)
                        {
                            elementsData.Add(new UIElementData
                            {
                                Text = checkBox.Content?.ToString(),
                                IsChecked = checkBox.IsChecked,
                            });
                        }
                    }

                    string jsonString = JsonConvert.SerializeObject(elementsData);
                    File.WriteAllText(filePath, jsonString);


                    List<UICanvasData> elementCData = new List<UICanvasData>();

                    foreach (UIElement element in canvas.Children)
                        if (element is TextBlock textBlock)
                        {
                            elementCData.Add(new UICanvasData
                            {
                                Text = textBlock.Text?.ToString(),
                            });
                        }
                    string jsonStringC = JsonConvert.SerializeObject(elementCData);
                    File.WriteAllText(filePathC, jsonStringC);
        }

        public void LoadData(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            string jsonString = File.ReadAllText(filePath);
            List<UIElementData> elementsData = JsonConvert.DeserializeObject<List<UIElementData>>(jsonString);

            foreach (UIElementData data in elementsData)
                if (!string.IsNullOrEmpty(data.Text))
                {
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
                            Stroke = Brushes.Gray,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        ChekboxPanel.Children.Add(lineUnderCheckBox);
                    }
                }
        }
        double Gap_Between_Completed_Tasks;

        private void LoadCanvasData(string filePathC)
        {
            if (File.Exists(filePathC))
            {
                string jsonString = File.ReadAllText(filePathC);
                List<UICanvasData> elementCData = JsonConvert.DeserializeObject<List<UICanvasData>>(jsonString);
                
                Gap_Between_Completed_Tasks = 0;
                foreach (UICanvasData data in elementCData)
                {
                    if (!string.IsNullOrEmpty(data.Text) && data.Text.Length > 0)
                    {
                        TextBlock textBlock = new TextBlock();
                        textBlock.VerticalAlignment = VerticalAlignment.Top;
                        textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                        textBlock.Text = data.Text;
                        textBlock.Foreground = Brushes.White;
                        textBlock.FontSize = 14;
                        textBlock.Height = 14;


                        textBlock.Loaded += (sender, e) =>
                        {
                            textBlock.Margin = new Thickness(0, Gap_Between_Completed_Tasks + 11, 0, 0);
                            Gap_Between_Completed_Tasks += textBlock.ActualHeight + 23;
                            add_gap_rect(Gap_Between_Completed_Tasks);
                        };
                        canvas.Children.Add(textBlock);
                    }
                }
            }
        }

        private void autoSave(string filePathC)
        {
            List<UICanvasData> elementCData = new List<UICanvasData>();

            foreach (UIElement element in canvas.Children)
                if (element is TextBlock textBlock)
                {
                    elementCData.Add(new UICanvasData
                    {
                        Text = textBlock.Text?.ToString(),
                    });
                }
            string jsonStringC = JsonConvert.SerializeObject(elementCData);
            File.WriteAllText(filePathC, jsonStringC);
        }

        private void add_gap_rect(double gap)
        {

            Rectangle rectangle = new Rectangle
            {
                Height = 1,
                Width = 610,
                Stroke = Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, gap, 0, 0),
            };
            canvas.Children.Add(rectangle);
        }

        bool flag = false;
        bool flagl = false;
        private void test(object sender, RoutedEventArgs e)
        {
            string filePathC = "uielementC.json";
            Button button1 = (Button)sender;
            
            DependencyObject parent = button1.Parent;
            Border border = new Border();
            
            if (flag == false)
            {
                Grid.SetColumn(border, 1);
                Grid.SetRow(border, 1);
                Grid.SetRowSpan(border, 2);

                border.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));

                Button button = sender as Button;
                if (button != null)
                {
                    while (parent != null && !(parent is Grid))
                        parent = VisualTreeHelper.GetParent(parent);

                    if (parent is Grid parentGrid)
                    {
                        DoubleAnimation animationButton = new DoubleAnimation
                        {
                            From = 0,
                            To = 1,
                            Duration = TimeSpan.FromSeconds(0.15),
                            AutoReverse = false,
                            FillBehavior = FillBehavior.HoldEnd
                        };

                        Storyboard buttonStoryboard = new Storyboard();
                        buttonStoryboard.Children.Add(animationButton);
                        Storyboard.SetTarget(animationButton, border);
                        Storyboard.SetTargetProperty(animationButton, new PropertyPath(UIElement.OpacityProperty)); 

                        buttonStoryboard.Begin(border);


                        parentGrid.Children.Add(border);
                        if (flagl == false)
                        {
                            border.Child = canvas;
                            flagl = true;
                        }


                    }

                    else
                        Console.WriteLine("Grid не найден");
                }

                flag = true;

            }

            else
            {
                if (parent is Grid parentGrid)
                {
                    //canvas.Children.Clear();
                    parentGrid.Children.RemoveAt(12);//last border

                    DoubleAnimation animation = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(0.3),
                        AutoReverse = false,
                        FillBehavior = FillBehavior.HoldEnd,
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                    };

                    Storyboard storyboard = new Storyboard();
                    storyboard.Children.Add(animation);
                    Storyboard.SetTarget(animation, br);
                    Storyboard.SetTarget(animation, ChekboxPanel);
                    Storyboard.SetTargetProperty(animation, new PropertyPath(UIElement.OpacityProperty));

                    storyboard.Begin(ChekboxPanel);


                }

                flag = false;

                return;
            }


            canvas = new Canvas();

            LoadCanvasData(filePathC);

            canvas.VerticalAlignment = VerticalAlignment.Top;

            canvas.Width = 610;
            canvas.Height = 40 * canvas.Children.Count * 1.1; // TODO AUTO SIZE
            canvas.Margin = new Thickness(0, 50, 0, 0);

            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.Content = canvas;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            scrollViewer.HorizontalAlignment = HorizontalAlignment.Stretch;
            scrollViewer.VerticalAlignment = VerticalAlignment.Stretch;
            border.Child = scrollViewer;

            

            newTextBlock = AddNewTextBlock(str);
            if (newTextBlock != null )
                canvas.Children.Add(newTextBlock);

            autoSave(filePathC);
            //LoadCanvasData(filePathC);
        }

        

        TextBlock newTextBlock;
        private TextBlock AddNewTextBlock(string text)
        {
            if (text == null)
                return null;

            newTextBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = text,
                Foreground = Brushes.White,
                FontSize = 14,
            };

            newTextBlock.Margin = new Thickness(0, 80 + 11, 0, 0);
            
            add_gap_rect(80 + 41 + newTextBlock.ActualHeight);


            str = null;
            return newTextBlock;

        }
    }
}
