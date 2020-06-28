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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyScheduleData;

namespace MySchedule
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// スケジュールデータ用DB
        /// </summary>
        protected List<Schedule> db;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var d = new Schedule();
            var calSelect = MyCalendar.SelectedDate;
            var begin = new DateTime(calSelect.Value.Year, calSelect.Value.Month, calSelect.Value.Day, 9, 0, 0);
            var end = new DateTime(calSelect.Value.Year, calSelect.Value.Month, calSelect.Value.Day, 15, 0, 0);
            var window = new addSchedule(new Schedule("無題の予定", begin, end, true, "めもも"));
            



            //表示！
            window.ShowDialog();
        }

        /// <summary>
        /// カレンダー初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCalendar_Initialized(object sender, EventArgs e)
        {
            MyCalendar.SelectedDate = DateTime.Now;
        }


    }
}
