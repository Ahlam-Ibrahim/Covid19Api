using Covid19Api.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid19Api.Models;
using Covid19Api.Dtos;
using SQLitePCL;

namespace Covid19Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : Controller
    {
        private ICountryRepository _countryRepository;
        public CountriesController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        //api/countries
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CountryDto>))]
        public IActionResult GetCountries()
        {
            var countries = _countryRepository.GetCountries().ToList();

            if (!ModelState.IsValid)
                return BadRequest();

            var countriesDto = new List<CountryDto>();
            foreach (var country in countries)
            {
                countriesDto.Add(new CountryDto
                {
                    id = country.id,
                    Name = country.Name
                });
            }

            return Ok(countriesDto);
        }

        //api/countries/id
        [HttpGet("{countryId}", Name = "GetCountry")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CountryDto))]
        public IActionResult GetCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var country = _countryRepository.GetCountry(countryId);

            if (!ModelState.IsValid)
                return BadRequest();

            var countryDto = new CountryDto()
            {
                id = country.id,
                Name = country.Name
            };

            return Ok(countryDto);
        }

        //api/countries
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Country))]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateCountry([FromBody] Country country)
        {
            if (country == null)
                return BadRequest();

            var duplicateCountry = _countryRepository.GetCountries()
                            .Where(c => c.Name.Trim().ToUpper() == country.Name.Trim().ToUpper())
                            .FirstOrDefault();

            if (duplicateCountry != null)
            {
                ModelState.AddModelError("", $"Country {country.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_countryRepository.CreateCountry(country))
            {
                ModelState.AddModelError("", $"Something went wrong saving {country.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCountry", new { countryId = country.id }, country);
        }

        //api/countries/countryId
        [HttpPut("{countryId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCountry(int countryId, [FromBody] Country country)
        {
            if (country == null)
                return BadRequest();

            if (countryId != country.id)
                return BadRequest();

            var duplicateCountry = _countryRepository.IsDuplicateCountryName(countryId, country.Name);
            if (duplicateCountry == true)
            {
                ModelState.AddModelError("", $"Country {country.Name} already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_countryRepository.UpdateCountry(country))
            {
                ModelState.AddModelError("", $"Something went wrong updating {country.Name}");
                return StatusCode(500, ModelState);
            }

            return NoContent();

        }

        //api/countries/countryId
        [HttpDelete("{countryId}")]
        [ProducesResponseType(204)] //no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCountry(int countryId)
        {
            if (!_countryRepository.CountryExists(countryId))
                return NotFound();

            var countryToDelete = _countryRepository.GetCountry(countryId);

            var casesNumber = _countryRepository.GetCasesFromACountry(countryId).Count();

            if (casesNumber > 0)
            {
                ModelState.AddModelError("", $"Country {countryToDelete.Name} " +
                                              "cannot be deleted because it has"
                                              + casesNumber + "case/s");
                return StatusCode(409, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (!_countryRepository.DeleteCountry(countryToDelete))
            {
                ModelState.AddModelError("", $"Something went wrong deleting {countryToDelete.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
