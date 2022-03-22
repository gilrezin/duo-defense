 using UnityEngine;
 using System.Collections.Generic;
 using System.Collections;

public class PlayerMouseController : MonoBehaviour
{  
    [SerializeField]
    private float lineSeperationDistance = 0.2f;
    [SerializeField]
    private float lineWidth = 0.1f;
    [SerializeField]
    private Color lineColor = Color.black;
    [SerializeField]
    private int lineCapVertices = 5;
    public GameManager gameManager;
    private GameObject currentLineObject;
    private List<GameObject> lines;
    private List<Vector2> currentLine;
    private LineRenderer currentLineRenderer;
    private EdgeCollider2D currentLineEdgeCollider;
    
    private EdgeCollider2D currentLineEdgeTrigger;
    private bool drawing = false;

    private Camera mainCamera;

    private void Awake() 
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // if the mouse is pressed down and there exists some draw value, then create a new brush instance
        if (Input.GetKeyDown(KeyCode.Mouse0) && gameManager.getDrawBarValue() > 0)
        {
            OnStartDraw();
        }
        else if (!Input.GetKey(KeyCode.Mouse0) && drawing) // disables drawing function if currently drawing
        {
            /*Debug.Log("GetKey: " + Input.GetKey(KeyCode.Mouse0) + "\nDrawing: " + drawing);
            // debugging code informing why the line ended
            if (gameManager.getDrawBarValue() <= 0) 
            {
                Debug.Log("Draw bar depleted");
            }
            else if (!Input.GetKey(KeyCode.Mouse0)) 
            {
                Debug.Log("Mouse click lifted");
            }
            else 
            {
                Debug.Log("I don't know why, but drawing ended");
            }*/
            OnEndDraw();
        }
    }

    private void OnStartDraw() 
    {
        //Debug.Log("Started line");
        StartCoroutine("Drawing");
    }

    private void OnEndDraw() 
    {
        drawing = false;
        //Debug.Log("ended line");
        // allows for collisions only once the line is complete
        currentLineObject.AddComponent<PlayerWall>();
        //Debug.Log("wall script enabled");
    }

    IEnumerator Drawing()
    {
        drawing = true;
        StartLine();
        while(drawing && gameManager.getDrawBarValue() > 0) {
            //Debug.Log("Adding Point");
            AddPoint(GetCurrentWorldPoint());
            yield return null;
        }
        EndLine();
    }

    private void StartLine() 
    {
        // instantiates a new line
        currentLine = new List<Vector2>();
        currentLineObject = new GameObject();
        currentLineObject.name = "Line";
        currentLineObject.transform.parent = transform;
        currentLineRenderer = currentLineObject.AddComponent<LineRenderer>();
        currentLineEdgeCollider = currentLineObject.AddComponent<EdgeCollider2D>();

        
        

        // set settings
        currentLineRenderer.positionCount = 0;
        currentLineRenderer.startWidth = lineWidth;
        currentLineRenderer.endWidth = lineWidth;
        currentLineRenderer.numCapVertices = lineCapVertices;
        currentLineRenderer.material = new Material (Shader.Find("Particles/Standard Unlit"));
        currentLineRenderer.startColor = lineColor;
        currentLineRenderer.endColor = lineColor;
        currentLineEdgeCollider.edgeRadius = 0.1f;
    }

    private void EndLine() 
    {
        if (currentLine.Count == 1) 
        {
            Destroy(currentLineObject);
        }
        else 
        {
            currentLineEdgeCollider.SetPoints(currentLine);
            //Debug.Log("GetKeyDown: " + Input.GetKeyDown(KeyCode.Mouse0) + "\nGetKey: " + Input.GetKey(KeyCode.Mouse0));
        }
        

    }

    private Vector2 GetCurrentWorldPoint()
    {
        return mainCamera.ScreenToWorldPoint(Input.mousePosition);
    }

    private void AddPoint(Vector2 point) 
    {
        if (PlacePoint(point)) {
            currentLine.Add(point);
            gameManager.adjustDrawBarValue(-1);
            currentLineRenderer.positionCount++;
            currentLineRenderer.SetPosition(currentLineRenderer.positionCount - 1, point);
        }
    }

    private bool PlacePoint(Vector2 point) // places point only if certain conditions are met
    {
        if (currentLine.Count == 0) return true;
        if (Vector2.Distance(point, currentLine[currentLine.Count - 1]) < lineSeperationDistance) return false;
        return true;
    } 



}