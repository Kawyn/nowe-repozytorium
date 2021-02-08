using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    const int SIZE_X = 14;
    const int SIZE_Y = 7;

    public AudioClip[] sounds;

    public bool testMode = false;

    public Sprite[] numbers = new Sprite[10];

    public SpriteRenderer tens;
    public SpriteRenderer unities;
    

    public int length = 10;

    public Vector2Int enterance = new Vector2Int(0, 0);
    public Vector2Int plugPosition = new Vector2Int(1, 0);
    public GameObject plug;

    private List<Plug> plugs = new List<Plug>();
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        plug = GameObject.Find("Plug");
        plug.transform.position = (Vector2)plugPosition;
        plug.GetComponent<Plug>().SetRotation(plugPosition - enterance);

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Player"))
        {
            plugs.Add(g.GetComponent<Plug>());
        }
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("Battery"))
        {
            batteries.Add(g.GetComponent<Battery>());
        }
        Refresh();
    }


    List<Battery> batteries = new List<Battery>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void Refresh()
    {
        int u = length % 10;
        int t = (length - u) / 10;

        if (t == 0)
            tens.sprite = null;
        else
            tens.sprite = numbers[t];

        unities.sprite = numbers[u];
        foreach (Battery b in batteries)
            b.Refresh();
       
    }

    public void NextLevel()
    {
        if (testMode)
            return;

        int i = PlayerPrefs.GetInt("save", 0);
        int n = SceneManager.GetActiveScene().buildIndex;

        if (i < n)
            PlayerPrefs.SetInt("save", n);

        SceneManager.LoadScene(n + 1);
    }


    private AudioSource audioSource;
    public void PlaySound(string clipName)
    {
        if (clipName == "m")
            audioSource.clip = sounds[1];
        else if (clipName == "c")
            audioSource.clip = sounds[0];
        else if (clipName == "b")
            audioSource.clip = sounds[2];
        else if (clipName == "s")
            audioSource.clip = sounds[3];
        else if (clipName == "sss")
            audioSource.clip = sounds[4];
        audioSource.Play();
    }
}

public struct Arguments
{
    public bool control;
    public Vector2 source;
    public bool off;

    public Arguments(Vector2 s, bool c, bool off)
    {
        source = s;
        control = c;
        this.off = off;
    }
}