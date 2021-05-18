using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class HoleParentScript : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;

    public MeshCollider generatedMeshCollider = new MeshCollider();
    public MeshCollider generatedMeshCollider1 = new MeshCollider();
    public MeshCollider generatedMeshCollider2 = new MeshCollider();
    public MeshCollider generatedMeshCollider3 = new MeshCollider();

    Mesh generatedMesh;

    public Collider groundCollider;


    public float initialScale = 0.5f;

    GameObject[] obstacles;

    Vector3 dragBeginPoint;
    bool dragging = false;
    Vector3 cameraFirstPosition;

    float cameraScaler = 2;


    Vector3 previousPosition = new Vector3();
    bool borderTop = false;
    bool borderBottom = false;
    bool borderLeft = false;
    bool borderRight = false;

    public GameObject holeScoreCanvas;
    Vector3 holeScoreCanvasStartScale;

    public float speedScale = 70;

    void Start()
    {
        holeScoreCanvasStartScale = holeScoreCanvas.transform.localScale;
        previousPosition = transform.position;
        obstacles = GameObject.FindGameObjectsWithTag("G4Obstacle");
        for (int i = 0; i < obstacles.Length; i++)
        {
            Physics.IgnoreCollision(obstacles[i].GetComponent<Collider>(), generatedMeshCollider, true);
            if (obstacles[i].GetComponent<BoxCollider>())
                Physics.IgnoreCollision(obstacles[i].GetComponent<BoxCollider>(), generatedMeshCollider, true);

        }
        cameraFirstPosition = Camera.main.transform.position - new Vector3(0, transform.localScale.x * cameraScaler, -transform.localScale.x * cameraScaler);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            dragBeginPoint = Input.mousePosition;
            if (Input.touchCount > 0)
            {
                dragBeginPoint = Input.GetTouch(0).position;
            }
            dragging = true;
        }
        if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            dragging = false;
        }
    }
    void FixedUpdate()
    {

        if ((Input.GetMouseButton(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)) && dragging)
        {
            Vector2 normalizeDrag = Input.mousePosition - dragBeginPoint;
            float speed = Mathf.Clamp(normalizeDrag.magnitude, 0, 200) * (speedScale + transform.localScale.x * 0.002f);// * transform.localScale.x;

            normalizeDrag.Normalize();
            Vector3 newPosition = transform.position + new Vector3(normalizeDrag.x * speed * Time.fixedDeltaTime, 0, normalizeDrag.y * speed * Time.fixedDeltaTime);
            transform.position = boundersRequest(newPosition);
        }

        if (transform.hasChanged == true)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2DCollider.transform.localScale = new Vector3(transform.localScale.x * initialScale, transform.localScale.z * initialScale, hole2DCollider.transform.localScale.y);
            MakeHole2D();
            Make3DMeshCollider();
        }
        holeScoreCanvas.transform.localScale = new Vector3(holeScoreCanvas.transform.localScale.x, holeScoreCanvasStartScale.y * transform.localScale.x, holeScoreCanvas.transform.localScale.z);
        Camera.main.transform.position = cameraFirstPosition + new Vector3(transform.position.x, transform.localScale.x * cameraScaler, transform.position.z - transform.localScale.z * cameraScaler);
    }

    void MakeHole2D()
    {
        Vector2[] PointPositions = hole2DCollider.GetPath(0);
        for (int i = 0; i < PointPositions.Length; i++)
        {
            Vector3 transformPoint = hole2DCollider.transform.TransformPoint(PointPositions[i]);
            PointPositions[i] = new Vector2(transformPoint.x, transformPoint.y);
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, PointPositions);
    }

    void Make3DMeshCollider()
    {
        if (generatedMesh != null) Destroy(generatedMesh);

        generatedMesh = ground2DCollider.CreateMesh(true, true);
        generatedMeshCollider.sharedMesh = generatedMesh;
        generatedMeshCollider1.sharedMesh = generatedMesh;
        generatedMeshCollider2.sharedMesh = generatedMesh;
        generatedMeshCollider3.sharedMesh = generatedMesh;


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "G4Obstacle")
        {
            Physics.IgnoreCollision(other, groundCollider, true);
            Physics.IgnoreCollision(other, generatedMeshCollider, false);
        }

        if (other.name == "Top")
            borderTop = true;
        if (other.name == "Bottom")
            borderBottom = true;
        if (other.name == "Left")
            borderLeft = true;
        if (other.name == "Right")
            borderRight = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "G4Obstacle")
        {
            Physics.IgnoreCollision(other, groundCollider, false);
            Physics.IgnoreCollision(other, generatedMeshCollider, true);
        }
        if (other.name == "Top")
            borderTop = false;
        if (other.name == "Bottom")
            borderBottom = false;
        if (other.name == "Left")
            borderLeft = false;
        if (other.name == "Right")
            borderRight = false;
    }



    Vector3 boundersRequest(Vector3 newPosition)
    {
        Vector3 _newPosition = newPosition;
        previousPosition = transform.position;
        if (borderTop && _newPosition.z > previousPosition.z)
            _newPosition.z = previousPosition.z;
        if (borderBottom && _newPosition.z < previousPosition.z)
            _newPosition.z = previousPosition.z;
        if (borderLeft && _newPosition.x < previousPosition.x)
            _newPosition.x = previousPosition.x;
        if (borderRight && _newPosition.x > previousPosition.x)
            _newPosition.x = previousPosition.x;
        return _newPosition;
    }
}