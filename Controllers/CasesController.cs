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
    public class CasesController : Controller
    {
        // private ICountryRepository _countryRepository;
        private ICaseRepository _caseRepository;
        public CasesController(ICaseRepository caseRepository)
        {
            _caseRepository = caseRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        //api/cases
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CaseDto>))]
        public IActionResult GetCases()
        {

            var Cases = _caseRepository.GetCases().ToList();

            if (!ModelState.IsValid)
                return BadRequest();

            var CasesDto = new List<CaseDto>();
            foreach (var Case in Cases)
            {
                CasesDto.Add(new CaseDto()
                {
                    id = Case.id,
                    FirstName = Case.FirstName,
                    LastName = Case.LastName,
                    DateOfBirth = Case.DateOfBirth,
                    WithChronicDiseases = Case.WithChronicDiseases,
                    Status = Case.Status,
                    DateOfConfirmation = Case.DateOfConfirmation
                });
            }
            return Ok(CasesDto);
        }

        //api/cases/caseId
        [HttpGet("{caseId}", Name = "GetCase")]
        [ProducesResponseType(200, Type = typeof(CaseDto))]
        [ProducesResponseType(400)]

        public IActionResult GetCase(int caseId)
        {
            if (!_caseRepository.CaseExists(caseId))
                return NotFound();
            var Case = _caseRepository.GetCase(caseId);
            if (!ModelState.IsValid)
                return BadRequest();
            var CaseDto = new CaseDto()
            {
                id = Case.id,
                FirstName = Case.FirstName,
                LastName = Case.LastName,
                DateOfBirth = Case.DateOfBirth,
                WithChronicDiseases = Case.WithChronicDiseases,
                Status = Case.Status,
                DateOfConfirmation = Case.DateOfConfirmation
            };
            return Ok(CaseDto);

        }
        //api/cases
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Case))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCase([FromBody] Case _case)
        {
            if (_case == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return BadRequest();

            if (!_caseRepository.CreateCase(_case))
            {
                ModelState.AddModelError("", $"Something went wrong saving {_case.FirstName} info");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCase", new { caseId = _case.id }, _case);
        }

        //api/cases/caseId
        [HttpPut("{caseId}")]
        [ProducesResponseType(200, Type = typeof(Case))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCase(int caseId, [FromBody] Case _case)
        {
            if (!_caseRepository.CaseExists(caseId))
                return NotFound();

            if (caseId != _case.id)
                return BadRequest();

            if(!_caseRepository.UpdateCase(_case))
            {
                ModelState.AddModelError("", $"Something went wrong updating {_case.FirstName} Case");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        //api/cases
        [HttpDelete("{caseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCase(int caseId)
        {
            if (!_caseRepository.CaseExists(caseId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest();

            var Case = _caseRepository.GetCase(caseId);
            
            if (!_caseRepository.DeleteCase(Case))
            {
                ModelState.AddModelError("", $"Something went wrong deleting this case");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
