using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class ARPlaceObjectBySize : MonoBehaviour
{
    [SerializeField] private MeshRenderer _objectSize; //Permite leer las dimensiones del objeto
    [SerializeField] private GameObject _objectPrefab; //Objeto que puede tener varios Meshy que es el que se va a colocar

    private ARPlaneManager _planeManager;
    private List<ARPlane> _listPlanes; //Planos que se van generando 
    private GameObject _objectPlaced;

    private void Awake()
    {
        _planeManager = GetComponent<ARPlaneManager>();
        _listPlanes = new List<ARPlane>();
    }

    private void OnEnable()
    {
        _planeManager.planesChanged += PlanesFound;
    }

    private void OnDisable()
    {
        _planeManager.planesChanged -= PlanesFound;
    }

    private void PlanesFound(ARPlanesChangedEventArgs eventData)
    {
        //Se agregan los planos
        if (eventData.added != null && eventData.added.Count > 0)
        {
            _listPlanes.AddRange(eventData.added);
        }

        //Se recorre y se revisa uno a uno el tamaño de cada plano detectado
        foreach (var plane in _listPlanes)
        {
            //Se coloca el objeto y se detiene la detección de planos
            if (CompareSizeWithObject(plane) && _objectPlaced == null)
            {
                _objectPlaced = Instantiate(_objectPrefab.gameObject);
                _objectPlaced.transform.position = plane.center;
                _objectPlaced.transform.up = plane.normal;

                StopPlaneDetection(plane);
            }
        }
    }

    private bool CompareSizeWithObject(ARPlane plane)
    {
        return plane.extents.x > _objectSize.bounds.size.x && plane.extents.y > _objectSize.bounds.size.z;
    }

    private void StopPlaneDetection(ARPlane planeException)
    {
        StopPlaneDetection();

        foreach (var plane in _listPlanes)
        {
            //Si se coloca el tamaño adecuado, se coloca el objeto necesario y se apagan los demás planos excepto el del objeto
            if (plane == planeException)
            {
                plane.gameObject.SetActive(true);
            }
        }
    }

    private void StopPlaneDetection()
    {
        _planeManager.requestedDetectionMode = PlaneDetectionMode.None;

        foreach (var plane in _listPlanes)
        {
            plane.gameObject.SetActive(false);
        }
    }

}