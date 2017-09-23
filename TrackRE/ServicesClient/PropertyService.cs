using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TrackRE.DTOs;
using TrackRE.Models;

namespace TrackRE.ServicesClient
{
    public class PropertyService
    {
        //public async Task<List<PropertyRE>> GetPropertyREsAsync(
        public async Task<List<PropertyREDTO>> GetPropertyREsAsync(
            CancellationToken cancelToken = default(CancellationToken))
        {
            var uri = Util.getServiceUri("Properties");
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri, cancelToken);
                //return (await response.Content.ReadAsAsync<List<PropertyRE>>());
                return (await response.Content.ReadAsAsync<List<PropertyREDTO>>());
            }
        }

        public async Task<PropertyRE> GetPropertyREAsync(int? id,
            CancellationToken cancelToken = default(CancellationToken))
        {
            var uri = Util.getServiceUri("Properties" + "/" + id);
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri, cancelToken);
                return (await response.Content.ReadAsAsync<PropertyRE>());
            }
        }
        
        public async Task<PropertyREDTO> GetPropertyREDTOAsync(int id,
            CancellationToken cancelToken = default(CancellationToken))
        {
            var uri = Util.getServiceUri("Properties" + "/" + id + "/DTO");
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri, cancelToken);
                return (await response.Content.ReadAsAsync<PropertyREDTO>());
            }
        }

        public async Task<PropertyRE> DeletePropertyREAsync(int id,
            CancellationToken cancelToken = default(CancellationToken))
        {
            var uri = Util.getServiceUri("Properties" + "/" + id + "/delete");
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.DeleteAsync(uri, cancelToken);
                return (await response.Content.ReadAsAsync<PropertyRE>());
            }
        }

        public async Task<HttpResponseMessage> EditPropertyREAsync(int id, PropertyRE propertyRE,
            CancellationToken cancelToken = default(CancellationToken))
        //public HttpResponseMessage EditPropertyRE(int id, PropertyRE propertyRE)
        {
            var uri = Util.getServiceUri("Properties" + "/" + id + "/edit");
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.PutAsJsonAsync(uri, propertyRE, cancelToken);
                return response.EnsureSuccessStatusCode();
            }
        }

        //public async Task<HttpResponseMessage> PostPropertyREAsync(PropertyRE propertyRE,
        public async Task<PropertyRE> PostPropertyREAsync(PropertyRE propertyRE,

            CancellationToken cancelToken = default(CancellationToken))
        {
            var uri = Util.getServiceUri("Properties" + "/add");
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsJsonAsync(uri, propertyRE);
                //return response.EnsureSuccessStatusCode();
                return (await response.Content.ReadAsAsync<PropertyRE>());
            }
        }

        public async Task<List<PropertyRE>> GetPropertyREsAsyncByType(string proptype,
            CancellationToken cancelToken = default(CancellationToken))
        {
            var uri = Util.getServiceUri("Properties" + "/" + proptype);
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri, cancelToken);
                return (await response.Content.ReadAsAsync<List<PropertyRE>>());
            }
        }

        public async Task<List<PropertyREDTO>> GetPropertyREsDTOAsyncByType(string proptype,
            CancellationToken cancelToken = default(CancellationToken))
        {
            var uri = Util.getServiceUri("Properties" + "/" + proptype);
            using (HttpClient httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri, cancelToken);
                return (await response.Content.ReadAsAsync<List<PropertyREDTO>>());
            }
        }

        public List<PropertyRE> GetPropertyREs()
        {
            var uri = Util.getServiceUri("PropertyREs"); ;
            using (WebClient webClient = new WebClient())
            {
                return JsonConvert.DeserializeObject<List<PropertyRE>>(
                    webClient.DownloadString(uri)
                );
            }
        }
    }
}