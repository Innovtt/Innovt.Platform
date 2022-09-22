using System;
using System.ComponentModel.DataAnnotations;

namespace SampleAspNetWebApiTest.Controllers
{
    public class AddWeatherForecastCommand
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int? Age { get; set; }
    }
}