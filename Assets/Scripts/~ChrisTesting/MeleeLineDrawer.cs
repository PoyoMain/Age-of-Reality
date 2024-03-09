using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeLineDrawer : MonoBehaviour
{
    private Camera _cam;

    [SerializeField] private LineRenderer linePrefab;
    private LineRenderer line;
    private EdgeCollider2D lineCollider;

    private Vector3 previousPosition;
    [SerializeField] private float minDistance = 0.01f;

    [HideInInspector] public bool letGo;

    private void OnEnable()
    {
        _cam = GameManager.Instance.GetBattleCamera();
        previousPosition = transform.position;
        linePrefab.positionCount = 1;
        letGo = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            line = Instantiate(linePrefab, transform);
            previousPosition = transform.position;
            lineCollider = line.gameObject.GetComponent<EdgeCollider2D>();
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 currentPosition = _cam.ScreenToWorldPoint(Input.mousePosition);
            currentPosition.z = 0f;

            if (Vector3.Distance(currentPosition, previousPosition) > minDistance)
            {
                if (previousPosition == transform.position)
                {
                    line.SetPosition(0, currentPosition);
                }
                else
                {
                    line.positionCount++;
                    line.SetPosition(line.positionCount - 1, currentPosition);
                    SetCollider(line);
                }

                previousPosition = currentPosition;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            letGo = true;
        }
    }

    void SetCollider(LineRenderer lRend)
    {
        List<Vector2> edges = new();

        for (int point = 0; point < lRend.positionCount; point++)
        {
            Vector3 lRendPoint = lRend.GetPosition(point);
            Vector3 localLRendPoint = transform.InverseTransformPoint(lRendPoint);
            edges.Add(new Vector2(localLRendPoint.x, localLRendPoint.y));
        }

        lineCollider.SetPoints(edges);
    }

    public void LineHitPoint()
    {
        Destroy(line.gameObject);
        line = Instantiate(linePrefab, transform);
        previousPosition = transform.position;
        lineCollider = line.gameObject.GetComponent<EdgeCollider2D>();
    }
}
