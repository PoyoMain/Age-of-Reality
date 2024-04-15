using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableProp : MonoBehaviour
{
    [SerializeField] private SpriteRenderer outline;
    private bool active;
    
    public void Activate(bool activate)
    {
        if (outline != null) outline.enabled = activate;
        active = activate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out PlayerController _))
        {
            Activate(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent(out PlayerController _))
        {
            Activate(false);
        }
    }

    private void Update()
    {
        if (active && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("WinScene");
        }
    }
}
