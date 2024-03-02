using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawGrid : MonoBehaviour
{
    private Camera _cam;
    private DrawLine _line;

    [SerializeField] private int pointCountW;
    [SerializeField] private int pointCountH;

    [SerializeField] private DrawPoint pointPrefab;
    private List<DrawPoint> points;

    [SerializeField] private int pointsToPut;
    public bool DoneDrawing
    {
        get
        {
            return _line.letGo;
        }
    }
 
    private void Awake()
    {
        _cam = Camera.main;
        _line = GetComponentInChildren<DrawLine>();
        GetStats();

        StartCoroutine(Minigame());
    }

    private void GetStats()
    {
        points = new List<DrawPoint>();

        for (int y = 1; y <= pointCountH; y++)
        {
            for (int x = 1; x <= pointCountW; x++)
            {
                Vector3 pointPos = new((Screen.width / (pointCountW + 1)) * x, (Screen.height / (pointCountH + 1))  * y, 1f);
                DrawPoint spawnedPoint = Instantiate(pointPrefab, _cam.ScreenToWorldPoint(pointPos), Quaternion.identity, this.transform);
                points.Add(spawnedPoint);
            }
        }
    }

    IEnumerator Minigame()
    {
        int pointNum = pointsToPut;

        Vector2Int pointSpot = new(Random.Range(0, pointCountW - 1), Random.Range(0, pointCountH - 1));
        int pointSpotInList = pointSpotInList = (Mathf.Clamp(pointSpot.y - 1, 0, pointCountH) * pointCountW) + pointSpot.x;
        DrawPoint chosenPoint = points[pointSpotInList];
        
        for (int i = 1; i <= pointNum; i++)
        {
            chosenPoint.Activate();

            while (chosenPoint.Active)
            {
                yield return null;
            }

            bool newPointPicked = false;
            Vector2Int prevPointSpot = pointSpot;

            while (!newPointPicked)
            {
                pointSpot = new(Random.Range(0, pointCountW), Random.Range(0, pointCountH));

                if (!(Mathf.Abs(prevPointSpot.x - pointSpot.x) < 2 || Mathf.Abs(prevPointSpot.y - pointSpot.y) < 2))
                {

                    pointSpotInList = ((pointSpot.y) * (pointCountH - 2)) + pointSpot.x;
                    chosenPoint = points[pointSpotInList];
                    newPointPicked = true;
                }
            }

            if (i != pointNum)
            {
                _line.LineHitPoint();
            }
            else
            {
                _line.enabled = false;
            }
            
        }

        print("Done");
    }

    private void Update()
    {
        if (DoneDrawing)
        {
            _line.enabled = false;
            StopCoroutine(Minigame());
        }
    }
}