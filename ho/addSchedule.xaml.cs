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
        protected Schedule ScheculeData;
        

        public addSchedule(Schedule dat)
        {
            InitializeComponent();


            ScheculeData = dat;

            //時間のコレクションを生成する
            for (int i=0; i <= 23; i++) {
                begin_h.Items.Add(i.ToString());
                end_h.Items.Add(i.ToString());
            }

            for (int i = 0; i <= 59; i++)
            {
                begin_m.Items.Add(i.ToString());
                end_m.Items.Add(i.ToString());
            }


            reflesh();

        }
        
        /// <summary>
        /// 情報の更新を行う
        /// </summary>
        protected void reflesh()
        {
            //件名
            ScheName.Text = ScheculeData.Title;

            //年月日
            DateLabel.Content = String.Format("{0:0000}年{1:00}月{2:00}日", ScheculeData.Begins.Year, ScheculeData.Begins.Month, ScheculeData.Begins.Day);


            begin_h.SelectedIndex = ScheculeData.Begins.Hour;
            begin_m.SelectedIndex = ScheculeData.Begins.Minute;

            end_h.SelectedIndex = ScheculeData.Ends.Hour;
            end_m.SelectedIndex = ScheculeData.Ends.Minute;

            scheMemo.Text = ScheculeData.Memo;


            chk_allTime.IsChecked = ScheculeData.AllTime;

        }

        private void addSc_close_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            this.Hide();
        }

        /// <summary>
        /// 終日の予定チェック変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chk_allTime_Checked(object sender, RoutedEventArgs e)
        {
            //終日？
            ScheculeData.AllTime = chk_allTime.IsChecked == null ? false: chk_allTime.IsChecked.HasValue;
            begin_h.IsEnabled = (chk_allTime.IsChecked == true) ? false : true;
            begin_m.IsEnabled = (chk_allTime.IsChecked == true) ? false : true;

            end_h.IsEnabled = (chk_allTime.IsChecked == true) ? false : true;
            end_m.IsEnabled = (chk_allTime.IsChecked == true) ? false : true;
        }
    }
}
