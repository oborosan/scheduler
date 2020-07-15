using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
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

using MyScheduleData;

namespace MySchedule
{
    /// <summary>
    /// addSchedule.xaml の相互作用ロジック
    /// </summary>
    public partial class addSchedule : Window
    {
        /// <summary>
        /// スケジュール情報
        /// </summary>
        protected Schedule ScheculeData { get; set; }        

        public addSchedule(Schedule dat, string title)
        {
            InitializeComponent();

            this.Title = title;

            if (title == "スケジュールの編集") {
                btn_ok.Content = "更新";
            }

            ScheculeData = dat;

            //時間のコレクションを生成する
            for (int i=0; i <= 23; i++) {
                begin_h.Items.Add(i.ToString("00"));
                end_h.Items.Add(i.ToString("00"));
            }

            for (int i = 0; i <= 3; i++)
            {
                begin_m.Items.Add((i*15).ToString("00"));
                end_m.Items.Add((i*15).ToString("00"));
            }

            //件名
            ScheName.Text = ScheculeData.Title;

            //年月日
            DateLabel.Content = String.Format("{0:0000}年{1:00}月{2:00}日", ScheculeData.Begins.Year, ScheculeData.Begins.Month, ScheculeData.Begins.Day);


            begin_h.SelectedIndex = ScheculeData.Begins.Hour;
            begin_m.SelectedIndex = ScheculeData.Begins.Minute / 15;

            end_h.SelectedIndex = ScheculeData.Ends.Hour;
            end_m.SelectedIndex = ScheculeData.Ends.Minute / 15;

            scheMemo.Text = ScheculeData.Memo;


            chk_allTime.IsChecked = ScheculeData.AllTime;

            reflesh();

        }
        
        /// <summary>
        /// 情報の更新を行う
        /// </summary>
        protected void reflesh()
        {

            //合計時間を更新
            if (chk_allTime.IsChecked == false)
            {
                TimeSpan delta = ScheculeData.Ends - ScheculeData.Begins;
                Label_Span.Content = delta.ToString();
            }
            else
            {
                Label_Span.Content = "終日";
            }
        }

        /// <summary>
        /// 閉じる
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addSc_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 予定の確定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            MainWindow owner = (MainWindow)Owner;

            //データの更新
            ScheculeData.Title = ScheName.Text;
            ScheculeData.Memo = scheMemo.Text;
            ScheculeData.AllTime = (bool)chk_allTime.IsChecked;

            owner.createSchecule(ScheculeData);


            this.Close();
            owner.refleshScheduleArea();
        }

        /// <summary>
        /// 終日の予定チェック変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_allTime_Checked(object sender, RoutedEventArgs e)
        {
            //終日？
            ScheculeData.AllTime = chk_allTime.IsChecked != true && chk_allTime.IsChecked.HasValue;
            begin_h.IsEnabled = chk_allTime.IsChecked != true;
            begin_m.IsEnabled = chk_allTime.IsChecked != true;

            end_h.IsEnabled = chk_allTime.IsChecked != true;
            end_m.IsEnabled = chk_allTime.IsChecked != true;

            reflesh();
        }


        /// <summary>
        /// 時刻の変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void time_DropDownClosed(object sender, EventArgs e)
        {
            ScheculeData.Begins = new DateTime(ScheculeData.Begins.Year, ScheculeData.Begins.Month, ScheculeData.Begins.Day, begin_h.SelectedIndex, begin_m.SelectedIndex * 15, 0);

            ScheculeData.Ends = new DateTime(ScheculeData.Ends.Year, ScheculeData.Ends.Month, ScheculeData.Ends.Day, end_h.SelectedIndex, end_m.SelectedIndex * 15, 0);

            //時間が逆転しているなら入れ替えましょう
            if(ScheculeData.Begins > ScheculeData.Ends)
            {
                (ScheculeData.Begins, ScheculeData.Ends) = (ScheculeData.Ends, ScheculeData.Begins);
                (begin_h.SelectedIndex, end_h.SelectedIndex) = (end_h.SelectedIndex, begin_h.SelectedIndex);
                (begin_m.SelectedIndex, end_m.SelectedIndex) = (end_m.SelectedIndex, begin_m.SelectedIndex);

            }

            //合計時間を更新
                TimeSpan delta = ScheculeData.Ends - ScheculeData.Begins;
            Label_Span.Content = delta.ToString();
        }
    }
}
