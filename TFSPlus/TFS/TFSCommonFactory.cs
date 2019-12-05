using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TFSPlus.TFS
{
    class TFSCommonFactory
    {
        public static TFSCommon GetInstance(int function)
        {
            if (function == 0x01) return new QueryAddLink();
            else return null;
        }
    }
}
