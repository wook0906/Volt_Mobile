using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using UnityEngine;

namespace Volt
{
    public class Time : MonoBehaviour
    {
        public static DateTime GetGoogleDateTime()
        {
            DateTime dateTime = DateTime.MinValue;
            try
            {
                using (var response = WebRequest.Create("http://www.google.com").GetResponse())
                    dateTime = DateTime.ParseExact(response.Headers["date"],
                                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                                        CultureInfo.InvariantCulture.DateTimeFormat,
                                        DateTimeStyles.AssumeUniversal);
            }
            catch (Exception)
            {
                Debug.LogWarning("Error Get google server Time");
                dateTime = DateTime.Now;
            }

            return dateTime;
        }
    }
}