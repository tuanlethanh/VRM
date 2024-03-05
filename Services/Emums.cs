using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRM.Services 
{ 
    public class Emums
    {
        public enum ExportReportType
        {
            EKYCReport,
            KYCReport,
            DetailReport,
            SumaryReport
        }
        public enum ValueDataType
        {
            DateTime = 1,
            Number = 2,
            String = 3,
            Boolean = 4
        }
    }
}
