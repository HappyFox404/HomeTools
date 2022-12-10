using HomeTools.Extensoins;
using HomeTools.Models;
using HomeTools.Models.ResponseModels;
using HomeTools.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HomeTools.Controllers.Statistic;

public partial class StatisticController : Controller
{
    private readonly ILogger<StatisticController> _logger;
    // GET
    public StatisticController(ILogger<StatisticController> logger)
    {
        _logger = logger;
    }

    public IActionResult SpaceManage(string? path, string? selectDirectory, bool isDrive = false)
    {
        SpaceManageViewModel model = new()
        {
            CurrentPath = path ?? String.Empty,
            Drivers = DriveInfo.GetDrives().ToList()
        };

        string fixedPath = path;

        if (fixedPath == default && selectDirectory != default)
        {
            model.CurrentPath = selectDirectory;
        }

        if (fixedPath != default && selectDirectory != default)
        {
            List<string> items = fixedPath.Split("\\", StringSplitOptions.RemoveEmptyEntries).ToList();
            if (isDrive == false)
            {
                if (items.Contains(selectDirectory))
                {
                    model.CurrentPath = Path.Combine(items.Take(items.IndexOf(selectDirectory) + 1).ToArray());
                }
                else
                {
                    model.CurrentPath = Path.Combine(fixedPath, selectDirectory);
                }
            }
            else
            {
                string drive = model.Drivers.Select(x => x.Name).First(x => x.StartsWith(selectDirectory));
                model.CurrentPath = drive;
                model.SelectDirectory = drive;
            }
        }

        if (String.IsNullOrWhiteSpace(model.CurrentPath) == false)
        {
            if (Directory.Exists(model.CurrentPath))
            {
                try
                {
                    foreach (var directory in Directory.GetDirectories(model.CurrentPath).ToList())
                    {
                        model.Catalogs.Add(new DirectoryInfo(directory));
                    }
                    foreach (var file in Directory.GetFiles(model.CurrentPath))
                    {
                        model.Files.Add(new FileInfo(file));
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    _logger.LogError("Попытка обратиться в защищённый каталог: {}", model.CurrentPath);
                    model.IsProtected = true;
                }
            }
        }
        
        return View(model);
    }

    [HttpPost]
    public IActionResult GetFullFilesInfo(string[] files)
    {
        List<DiagrammFileData> data = new();
        if (files.Length > 0)
        {
            foreach (var file in files)
            {
                FileInfo info = new FileInfo(file);
                data.Add(new ()
                {
                    FileName = info.Name,
                    Size = info.Length
                });
            }

            return Json(ServerResponseMessages.Ok<List<DiagrammFileData>>(data));
        }
        return Json(ServerResponseMessages.Error());
    }
}