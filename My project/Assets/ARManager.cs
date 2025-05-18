using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
public class ARManager : MonoBehaviour
{
    public Camera arCamera;
    public GameObject markerPrefab;
    public LineRenderer lineRenderer;
    public TextMeshProUGUI heightText;

    private ARRaycastManager raycastManager;
    private GameObject startMarker, endMarker;
    private bool firstPlaced = false;
    private bool heightMeasured = false;

    void Start()
    {
        heightText.text = "Length: 0.00 cm";
        raycastManager = Object.FindFirstObjectByType<ARRaycastManager>();
        lineRenderer.positionCount = 0;
    }

    public void PlaceMarkerFromButton()
    {
        Vector2 centerScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        if (raycastManager.Raycast(centerScreen, hits, TrackableType.Planes))
        {
            Pose hitPose = hits[0].pose;

            if (!firstPlaced)
            {
                startMarker = Instantiate(markerPrefab, hitPose.position, Quaternion.identity);
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, startMarker.transform.position);
                lineRenderer.SetPosition(1, startMarker.transform.position); // initialize second point
                firstPlaced = true;
            }
            else if (!heightMeasured)
            {
              
                endMarker = Instantiate(markerPrefab, hitPose.position, Quaternion.identity);
                lineRenderer.SetPosition(1, endMarker.transform.position);
                heightMeasured = true;

                float heightMeters = Vector3.Distance(startMarker.transform.position, endMarker.transform.position);
                float heightCm = heightMeters * 100f;
                heightText.text = $"Length: {heightCm:F2} to {heightCm+4:F2} cm";
            }
        }
    }

    void Update()
    {
        
        if (firstPlaced && !heightMeasured)
        {
            Vector2 centerScreen = new Vector2(Screen.width / 2f, Screen.height / 2f);
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            if (raycastManager.Raycast(centerScreen, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;
                lineRenderer.SetPosition(1, hitPose.position);
            }
        }
    }
}
