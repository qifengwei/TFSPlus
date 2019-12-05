using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSPlus.TFS
{
    public class TFSWorkflow
    {
        public static bool ExecuteReadQuery(int function, params string [] args)
        {
            TFSCommon tfs = TFSCommonFactory.GetInstance(function);
            if (tfs == null) return false;
            tfs.ReadQuery(args[0]);
            tfs.Operate();
            tfs.Finish();
            return true;
        }

        public static bool ExecuteWithoutReadQuery(int function, params string[] args)
        {
            TFSCommon tfs = TFSCommonFactory.GetInstance(function);
            if (tfs == null) return false;
            tfs.Init();
            tfs.Operate();
            tfs.Finish();
            return true;
        }
    }
}
