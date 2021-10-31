using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Object = UnityEngine.Object;

public class Script : MonoBehaviour
{
    public GameObject currentSongs;
    public GameObject allPlayLists;

    public AudioSource audioPlayer;
    
    public GameObject songButton;

    private ArrayList _songsList = new ArrayList();
    private Playlist _currentPlayList;
    private int _currentIndex = -1;

    private ArrayList _playLists = new ArrayList();
    private readonly ArrayList _playListsButton = new ArrayList();

    public Slider songTimer;

    public Text currentPlayListName;
    
    private FilesOpener _filesOpener;
    
    

    public Text repeatButtonText;
    private bool _repeatMode;
    
    public Text pauseButtonText;
    private bool _pause;
    private float _beforePauseValue = -1f;

    private bool _started;
    
    void Start()
    {
        _filesOpener = new FilesOpener(this);
        _playLists = Playlist.LoadPlaylists();
        CreatePlayListsButtons();
        _currentPlayList = new Playlist("");
    }

    private void Update()
    {
        if(Input.GetKeyDown("escape"))
            Application.Quit();
        
        if(Input.GetKeyDown("space"))
        {
            Pause();
        }
        if(_currentIndex < 0 || !_started) return;
        songTimer.value = audioPlayer.time;
        if(_pause) return;
        if (!audioPlayer.isPlaying)
        {
            if (_repeatMode)
            {
                audioPlayer.Play();
            }
            else
            {
                StartCoroutine(PlaySong(_currentPlayList.GetSong((_currentIndex+1) % _currentPlayList.TotalSongs())));
            }
        }
    }


    public void OpenFolderExplorer()
    {
        _filesOpener.OpenFolderExplorer();
    }
    IEnumerator PlaySong(Song song)
    {
        _currentIndex = _currentPlayList.Find(song);
        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(song.GetPath(), song.GetType()))
        {
            yield return www.SendWebRequest();
            AudioClip myClip = DownloadHandlerAudioClip.GetContent(www);
            audioPlayer.clip = myClip;
            songTimer.maxValue = myClip.length;
            _beforePauseValue = -1f;
            songTimer.value = 0;
            if(!_pause) audioPlayer.Play();
            _started = true;
        }
    }
    public void AddSong(string path)
    {
        var btn = Instantiate(songButton, currentSongs.transform, false);
        Song song = new Song(path);
        btn.GetComponentInChildren<Text>().text = song.GetName();
        btn.GetComponent<Button>().onClick.AddListener(delegate {StartCoroutine(PlaySong(song));});
        _songsList.Add(btn);
        if (!_currentPlayList.AddSong(song))
            Debug.LogWarning("Already in playlsit");
    }


    void CreatePlayListsButtons()
    {
        foreach (Playlist playlist in _playLists)
        {
           AddPlayListButton(playlist);
        }
    }

    void AddPlayListButton(Playlist playlist)
    {
        var btn = Instantiate(songButton, allPlayLists.transform, false);
        btn.GetComponentInChildren<Text>().text = playlist.GetName();
        btn.GetComponent<Button>().onClick.AddListener(
            delegate
            {
                LoadPlayList(playlist);
            });
        _playListsButton.Add(btn);
    }

    void LoadPlayList(Playlist playlist)
    {
        ClearAllSongs();
        currentPlayListName.text = playlist.GetName();
        _currentPlayList = new Playlist(playlist.GetName());
        _currentIndex = -1;
        playlist.Load(this);
    }

    public void SavePlaylist()
    {
        if(currentPlayListName.text.Length < 1)
            return;
        _currentPlayList.SetName(currentPlayListName.text);
        _currentPlayList.Save();
        AddPlayListButton(_currentPlayList);
    }

    public void DeleteSong()
    {
        if(_currentIndex < 0) return;
        Destroy((Object) _songsList[_currentIndex]);
        _songsList.RemoveAt(_currentIndex);
        _currentPlayList.RemoveSongAt(_currentIndex);
        _currentIndex = -1;
    }

    public void ClearAllSongs()
    {
        _currentPlayList = new Playlist("");
        foreach (GameObject song in _songsList)
        {
            Destroy(song);
        }
        _songsList.Clear();
        _currentIndex = -1;
    }

    public void PlayListNameChange()
    {
        _currentPlayList.SetName(name);
    }

    public void ChangeMode()
    {
        if (_repeatMode)
        {
            _repeatMode = false;
            repeatButtonText.text = "Auto Next Song";
        }
        else
        {
            _repeatMode = true;
            repeatButtonText.text = "Repeat Song";
        }
    }
    
    public void Pause()
    {
        if (_pause)
        {
            _pause = false;
            pauseButtonText.text = "Play";
            audioPlayer.Play();
            if (_beforePauseValue > 0)
                audioPlayer.time = _beforePauseValue;
        }
        else
        {
            _pause = true;
            pauseButtonText.text = "Pause";
            _beforePauseValue = audioPlayer.time;
            audioPlayer.Pause();
        }
    }
    
    public void SongTimeChange(Single time){
        if(_currentIndex < 0 || _beforePauseValue > 0) return;
        audioPlayer.time = time;
    }

    public void VolumeChange(Single vol)
    {
        audioPlayer.volume = vol;
    }
    
}
