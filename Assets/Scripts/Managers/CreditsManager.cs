using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(PlayableDirector))]
public class CreditsManager : MonoBehaviour
{
    private PlayableDirector _director;

    private void Awake()
    {
        _director = GetComponent<PlayableDirector>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_director.time >= _director.duration)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
