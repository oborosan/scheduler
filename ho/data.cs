using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyScheduleData
{

    public class Schedule
    {
        public string Title = "無題";

        /// <summary>
        /// 予定開始時
        /// </summary>
        public DateTime Begins { get; set; }

        /// <summary>
        /// 予定終了時
        /// </summary>
        public DateTime Ends{ get; set; }

        /// <summary>
        /// メモ
        /// </summary>
        public string Memo = "無題";

        /// <summary>
        /// 終日の予定か
        /// </summary>
        public bool AllTime = false;

        public Schedule(string title, DateTime b, DateTime e, bool alltime, string memo)
        {
            Title = title;
            Begins = b;
            Ends = e;
            Memo = memo;
            AllTime = alltime;
        }

        public Schedule()
        {
            Begins = new DateTime(2020,1,1,9,0,0);
            Ends = new DateTime(2020, 1, 1, 17, 0, 0);
}


    }
}
