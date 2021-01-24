using Disney.API.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Disney.API.API
{
    [Route("api/disney/[controller]")]
    [ApiController]
    public class AvengersController : ControllerBase
    {
        
        [HttpGet]
        public IEnumerable<Avenger> Get()
        {
            var file = System.IO.File.ReadAllText("./Data/data.json");
            var  result = JsonConvert.DeserializeObject<IEnumerable<Avenger>>(file);
            return result;           
        }
       
    }
}
