using System;
using System.Collections.Generic;
using GoogleARCore;
using Toastapp.DesignPatterns;
using UnityEngine;
using VRHouse.ARTools;

public class GameManager : SceneSingleton<GameManager>
{
    private Vector3 inputPos;
    private RaycastHit hit;

    private List<EazyARDetectedPlane> newPlanes = new List<EazyARDetectedPlane>();
    private List<EazyARDetectedPlane> allPlanes = new List<EazyARDetectedPlane>();

    private GameObject trash;
    private GameObject item;
    private ItemHandler draggedItem;

    [SerializeField]
    private GameObject trashPrefab;

    [SerializeField]
    private List<GameObject> itemPrefabs;

    [Space(20)]

    [SerializeField]
    private Transform cameraAnchor;

    [SerializeField]
    private GameObject searchingForPlaneUI;

    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (EazyARSession.Status != SessionStatus.Tracking)
        {
            const int lostTrackingSleepTimeout = 15;
            Screen.sleepTimeout = lostTrackingSleepTimeout;
            if (EazyARSession.Status.IsARValid())
            {
                this.searchingForPlaneUI.SetActive(true);
            }

            return;
        }

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        this.newPlanes = EazyARSession.GetTrackablePlanes(TrackableQueryFilter.New);
        for (int i = 0; i < this.newPlanes.Count; i++)
        {
            EazyARCoreInterface.CreateDetectedPlane(this.newPlanes[i]);
        }

        this.allPlanes = EazyARSession.GetTrackablePlanes();
        bool showSearchingUI = true;
        for (int i = 0; i < this.allPlanes.Count; i++)
        {
            if (this.allPlanes[i].TrackingState == TrackingState.Tracking)
            {
                showSearchingUI = false;
                break;
            }
        }

        this.searchingForPlaneUI.SetActive(showSearchingUI);

        this.inputPos = Vector3.zero;
#if UNITY_EDITOR
        inputPos = Input.mousePosition;
        if (Input.GetMouseButton(0))
        {
            this.Tap();
        }
        else
        {
            this.Release();
            return;
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            this.inputPos = Input.GetTouch(0).position;
            this.Tap();
        }
        else
        {
            this.Release();
            return;
        }
#endif

        if (trash == null)
        {
            EazyARRaycastHit arHit;
            if (EazyARCoreInterface.ARRaycast(inputPos.x, inputPos.y, out arHit))
            {
                this.InstantiateTrash(arHit);
            }
        }
        else if (item != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(inputPos);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.CompareTag("Props"))
                {
                    this.draggedItem = hit.collider.GetComponent<ItemHandler>();
                }
            }
        }
    }

    private void Tap()
    {
        if (item == null)
        {
            this.InstantiateItem();
            this.draggedItem = this.item.GetComponent<ItemHandler>();
        }
        this.draggedItem?.Drag(Camera.main.ScreenToWorldPoint(this.inputPos + new Vector3(0, -0.1f, 0.5f)));
    }

    private void Release()
    {
        this.draggedItem?.Release();
        this.draggedItem = null;
        this.item = null;
        return;
    }

    private void InstantiateItem()
    {
        this.item = Instantiate(this.itemPrefabs[UnityEngine.Random.Range(0, this.itemPrefabs.Count - 1)], Camera.main.ScreenToWorldPoint(this.inputPos + new Vector3(0, -0.1f, 0.5f)), this.cameraAnchor.rotation);
    }

    private void InstantiateTrash(EazyARRaycastHit arHit)
    {
        trash = Instantiate(this.trashPrefab, arHit.Pose.position, arHit.Pose.rotation);

        Anchor anchor = EazyARCoreInterface.CreateAnchor(arHit.Trackable, arHit.Pose);

        if ((arHit.Flags & TrackableHitFlags.PlaneWithinPolygon) != TrackableHitFlags.None)
        {
            Vector3 cameraPositionSameY = EazyARCoreInterface.ARCamera.transform.position;
            cameraPositionSameY.y = arHit.Pose.position.y;

            Vector3 direction = EazyARCoreInterface.ARCamera.transform.position - this.trashPrefab.transform.position;
            float dot = Vector3.Dot(direction, Vector3.up);
            bool orthogonal = ((dot == 1f) || (dot == -1f));
            if (orthogonal)
            {
                return;
            }

            Vector3 fwd = Vector3.ProjectOnPlane(direction, Vector3.up);
            Quaternion fwdRot = Quaternion.LookRotation(fwd, Vector3.up);
            trash.transform.localRotation = trash.transform.localRotation * fwdRot;
        }

        trash.transform.parent = anchor.transform;
    }
}
