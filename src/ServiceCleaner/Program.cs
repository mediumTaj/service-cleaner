using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceCleaner
{
    public class Program
    {
        HttpClient client = new HttpClient();

        public static void Main(string[] args)
        {
            string credentials = string.Empty;

            credentials = GetCredentials(
                Environment.GetEnvironmentVariable("VCAP_URL"),
                Environment.GetEnvironmentVariable("VCAP_USERNAME"),
                Environment.GetEnvironmentVariable("VCAP_PASSWORD")).Result;
            Task.WaitAll();

            //  Loading from local file
            //var environmentVariable = Environment.GetEnvironmentVariable("VCAP_SERVICES");
            //var fileContent = File.ReadAllText(environmentVariable);
            //var vcapServices = JObject.Parse(fileContent);

            var vcapServices = JObject.Parse(credentials);


            DiscoveryCleaner _discoveryCleaner =
                new DiscoveryCleaner(
                    vcapServices["discovery"]["username"].ToString(),
                    vcapServices["discovery"]["password"].ToString()
                    );
            _discoveryCleaner.Clean();

            LanguageTranslatorCleaner _LanguageTranslatorCleaner =
                new LanguageTranslatorCleaner(
                    vcapServices["language_translator"]["username"].ToString(),
                    vcapServices["language_translator"]["password"].ToString()
                    );
            _LanguageTranslatorCleaner.Clean();

            SpeechToTextCleaner _speechToTextCleaner =
                new SpeechToTextCleaner(
                    vcapServices["speech_to_text"]["username"].ToString(),
                    vcapServices["speech_to_text"]["password"].ToString()
                    );
            _speechToTextCleaner.Clean();

            VisualRecognitionCleaner _visualRecognitionCleaner =
                new VisualRecognitionCleaner(
                    vcapServices["visual_recognition"]["api_key"].ToString(),
                    vcapServices["visual_recognition"]["url"].ToString()
                    );
            _visualRecognitionCleaner.Clean();

            Console.WriteLine(string.Format("\nServices are clean."));

            Console.ReadKey();
        }

        private static async Task<string> GetCredentials(string url, string username, string password)
        {
            var credentials = new NetworkCredential(username, password);
            var handler = new HttpClientHandler()
            {
                Credentials = credentials
            };

            var client = new HttpClient(handler);
            var stringTask = client.GetStringAsync(url);
            var msg = await stringTask;

            return msg;
        }
    }
}
