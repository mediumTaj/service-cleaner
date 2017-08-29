using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace ServiceCleaner
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environmentVariable = Environment.GetEnvironmentVariable("VCAP_SERVICES");
            var fileContent = File.ReadAllText(environmentVariable);
            var vcapServices = JObject.Parse(fileContent);

            DiscoveryCleaner _discoveryCleaner = 
                new DiscoveryCleaner(
                    vcapServices["discovery"][0]["credentials"]["username"].ToString(), 
                    vcapServices["discovery"][0]["credentials"]["password"].ToString()
                    );
            _discoveryCleaner.Clean();

            //VisualRecognitionCleaner _visualRecognitionCleaner = 
            //    new VisualRecognitionCleaner(
            //        vcapServices["visual_recognition"][0]["credentials"]["apikey"].ToString(),
            //        vcapServices["visual_recognition"][0]["credentials"]["url"].ToString()
            //        );
            //_visualRecognitionCleaner.Clean();

            Console.ReadKey();
        }
    }
}
