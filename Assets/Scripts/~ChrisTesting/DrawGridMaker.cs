using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGridMaker : MonoBehaviour
{
    private Camera _cam;

    private float screenWidth;
    private float screenHeight;

    [Range(1f, 1920f)]
    [SerializeField] private float densityW;
    [Range(1f, 1080f)]
    [SerializeField] private float densityH;

    [SerializeField] private GameObject point;

    private void Awake()
    {
        _cam = Camera.main;
        GetStats();
    }

    private void GetStats()
    {
        screenWidth = _cam.orthographicSize * Screen.width / Screen.height;

        screenHeight = _cam.orthographicSize * Screen.height / Screen.width;

        Debug.LogFormat("Width: {0}\nHeight: {1}", Screen.width, Screen.height);
        Debug.Log(_cam.ScreenToWorldPoint(new Vector3(1920, 0, -10)));

        float pointCountW = Screen.width / densityW;
        float pointCountH = Screen.height / densityH;

        for (int y = 0; y <= pointCountH; y++)
        {
            for (int x = 0; x <= pointCountW + 1; x++)
            {
                Vector2 pointPos = new(densityW * x, densityH * y);
                Instantiate(point, _cam.ScreenToWorldPoint(pointPos), Quaternion.identity, this.transform);
            }
        }
    }

}
