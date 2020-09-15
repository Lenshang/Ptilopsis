using NCrontab;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ptilopsis.PtiTask
{
    public class DateSchedule
    {
        public DateTime CalculateDateScheduleFromNow(string scheduleStr)
        {
            var cron = CrontabSchedule.Parse(scheduleStr);
            return cron.GetNextOccurrence(DateTime.Now);
            //try
            //{
            //    DateTime sdate = DateTime.Now;
            //    string[] schArr = schedule.Split(",");
            //    for (int i = schArr.Length - 1; i >= 0; i--)
            //    {
            //        string v = schArr[i];
            //        switch (i)
            //        {
            //            case 4://分钟
            //                if (v != "*")
            //                {
            //                    int numv = Convert.ToInt32(v);
            //                    if (sdate.Minute > numv)
            //                    {
            //                        sdate = sdate.AddHours(1);
            //                    }
            //                    sdate = new DateTime(sdate.Year, sdate.Month, sdate.Day, sdate.Hour, numv, 0);
            //                }
            //                break;
            //            case 3://小时
            //                if (v != "*")
            //                {
            //                    int numv = Convert.ToInt32(v);
            //                    if (sdate.Hour > numv)
            //                    {
            //                        sdate = sdate.AddDays(1);
            //                    }
            //                    sdate = new DateTime(sdate.Year, sdate.Month, sdate.Day, numv, sdate.Minute, 0);
            //                }
            //                break;
            //            case 2://星期中的一天
            //                if (v != "*")
            //                {
            //                    int numv = Convert.ToInt32(v);
            //                    int nowWeek = Convert.ToInt32(sdate.DayOfWeek);
            //                    if (nowWeek > numv)
            //                    {
            //                        sdate = sdate.AddDays(7);
            //                        sdate = sdate.AddDays(nowWeek - numv);
            //                    }
            //                    else
            //                    {
            //                        sdate = sdate.AddDays(numv - nowWeek);
            //                    }
            //                }
            //                break;
            //            case 1://月中的一天
            //                if (v != "*")
            //                {
            //                    int numv = Convert.ToInt32(v);
            //                    if (sdate.Day > numv)
            //                    {
            //                        sdate = sdate.AddMonths(1);
            //                    }
            //                    sdate = new DateTime(sdate.Year, sdate.Month, numv, sdate.Month, sdate.Minute, 0);
            //                }
            //                break;
            //            case 0://月
            //                if (v != "*")
            //                {
            //                    int numv = Convert.ToInt32(v);
            //                    if (sdate.Month > numv)
            //                    {
            //                        sdate = sdate.AddYears(1);
            //                    }
            //                    sdate = new DateTime(sdate.Year, numv, sdate.Day, sdate.Month, sdate.Minute, 0);
            //                }
            //                break;
            //        }
            //    }
            //    return sdate;
            //}
            //catch (Exception e)
            //{
            //    var new_e = new Exception("不合法的任务计划格式！",e);
            //    throw new_e;
            //}
        }
    }
}
