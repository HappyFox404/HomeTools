namespace HomeTools.Models.ViewModels;

public class SpaceManageViewModel
{
    public string CurrentPath { get; set; }
    public string SelectDirectory { get; set; }
    public List<DriveInfo> Drivers { get; set; } = new();
    public List<FileInfo> Files { get; set; } = new();
    public List<DirectoryInfo> Catalogs { get; set; } = new();
    public bool IsProtected { get; set; } = false;
}