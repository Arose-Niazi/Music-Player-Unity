
using System;
using System.Collections;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;
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
    }

    // Update is called once per frame

    public void OpenFolderExplorer()
    {
        string path = EditorUtility.OpenFolderPanel("Load mp3 files", "C:\\Users\\arose\\Downloads\\Music\\New folder", "");
        if(path.Length < 1) return; 
        string[] files = Directory.GetFiles(path);

        foreach (string file in files)
            if (file.EndsWith(".mp3"))
            {
                AddSong(file);
            }
                
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
    }
}
