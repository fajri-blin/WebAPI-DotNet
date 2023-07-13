using API.DTOs.Employee;
using API.DTOs.University;
using API.Models;
using Client.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Client.Controllers;

[Controller]
public class UniversityController : Controller
{
    private readonly IUniversityRepository _universityRepository;

    public UniversityController(IUniversityRepository universityRepository)
    {
        _universityRepository = universityRepository;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _universityRepository.Get();
        var listUniversity = new List<GetUniversityDto>();

        if (result != null)
        {
            listUniversity = result.Data.Select(entity => new GetUniversityDto
            {
                Guid = entity.Guid,
                Code = entity.Code,
                Name = entity.Name,
            }).ToList();
        }

        return View(listUniversity);    
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> Create(NewUniversityDto newUniversityDto)
    {
        var entity = new University
        {
            Guid = new Guid(),
            Code = newUniversityDto.Code,
            Name = newUniversityDto.Name,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
        };

        var result = await _universityRepository.Post(entity);
        if(result.Status == "200")
        {
            TempData["Success"] = "Data Berhasil Masuk";
            return RedirectToAction(nameof(Index));
        }
        else if(result.Status == "409")
        {
            ModelState.AddModelError(string.Empty, result.Message);
            return View();
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid guid)
    {
        var result = await _universityRepository.Get(guid);
        if (result.Data == null)
        {
            return NotFound();
        }

        var universityDto = new GetUniversityDto
        {
            Guid = result.Data.Guid,
            Code = result.Data.Code,
            Name = result.Data.Name
        };

        return View(universityDto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(GetUniversityDto universityDto)
    {
        if (ModelState.IsValid)
        {
            var university = new GetUniversityDto
            {
                Guid = universityDto.Guid,
                Code = universityDto.Code,
                Name = universityDto.Name
            };

            var result = await _universityRepository.Put(university.Guid, university);
            if (result.Code == 200)
            {
                TempData["Success"] = "Data Berhasil Diupdate";
                return RedirectToAction(nameof(Index));
            }
            else if (result.Code == 409)
            {
                ModelState.AddModelError(string.Empty, result.Message);
                return View(universityDto);
            }
        }

        return View(universityDto);
    }
    [HttpGet]
    public async Task<IActionResult> Remove(Guid guid)
    {
        var result = await _universityRepository.Get(guid);
        if (result.Data == null)
        {
            return NotFound();
        }

        var universityDto = new GetUniversityDto
        {
            Guid = result.Data.Guid,
            Code = result.Data.Code,
            Name = result.Data.Name
        };

        return View(universityDto);
    }

    [HttpPost]
    public async Task<IActionResult> RemoveConfirmed(Guid Guid)
    {
        var result = await _universityRepository.Delete(Guid);
        if (result.Code == 401)
        {
            return NotFound();
        }

        return RedirectToAction(nameof(Index));
    }


}
