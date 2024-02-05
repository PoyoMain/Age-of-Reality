using System.Collections.Generic;
using UnityEngine;


public class LineGenerator : MonoBehaviour
{

    public GameObject linePrefab;
    public Camera cam;
    Line activeLine;

    public float totalPercentage = 100;

    //Start and endpoint of line on screen
    //has to be manually put in, update to automatically get for each line based off of minigame
    public Vector2 lineStart;
    public Vector2 endOfLine;
    public GameObject[] lines;

    public List<LineRenderer> lineRenderers;
    //keeps the starting end end point of each line recorded.
    //might be needed?
    public List<Vector2> startingPos;
    public List<Vector2> endingPos;
    public int listLength { get; set; }
    public void Start()
    {
        cam = Camera.main;
        lineRenderers = new List<LineRenderer>();
        startingPos = new List<Vector2>();
        endingPos = new List<Vector2>();

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
                startingPos.Add(obj.GetPosition(0));
                endingPos.Add(obj.GetPosition(obj.positionCount - 1));
                listLength = startingPos.Count;
                //Debug.Log(listLength);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject newLine = Instantiate(linePrefab);
            activeLine = newLine.GetComponent<Line>();
            activeLine.setRefrence(this);
            //startingPos.Add(mousePos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            //endingPos.Add(mousePos);
            activeLine = null;
        }

        if (activeLine != null)
        {
            activeLine.UpdateLine(mousePos);
        }
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
