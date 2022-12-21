using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TimMovie.Core.DTO.Person;
using TimMovie.Core.Services.Person;
using TimMovie.Web.ViewModels.Person;

namespace TimMovie.Web.Controllers.Person;

public class PersonController: Controller
{
    private readonly PersonService _personService;
    private readonly IMapper _mapper;

    public PersonController(PersonService personService, IMapper mapper)
    {
        _personService = personService;
        _mapper = mapper;
    }

    [HttpGet("actor/{id:guid}")]
    public IActionResult GetActorPage(Guid id)
    {
        return GetPersonPage(id, _personService.GetActorById);
    }

    [HttpGet("producer/{id:guid}")]
    public IActionResult GetProducerPage(Guid id)
    {
        return GetPersonPage(id, _personService.GetProducerById);
    }
    
    private IActionResult GetPersonPage(Guid id, Func<Guid, PersonDto?> getPersonById)
    {
        var person = getPersonById(id);
        if (person is null)
        {
            return NotFound();
        }
        
        var personView = _mapper.Map<PersonViewModel>(person);
        return View("~/Views/Person/Person.cshtml", personView);
    }
}