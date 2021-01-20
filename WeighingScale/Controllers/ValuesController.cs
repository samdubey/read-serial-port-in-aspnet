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
        public long Get(string comPort, int boudRate)
        {
            if (boudRate == 0)
                boudRate = 2400;

            MySerialPortClass mySerialPortClass
                = new MySerialPortClass(comPort, boudRate);
            if (mySerialPortClass.Open)
            {
                string portData = mySerialPortClass.ReadExisting();
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
