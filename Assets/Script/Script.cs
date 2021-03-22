using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class Script : MonoBehaviour
{
    public GameObject currentSongs;
    public GameObject allPlayLists;

    public AudioSource AudioPlayer;
    
    public GameObject SongButton;

    private ArrayList _songsList = new ArrayList();

    private ArrayList _PlayLists = new ArrayList();
    private ArrayList _PlayListsButton = new ArrayList();

    private Playlist _currentPlayList;

    public Text _currentPlayListName;


    private FilesOpener _filesOpener;
    
    void Start()
    {
        _filesOpener = new FilesOpener(this);
        _PlayLists = Playlist.LoadPlaylists();
        CreatePlayListsButtons();
    }
    

    public void OpenFolderExplorer()
    {
        _filesOpener.OpenFolderExplorer();
    }
    IEnumerator PlaySong(Song song)
    {
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(song.GetPath(), song.GetType()))
        {
            yield return www.SendWebRequest();
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            AudioPlayer.clip = myClip;
            AudioPlayer.Play();
        }
    }
    public void AddSong(string path)
    {
        var btn = Instantiate(SongButton, currentSongs.transform, false);
        Song song = new Song(path);
        btn.GetComponentInChildren<Text>().text = song.GetName();
        btn.GetComponent<Button>().onClick.AddListener(delegate {StartCoroutine(PlaySong(song));});
        _songsList.Add(btn);
        _currentPlayList.AddSong(song);
    }


    void CreatePlayListsButtons()
    {
        foreach (Playlist playlist in _PlayLists)
        {
           AddPlayListButton(playlist);
        }
    }

    void AddPlayListButton(Playlist playlist)
    {
        var btn = Instantiate(SongButton, allPlayLists.transform, false);
        btn.GetComponentInChildren<Text>().text = playlist.GetName();
        btn.GetComponent<Button>().onClick.AddListener(delegate {LoadPlayList(playlist);});
        _PlayListsButton.Add(btn);
    }

    void LoadPlayList(Playlist playlist)
    {
        foreach (Button song in _songsList)
        {
            Destroy(song);
        }

        _currentPlayListName.text = playlist.GetName();
        _currentPlayList = new Playlist(playlist.GetName());
        playlist.Load(this);
    }

    public void SavePlaylist()
    {
        _currentPlayList.Save();
        AddPlayListButton(_currentPlayList);
    }

    public void DeleteSong()
    {
        
    }

    public void ClearAllSongs()
    {
        
    }

    public void PlayListNameChange()
    {
        _currentPlayList.SetName(name);
    }
    
}
