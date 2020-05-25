using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace WebApplication8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForecastController : ControllerBase
    {
        string AVkey = "f9025600-ffd8-4227-945e-cb0674bd1792";
        // GET api/forecast
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult<string>> GetAsync()
        {            
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://api.airvisual.com");
                client.DefaultRequestHeaders.Add("User-Agent", "AQapi");
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"v2/countries?key={AVkey}"; 
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                return resp;
            }
            catch 
            {
                Url error = new Url();
                error.status = "failure";
                return JsonConvert.SerializeObject(error); 
            }
        }
        // GET api/forecast/{country}
        [HttpGet("{country}")]
        public async System.Threading.Tasks.Task<ActionResult<string>> GetAsync(string country)
        {           
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://api.airvisual.com");
                client.DefaultRequestHeaders.Add("User-Agent", "AQapi");
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"v2/states?country={country}&key={AVkey}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                return resp;
            }
            catch 
            {

                Url error = new Url();
                error.status = "failure";
                return JsonConvert.SerializeObject(error);
            }
        }
        // GET api/forecast/{country}/{state}
        [HttpGet("{country}/{state}")]
        public async System.Threading.Tasks.Task<ActionResult<string>> GetAsync(string country, string state)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://api.airvisual.com");
                client.DefaultRequestHeaders.Add("User-Agent", "AQapi");
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"v2/cities?state={state}&country={country}&key={AVkey}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                return resp;
            }
            catch
            {

                Url error = new Url();
                error.status = "failure";
                return JsonConvert.SerializeObject(error);
            }
        }
        // GET api/forecast/{country}/{state}/{city}
        [HttpGet("{country}/{state}/{city}")]
        public async System.Threading.Tasks.Task<ActionResult<string>> GetAsync(string country, string state, string city)
        {            
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://api.airvisual.com");
                client.DefaultRequestHeaders.Add("User-Agent", "AQapi");
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"v2/city?city={city}&state={state}&country={country}&key={AVkey}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                return resp;
            }
            catch
            {

                Url error = new Url();
                error.status = "failure";
                return JsonConvert.SerializeObject(error);
            }
        }
        // GET api/forecast/location={langtitude}&{longtitude}
        [HttpGet("location={langtitude}&{longtitude}")]
        public async System.Threading.Tasks.Task<ActionResult<string>> GetLocationAsync(string langtitude, string longtitude)
        {
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://api.airvisual.com");
                client.DefaultRequestHeaders.Add("User-Agent", "AQapi");
                client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));
                var url = $"v2/nearest_city?lat={langtitude}&lon={longtitude}&key={AVkey}";
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var resp = await response.Content.ReadAsStringAsync();
                return resp;
            }
            catch
            {

                Url error = new Url();
                error.status = "failure";
                return JsonConvert.SerializeObject(error);
            }
        }
    }
}
