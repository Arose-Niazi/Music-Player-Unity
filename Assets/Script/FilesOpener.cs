using System.IO;
using SimpleFileBrowser;

public class FilesOpener
{
    private Script _mainScript;
    public FilesOpener(Script mainScript)
    {
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "Music", ".mp3") );
        FileBrowser.SetDefaultFilter( ".mp3" );
        FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );
        _mainScript = mainScript;
    }
    
    public void OpenFolderExplorer()
    {
        FileBrowser.ShowLoadDialog( DialogResponse,
        						   null,
        						   FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Songs and Folders", "Add" );

    }

    private void DialogResponse(string[] result)
    {
        for (int i = 0; i < result.Length; i++)
        {
            if (result[i].Contains(".mp3"))
                _mainScript.AddSong(result[i]);
            else
            {
                string[] files = Directory.GetFiles(result[i]);
                foreach (string file in files)
                    if (file.EndsWith(".mp3"))
                        _mainScript.AddSong(file);
            }
        }
    }
}
