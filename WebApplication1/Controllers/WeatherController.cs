using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Runtime.Intrinsics.X86;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {
       // string apiKey = "0K1QdzPWOKfdFEtuhLt7iJG5GfdLBc0P";

        [HttpGet("getlocationkey")]
        public IActionResult GetLocationKey(string apiKey,string city)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request2 = new HttpRequestMessage(HttpMethod.Get, $"http://dataservice.accuweather.com/locations/v1/cities/search?apikey={apiKey}&q={city}");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            HttpResponseMessage response = client.Send(request2);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            string responseBody = response.Content.ReadAsStringAsync().Result;
            var alerts = JsonConvert.DeserializeObject<IEnumerable<LocationKey>>(responseBody);
            return Ok(alerts);
        }

       //1 günlük tahmin
        [HttpGet]
        public IActionResult GetWeather(string apiKey, string locationKey, string language)
        {
            HttpClient client = new HttpClient();
            HttpRequestMessage request = new(HttpMethod.Get, $"http://dataservice.accuweather.com/forecasts/v1/daily/1day/{locationKey}?apikey={apiKey}&language={language}");
            request.Headers.Authorization = new("Bearer", apiKey);
            HttpResponseMessage response = client.Send(request);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest();
            }
            string responseBody = response.Content.ReadAsStringAsync().Result;

            var alerts = JsonConvert.DeserializeObject<WeatherForecast>(responseBody);

            return Ok(alerts);
        }

        //5 günlük
        //[HttpGet("fiveday")]
        //public IActionResult GetWeatherFiveDay()
        //{
        //    HttpClient client = new HttpClient();
        //    HttpRequestMessage request = new(HttpMethod.Get, $"http://dataservice.accuweather.com/forecasts/v1/daily/5day/318251?apikey=0K1QdzPWOKfdFEtuhLt7iJG5GfdLBc0P&language=tr&details=false");
        //    request.Headers.Authorization = new("Bearer", apiKey);
        //    HttpResponseMessage response = client.Send(request);
        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return BadRequest();
        //    }
        //    string responseBody = response.Content.ReadAsStringAsync().Result;

        //    var alerts = JsonConvert.DeserializeObject<WeatherData>(responseBody);

        //    return Ok(alerts);
        //}
    }
    public class LocationKey
    {
        public string Key { get; set; }
        
    }

    /// <summary>
    /// //////////////////////////////////   1günlük
    /// </summary>
    public class Minimum
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
        public double MinimumCelsius => (Value - 32) * 5 / 9;
    }
    public class Maximum
    {
        public double Value { get; set; }
        public string Unit { get; set; }
        public int UnitType { get; set; }
        public double MaximumCelsius => (Value - 32) * 5 / 9;
    }
    public class Temperature
    {
        public Minimum Minimum { get; set; }
        public Maximum Maximum { get; set; }
    }
    public class Day
    {
        public int Icon { get; set; }
        public string IconPhrase { get; set; }
        public bool HasPrecipitation { get; set; }
        public string PrecipitationType { get; set; }
        public string PrecipitationIntensity { get; set; }
    }
    public class Headline
    {
        public int Severity { get; set; }
        public string Text { get; set; }
    }
    public class DailyForecast
    {
        public string Date { get; set; }
        public Temperature Temperature { get; set; }
        public Day Day { get; set; }
        public object Night { get; set; }
        public List<string> Sources { get; set; }
    }
    public class WeatherForecast
    {
        public Headline Headline { get; set; }
        public List<DailyForecast> DailyForecasts { get; set; }
    }



    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    //public class Minimum
    //{
    //    public double Value { get; set; }
    //    public string Unit { get; set; }
    //    public int UnitType { get; set; }
    //    public double MinimumCelsius => (Value - 32) * 5 / 9;
    //}

    //public class Maximum
    //{
    //    public double Value { get; set; }
    //    public string Unit { get; set; }
    //    public int UnitType { get; set; }
    //    public double MaximumCelsius => (Value - 32) * 5 / 9;

    //}

    //public class Temperature
    //{
    //    public Minimum Minimum { get; set; }
    //    public Maximum Maximum { get; set; }
    //}

    //public class Day
    //{
    //    public int Icon { get; set; }
    //    public string IconPhrase { get; set; }
    //    public bool HasPrecipitation { get; set; }
    //    public string PrecipitationType { get; set; }
    //    public string PrecipitationIntensity { get; set; }
    //}

    //public class Night
    //{
    //    public int Icon { get; set; }
    //    public string IconPhrase { get; set; }
    //    public bool HasPrecipitation { get; set; }
    //    public string PrecipitationType { get; set; }
    //    public string PrecipitationIntensity { get; set; }
    //}

    //public class DailyForecast
    //{
    //    public DateTime Date { get; set; }
    //    public long EpochDate { get; set; }
    //    public Temperature Temperature { get; set; }
    //    public Day Day { get; set; }
    //    public Night Night { get; set; }
    //    public List<string> Sources { get; set; }
    //    public string MobileLink { get; set; }
    //    public string Link { get; set; }
    //}

    //public class Headline
    //{
    //    public DateTime EffectiveDate { get; set; }
    //    public long EffectiveEpochDate { get; set; }
    //    public int Severity { get; set; }
    //    public string Text { get; set; }
    //    public string Category { get; set; }
    //    public object EndDate { get; set; }
    //    public object EndEpochDate { get; set; }
    //    public string MobileLink { get; set; }
    //    public string Link { get; set; }
    //}

    //public class WeatherData
    //{
    //    public Headline Headline { get; set; }
    //    public List<DailyForecast> DailyForecasts { get; set; }
    //}

}
