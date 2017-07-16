using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour {

    private SceneChanger changer;
    private WaveEndExtraManager extra;

	void Start () {
        changer = GetComponent<SceneChanger>();
        extra = FindObjectOfType<WaveEndExtraManager>();
	}
	
	void Update () {
        if(Input.GetKeyDown("escape"))
        {
            changer.LoadScene("End");
        }

        if (Input.GetKeyDown("1"))
        {
            extra.ChooseExtra(1);
        }

        if (Input.GetKeyDown("2"))
        {
            extra.ChooseExtra(2);
        }
	}
}
