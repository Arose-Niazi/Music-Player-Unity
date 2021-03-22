using System;
using UnityEngine;

public class Song
{
    private string _name;
    private string _path;
    private string _run;

    private AudioType _songType;

    public Song(string path)
    {
        _path = path;
        _name = path.Remove(0, path.LastIndexOf("\\", StringComparison.Ordinal)+1);
        _name = _name.Remove(_name.Length - 4, 4);
        _run = "file:///" + _path;
        _songType = AudioType.MPEG;
    }

    public string GetName()
    {
        return _name;
    }

    public string GetPath()
    {
        return _run;
    }

    public string GetSavePath()
    {
        return _path;
    }

    public new AudioType GetType()
    {
        return _songType;
    }
}
