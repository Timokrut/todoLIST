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
using todoLIST;

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
        CheckBox checkBoxInCheckBoxPanel;
        Rectangle lineUnderCheckBox;
        Button submitButton;
        TextBlock textBlock;
        TextBlock newTextBlock;

        Canvas canvasForStoringTheHistoryOfCompletedTasks = new Canvas();

        string filePathC = "uielementC.json";
        string stringForSavingInCanvasForStoring;

        bool flagFullScrean = false;
        bool flagForSwitchingBetweenBorders = false;
        bool flagAddingCanvasOnceForStoringHistiry = false;

        double Gap_Between_Completed_Tasks;

        int countSizeCanvas = 0;
        double height = 0;

        // Main
        public MainWindow()
        {
            InitializeComponent();

            string filePath = "uielements.json";
            ControlTemplate templateOfButtonComplited = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderOfButtonComplited = new FrameworkElementFactory(typeof(Border));
            borderOfButtonComplited.SetValue(Border.CornerRadiusProperty, new CornerRadius(10));
            borderOfButtonComplited.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            borderOfButtonComplited.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter))
            {
                Name = "PART_ContentHost"
            });
            templateOfButtonComplited.VisualTree = borderOfButtonComplited;

            Completed.Content = " Completed";
            Completed.Template = templateOfButtonComplited;
            Completed.Background = new SolidColorBrush(Color.FromArgb(200, 123, 124, 129));
            LoadData(filePath);
            LoadCanvasData(filePathC);

            this.Closed += Window_Closed;
        }

        public  void SaveData(string filePath, string filePathC)
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

            foreach (UIElement element in canvasForStoringTheHistoryOfCompletedTasks.Children)
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

                        height += 6;
                        ChekboxPanel.Height = height;

                        checkBox.Margin = new Thickness(0, height, 0, 0);

                        height += (data.Text.Count(x => x == '\n') > 0) ? 22.5 * data.Text.Count(x => x == '\n') : 35;
                        
                        ChekboxPanel.Children.Add(checkBox);
                        
                        Rectangle abbGapRectangle = new Rectangle
                        {
                            Height = 1,
                            Width = 610,
                            Stroke = Brushes.Gray,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            Margin = new Thickness(0, height, 0, 0),
                        };
                        ChekboxPanel.Children.Add(abbGapRectangle);
                    }
                }
        }
        public void LoadCanvasData(string filePathC)
        {
            if (File.Exists(filePathC))
            {
                string jsonString = File.ReadAllText(filePathC);
                List<UICanvasData> elementCData = JsonConvert.DeserializeObject<List<UICanvasData>>(jsonString);

                double CanvasHeight = 0;
                Gap_Between_Completed_Tasks = 0;
                foreach (UICanvasData data in elementCData)
                {
                    if (!string.IsNullOrEmpty(data.Text) && data.Text.Length > 0)
                    {
                        CanvasHeight += (data.Text.Count(x => x == '\n') > 0) ? 22.5 * data.Text.Count(x => x == '\n') : 43;

                        TextBlock loadTextBlockIntoCanvas = new TextBlock();
                        loadTextBlockIntoCanvas.VerticalAlignment = VerticalAlignment.Top;
                        loadTextBlockIntoCanvas.HorizontalAlignment = HorizontalAlignment.Center;
                        loadTextBlockIntoCanvas.Text = data.Text;
                        loadTextBlockIntoCanvas.Foreground = Brushes.White;
                        loadTextBlockIntoCanvas.FontSize = 14;

                        loadTextBlockIntoCanvas.Loaded += (sender, e) =>
                        {
                            loadTextBlockIntoCanvas.Margin = new Thickness(0, Gap_Between_Completed_Tasks + 11, 0, 0);
                            Gap_Between_Completed_Tasks += loadTextBlockIntoCanvas.ActualHeight + 23;
                            add_gap_rect(Gap_Between_Completed_Tasks);
                        };
                        canvasForStoringTheHistoryOfCompletedTasks.Children.Add(loadTextBlockIntoCanvas);
                    }
                }
                        canvasForStoringTheHistoryOfCompletedTasks.Height = CanvasHeight;
            }
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            string filePathC = "uielementC.json";
            LoadCanvasData(filePathC);
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

        private void NewTaskButton_Click(object sender, RoutedEventArgs e)
        {
            NewTask.Visibility = Visibility.Collapsed;

            // creating entities
            checkBoxInCheckBoxPanel = new CheckBox();
            textInCheckBox = new TextBox();
            lineUnderCheckBox = new Rectangle();

            ControlTemplate textInCheckBoxTemplate = new ControlTemplate(typeof(TextBox));
            FrameworkElementFactory borderForTextInCheckBox = new FrameworkElementFactory(typeof(Border));
            borderForTextInCheckBox.SetValue(Border.CornerRadiusProperty, new CornerRadius(5));
            borderForTextInCheckBox.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            borderForTextInCheckBox.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Control.BorderThicknessProperty));
            borderForTextInCheckBox.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Control.BorderBrushProperty));
            borderForTextInCheckBox.AppendChild(new FrameworkElementFactory(typeof(ScrollViewer))
            {
                Name = "PART_ContentHost"
            });

            textInCheckBoxTemplate.VisualTree = borderForTextInCheckBox;
            textInCheckBox.Template = textInCheckBoxTemplate;

            textInCheckBox.Text += "Wtrite Somesthing";
            

            textInCheckBox.KeyDown += textInCheckBox_KeyDown;
            textInCheckBox.GotFocus += clickOnTextInCheckBox;
            
            

            submitButton = new Button();
            ControlTemplate submitButtonTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory borderForSubmitButton = new FrameworkElementFactory(typeof(Border));
            borderForSubmitButton.SetValue(Border.CornerRadiusProperty, new CornerRadius(3));
            borderForSubmitButton.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Control.BackgroundProperty));
            borderForSubmitButton.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter))
            {
                Name = "PART_ContentHost"
            });
            submitButtonTemplate.VisualTree = borderForSubmitButton;

            submitButton.Template = submitButtonTemplate;

            submitButton.Click += Submit_CLick;
            TextPanel.Children.Add(submitButton);

            submitButton.Content = "  Submit";    

            TextPanel.Children.Add(textInCheckBox);

            DoubleAnimation animationTextInCheckBox = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
                AutoReverse = false,
                FillBehavior = FillBehavior.HoldEnd
            };

            Storyboard textInCheckBoxStoryboard = new Storyboard();
            textInCheckBoxStoryboard.Children.Add(animationTextInCheckBox);
            Storyboard.SetTarget(animationTextInCheckBox, textInCheckBox);
            Storyboard.SetTargetProperty(animationTextInCheckBox, new PropertyPath(Rectangle.OpacityProperty));

            textInCheckBoxStoryboard.Begin(textInCheckBox);

            DoubleAnimation animationSubmitButton = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.3),
                AutoReverse = false,
                FillBehavior = FillBehavior.HoldEnd
            };

            Storyboard submitButtonStoryboard = new Storyboard();
            submitButtonStoryboard.Children.Add(animationSubmitButton);
            Storyboard.SetTarget(animationSubmitButton, submitButton);
            Storyboard.SetTargetProperty(animationSubmitButton, new PropertyPath(UIElement.OpacityProperty));

            submitButtonStoryboard.Begin(submitButton);


            lineUnderCheckBox.Height = 1;
            lineUnderCheckBox.Width = 610;
            lineUnderCheckBox.Stroke = Brushes.Gray;
            lineUnderCheckBox.HorizontalAlignment = HorizontalAlignment.Center;
            lineUnderCheckBox.VerticalAlignment = VerticalAlignment.Center;

            // Start Customization
            Canvas.SetBottom(submitButton, 0);
            Canvas.SetLeft(submitButton, 20);

            submitButton.Height = 20;
            submitButton.Width = 50;

            submitButton.Background = Brushes.Green;
            submitButton.Foreground = Brushes.White;
           
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
            Grid.SetRow(checkBoxInCheckBoxPanel, 2);
            Grid.SetColumn(checkBoxInCheckBoxPanel, 1);
            //checkBoxInCheckBoxPanel.Margin = new Thickness(5);
            checkBoxInCheckBoxPanel.FontSize = 14;
            checkBoxInCheckBoxPanel.Foreground = Brushes.White;
        }

        private async void Is_CheckedAsync(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox Help_Check_Box && Help_Check_Box.IsChecked == true )
            {
                Help_Check_Box.IsEnabled = false;

                stringForSavingInCanvasForStoring = (string)Help_Check_Box.Content;

                countSizeCanvas += stringForSavingInCanvasForStoring.Count(x => x =='\n') + 1;

                newTextBlock = AddNewTextBlock(stringForSavingInCanvasForStoring);
                if (newTextBlock != null)
                    canvasForStoringTheHistoryOfCompletedTasks.Children.Add(newTextBlock);

                autoSave(filePathC);

                textBlock = new TextBlock();
                textBlock.Text = (string)Help_Check_Box.Content;
                textBlock.TextDecorations = TextDecorations.Strikethrough;
                textBlock.Foreground = Brushes.Black;
                textBlock.FontSize = 15;
                Help_Check_Box.Content = textBlock;

                await Task.Delay(1000);

                int index = ChekboxPanel.Children.IndexOf(Help_Check_Box);
                ChekboxPanel.Children.RemoveAt(index+1);

                DoubleAnimation widthAnimation = new DoubleAnimation
                {
                    From = Help_Check_Box.ActualWidth,
                    To = 0,
                    Duration = TimeSpan.FromSeconds(0.5),
                    FillBehavior = FillBehavior.Stop
                };

                DoubleAnimation heightAnimation = new DoubleAnimation
                {
                    From = Help_Check_Box.ActualHeight,
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

                Storyboard checkBoxStoryboard = new Storyboard();
                checkBoxStoryboard.Children.Add(widthAnimation);
                checkBoxStoryboard.Children.Add(heightAnimation);
                checkBoxStoryboard.Children.Add(opacityAnimation);

                Storyboard.SetTarget(widthAnimation, Help_Check_Box);
                Storyboard.SetTarget(heightAnimation, Help_Check_Box);
                Storyboard.SetTarget(opacityAnimation, Help_Check_Box);

                Storyboard.SetTargetProperty(widthAnimation, new PropertyPath(FrameworkElement.WidthProperty));
                Storyboard.SetTargetProperty(heightAnimation, new PropertyPath(FrameworkElement.HeightProperty));
                Storyboard.SetTargetProperty(opacityAnimation, new PropertyPath(UIElement.OpacityProperty));

                checkBoxStoryboard.Begin(Help_Check_Box);

                 
                await Task.Delay(490);

                ChekboxPanel.Children.Remove(Help_Check_Box);
                double change = 0;
                double u = 0;
                for (int i = 0; i < ChekboxPanel.Children.Count; i++)
                {
                    if(i%2 == 0 && i != 0)
                    {
                        u += (textBlock.Text.Count(x => x == '\n') > 0) ? 22.5 * textBlock.Text.Count(x => x == '\n') : 35;
                        change = u;
                    }
                    
                    if(index == i)
                    {
                        bool flag = false;
                        for (int j = i; j < ChekboxPanel.Children.Count ; j+=2)
                        {
                            if (flag == true)
                                u += 6;
                            CheckBox b = (CheckBox)ChekboxPanel.Children[j];
                            b.Margin = new Thickness(0, u + 18, 0, 0);
                            u += (textBlock.Text.Count(x => x == '\n') > 0) ? 22.5 * textBlock.Text.Count(x => x == '\n') : 35;
                            
                            flag = true;
                            Rectangle p = (Rectangle)ChekboxPanel.Children[j + 1];
                            p.Margin = new Thickness(0, u + 18, 0, 0);
                            
                        }
                        break;
                    }
                }
                height -= change;
                
                
                


            }
        }

        private void clickOnTextInCheckBox(object sender, RoutedEventArgs e)
        {
            textInCheckBox.SpellCheck.IsEnabled = true;

            textInCheckBox.Text = "";

            textInCheckBox.Foreground = Brushes.White;
        }

        private void textInCheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && (Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
            {
                var Help_textBox = sender as TextBox;
                if (Help_textBox != null)
                {
                    int caretPosition = Help_textBox.CaretIndex;
                    Help_textBox.Text = Help_textBox.Text.Insert(caretPosition, "\n");
                    Help_textBox.CaretIndex = caretPosition + 1;
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
            height += 6;
            ChekboxPanel.Height = height;
            
            textInCheckBox.Text.Trim();

            checkBoxInCheckBoxPanel.Content = textInCheckBox.Text;
            TextPanel.Children.Remove(textInCheckBox);

            checkBoxInCheckBoxPanel.Checked += Is_CheckedAsync;

            ChekboxPanel.Children.Add(checkBoxInCheckBoxPanel);

            checkBoxInCheckBoxPanel.Margin = new Thickness(0, height, 0, 0);

            height += (textInCheckBox.Text.Count(x => x == '\n') > 0) ? 22.5 * textInCheckBox.Text.Count(x => x == '\n') : 35;

            Rectangle abbGapRectangle = new Rectangle
            {
                Height = 1,
                Width = 610,
                Stroke = Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, height, 0, 0),
            };
            ChekboxPanel.Children.Add(abbGapRectangle);

            TextPanel.Children.Remove(submitButton);
            NewTask.Visibility = Visibility.Visible;
        }

        //
        //End of check boxes and their logic
        //



        private void autoSave(string filePathC)
        {
            List<UICanvasData> elementCData = new List<UICanvasData>();

            foreach (UIElement element in canvasForStoringTheHistoryOfCompletedTasks.Children)
                if (element is TextBlock Help_textBlock)
                {
                    elementCData.Add(new UICanvasData
                    {
                        Text = Help_textBlock.Text?.ToString(),
                    });
                }
            string jsonStringC = JsonConvert.SerializeObject(elementCData);
            File.WriteAllText(filePathC, jsonStringC);
        }

        private void add_gap_rect(double gap)
        {

            Rectangle abbGapRectangle = new Rectangle
            {
                Height = 1,
                Width = 610,
                Stroke = Brushes.Gray,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, gap, 0, 0),
            };
            canvasForStoringTheHistoryOfCompletedTasks.Children.Add(abbGapRectangle);
        }
        
        private void CompletedButton_Click(object sender, RoutedEventArgs e)
        {
            string filePathC = "uielementC.json";
            Button button1 = (Button)sender;
            
            DependencyObject parent = button1.Parent;
            Border borderAfterClickingOnComplited = new Border();
            
            if (flagForSwitchingBetweenBorders == false)
            {
                Grid.SetColumn(borderAfterClickingOnComplited, 1);
                Grid.SetRow(borderAfterClickingOnComplited, 1);
                Grid.SetRowSpan(borderAfterClickingOnComplited, 2);

                borderAfterClickingOnComplited.Background = new SolidColorBrush(Color.FromRgb(30, 30, 30));

                Button button = sender as Button;
                if (button != null)
                {
                    while (parent != null && !(parent is Grid))
                        parent = VisualTreeHelper.GetParent(parent);

                    if (parent is Grid parentGrid)
                    {
                        DoubleAnimation animationAppearanceBorderAfterCkick = new DoubleAnimation
                        {
                            From = 0,
                            To = 1,
                            Duration = TimeSpan.FromSeconds(0.15),
                            AutoReverse = false,
                            FillBehavior = FillBehavior.HoldEnd
                        };

                        Storyboard appearanceBorderAfterCkickStoryboard = new Storyboard();
                        appearanceBorderAfterCkickStoryboard.Children.Add(animationAppearanceBorderAfterCkick);
                        Storyboard.SetTarget(animationAppearanceBorderAfterCkick, borderAfterClickingOnComplited);
                        Storyboard.SetTargetProperty(animationAppearanceBorderAfterCkick, new PropertyPath(UIElement.OpacityProperty));

                        appearanceBorderAfterCkickStoryboard.Begin(borderAfterClickingOnComplited);

                        parentGrid.Children.Add(borderAfterClickingOnComplited);
                        if (flagAddingCanvasOnceForStoringHistiry == false)
                        {
                            borderAfterClickingOnComplited.Child = canvasForStoringTheHistoryOfCompletedTasks;
                            flagAddingCanvasOnceForStoringHistiry = true;
                        }
                    }
                }
                flagForSwitchingBetweenBorders = true;
            }
            else
            {
                if (parent is Grid parentGrid)
                {
                    parentGrid.Children.RemoveAt(13);//last border

                    DoubleAnimation animationAppearanceCheckboxPanel = new DoubleAnimation
                    {
                        From = 0,
                        To = 1,
                        Duration = TimeSpan.FromSeconds(0.3),
                        AutoReverse = false,
                        FillBehavior = FillBehavior.HoldEnd,
                        EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                    };

                    Storyboard appearanceCheckboxPanelStoryboard = new Storyboard();
                    appearanceCheckboxPanelStoryboard.Children.Add(animationAppearanceCheckboxPanel);
                    Storyboard.SetTarget(animationAppearanceCheckboxPanel, br);
                    Storyboard.SetTarget(animationAppearanceCheckboxPanel, ChekboxPanel);
                    Storyboard.SetTargetProperty(animationAppearanceCheckboxPanel, new PropertyPath(UIElement.OpacityProperty));

                    appearanceCheckboxPanelStoryboard.Begin(ChekboxPanel);
                }
                flagForSwitchingBetweenBorders = false;

                return;
            }


            canvasForStoringTheHistoryOfCompletedTasks = new Canvas();

            LoadCanvasData(filePathC);

            canvasForStoringTheHistoryOfCompletedTasks.VerticalAlignment = VerticalAlignment.Top;

            canvasForStoringTheHistoryOfCompletedTasks.Width = 610;

            canvasForStoringTheHistoryOfCompletedTasks.Margin = new Thickness(0, 50, 0, 0);

            ScrollViewer scrollViewerForCanvasForStoring = new ScrollViewer();
            scrollViewerForCanvasForStoring.Content = canvasForStoringTheHistoryOfCompletedTasks;
            scrollViewerForCanvasForStoring.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewerForCanvasForStoring.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
            scrollViewerForCanvasForStoring.HorizontalAlignment = HorizontalAlignment.Stretch;
            scrollViewerForCanvasForStoring.VerticalAlignment = VerticalAlignment.Stretch;
            borderAfterClickingOnComplited.Child = scrollViewerForCanvasForStoring;

            newTextBlock = AddNewTextBlock(stringForSavingInCanvasForStoring);
            if (newTextBlock != null )
                canvasForStoringTheHistoryOfCompletedTasks.Children.Add(newTextBlock);
        }

        private TextBlock AddNewTextBlock(string stringForSaving)
        {
            if (stringForSaving == null)
                return null;

            newTextBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center,
                Text = stringForSaving,
                Foreground = Brushes.White,
                FontSize = 14,
            };

            newTextBlock.Margin = new Thickness(0, 80 + 11, 0, 0);
            
            add_gap_rect(80 + 41 + newTextBlock.ActualHeight);

            stringForSavingInCanvasForStoring = null;
            return newTextBlock;
        }


        private void SmallWindowCreated(object sender, RoutedEventArgs e)
        {
            CompletedSqueezed completedSqueezed = new CompletedSqueezed();
            completedSqueezed.Show();
            //this.Visibility = Visibility.Collapsed;
            Close();
        }
    }
}
