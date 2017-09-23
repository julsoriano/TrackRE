using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrackRE.Models;
using Newtonsoft.Json;

namespace TrackRE.ServicesClient
{
    public class PropertyTypeService
    {
        public async Task<List<PropertyType>> GetPropertyTypesAsync(
            CancellationToken cancelToken = default(CancellationToken))
        {
            var uri = Util.getServiceUri("PropertyTypes");
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri, cancelToken);
                return (await response.Content.ReadAsAsync<List<PropertyType>>());
            }

        }

        public List<PropertyType> GetPropertyTypes()
        {
            var uri = Util.getServiceUri("PropertyTypes"); ;
            using (WebClient webClient = new WebClient())
            {
                return JsonConvert.DeserializeObject<List<PropertyType>>(
                    webClient.DownloadString(uri)
                );
            }
        }
    }
}