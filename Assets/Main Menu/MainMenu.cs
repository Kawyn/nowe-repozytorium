using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private static int NUMBER_OF_LEVELS = 14;
    public Sprite[] mains;
    public Sprite[] levels;

    private bool main = true;
    
    private SpriteRenderer spriteRenderer;
    

    private List<GameObject> padlocks = new List<GameObject>();

    int page = 0;
    int level = 0;
    int available = 0;


    int index = 0;

    AudioSource audioSource;
    public AudioClip[] clips = new AudioClip[2];

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject c = transform.GetChild(i).gameObject;

            if (c.CompareTag("Padlock"))
                padlocks.Add(c);

            c.SetActive(false);
        }



        available = PlayerPrefs.GetInt("save", 0) + 1; 
        Debug.Log(available);
    }
    
    private void Update()
    {
        if (main)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                index--;

                if (index < 0)
                    index = mains.Length - 1;

                audioSource.clip = clips[0];
                audioSource.Play();

                spriteRenderer.sprite = mains[index];
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                index++;

                if (index > mains.Length - 1)
                    index = 0;

                audioSource.clip = clips[0];
                audioSource.Play();

                spriteRenderer.sprite = mains[index];
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {

                audioSource.clip = clips[1];
                audioSource.Play();

                // Kto powiedział, że nie umiem w słicze?
                switch (index)
                {
                    case 0:

                        for (int i = 0; i < 8; i++)
                        {
                            if (i < available && i < NUMBER_OF_LEVELS)
                                continue;

                           
                            padlocks[i].SetActive(true);
                        }
                        page = 0;
                        index = 0;
                        spriteRenderer.sprite = levels[0];
                        main = false;
                        break;

                    case 1:
                        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
                        break;
                }
            }
        }
        // Wybór poziomów!
        else
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (index <= 3)
                    return;

                ChangeLevel(-4);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                if (index == 4)
                    return;

                ChangeLevel(-1);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (index >= 4)
                    return;

                ChangeLevel(4);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (index == 3 )
                    return;

                ChangeLevel(1);
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {

                for (int i = 0; i < 8; i++)
                {
                    padlocks[i].SetActive(false);
                }

                spriteRenderer.sprite = mains[0];
                index = 0;
                main = true;
            }
            else if (Input.GetKeyDown(KeyCode.Return))
            {
                if (available > index && index < NUMBER_OF_LEVELS)
                {
                    StartCoroutine(LoadScene(1 + index + page * 8));

                    audioSource.clip = clips[1];
                    audioSource.Play();
                }
            }
        }
    }
    IEnumerator LoadScene(int id)
    {
        yield return new WaitForSeconds(0.05f);
        SceneManager.LoadScene(id);
    }

    private void ChangeLevel(int offset) 
    {
        if (index + 8 * page + offset >= available -1)
            return;
       if(index + offset < 0)
        {
            if (page > 0)
            {
                page--;
                for (int i = page * 8; i < page * 8 + 8; i++)
                {
                    if (i < available && i < NUMBER_OF_LEVELS)
                        continue;
                    if (i <= NUMBER_OF_LEVELS)
                    {
                        padlocks[i - 8 * page].SetActive(false);
                        continue;
                    }

                    padlocks[i - 8 * page].SetActive(true);
                }
                index = 0;
                spriteRenderer.sprite = levels[page * 8 + index];
                main = false;
                index += offset;
              
                return;
            }
            return;
        }
       else if(index + offset > 7)
        {
            if(page < 1)
            {   
                page++;
                for (int i = page * 8; i < page * 8 + 8; i++)
                {
                    if (i < available && i < NUMBER_OF_LEVELS)
                        continue;

                    if (i >= NUMBER_OF_LEVELS)
                    {
                        padlocks[i - 8 * page].SetActive(false);
                        continue;
                    }
                    padlocks[i - 8 * page].SetActive(true);
                }
                index = 0;
                spriteRenderer.sprite = levels[page * 8 + index];
                main = false;
        
                return;
            }
            return;
        }

        if (index + offset >= 0)
        {
            index += offset;

            audioSource.clip = clips[0];
            audioSource.Play();
            if (index + page * 8 > 0)
                spriteRenderer.sprite = levels[page * 8 + index];
        }
    }
}
