using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace FtoCAPI_ShowcaseDotNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConvertController : ControllerBase
    {
        [HttpPost]
        public ActionResult<ConvertResponse> Convert(ConvertRequest request)
        {
            if (request.Units == UnitsEnum.c)
            {
                var fahrenheit = CelsiusToFahrenheit(request.Value);
                return Ok(new ConvertResponse { Units = "f", Value = fahrenheit });
            }
            else if (request.Units == UnitsEnum.f)
            {
                var celsius = FahrenheitToCelsius(request.Value);
                return Ok(new ConvertResponse { Units = "c", Value = celsius });
            }
            else
            {
                return BadRequest("Invalid units supplied");
            }
        }

        private double CelsiusToFahrenheit(double celsius)
        {
            return (9.0 / 5.0) * celsius + 32;
        }

        private double FahrenheitToCelsius(double fahrenheit)
        {
            return (5.0 / 9.0) * (fahrenheit - 32);
        }
    }

    public class ConvertRequest
    {
        [Required]
        public UnitsEnum Units { get; set; }

        [Required]
        public double Value { get; set; }
    }

    public class ConvertResponse
    {
        public string? Units { get; set; }
        public double Value { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UnitsEnum
    {
        c,
        f
    }
}