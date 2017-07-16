using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChoiceController : MonoBehaviour {

    public GameObject choiceMenu;
    private WaveDefiner waveDefiner;
    private WaveEndExtraManager extra;

    private Text timeText;
    private Text waitText;
    private GameObject choice1;
    private GameObject choice2;
    private GameObject choice3;

    void Start()
    {
        waveDefiner = FindObjectOfType<WaveDefiner>();
        extra = FindObjectOfType<WaveEndExtraManager>();
        timeText = choiceMenu.transform.Find("time").GetComponent<Text>();

        choice1 = choiceMenu.transform.Find("left").gameObject;
        choice2 = choiceMenu.transform.Find("right").gameObject;
        choice3 = choiceMenu.transform.Find("points").gameObject;

        waitText = choice3.GetComponentInChildren<Text>();
    }

    void Update()
    {
        if (waveDefiner.spawning)
        {
            choiceMenu.SetActive(false);
        }
        else
        {
            if (extra.extraAvailable)
            {
                choice1.SetActive(true);
                choice2.SetActive(true);
                choice3.SetActive(true);
            }
            else
            {
                choice1.SetActive(false);
                choice2.SetActive(false);
                choice3.SetActive(false);
            }

            choiceMenu.SetActive(true);
            timeText.text = ""+Mathf.Max((int) Mathf.Floor(waveDefiner.TimeToNextWave()), 0);
            waitText.text = "Skip for " + waveDefiner.waveNumber + " points";
        }
    }
}
