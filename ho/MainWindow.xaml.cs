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
        protected List<Schedule> db = new List<Schedule>();

        protected DateTime NowSelectDay;

        int ScrollOffset = 0;

        public MainWindow()
        {
            InitializeComponent();

            //初期化
            initDate();

            //スケジュール生成
            createScheduleArea();
            refleshScheduleArea();
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

        //----独自I/F-------------------------------------------------------------------------

        /// <summary>
        /// スケジュール表示初期生成
        /// </summary>
        protected void createScheduleArea()
        {
            ScrollOffset = (int)PanelScroll.Value;


            for (int i = ScrollOffset; i < 24 * 4; i++)
            {
                //スケジュール枠の更新
                var contents = new System.Windows.Controls.Label();
                contents.Foreground = Brushes.WhiteSmoke;

                contents.Height = 40;
                contents.Width = SchedulePanelBase.ActualWidth;

                contents.Background = null;
                contents.HorizontalAlignment = HorizontalAlignment.Stretch;
                contents.Padding = new Thickness(5,0,0,0);

                //境界線
                var bd = new Border();
                bd.Child = contents;
                bd.BorderThickness = new Thickness(1,1,1,0);
                bd.Visibility = Visibility.Visible;
                bd.BorderBrush = Brushes.Gray;

                bd.HorizontalAlignment = HorizontalAlignment.Stretch;
                bd.VerticalAlignment = VerticalAlignment.Stretch;



                TimeHeader.Children.Add(bd);
            }
            //contents.Name = $"{i / 4:0}:{i % 4:00}";


        }

        /// <summary>
        /// スケジュール表示更新
        /// </summary>
        protected void refleshScheduleArea()
        {
            int offset = ScrollOffset;
            foreach (Border bd in TimeHeader.Children)
            {

                var contents = (System.Windows.Controls.Label)bd.Child;
                if (offset > 24 * 4)
                {
                    contents.Content = "";
                    contents.Background = Brushes.Gray;
                    bd.BorderThickness = new Thickness(0, 0, 0, 0);

                    continue;
                }

                contents.Background = Brushes.DarkGray;
                if ((offset % 4 == 0) || (bd == TimeHeader.Children[0]))
                {
                    contents.Content = $"{offset / 4:00}:{(offset % 4)*15:00}";
                    contents.FontWeight = FontWeights.Bold;//太字
                    bd.BorderThickness = new Thickness(1, 1, 1, 0);

                }
                else
                {
                    contents.Content = $"    {(offset % 4) * 15:00}";
                    contents.FontWeight = FontWeights.Normal;
                    bd.BorderThickness = new Thickness(1, 0, 1, 0);
                    
                }

                offset++;

                contents.Width = SchedulePanelBase.ActualWidth;

            }


            //お試しで、スケジュール１つ表示してみる
            System.Windows.Controls.Label scheParts = new System.Windows.Controls.Label();
            scheParts.Content = "●●の予定";
            scheParts.Height = 100;
            scheParts.Width = 200;
            scheParts.Visibility = Visibility.Visible;
            scheParts.Height = 40;
            scheParts.Background = Brushes.Red;
            scheParts.Name = "CustomPanel";
            

            MainGrid.Children.Add(scheParts);
            

        }


        /// <summary>
        /// 予定の生成(addScheduleダイアログからコールされるはず)
        /// </summary>
        /// <param name="schedule"></param>
        public void createSchecule(Schedule schedule)
        {
            db.Add(schedule);
        }


        //----ボタン関連-------------------------------------------------------------------------

        /// <summary>
        /// 予定追加ボタン
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var calSelect = MyCalendar.SelectedDate;
            var begin = new DateTime(calSelect.Value.Year, calSelect.Value.Month, calSelect.Value.Day, 9, 15, 0);
            var end = new DateTime(calSelect.Value.Year, calSelect.Value.Month, calSelect.Value.Day, 15, 30, 0);
            var window = new addSchedule(new Schedule("無題の予定", begin, end, false, "めもも"));
            window.Owner = this; //オーナーを設定してやることで、戻り値を受ける

            //表示！
            window.ShowDialog();
        }


        //----カレンダー関連-------------------------------------------------------------------------

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


        //----その他------------------------------------------------------------------------------------

        /// <summary>
        /// 画面の表示サイズが変わった時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            refleshScheduleArea();
        }

        private void ScrollBar_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

            ScrollOffset = (int)PanelScroll.Value;
            refleshScheduleArea();
        }
    }
}
