
using Instana.ManagedTracing.Sdk.Spans;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace InstanaSDKExampleApp.Services.Countries
{
    public class CountryService
    {
        private HttpClient _client;
        public CountryService(HttpClient client)
        {
            _client = client;
        }

        public ServiceResult<List<Country>> GetCountriesByRegion(string region)
        {
            // we do NOT explicitly create a span here. HttpClient will be autoinstrumented
            // and thus we will see how long the call took.
            ServiceResult<List<Country>> serviceResult = new ServiceResult<List<Country>>();
            try
            {
                string byRegionUrl = $"https://restcountries.eu/rest/v2/region/{region.ToLower()}";
                var result = _client.GetAsync(byRegionUrl).Result;
                serviceResult.StatusCode = (int)result.StatusCode;
                if (result.IsSuccessStatusCode)
                {
                    serviceResult.Result = TransformApiResultToCountryList(result);
                }

            }
            catch(Exception e)
            {
                serviceResult.CallException = e;
            }

            return serviceResult;

        }

        private List<Country> TransformApiResultToCountryList(HttpResponseMessage response)
        {
            // let's create an intermediate span, which will show us how much the
            // transformation-logic  contributes to the overall runtime.
            using (var span = CustomSpan.Create(this, SpanType.INTERMEDIATE))
            {
                HttpContent content = response.Content;
                var decoded = content.ReadAsStringAsync().Result;
                var countries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Country>>(decoded);
                span.SetData("count", countries.Count.ToString());
                return countries;
            }
        }
    }
}
