using UnityEngine;

public class BarSpawner : MonoBehaviour
{
    public GameObject bar;

    private static GameObject[] bars = new GameObject[512];

    public GameObject panel;
    
    public AudioSource audioPlayer;
    public int objectToMake;
    public float height;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < objectToMake; i++)
        {
            bars[i] = Instantiate(bar, panel.transform);
            bars[i].transform.SetParent(panel.transform, true);
            bars[i].transform.localPosition = new Vector3(-64 + i, 90);
            //bars[i].transform.localScale = 
        }
    }

    private void Update()
    {
        float[] data = new float[objectToMake];
        audioPlayer.GetSpectrumData(data, 0, FFTWindow.Blackman);
        for (int i = 0; i < objectToMake; i++)
        {
            bars[i].transform.localScale =new Vector3(10, 100 + (data[i]<0?-1:1) * (height * data[i]), 5) ;
        }
    }
}
