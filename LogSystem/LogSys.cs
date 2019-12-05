using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LogSystem
{
    public enum DataType
    {
        FileName,//作为文件名
        LogTime,//作为日志中日期
    }

    public class LogSys
    {
        private static LogSys instance = new LogSys();
        public List<int> TotleOperID { get; set; }
        public List<int> SuccessedOperID { get; set; }
        public List<int> FailedOperID { get; set; }
        public List<int> NotOperID { get; set; }
        public FileStream LogFile = null;
        public DateTime NowTime { get; set; }

        private LogSys()
        {
            TotleOperID = new List<int>();
            SuccessedOperID = new List<int>();
            FailedOperID = new List<int>();
            NotOperID = new List<int>();
            String path = String.Format(@"/Log/{0}.logfile",GetNowTime(DataType.FileName));
            File.Create(path);
            
         }

        public static LogSys GetInstance()
        {
            return instance;
        }

        public String GetNowTime(DataType type)
        {
            NowTime = DateTime.Now;
            switch (type)
            {
                case DataType.FileName:
                    return String.Format("{0}-{1}-{2}-{3}-{4}-{5}",NowTime.Year,NowTime.Month,NowTime.Day,NowTime.Hour,NowTime.Minute,NowTime.Second);
                case DataType.LogTime:
                    return String.Format("{0}-{1}-{2} {3}:{4}:{5}",NowTime.Year,NowTime.Month,NowTime.Day,NowTime.Hour,NowTime.Minute,NowTime.Second);;
                default:
                    return "Error";
            }
            
        }
    }
}
