using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneMaster : MonoBehaviour
{
    public static SceneMaster instance;
    Animator anim;
    public GameObject options;
    bool paused;
    bool lockManager;
    bool changingScenes;
    string nextSceneName;
    Canvas cnvs;
    //Loading
    public Image loadingTex;
    public Sprite[] loadingTexsGood;
    public Sprite[] loadingTexsBad;
    public Sprite[] loadingTexsNormal;
    public enum Context { normal,good,bad}
    public Context currentContext = Context.normal;

    //Parts
    public bool hasHead = true;
    public bool hasArms = true;
    public bool hasLegs = true;

    //Sounds
    [Header("Sounds")]
    public AudioSource source;
    public AudioClip badSound;
    public AudioClip normalSound;
    public AudioClip goodSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        anim = GetComponent<Animator>();
        cnvs = GetComponentInChildren<Canvas>();
        cnvs.worldCamera = FindObjectOfType<Camera>();
        FindNewCam();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !changingScenes)
        {

            if (paused == false)
            {
                OpenOptions();
            }
            else
            {
                CloseOptions();
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && !changingScenes && !paused)
        {
            LoadLevel(SceneManager.GetActiveScene().name,Context.bad);
            hasLegs = true;
            hasArms = true;
            hasHead = true;
        }
    }

    IEnumerator GoSceneRoutine(string name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);

        while (!operation.isDone)
        {
            float prg = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
        if (operation.isDone)
        {

            lockManager = false;
            changingScenes = false;
            anim.SetTrigger("Finish");
        }

        FindNewCam();
    }
    public void FinishedAnimation()
    {
        StartCoroutine(GoSceneRoutine(nextSceneName));
    }
    //Call this one !!
    public void LoadLevel(string name)
    {
        if (lockManager == true) return;

        CloseOptions();
        nextSceneName = name;
        lockManager = true;
        changingScenes = true;
        currentContext = Context.normal;
        anim.SetTrigger("Start");

    }
    public void LoadLevel(string name,Context con)
    {
        if (lockManager == true) return;

        CloseOptions();
        nextSceneName = name;
        lockManager = true;
        changingScenes = true;
        currentContext = con;
        anim.SetTrigger("Start");

    }

    void CloseOptions()
    {
        Time.timeScale = 1f;
        options.SetActive(false);
        paused = false;
    }
    void OpenOptions()
    {
        Time.timeScale = 0f;
        options.SetActive(true);
        paused = true;
    }

    public void ChangeLoadingTex()
    {

        switch (currentContext)
        {
            case Context.normal:
                loadingTex.sprite = loadingTexsNormal[Random.Range(0, loadingTexsNormal.Length)];
                loadingTex.color = Color.white;
                break;
            case Context.good:
                loadingTex.sprite = loadingTexsGood[Random.Range(0, loadingTexsGood.Length)];
                loadingTex.color = Color.green;
                break;
            case Context.bad:
                loadingTex.sprite = loadingTexsBad[Random.Range(0, loadingTexsBad.Length)];
                loadingTex.color = Color.red;
                break;
            default:
                break;
        }
    }
    public void ReleasePlayer()
    {
        PlayerInfo.ChangeControl(true);
    }
    public void LockPlayer()
    {
        PlayerInfo.ChangeControl(false);
    }

    public void HoloAnimPlay()
    {
        StartCoroutine(FindObjectOfType<PlayerInfo>().HoloAnim());
        hasLegs = true;
        hasArms = true;
        hasHead = true;
    }

    public void FindNewCam() {
        Camera newCam = FindObjectOfType<Camera>(); ;
        cnvs.worldCamera = newCam;
        newCam.aspect = 1.7777f;
    }

    public void PlayClip()
    {
        switch (currentContext)
        {
            case Context.normal:
                source.clip = normalSound;
                break;
            case Context.good:
                source.clip = goodSound;
                break;
            case Context.bad:
                source.clip = badSound;
                break;
            default:
                break;
        }
        source.Play();
    }

    public void ReloadScene()
    {
        if (!changingScenes && !paused)
        {
            LoadLevel(SceneManager.GetActiveScene().name, Context.bad);
            hasLegs = false;
            hasArms = false;
            hasHead = false;
        }
    }
}
