using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Extensions.MonoHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace WebApi.Models
{
    public class TTWebApiHelper
    {
        public string WebApiEndpoint { get; set; } = "https://ttdemowebapi.soft-fx.com:8443";
        public string WebApiId { get; set; } = "1de621ca-e686-4ee2-92a5-45c87b4b3fe5";
        public string WebApiKey { get; set; } = "czNhCcnK6ydePCHZ";
        public string WebApiSecret { get; set; } = "J6Jxc2xPr8JyNpWtyEaCPYpkpJpsSQ38xb9AZNxBAGdtQrNDhQwf9mkWQygCKd6K";

        public List<Symbol> GetSymbols()
        {
            var client = new RestClient(WebApiEndpoint);
            string method = "api/v1/symbol";
            var request = new RestRequest(method, Method.GET);
            SetupRequest(request);
            var response = client.Execute(request);
            return JToken.Parse(response.Content)
                .Select(i => new Symbol
                {
                    Id = Guid.NewGuid(),
                    Name = i["Symbol"].ToString()
                })
                .ToList();
        }

        public List<Quote> GetQuotes(Symbol symbol)
        {
            int count = 72;
            var client = new RestClient(WebApiEndpoint);
            string method =
                $"api/v1/quotehistory/{HttpUtility.UrlEncode(symbol.Name)}/H1/bars/ask?timestamp={DateTimeToTimeStamp(DateTime.Now)}&count={count}";
            var request = new RestRequest(method, Method.GET);
            SetupRequest(request);
            var response = client.Execute(request);
            return JToken.Parse(response.Content)["Bars"]
                                .Select(e => new Quote
                                {
                                    Id = Guid.NewGuid(),
                                    Symbol = symbol,
                                    DateTime = TimeStampToDateTime(e["Timestamp"].ToObject<double>()),
                                    Open = e["Open"].ToObject<double>(),
                                    High = e["High"].ToObject<double>(),
                                    Low = e["Low"].ToObject<double>(),
                                    Close = e["Close"].ToObject<double>(),
                                    Volume = e["Volume"].ToObject<long>()
                                }).ToList();
        }

        private DateTime TimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dt;
        }

        private double DateTimeToTimeStamp(DateTime dt)
        {
            return (dt.Ticks - 621355968000000000) / 10000;
        }

        private void SetupRequest(RestRequest request)
        {
            string url = $"{WebApiEndpoint}/{request.Resource}";
            var timestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
            var signature = timestamp + WebApiId + WebApiKey + request.Method + url;
            var hash = CalculateHmacWithSha256(signature);
            request.AddParameter("Authorization",
                $"HMAC {WebApiId}:{WebApiKey}:{timestamp}:{hash}",
                ParameterType.HttpHeader);
        }

        private string CalculateHmacWithSha256(string signature)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(WebApiSecret);
            byte[] messageBytes = encoding.GetBytes(signature);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}