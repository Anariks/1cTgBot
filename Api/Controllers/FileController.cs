using Microsoft.AspNetCore.Mvc;
using Telegram.Bot;

using Api.Services;
using Domain.Services;
using System.Text;
using Api.Controllers.Helpers;
using System.IO.Compression;
using Domain.XmlData;

namespace Api.Controllers;

public class FileController : ControllerBase
{
    private readonly DatabaseService _databaseService;
    private readonly ILogger<FileController> _logger;
    private readonly Authorization _authorization;
    private readonly string _dirPath;

    public FileController(DatabaseService DatabaseService, ILogger<FileController> logger, Authorization authorization)
    {
        _databaseService = DatabaseService;
        _logger = logger;
        _authorization = authorization;
        _dirPath = RawDataFromXml.DirPath;
    }

    [HttpPost("api/file")]
    public async Task<IActionResult> Post(CancellationToken cancellationToken = default)
    {
        if (!_authorization.IsAuthorized(Request))
            return Unauthorized("Wrong credentials");

        if (!Request.QueryString.Value.Contains("file"))
            return NotFound("Wrong request");

        //Create the Directory and filename.
        var filename = Request.Query.Single(x => x.Key == "filename").Value;

        if (!Directory.Exists(_dirPath))
        {
            Directory.CreateDirectory(_dirPath);
        }

        _logger.LogDebug("Filename: {1}", filename);

        var fullPath = Path.Combine(_dirPath, filename);

        try
        {
            using (var fileStreamTo = System.IO.File.Create(fullPath))
            {
                await Request.BodyReader.AsStream().CopyToAsync(fileStreamTo);
            }

            if (System.IO.File.Exists(fullPath))
            {
                string[] filesToDelete = System.IO.Directory.GetFiles(_dirPath, "*.xml");
                filesToDelete.ToList().ForEach(sFile => System.IO.File.Delete(sFile));

                ZipFile.ExtractToDirectory(fullPath, _dirPath);
                System.IO.File.Delete(fullPath);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }

        //await _databaseService.GetXmlFromRemote(files, cancellationToken);

        return Ok("success");
    }

    [HttpGet("api/file")]
    public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
    {
        if (Request.QueryString.Value.Contains("checkauth"))
        {
            if (_authorization.IsAuthorized(Request))
            {
                _logger.LogInformation("Data exchange started");
                return Ok("success\nCookieNameGetXML\nCookieValueGetXML");
            }

            return Unauthorized("Wrong credentials");
        }

        if (Request.QueryString.Value.Contains("init"))
            return Ok("zip=yes\nfile_limit=4000000");

        if (Request.QueryString.Value.Contains("import"))
        {
            var filename = Request.Query.Single(x => x.Key == "filename").Value;
            if (System.IO.File.Exists(Path.Combine(_dirPath, filename)))
            {
                try
                {
                    if (filename == "offers.xml") _databaseService.FillDatabase().Wait();
                    _logger.LogInformation("Data Exchange and filling finished");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
                return Ok("success");
            }
        }
        return Ok("Here we are");
    }
}
