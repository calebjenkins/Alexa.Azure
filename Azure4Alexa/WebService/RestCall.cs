using AlexaSkillsKit.Speechlet;
using Azure4Alexa.Alexa;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Azure4Alexa.WebService
{
    public class ApiRequest
    {
        public static string apiUrl = "http://api.icndb.com/jokes/random/";
        // Call the remote web service.  Invoked from AlexaSpeechletAsync
        // Then, call another function with the raw JSON results to generate the spoken text and card text
        public static async Task<SpeechletResponse> GetResults(Session session, HttpClient httpClient)
        //public static SpeechletResponse GetResults(Session session, HttpClient httpClient)
        {
            string httpResultString = "";
            httpClient.DefaultRequestHeaders.Clear();

            var httpResponseMessage = await httpClient.GetAsync(apiUrl);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                httpResultString = await httpResponseMessage.Content.ReadAsStringAsync();
            }
            else
            {
                httpResponseMessage.Dispose();
                return AlexaUtils.BuildSpeechletResponse(new AlexaUtils.SimpleIntentResponse() { cardText = AlexaConstants.AppErrorMessage }, true);
            }


            var simpleIntentResponse = ParseResults(httpResultString);
            httpResponseMessage.Dispose();
            return AlexaUtils.BuildSpeechletResponse(simpleIntentResponse, true);

        }

        private static AlexaUtils.SimpleIntentResponse ParseResults(string resultString)
        {
            var result = "Want a joke, huh? Well, the joke server sent a Fuck Of response so no joke for you. Bye.";

            // Build the response
            if (!string.IsNullOrWhiteSpace(resultString))
            {
                var response = JsonConvert.DeserializeObject<Models.Model>(resultString);
                result = response.Value.Joke ?? result;
            }

            //return new AlexaUtils.SimpleIntentResponse() { cardText = stringForCard, ssmlString = stringToRead };

            // if you want to add images, you can include them in the reply
            // images should be placed into the ~/Images/ folder of this project
            // 

            // JPEG or PNG supported, no larger than 2MB
            // 720x480 - small size recommendation
            // 1200x800 - large size recommendation

            return new AlexaUtils.SimpleIntentResponse()
            {
                cardText = result,
                ssmlString = $"<speak>{result}</speak>",
                largeImage = "chuck.png",
                smallImage = "chuck.png",
            };

        }
    }
} 