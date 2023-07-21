using System;
using System.Collections.Generic;
using System.Data;

namespace ConsoleApp2
{
    internal class Program
    {
        List<string> list;  

        static void Main(string[] args)
        {
            try
            {
                //InitData();
                //Site1Helper.ParseSite1();
                //ParseSite2();
                //ParseSite3("aweqweqew","qwerqr3qwer");
                //decimal middle = ParseSite4(true, "4444444");
                //ParseSite5();
                //ParseSite6();
                //ParseSite7();
                ParseSite8();
                //ParseSite9();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        private static void ParseSite9()
        {
            throw new NotImplementedException();
        }

        private static void ParseSite8()
        {
            //throw new NotImplementedException();
            throw new Exception("Не реализовано");
        }

        private static void ParseSite7()
        {
            throw new NotImplementedException();
        }

        private static void ParseSite6()
        {
            throw new NotImplementedException();
        }

        private static void ParseSite5()
        {
            throw new NotImplementedException();
        }

        private static decimal ParseSite4(bool v1, string v2)
        {
            throw new NotImplementedException();
        }

        private static void ParseSite3(string v1, string v2)
        {
            throw new NotImplementedException();
        }

        private static void ParseSite2()
        {
            throw new NotImplementedException();
        }

        

        private static void InitData()
        {
            throw new NotImplementedException();
        }
    }
}
