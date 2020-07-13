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

        /// <summary>
        /// スクロール量
        /// </summary>
        int ScrollOffset = 0;

        /// <summary>
        /// スケジュール1つあたりの高さ
        /// </summary>
        int ScheduleHeight = 40;

        public MainWindow()
        {
            InitializeComponent();

            //表示テスト
            //var window = new testWindow();
            //window.Show();

        }


        /// <summary>
        /// 画面表示時初期化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

            if(label_nowDate != null)   
            label_nowDate.Content = NowSelectDay.ToString("yyyy年 MM月 dd日");


            //スケジュールエリア更新
            refleshScheduleArea();

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

                contents.Height = ScheduleHeight;
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
        public void refleshScheduleArea()
        {
            
            if (TimeHeader == null)
            {
                //初期化前なので、ここでは更新しない
                return;
            }

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
                    contents.Content = $"{offset / 4:00}:{(offset % 4) * 15:00}";
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

            //スケジュール表示更新
            SchedulePanelBase.Children.Clear();
            if (db.Count != 0)
            {
                var list1 =
                    from p in db
                    where p.Begins.Year == MyCalendar.SelectedDate.Value.Year
                    where p.Begins.Month == MyCalendar.SelectedDate.Value.Month
                    where p.Begins.Date == MyCalendar.SelectedDate.Value.Date
                     //orderby p.id
                    select (Schedule)p;

                if (list1.Count() != 0)
                {
                    foreach (Schedule data in list1) { CreateScheculeView(data); }
                }
            }
                
        }
            //MainGrid.Children.Add(scheParts);


        

        /// <summary>
        /// スケジュールエリアの描画オブジェクトを生成
        /// </summary>
        public void CreateScheculeView(Schedule Sche)
        {
            //表示エリアの検索
            //スケジュールの表示高さを計算
            TimeSpan delta = Sche.Ends - Sche.Begins;
            long deltaSize = ((delta.Hours * 4) + (delta.Minutes / 15));
            long deltaOffset = (Sche.Begins.Hour * 4) + (Sche.Begins.Minute / 15) - ScrollOffset;

            //もし上がはみ出す場合
            if(deltaOffset　< 0)
            {
                deltaSize += deltaOffset;
                deltaOffset = 0;

                //その結果表示しなくてもよくなった場合
                if (deltaSize < 0)
                    return;
            }



            System.Windows.Controls.Label scheParts = new System.Windows.Controls.Label
            {
                Content = Sche.Title,
                Name = "HocchiPanel",
                Height = deltaSize * ScheduleHeight,
//              Width = 200,
                Visibility = Visibility.Visible,
                Background = Brushes.Beige,

            };


            Border Bdr = new Border();
            Bdr.Child = scheParts;
            Bdr.BorderBrush = Brushes.Red;
            Bdr.BorderThickness = new Thickness(1);
            Bdr.Margin = new Thickness(0, deltaOffset* ScheduleHeight, 0, 0);
            Bdr.VerticalAlignment = VerticalAlignment.Top;


            SchedulePanelBase.Children.Add(Bdr);

            scheParts.Tag = Sche; //スケジュール情報をオブジェクトとして持たせておく
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

        /// <summary>
        /// マウススクロール
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SchedulePanel_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta < 0)
            {
                //ホイール下降
                

                if (ScrollOffset < 24*4)
                {
                    ScrollOffset += 1;
                }

            }
            else
            {
                //ホイール上昇
                if (ScrollOffset > 0) 
                {
                    ScrollOffset -= 1; 
                }
                

            }
            PanelScroll.Value = ScrollOffset;
            refleshScheduleArea();
        }

    }
}
