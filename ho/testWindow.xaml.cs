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

namespace MySchedule
{
    /// <summary>
    /// testWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class testWindow : Window
    {
        public testWindow()
        {
            InitializeComponent();


            System.Windows.Controls.Label scheParts = new System.Windows.Controls.Label
            {
                Content = "●●の予定2",
                Height = 100,
                Width = 200,
                Visibility = Visibility.Visible,
                Background = Brushes.Beige
                //Grid.Row = 1
            };
            scheParts.Height = 40;
            scheParts.Background = Brushes.Red;
            scheParts.Name = "HocchiPanel";
            scheParts.Visibility = Visibility.Visible;


            Border Bdr = new Border();
            Bdr.Child = scheParts;
            Bdr.BorderBrush = Brushes.Red;
            Bdr.BorderThickness = new Thickness(1);
            Bdr.Height = 60;
            Bdr.Margin = new Thickness(100, 100, 100, 0);
            Bdr.VerticalAlignment = VerticalAlignment.Top;
            Bdr.Visibility = Visibility.Visible;


            
            TestPanel.Children.Add(Bdr);

        }
    }
}
