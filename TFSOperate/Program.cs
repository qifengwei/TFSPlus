using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TFSPlus.TFS;

namespace TFSOperate
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("请输入Queries文件路径：");
            TFSWorkflow.ExecuteReadQuery(1, Console.ReadLine());
            Console.WriteLine("按任意键结束");
            Console.ReadKey();
        }
    }
}
