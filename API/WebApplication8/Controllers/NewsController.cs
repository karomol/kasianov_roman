using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebApplication8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        static IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        string CurApikey = "fSo9GNG7sHMaXqS9Bz3XA2kw1mvZlVNCdHKZy9tGIz9kOQdG";
        
        // GET api/news/{keyword}&{language}
        [HttpGet("{keyword}&{language}")]
        public async System.Threading.Tasks.Task<ActionResult<string>> GetAsync(string keyword, string language)
        {
            Url news = (Url)cache.Get(keyword + " " + language);
            try
            {                                                
                if (news == null)
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://api.currentsapi.services");
                    client.DefaultRequestHeaders.Add("User-Agent", "AQapi");
                    client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = "v1/search?" + $"keywords={keyword}&language={language}&" + $"apiKey={CurApikey}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var resp = await response.Content.ReadAsStringAsync();
                    news = JsonConvert.DeserializeObject<Url>(resp);
                    if (news.news.Count != 0)
                    {
                        cache.Set(keyword+" "+language, news, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));
                        return JsonConvert.SerializeObject(news);
                    }                        
                    else
                    {
                        Url u = new Url();
                        u.status = "failure";
                        cache.Set(keyword + " " + language, u, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));                                               
                        return JsonConvert.SerializeObject(u);
                    }
                }
                else
                {
                    return JsonConvert.SerializeObject(news);
                }
                
            }
            catch
            {
                Url u = new Url();
                u.status = "failure";
                cache.Set(keyword + " " + language, u, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(5)));
                return JsonConvert.SerializeObject(u);
            }
        }
        // GET api/news/{keyword}&{language}/{category}
        [HttpGet("{keyword}&{language}/{category}")]
        public async System.Threading.Tasks.Task<ActionResult<string>> GetAsync(string keyword, string language, string category)
        {
            Url news = (Url)cache.Get(keyword + " " + language);
            Url result = new Url();
            try
            {
                if (news == null)
                {
                    var client = new HttpClient();
                    client.BaseAddress = new Uri("https://api.currentsapi.services");
                    client.DefaultRequestHeaders.Add("User-Agent", "AQapi");
                    client.DefaultRequestHeaders.Accept.Add(
                            new MediaTypeWithQualityHeaderValue("application/json"));
                    var url = "v1/search?" + $"keywords={keyword}&language={language}&" + $"apiKey={CurApikey}";
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var resp = await response.Content.ReadAsStringAsync();
                    news = JsonConvert.DeserializeObject<Url>(resp);
                }
                int i = 0;
                if(news.news.Count != 0)
                {
                    foreach(Url_Items items in news.news)
                        foreach(string s in items.category)
                        {
                            if (s.StartsWith(category))
                            {                                
                                result.news.Add(items);
                                break;
                            }
                            i++;
                        }
                    if (result.news.Count != 0)
                        result.status = "success";
                    else
                        result.status = "failure";
                    return JsonConvert.SerializeObject(result);
                }
                else
                {
                    result.status = "failure";
                    return JsonConvert.SerializeObject(result);
                }
                
            }
            catch
            {
                result.status = "failure";
                return JsonConvert.SerializeObject(result);
            }
        }
    }
}
