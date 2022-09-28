using System.ComponentModel.DataAnnotations;

namespace SampleWebApiTests
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }

    public class WeatherForecastCommand
    {
        public string? Name { get; set; }
        
        public string? Email { get; set; }

    }

    public class AddWeatherForecastCommand:IValidatableObject
    {
        [Required]
        public string? Name { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return null;
        }
    }
}