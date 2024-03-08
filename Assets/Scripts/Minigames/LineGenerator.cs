using System.Collections.Generic;
using UnityEngine;


public class LineGenerator : MonoBehaviour
{

    public GameObject linePrefab;
    public Camera cam;
    Line activeLine;

    public float distanceNeeded = 0;
    public float distanceTravelled = 0;

    public float totalPercentage = 0;
    public float totalPoints = 0;
    public float closePoints = 0;

    public bool DoneDrawing = false;

    //Start and endpoint of line on screen
    public GameObject[] lines;
    public bool backwards = false;
    public float tempAngle;
    public float pastAngle;

    public List<LineRenderer> lineRenderers;
    //keeps the starting end end point of each line recorded.
    //might be needed?
    public List<Vector2> startingPos;
    public List<Vector2> endingPos;
    public GameObject battleParent;
    public List<Vector2> drawnPoints;
    public Vector2 lastMousePosition;
    public Vector2 lastDirection;
    public bool isDirectionFlipped = false;

    public float flipThreshold = 0f;
    public int listLength { get; set; }
    public void OnEnable()
    {
        cam = GameManager.Instance.GetBattleCamera();
        lineRenderers = new List<LineRenderer>();
        startingPos = new List<Vector2>();
        endingPos = new List<Vector2>();
        drawnPoints = new List<Vector2>();
        distanceNeeded = 0;
        battleParent = transform.parent.parent.gameObject;

        if (lines == null)
        {
            return;
        }
        else
        {
            //gets the linerenderer of each object in the lines array
            foreach (GameObject obj in lines)
            {
                LineRenderer templineRenderer = obj.GetComponent<LineRenderer>();
                lineRenderers.Add(templineRenderer);
            }
            //gets the first and last value of each linerender position into another list
            foreach (LineRenderer obj in lineRenderers)
            {
                startingPos.Add(obj.GetPosition(0) + gameObject.transform.position);
                endingPos.Add(obj.GetPosition(obj.positionCount - 1) + gameObject.transform.position);
                distanceNeeded += Vector2.Distance(obj.GetPosition(0), obj.GetPosition(obj.positionCount - 1));
                listLength = startingPos.Count;
                //Debug.Log(listLength);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(mousePos);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newLine = Instantiate(linePrefab, transform);
            activeLine = newLine.GetComponent<Line>();
            activeLine.setRefrence(this);
            //startingPos.Add(mousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //endingPos.Add(mousePos);
            activeLine = null;
            if (totalPercentage > 90)
            {
                DoneDrawing = true;
            }
        }

        if (activeLine != null)
        {
            activeLine.UpdateLine(mousePos);

        }


        // Check if the mouse has moved

    }

    public float DistanceToClosestPoint(Vector2 point, Vector2 start, Vector2 end)
    {
        // Calculate the direction vector of the line
        Vector2 lineDirection = end - start;

        // Calculate the vector from the start point to the input point
        Vector2 pointToStart = point - start;

        // Calculate the projection of pointToStart onto the lineDirection
        float t = Vector2.Dot(pointToStart, lineDirection) / Vector2.Dot(lineDirection, lineDirection);

        // Clamp t to ensure it's within the bounds of the line segment
        t = Mathf.Clamp01(t);

        // Calculate the closest point on the line
        Vector2 closestPoint = start + t * lineDirection;

        // Calculate the distance between the input point and the closest point on the line
        float distance = Vector2.Distance(point, closestPoint);

        return distance;
    }

    public int GetLineRenderersListLength()
    {
        return listLength;
    }
}
