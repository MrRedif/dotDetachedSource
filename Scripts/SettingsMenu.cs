using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public Image fullscreenIm;
    public Sprite fSprite;
    public Sprite tSprite;
    bool mode = true;
    //public Dropdown resolutionDrp;
    //Resolution[] resolutions;
    // Start is called before the first frame update

    private void Start()
    {
        
        /*
        resolutions = Screen.resolutions;
        resolutionDrp.ClearOptions();
        List<string> options = new List<string>();
        int currentRes = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + "@" + resolutions[i].refreshRate.ToString();
            options.Add(option);
            if (resolutions[i].width == Screen.width &&
                resolutions[i].height == Screen.height)
            {
                currentRes = i;
            }
        }
        resolutionDrp.AddOptions(options);
        resolutionDrp.value = currentRes;
        resolutionDrp.RefreshShownValue();
        */
    }
    public void SetVolume(float value)
    {
        if (value != 0)
        {
            mixer.SetFloat("SoundEffects", Mathf.Log10(value) * 20);
        }
        else
        {
            mixer.SetFloat("SoundEffects", -80f);
        }

    }
    public void SetMusicVolume(float value)
    {
        if (value != 0)
        {
            mixer.SetFloat("Music", Mathf.Log10(value) * 20);
        }
        else
        {
            mixer.SetFloat("Music", -80f);
        }

    }
    public void SetFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
        Debug.Log("ChangedFullScreen!");
        mode = !mode;
        if (mode)
        {
            fullscreenIm.sprite = tSprite;
        }
        else
        {
            fullscreenIm.sprite = fSprite;
        }

    }
    /*public void SetResolution(int value)
    {
        Resolution res = resolutions[value];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
    */
    public void ExitApp()
    {
        Debug.Log("EXIT");
        Application.Quit();

    }
}
