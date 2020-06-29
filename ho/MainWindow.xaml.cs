using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;
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

        protected DateTime NowSelectDay;

        public MainWindow()
        {
            InitializeComponent();

            //初期化
            initDate();

            //スケジュール生成
            refleshDisp();
        }

        /// <summary>
        /// 選択日付更新(初期化)
        /// </summary>
        protected void initDate(DateTime? dt= null)
        {
            NowSelectDay = dt ?? DateTime.Today; //nullなら今日を指しておこう・・・

            //本日の日付選択
            MyCalendar.SelectedDate = NowSelectDay;
            
            label_nowDate.Content = NowSelectDay.ToString("yyyy年 MM月 dd日");

        }

        /// <summary>
        /// スケジュール表示更新
        /// </summary>
        protected void refleshDisp()
        {
            
            //contents.Content = "9;00";

            for (int i = 0; i < 24*4; i++)
            {
                //スケジュール枠の更新
                var contents = new System.Windows.Controls.Label();

                //contents.Name = $"{i / 4:0}:{i % 4:00}";
                
                contents.Height = 40;
                contents.Background = null;
                if (i % 4 == 0)
                {
                    contents.Content = $"{i/4:0}:{i%4:00}";
                    
                }
                else
                {
                    contents.Content = $"   {(i % 4)*15:00}";
                }
                
                contents.Foreground = Brushes.WhiteSmoke;
                TimeHeader.Children.Add(contents);
            }

        }


        //予定追加
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var calSelect = MyCalendar.SelectedDate;
            var begin = new DateTime(calSelect.Value.Year, calSelect.Value.Month, calSelect.Value.Day, 9, 15, 0);
            var end = new DateTime(calSelect.Value.Year, calSelect.Value.Month, calSelect.Value.Day, 15, 30, 0);
            var window = new addSchedule(new Schedule("無題の予定", begin, end, false, "めもも"));

            //表示！
            window.ShowDialog();
        }

        /// <summary>
        /// カレンダー初期化・更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MyCalendar_Initialized(object sender, EventArgs e)
        {
            MyCalendar.SelectedDate = DateTime.Now;
        }

        private void btn_dayBack_MouseDown(object sender, MouseButtonEventArgs e)
        {
            initDate(NowSelectDay.AddDays(-1));
        }

        private void btn_dayNext_MouseUp(object sender, MouseButtonEventArgs e)
        {
            initDate(NowSelectDay.AddDays(1));
        }

        private void label_nowDate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            initDate(DateTime.Today);
        }

        private void MyCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            initDate(MyCalendar.SelectedDate);
        }



        /// <summary>
        /// カレンダーコントロールがフォーカスを離さない問題を回避する
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreviewMouseUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseUp(e);
            if ((Mouse.Captured is Calendar) || (Mouse.Captured is System.Windows.Controls.Primitives.CalendarItem))
            {
                Mouse.Capture(null);
            }
        }

        /// <summary>
        /// 画面の表示サイズが変わった時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            
        }
    }
}
