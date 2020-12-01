using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Converters;
using System.IO;

namespace Approval_Srv
{
    public class Notifications
    {
        public void PostInvoice(string FileName)
        {
            string endpoint = "https://outlook.office.com/webhook/1a15ab1f-41b7-4aee-8836-8a70f236e9c7@71f3c23f-dc5b-4d82-b598-c400fdaa89bf/IncomingWebhook/faa36c06e80c4e3ab22e90a99f40b823/94c57c3e-7c6b-4c0c-8f98-ee033b4ba9c1";

            WebRequest request = HttpWebRequest.Create(endpoint);
            request.ContentType = "application/json";
            request.Method = "POST";

            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    Welcome welcome = new Welcome();
                    Section section = new Section();
                    Fact fact1 = new Fact();
                    Fact fact2 = new Fact();

                    List<Section> ListSection = new List<Section>();
                    List<Fact> ListFact = new List<Fact>();


                    welcome.Type = "MessageCard";
                    welcome.Context = new Uri("http://schema.org/extensions");
                    welcome.ThemeColor = "0076D7";
                    welcome.Summary = "La facture " + FileName + " est en attente d'approbation";


                    section.ActivityTitle = "![TestImage](http://ventilationmanic.com/images/favicon.ico)La facture " + FileName + " est en attente d'approbation";
                    section.ActivitySubtitle = "Ventilation Manic";
                    section.ActivityImage = new Uri("http://ventilationmanic.com/images/favicon.ico");

                    section.Markdown = true;

                    fact1.Name = "Assigné à";
                    fact1.Value = "Daniel et Jean";
                    ListFact.Add(fact1);

                    fact2.Name = "Statut";
                    fact2.Value = "En cours";
                    ListFact.Add(fact2);

                    section.Facts = ListFact;
                    ListSection.Add(section);
                    welcome.Sections = ListSection;

                    string json = JsonConvert.SerializeObject(welcome);

                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)request.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }

            catch (WebException e)
            {
                var error = e;
            }

        }

        #region Objects
        public partial class Welcome
        {
            [JsonProperty("@type")]
            public string Type { get; set; }

            [JsonProperty("@context")]
            public Uri Context { get; set; }

            [JsonProperty("themeColor")]
            public string ThemeColor { get; set; }

            [JsonProperty("summary")]
            public string Summary { get; set; }

            [JsonProperty("sections")]
            public List<Section> Sections { get; set; }
        }

        public partial class Section
        {
            [JsonProperty("activityTitle")]
            public string ActivityTitle { get; set; }

            [JsonProperty("activitySubtitle")]
            public string ActivitySubtitle { get; set; }

            [JsonProperty("activityImage")]
            public Uri ActivityImage { get; set; }

            [JsonProperty("facts")]
            public List<Fact> Facts { get; set; }

            [JsonProperty("markdown")]
            public bool Markdown { get; set; }
        }

        public partial class Fact
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }
        #endregion
    }
}


