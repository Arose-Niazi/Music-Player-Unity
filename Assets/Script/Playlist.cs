using System;
using System.Collections;
using System.IO;

public class Playlist
{
    private ArrayList _songs;
    private string _name;
    
    
    public Playlist(string name)
    {
        _name = name;
        _songs = new ArrayList();
    }

    public bool AddSong(Song song)
    {
        foreach (Song oldSongs in _songs)
        {
            if (oldSongs.GetName().Equals(song.GetName()))
                return false;
        }

        _songs.Add(song);
        return true;
    }

    public void RemoveSong(Song song)
    {
        _songs.Remove(song);
    }
    
    public void RemoveSongAt(int song)
    {
        _songs.RemoveAt(song);
    }

    public void SavePlayList()
    {
        
    }

    public void SetName(string name)
    {
        _name = name;
    }

    public string GetName()
    {
        return _name;
    }

    public int Find(Song song)
    {
        return _songs.IndexOf(song);
    }

    public void Save()
    {
        string path = "playlists/"+_name+".playlist";
        StreamWriter writer = new StreamWriter(path, true);
        foreach (Song song in _songs)
        {
            writer.WriteLine(song.GetSavePath());
        }
        writer.Close();
    }

    public Song GetSong(int song)
    {
        if (song > _songs.Count) return null;
        return (Song) _songs[song];
    }

    public int TotalSongs()
    {
        return _songs.Count;
    }

    public void Load(Script mainScript)
    {
        string path = "playlists/"+_name+".playlist";
        try
        {
            StreamReader reader = new StreamReader(path);
            while (!reader.EndOfStream)
            {
                mainScript.AddSong(reader.ReadLine()); 
            }
            reader.Close();
        }
        catch (Exception e)
        {
            // ignored
        }
    }

    public static ArrayList LoadPlaylists()
    {
        ArrayList playlists = new ArrayList();
        //try
        //{
            string[] files = Directory.GetFiles("playlists");
            foreach (string file in files)
                if (file.EndsWith(".playlist"))
                {
                    string f = file.Remove(0, file.LastIndexOf("\\", StringComparison.Ordinal) + 1);
                    f = f.Remove(f.Length - 9, 9);
                    Playlist playlist = new Playlist(f);
                    playlists.Add(playlist);
                }
        /*}
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }*/

        return playlists;
    }
    
    
}
