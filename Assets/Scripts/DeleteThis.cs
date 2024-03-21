using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteThis : MonoBehaviour
{
    public void SwitchScenes(int SceneID)
    {
        SceneManager.LoadScene(SceneID);
    }
}
