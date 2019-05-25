using System;

namespace QTranser.QTranseLib
{ 
    class TanseTimes
    {
        public static void AddTodayTranseTime()
        {

            if(DateTime.Now.Day != Properties.Settings.Default.Yesterday)
            {
                if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
                {
                    Properties.Settings.Default.WeekTanseTime = 0;
                }
                Properties.Settings.Default.Yesterday = DateTime.Now.Day;
                Properties.Settings.Default.WeekTanseTime += Properties.Settings.Default.TodayTanseTime;
                Properties.Settings.Default.TodayTanseTime = 0;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.TodayTanseTime++;
                Properties.Settings.Default.WeekTanseTime++;
                QTranse.Mvvm.TodayTanseTime = Properties.Settings.Default.TodayTanseTime;
                QTranse.Mvvm.WeekTanseTime = Properties.Settings.Default.WeekTanseTime;
                Properties.Settings.Default.Save();
            }
        }
        public static void InitTranseTime()
        {
            QTranse.Mvvm.TodayTanseTime = Properties.Settings.Default.TodayTanseTime;
            QTranse.Mvvm.WeekTanseTime = Properties.Settings.Default.WeekTanseTime;
        }
    }
}
