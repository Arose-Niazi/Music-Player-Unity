
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
using SimpleFileBrowser;

public class Script : MonoBehaviour
{
    public Sprite defaultImage;
    public GameObject background;
    private Image _background;

    public GameObject filesList;

    public AudioSource AudioPlayer;
    
    public GameObject SongButton;

    private ArrayList _buttonsList = new ArrayList();
    private ArrayList _buttonsTextList = new ArrayList();
    private ArrayList _musicPathList = new ArrayList();
    
    
    // Start is called before the first frame update
    void Start()
    {
        _background = background.GetComponent<Image>();
        _background.sprite = defaultImage;
        
        FileBrowser.SetFilters( false, new FileBrowser.Filter( "Music", ".mp3") );
        FileBrowser.SetDefaultFilter( ".mp3" );
        FileBrowser.SetExcludedExtensions( ".lnk", ".tmp", ".zip", ".rar", ".exe" );

        LoadPlayList();
    }

    // Update is called once per frame

    public void OpenFolderExplorer()
    {
        StartCoroutine( ShowLoadDialogCoroutine() );
    }
    IEnumerator PlaySong(string path)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip("file:///"+ path, AudioType.MPEG))
        {
            yield return www.SendWebRequest();
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            AudioPlayer.clip = myClip;
            AudioPlayer.Play();
        }
        
    }
    private void AddSong(string path)
    {
        var btn = Instantiate(SongButton, filesList.transform, false);
        Debug.Log(path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal)+1));
        btn.GetComponentInChildren<Text>().text = path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal)+1);

        string musicpath = (string) path.Clone();
        btn.GetComponent<Button>().onClick.AddListener(delegate
        {
            StartCoroutine(PlaySong(musicpath));
        });

        _buttonsList.Add(btn);
        _musicPathList.Add(musicpath);
    }
    
    IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog( FileBrowser.PickMode.FilesAndFolders, true, null, null, "Load Files and Folders", "Add" );
		
        if( FileBrowser.Success )
        {
            for (int i = 0; i < FileBrowser.Result.Length; i++)
            {
				
                if (FileBrowser.Result[i].Contains(".mp3"))
                {
                    AddSong(FileBrowser.Result[i]);
                    Debug.Log( FileBrowser.Result[i] + " -> File");
                }
                else
                {
                    string[] files = Directory.GetFiles(FileBrowser.Result[i]);

                    foreach (string file in files)
                        if (file.EndsWith(".mp3"))
                        {
                            AddSong(file);
                        }
                    Debug.Log( FileBrowser.Result[i] + " -> File");
                }
            }
        }
    }
    
    public void SavePlayList()
    {
        string path = "playlists.txt";
 
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        foreach (var music in _musicPathList)
        {
            writer.WriteLine(music);
        }
        writer.Close();
    }
    
    public void LoadPlayList(){
 
        string path = "playlists.txt";
 
        //Read the text from directly from the test.txt file
        try
        {
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                AddSong(reader.ReadLine()); 
            }
            reader.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}
