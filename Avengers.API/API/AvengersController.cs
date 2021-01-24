using Avengers.API.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Polly;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Avengers.API.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvengersController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        
        public AvengersController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IEnumerable<Avenger> Get()
        {
            IEnumerable<Avenger> result =null ;
            var fallBack = Policy
                        .Handle<Exception>()
                        .Fallback(() => { result = this.GetAvengersLocal(); });

            fallBack.Execute(() => { result = this.GetAvengersDisney(); });            
            return result;
        }

        private  IEnumerable<Avenger> GetAvengersDisney()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/disney/avengers");
            var client = _clientFactory.CreateClient("disney");
            var response = client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                using var responseStream = response.Content.ReadAsStreamAsync().Result;
                var avenger = System.Text.Json.JsonSerializer.DeserializeAsync<IEnumerable<Avenger>>(responseStream).Result;
                return avenger;
            }  
            else
            {
                throw new Exception();
            }           
        }
        private IEnumerable<Avenger> GetAvengersLocal()
        {
            var file = System.IO.File.ReadAllText("./Data/data.json");
            var result = JsonConvert.DeserializeObject<IEnumerable<Avenger>>(file);
            return result;
        }
    }
}
