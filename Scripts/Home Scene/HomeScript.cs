using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScript : MonoBehaviour
{
    [SerializeField] private AudioSource music, button;
    private void Start()
    {
        music.Play();
    }
    public void playerScene()
    {
        button.Play();
        SceneManager.LoadScene("Player Scene");
    }

    public void quitApp()
    {
        button.Play();
        Application.Quit();
    }
}
