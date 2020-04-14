using Newtonsoft.Json;

namespace InstanaSDKExampleApp.Services.Countries
{
    public class Country
    {

        [JsonProperty("region")]
            public string Region
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty("alpha3code")]
        public string Code
        {
            get;
            set;
        }

        [JsonProperty("capital")]
        public string Capital
        {
            get;
            set;
        }
        [JsonProperty("population")]
        public long Population
        {
            get;
            set;
        }
    }
}
