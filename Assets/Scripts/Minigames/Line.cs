using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Line : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public LineGenerator lineGenerator;
    List<Vector2> points;
    public GameObject testLine;
    float tempDistance = 100;
    public Vector2 pastpastPoint;
    public Vector2 pastPoint;
    public float tempAngle;
    public float pastAngle;
    public EdgeCollider2D lineCollider;
    public float proximityThreshold = .1f;
    //bool backwards = false;
    public int timeCheck = 0;


    public void setRefrence(LineGenerator refrence)
    {
        lineGenerator = refrence;
    }

    public void UpdateLine(Vector2 position)
    {
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(position);

            for (int i = 0; i < lineGenerator.listLength; i++)
            {
                //Debug.Log(lineGenerator.DistanceToClosestPoint(position, lineGenerator.startingPos[i], lineGenerator.endingPos[i]));
                if (tempDistance > lineGenerator.DistanceToClosestPoint(position, lineGenerator.startingPos[i], lineGenerator.endingPos[i]))
                {
                    tempDistance = lineGenerator.DistanceToClosestPoint(position, lineGenerator.startingPos[i], lineGenerator.endingPos[i]);
                }

            }
            //lineGenerator.DistanceToClosestPoint(position, lineGenerator.lineStart, lineGenerator.endOfLine);
            //Debug.Log(index);
            if (tempDistance < .6f)
            {
                lineGenerator.closePoints++;
                pastPoint = position;
            }
            pastPoint = position;
            pastpastPoint = pastPoint;
            SetCollider(lineRenderer);
            //lineGenerator.totalPercentage = 100 * (lineGenerator.closePoints / lineGenerator.totalPoints);
            //Debug.Log(index);
            //Debug.Log(tempDistance);
            return;
        }





        if (Vector2.Distance(points.Last(), position) > proximityThreshold)
        {
            SetPoint(position);
            //Debug.Log(lineGenerator.listLength);
            for (int i = 0; i < lineGenerator.listLength; i++)
            {
                //Debug.Log(lineGenerator.DistanceToClosestPoint(position, lineGenerator.startingPos[i], lineGenerator.endingPos[i]));
                if (tempDistance > lineGenerator.DistanceToClosestPoint(position, lineGenerator.startingPos[i], lineGenerator.endingPos[i]))
                {
                    tempDistance = lineGenerator.DistanceToClosestPoint(position, lineGenerator.startingPos[i], lineGenerator.endingPos[i]);

                    //Debug.Log(index);
                }

            }
            /*
            if (lineGenerator.backwards == false)
            {
                pastAngle = (Mathf.Atan2(pastPoint.y - pastpastPoint.y, pastPoint.x - pastpastPoint.x) * 180 / Mathf.PI);
                pastAngle = Mathf.RoundToInt(pastAngle);
            }
            tempAngle = (Mathf.Atan2(position.y - pastPoint.y, position.x - pastPoint.x) * 180 / Mathf.PI);
            tempAngle = Mathf.RoundToInt(tempAngle);


            if (pastAngle < 0)
            {
                pastAngle *= -1;
                pastAngle += -180;
            }

            if (tempAngle < 0)
            {
                tempAngle *= -1;
                tempAngle += -180;
            }
            //Debug.Log("past angle: " + pastAngle);
            //Debug.Log("tempAngle" + -tempAngle);
            */
            if (tempDistance < .6f)
            {
                lineGenerator.closePoints++;
                lineGenerator.distanceTravelled += Vector2.Distance(pastPoint, position);
                if (ArePointsInProximity2(position))
                {
                    //Debug.Log("overlap");
                    lineGenerator.closePoints--;
                    lineGenerator.distanceNeeded += Vector2.Distance(pastPoint, position);
                }
            }

            //Debug.Log(-tempAngle + " < " + pastAngle + "+15 && " + -tempAngle + " > " + pastAngle + "-15");
            /*
            if (-tempAngle < pastAngle + 35 && -tempAngle > pastAngle - 35)
                if (lineGenerator.backwards)
                {

                    //Debug.Log(backwards);
                    //Debug.Log("past angle: " + pastAngle);
                    //Debug.Log("Current angle Mathf.Atan2(" + position.y + " - " + pastPoint.y + ", " + position.x + " - " + pastPoint.x + ") * 180 / Mathf.PI =" + tempAngle);
                    //Debug.Log(tempAngle);
                    lineGenerator.closePoints--;
                    lineGenerator.backwards = true;
                    //lineGenerator.distanceTravelled += Vector2.Distance(pastPoint, position);
                    //Debug.Log(lineGenerator.backwards);
                }
                else
                {
                    pastAngle = tempAngle;
                    lineGenerator.backwards = false;
                    //Debug.Log(lineGenerator.backwards);
                }
            */
            //Debug.Log(lineGenerator.backwards);
            pastpastPoint = pastPoint;
            pastPoint = position;

            //Debug.Log(index);
            //Debug.Log(tempDistance);


            /*
            if (position != pastPoint)
            {
                // Calculate the direction from the last position to the current position
                Vector2 direction = position - pastPoint;

                // Normalize the direction vector if needed
                direction.Normalize();

                // Output the direction to the console (you can modify this part based on your requirements)
                //Debug.Log("Mouse Direction: " + direction);

                // Check if the direction has flipped within the specified threshold
                if (Vector2.Dot(direction, lineGenerator.lastDirection) < lineGenerator.flipThreshold)
                {
                    lineGenerator.isDirectionFlipped = true;
                    Debug.Log("Direction Flipped!");
                }
                else
                {
                    lineGenerator.isDirectionFlipped = false;
                }

                // Update the last mouse position and direction
                //lastMousePosition = position;
                //lastDirection = direction;
            }
            */
            // Use isDirectionFlipped in your game logic to determine if the direction has changed within the specified threshold
            //if (lineGenerator.isDirectionFlipped)
            //{
            //    // Your code for handling the flipped direction goes here
            //    //Debug.Log("Handling Flipped Direction!");
            //}

            if (lineGenerator.distanceNeeded > lineGenerator.distanceTravelled)
            {
                lineGenerator.totalPercentage = 100 * ((lineGenerator.distanceTravelled / lineGenerator.distanceNeeded) * (lineGenerator.closePoints / lineGenerator.totalPoints));
            }
            else
            {
                lineGenerator.totalPercentage = 100 * ((lineGenerator.distanceNeeded / lineGenerator.distanceTravelled) * (lineGenerator.closePoints / lineGenerator.totalPoints));
            }
            lineGenerator.totalPercentage = Mathf.RoundToInt(lineGenerator.totalPercentage);
            SetCollider(lineRenderer);
            //lineGenerator.totalPercentage = 100 * (lineGenerator.closePoints / lineGenerator.totalPoints);
            //Debug.Log(lineGenerator.closePoints);
            //Debug.Log(lineGenerator.closePoints / lineGenerator.totalPoints);
        }

    }

    void SetPoint(Vector2 point)
    {
        points.Add(point);
        tempDistance = 100f;
        lineGenerator.totalPoints++;

        lineGenerator.drawnPoints.Add(point);

        //lineGenerator.drawnPoints.Add(new Vector2(point.x - lineGenerator.transform.position.x, point.y - lineGenerator.transform.position.y));
        //Debug.Log(Vector2.Distance(point, testLine.transform.position));
        /*if (lineGenerator.drawnPoints.Count > 5)
        {
            pastpastPoint = lineGenerator.drawnPoints[lineGenerator.drawnPoints.Count - 4];
            pastPoint = lineGenerator.drawnPoints[lineGenerator.drawnPoints.Count - 2];
        }*/

        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
    }

    void SetCollider(LineRenderer lRend)
    {
        List<Vector2> edges = new();

        for (int point = 0; point < lRend.positionCount; point++)
        {
            Vector3 lRendPoint = lRend.GetPosition(point);
            edges.Add(new Vector2(lRendPoint.x - lineGenerator.transform.position.x, lRendPoint.y - lineGenerator.transform.position.y));
        }

        lineCollider.SetPoints(edges);
    }

    bool ArePointsInProximity2(Vector2 targetPos)
    {
        if (lineGenerator.drawnPoints.Count > 1)
        {
            for (int check = 1; check < lineGenerator.drawnPoints.Count; check++)
            {
                Vector2 point = lineGenerator.drawnPoints[check - 1];
                if (Mathf.Abs(Vector2.Distance(point, targetPos)) < proximityThreshold)
                {
                    //Debug.Log(proximityThreshold);
                    //Debug.Log(Mathf.Abs(Vector2.Distance(point, targetPos)));
                    return true;
                }
            }
        }
        return false;
    }
}
