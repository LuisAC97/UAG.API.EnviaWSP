using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Text;
using UAG.API.EnviaWSP.Utilerias;

namespace UAG.API.EnviaWSP.APIs
{
    public class SuncoAPI
    {
        static string urlPage = "https://app.smooch.io/v1/apps/60ad316a11feb200d3a1f834/notifications";

        internal static async Task<JObject> SendTemplate(string phone, string templateName, List<dynamic> body, IConfiguration config)
        {
            var jsonResponse = new JObject();
            try
            {
                if (phone != null && phone != "" && templateName != null && templateName != "")
                {
                    dynamic destino = new
                    {
                        integrationId = "62a73f3c36b08700f11a6592",
                        destinationId = $"{phone}"
                    };
                    dynamic components = new List<dynamic>
                    {
                        new
                        {
                            type = "body",
                            parameters = body
                        }

                    };

                    dynamic bodyWA = new
                    {
                        destination = destino,
                        author = new
                        {
                            role = "appMaker"
                        },
                        messageSchema = "whatsapp",
                        message = new
                        {
                            type = "template",
                            template = new
                            {
                                @namespace = "634b5671_9f45_45e3_955a_18412e76c771",
                                name = $"{templateName}",
                                language = new
                                {
                                    policy = "deterministic",
                                    code = "es_MX"
                                },
                                components = body
                            }
                        }
                    };
                    HttpClient clientTicket = new HttpClient();
                    clientTicket.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    clientTicket.DefaultRequestHeaders.Add("Authorization", $"Basic {UtilZendesk.GetSuncoEncoded(config)}");
                    string urlTicket = String.Format($"{urlPage}");
                    var jsonPost = JsonConvert.SerializeObject(bodyWA);
                    var dataPost = new StringContent(jsonPost, Encoding.UTF8, "application/json");
                    using HttpResponseMessage responseTicket = await clientTicket.PostAsync(urlTicket, dataPost).ConfigureAwait(false);
                    responseTicket.EnsureSuccessStatusCode();
                    string responseBody = await responseTicket.Content.ReadAsStringAsync();
                    jsonResponse = JObject.Parse(responseBody);
                }
            }
            catch (Exception e)
            {

            }
            return jsonResponse;
        }
    }
}
