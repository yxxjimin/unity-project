using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndingCam : MonoBehaviour {
    [SerializeField] private CutSceneDialogue dialogue;
    [SerializeField] Image blackImage;
    [SerializeField] private AudioClip bgMusic;
    private float walkTime = 3f;
    public PlayerController player;
    private Vector3 playerPosition;
    private float initX;
    private float offsetZ = -17f;
    private bool coroutineStarted;
    private bool fadeStarted;

    void Start() {
        playerPosition = player.transform.position;
        initX = playerPosition.x;
        transform.position = new Vector3(initX, 4f, playerPosition.z + offsetZ);
        Color c = blackImage.color;
        c.a = 0;
        blackImage.color = c;

        coroutineStarted = false;
        fadeStarted = false;

        DataManager.instance.GetComponent<AudioSource>().clip = bgMusic;
        DataManager.instance.GetComponent<AudioSource>().Play();

        StartCoroutine(FadeIn());
    }

    void Update() {
        if (dialogue.dialogueFinished) {
            if (walkTime > 0) walkTime -= Time.deltaTime;
            else if (!coroutineStarted) {
                walkTime = 4f;
                StartCoroutine(RotateCamera());
            } else if (!fadeStarted) {
                StopAllCoroutines();
                StartCoroutine(FadeOut());
            }
        } else {
            playerPosition = player.transform.position;
            transform.position = new Vector3(initX, 4f, playerPosition.z + offsetZ);
        }
    }

    IEnumerator RotateCamera() {
        coroutineStarted = true;
        Quaternion rot = transform.rotation;
        for (float angle = 0f; angle >= -10f; angle -= 0.0003f) {
            rot.x = angle;
            transform.rotation = rot;
            yield return null;
        }
    }

    IEnumerator FadeOut() {
        fadeStarted = true;
        Color c = blackImage.color;
        for (float alpha = 0f; alpha <= 1f; alpha += 0.02f) {
            c.a = alpha > 1 ? 1 : alpha;
            blackImage.color = c;
            yield return null;
        }
        SceneManager.LoadScene("Opening");
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
