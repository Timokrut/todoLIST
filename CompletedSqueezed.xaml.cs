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
using System.IO;
using Newtonsoft.Json;
using System.Windows.Shell;

namespace todoLIST
{
    /// <summary>
    /// Логика взаимодействия для CompletedSqueezed.xaml
    /// </summary>
    public partial class CompletedSqueezed : Window
    {
        public CompletedSqueezed()
        {
            InitializeComponent();
            this.Topmost = true;

            LoadCheckBoxData();
            
        }


        public void LoadCheckBoxData()
        {
            string filePatch = "uielements.json";
            if (!File.Exists(filePatch))
                return;

            string jsonString = File.ReadAllText(filePatch);
            List<UIElementData> uIElementData = JsonConvert.DeserializeObject<List<UIElementData>>(jsonString);

            foreach (UIElementData data in uIElementData)
                if(!string.IsNullOrEmpty(data.Text))
                {
                    if(data.IsChecked.HasValue)
                    {
                        CheckBox helpCheckBox = new CheckBox
                        {
                            Content = data.Text,
                            IsChecked = data.IsChecked.Value,
                            Margin = new Thickness(5),
                            FontSize = 14,
                            //Foreground = Brushes.White,
                            VerticalAlignment = VerticalAlignment.Top,
                            Opacity = 0.97
                        };

                        Style textInCheckBoxxStyle = System.Windows.Application.Current.FindResource("textInCheckBoxxStyle") as Style;
                        helpCheckBox.Style = textInCheckBoxxStyle;

                        PanelForCheckBox.Children.Add(helpCheckBox);

                        Rectangle lineUnderCheckBox = new Rectangle
                        {
                            Height = 1,
                            Width = 350,
                            //Stroke = Brushes.Gray,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            VerticalAlignment = VerticalAlignment.Center
                        };
                        Style RectangleStyle = System.Windows.Application.Current.TryFindResource("RectangleStyle") as Style;
                        lineUnderCheckBox.Style = RectangleStyle;
                        
                        PanelForCheckBox.Children.Add(lineUnderCheckBox);
                        secondWindow.Height += (data.Text.Count(x => x == '\n') > 0) ? 22.5 * data.Text.Count(x => x == '\n') : 43;

                    }
                }
        }

        private void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();

            main.Show();

            Close();
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            secondWindow.Opacity = e.NewValue;
        }

    }
}
