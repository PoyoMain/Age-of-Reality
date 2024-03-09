using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMinigame : LineMinigameBase
{
    private Camera _cam;
    private MeleeLineDrawer _line;

    [Space(30f)]
    [SerializeField] private int pointCountW;
    [SerializeField] private int pointCountH;
    [Space(15f)]
    [SerializeField] private Transform gridParent;
    [SerializeField] private MeleePoint pointPrefab;
    private List<MeleePoint> points;

    [SerializeField] private int pointsToPut;
 
    private void Awake()
    {
        _cam = GameManager.Instance.GetBattleCamera();
        _line = GetComponentInChildren<MeleeLineDrawer>();
        MakePointGrid();

        StartCoroutine(Minigame());
    }

    private void MakePointGrid()
    {
        points = new List<MeleePoint>();

        for (int y = 1; y <= pointCountH; y++)
        {
            for (int x = 1; x <= pointCountW; x++)
            {
                Vector3 pointPos = new((Screen.width / (pointCountW + 1)) * x, (Screen.height / (pointCountH + 1))  * y, 1f);
                MeleePoint spawnedPoint = Instantiate(pointPrefab, _cam.ScreenToWorldPoint(pointPos), Quaternion.identity, gridParent);
                points.Add(spawnedPoint);
            }
        }
    }

    IEnumerator Minigame()
    {
        int pointsHit = 0;
        int pointNum = pointsToPut;

        Vector2Int pointSpot = new(Random.Range(0, pointCountW - 1), Random.Range(0, pointCountH - 1));
        int pointSpotInList = pointSpotInList = (Mathf.Clamp(pointSpot.y - 1, 0, pointCountH) * pointCountW) + pointSpot.x;
        MeleePoint chosenPoint = points[pointSpotInList];
        
        for (int i = 1; i <= pointNum; i++)
        {
            chosenPoint.Activate();

            while (chosenPoint.Active)
            {
                yield return null;
            }

            pointsHit++;
            totalPercentage = ((float)pointsHit / (float)pointNum) * 100;

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
                DoneDrawing = true;
                _line.enabled = false;
            }
         
        }

        print("Done");
    }

    private void Update()
    {
        if (_line.letGo && !DoneDrawing)
        {
            DoneDrawing = true;
            _line.enabled = false;
            StopCoroutine(Minigame());
        }
    }
}