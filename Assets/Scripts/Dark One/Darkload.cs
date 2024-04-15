using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Darkload : MonoBehaviour
{
    // Start is called before the first frame update
    public Number n;
    public GameObject dark;
    void Start()
    {
        n = GetComponent<Number>();
        dark.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Number.won > 0){
            dark.SetActive(true);
        }
    }

    public void darkLands(){
        SceneManager.LoadScene(6);
    }
}
