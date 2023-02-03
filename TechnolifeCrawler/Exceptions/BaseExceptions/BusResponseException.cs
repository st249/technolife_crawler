using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechnolifeCrawler.Exceptions.BaseExceptions
{
    public class BusResponseException : Exception
    {
        private const string _defaultMessage = "can not handle the request.";
        public BusResponseException() : base(_defaultMessage)
        {

        }
        public BusResponseException(string message) : base(message)
        {

        }
    }
}
