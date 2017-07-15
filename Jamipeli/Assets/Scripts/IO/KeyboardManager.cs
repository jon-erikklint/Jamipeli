using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour {

    private SceneChanger changer;

	void Start () {
        changer = GetComponent<SceneChanger>();
	}
	
	void Update () {
        if(Input.GetKeyDown("escape"))
        {
            changer.LoadScene("End");
        }
	}
}
