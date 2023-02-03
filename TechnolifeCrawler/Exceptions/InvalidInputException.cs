﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TechnolifeCrawler.Exceptions
{
    public class InvalidInputException : Exception
    {
        private const string _defaultMessage = "Your input is not valid.";
        public InvalidInputException() : base(_defaultMessage)
        {

        }
        public InvalidInputException(string message) : base(message)
        {

        }
        public InvalidInputException(string message, HttpStatusCode statusCode) : this($"{message} Status Code:{statusCode}")
        {

        }
        public InvalidInputException(HttpStatusCode statusCode) : this(_defaultMessage, statusCode)
        {

        }
    }
}
