using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ConsoleApp2
{
    internal class Site1Helper
    {
        private static void ParseSite1()
        {
            DataTable table = Get1SiteData();
            Common.SaveResult(table);

        }

        private static DataTable Get1SiteData()
        {
            throw new NotImplementedException();
        }
    }
}
