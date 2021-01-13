using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace WeighingScale.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "test1", "test2" };
        }

        // GET api/values/5
        public long Get(int portNumber)
        {
            MySerialPortClass mySerialPortClass
                = new MySerialPortClass("100", 2400);
            if (mySerialPortClass.Open)
            {
                string portData = mySerialPortClass.ReadLine();
                if (!string.IsNullOrEmpty(portData))
                {
                    string resultString = Regex.Match(portData, @"\d+").Value;
                    long resultLong = Convert.ToInt64(resultString);
                    return resultLong;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
