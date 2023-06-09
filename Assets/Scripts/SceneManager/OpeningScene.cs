using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour {
    private GameObject pressButton;
    [SerializeField] private ControllerManager controller;
    [SerializeField] private int framePerBlink = 10;
    [SerializeField] private Image blackImage;
    [SerializeField] public AudioClip bgMusic;
    private int frameCount;
    private bool blink;
    public SerialPort joystick;

    void Start() {
        pressButton = GameObject.Find("PressButton");
        frameCount = 0;
        blink = true;

        StartCoroutine(FadeIn());
        joystick = controller.joystick;
        DataManager.instance.GetComponent<AudioSource>().clip = bgMusic;
        DataManager.instance.GetComponent<AudioSource>().Play();
    }

    void FixedUpdate() {
        frameCount %= framePerBlink;
        if (frameCount == 0) {
            pressButton.SetActive(blink);
            blink = !blink;
        }
        frameCount++;

        if (joystick.IsOpen) {
            joystick.Write("0");
            string readVal = joystick.ReadLine();
            if (readVal.Length > 0 && int.Parse(readVal) == 1) SceneManager.LoadScene("CutScene");
        } else {
            if (Input.GetKeyDown(KeyCode.Space)) SceneManager.LoadScene("CutScene");
        }
    }

    IEnumerator FadeIn() {
        Color c = blackImage.color;
        for (float alpha = 1f; alpha >= 0f; alpha -= 0.03f) {
            c.a = alpha < 0 ? 0 : alpha;
            blackImage.color = c;
            yield return null;
        }
    }
}
