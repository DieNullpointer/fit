using AutoMapper;
using FitManager.Application.Dto;
using FitManager.Application.Infrastructure;
using FitManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;

namespace FitManager.Webapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class CompanyController : ControllerBase
    {
        private readonly FitContext _db;
        private readonly IMapper _mapper;
        private readonly CompanyService _service;

        public CompanyController(FitContext db, IMapper mapper, CompanyService service)
        {
            _db = db;
            _mapper = mapper;
            _service = service;
        }

        //  api/company
        [HttpGet]
        public IActionResult GetAllCompanies()
        {
            var companies = _db.Companies.Include(a => a.Package).Include(a => a.Event).Include(a => a.ContactPartners).OrderBy(c => c.Name).ToList();
            var export = companies.Select(p => new
            {
                p.Guid,
                p.Name,
                p.Address,
                p.Country,
                p.Plz,
                p.Place,
                p.BillAddress,
                p.Description,
                p.HasPaid,
                Event = new { p.Event.Guid, p.Event.Name },
                Package = new { p.Package.Guid, p.Package.Name },
                partners = p.ContactPartners.Select(d => new
                {
                    d.Guid,
                    d.Title,
                    d.Firstname,
                    d.Lastname,
                    d.Email,
                    d.TelNr,
                    d.MobilNr,
                    d.Function,
                    d.MainPartner
                })
            });
            return Ok(export);
        }

        //  api/company/{guid}
        [HttpGet("{guid:Guid}")]
        public async Task<IActionResult> GetCompany(Guid guid)
        {
            var company = await _db.Companies.Include(a => a.Event).Include(a => a.Package).Include(c => c.ContactPartners).FirstAsync(c => c.Guid == guid);
            if (company is null) return BadRequest();
            return Ok(company);
        }

        // api/company/delete
        [HttpDelete("delete/{guid:Guid}")]
        public async Task<IActionResult> DeleteCompany(Guid guid)
        {
            try
            {
                if (await _service.DeleteCompany(guid))
                    return Ok();
                return BadRequest();
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }

        //  api/company/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CompanyCmd register)
        {
            try
            {
                return Ok(await _service.AddCompany(register));
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }

        [HttpPost("addinserat/{guid:Guid}")]
        public async Task<IActionResult> AddInserat([FromForm] IFormFile formFile, Guid guid)
        {
            if (_db.Companies.Any(a => a.Guid == guid))
                return BadRequest($"Firma {guid} gibt es nicht");
            if (!(await _db.Companies.Include(a => a.Package).FirstAsync(a => a.Guid == guid)).Package.Name.ToLower().Contains("inserat"))
                return BadRequest("Firma hat kein Package das ein Inserat erlaubt");
            if (formFile.ContentType != "application/pdf")
                return BadRequest("Es werden nur PDF Dokumente akzeptiert");
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{guid}");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, $"Inserat-{guid}.{formFile.FileName.Split(".").Last()}");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            return Ok(new { FileName = $"Inserat-{guid}", formFile.Length });
        }

        [HttpPost("addlogo/{guid:Guid}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> AddLogo([FromForm] IFormFile formFile, Guid guid)
        {
            if (_db.Companies.Any(a => a.Guid == guid))
                return BadRequest($"Firma {guid} gibt es nicht");
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{guid}");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            path = Path.Combine(path, $"Logo-{guid}.{formFile.FileName.Split(".").Last()}");
            using (var stream = new FileStream(path, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            return Redirect($"/companypage/{guid}");
        }

        [HttpPost("adddescription")]
        public async Task<IActionResult> AddDescription([FromBody] AddDescriptionDto descriptionDto)
        {
            try
            {
                return Ok(await _service.AddDescription(descriptionDto.description, descriptionDto.guid));
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }

        [HttpPut("change")]
        public async Task<IActionResult> EditCompany([FromBody] CompanyDto company)
        {
            try
            {
                return Ok(await _service.EditCompany(company));
            }
            catch (ServiceException e) { return BadRequest(e.Message); }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> NewEvent([FromBody] SignInCmd sign)
        {
            var company = await _db.Companies.Include(a => a.Package).Include(a => a.Event).FirstAsync(a => a.Guid == sign.guid);
            var events = await _db.Events.FirstAsync(a => a.Guid == sign.guid);
            if (events.Date < DateTime.Now)
                return BadRequest("Keine Vergangenen Events zur Anmeldung möglich");
            var package = await _db.Packages.FirstAsync(a => a.Guid == sign.guid);
            company.LastPackage = company.Package.Name;
            company.Event = events;
            company.Package = package;
            return Ok(company.Guid);
        }

        [HttpGet("getFiles/{guid:Guid}")]
        public async Task<IActionResult> GetFiles(Guid guid, [FromQuery] string fileName)
        {
            if (_db.Companies.Any(a => a.Guid == guid))
                return BadRequest($"Firma {guid} gibt es nicht");
            var files = Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{guid}"));
            if (files.Length == 0)
                return BadRequest("Keine Dateien vorhanden");
            if (fileName == "all")
            {
                using (var outStream = new MemoryStream())
                {
                    using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                    {
                        foreach (var f in files)
                        {
                            var fileInArchive = archive.CreateEntry(Path.GetFileName(f));
                            using (var entryStream = fileInArchive.Open())
                            {
                                using (var fileCompressionStream = new MemoryStream(System.IO.File.ReadAllBytes(f)))
                                {
                                    await fileCompressionStream.CopyToAsync(entryStream);
                                }
                            }
                        }
                    }
                    outStream.Position = 0;
                    return File(outStream.ToArray(), "application/zip", $"Files-{guid}.zip");
                }
            }
            if (files.Where(a => a.ToLower().Contains(fileName)).Count() == 0)
                return BadRequest("Datei gibt es nicht");
            var file = files.Where(a => a.ToLower().Contains(fileName)).First();
            var stream = await System.IO.File.ReadAllBytesAsync(file);
            if (file.Contains("Logo") && fileName.ToLower().Contains("logo"))
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, $"Logo-{guid}.{file.Split(".").Last()}");
            if (file.Contains("Inserat") && fileName.ToLower().Contains("inserat"))
                return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, $"Inserat-{guid}.{file.Split(".").Last()}");
            return BadRequest("Sollte nicht passieren.");
            //var name = file.Split(".");
            //return File(stream, System.Net.Mime.MediaTypeNames.Application.Octet, $"{name[name.Length-2]}.{name[name.Length - 1]}");
        }

        [HttpPost("addmultiple/{guid:Guid}")]
        public async Task<IActionResult> UploadMultiple([FromForm] List<IFormFile> files, Guid guid)
        {
            if (_db.Companies.Any(a => a.Guid == guid))
                return BadRequest($"Firma {guid} gibt es nicht");
            string defaultPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", $"{guid}");
            if (!Directory.Exists(defaultPath))
                Directory.CreateDirectory(defaultPath);
            if (files.IsNullOrEmpty())
                return BadRequest("Keine Dateien vorhanden");
            foreach (var f in files)
                if (f.Length > 30000000)
                    return BadRequest("Eine der Dateien ist größer als 30MB");
            foreach (var f in files)
            {
                var path = Path.Combine(defaultPath, $"{f.FileName.Split(".").First()}-{guid}.{f.FileName.Split(".").Last()}");
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await f.CopyToAsync(stream);
                }
            }
            return Redirect($"companypage/{guid}");
        }

        [HttpGet("allFiles")]
        public async Task<IActionResult> GetAllFiles()
        {
            var files = Directory.GetDirectories(Path.Combine(Directory.GetCurrentDirectory(), "Files"));
            if (files.Length == 0)
                return BadRequest("Keine Dateien vorhanden");
            using (var outStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(outStream, ZipArchiveMode.Create, true))
                {
                    foreach (var d in files)
                    {
                        foreach (var f in Directory.GetFiles(d))
                        {
                            var fileInArchive = archive.CreateEntry(Path.GetFileName(f));
                            using (var entryStream = fileInArchive.Open())
                            {
                                using (var fileCompressionStream = new MemoryStream(System.IO.File.ReadAllBytes(f)))
                                {
                                    await fileCompressionStream.CopyToAsync(entryStream);
                                }
                            }
                        }
                    }
                }
                outStream.Position = 0;
                return File(outStream.ToArray(), "application/zip", $"Files.zip");
            }
        }
    }
}
