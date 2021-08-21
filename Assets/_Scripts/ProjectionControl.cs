using UnityEngine;

public class ProjectionControl : MonoBehaviour
{

    [SerializeField] Camera viewCamera = null;
    [SerializeField] Camera projCamera = null;

    [SerializeField] RenderProjection renderProjection = null;

    [Space]
    [SerializeField] LayerMask interatableLayer = 0;

    [Space]
    [SerializeField] PlayerMovement movement = null;

    [Header("Debug")]
    public Texture2D projImage;

    bool draging;
    ProjectionObject dragObject;
    float dragDistance;

    Vector3? placePoint;

    void Update()
    {
        projCamera.transform.position = viewCamera.transform.position;
        projCamera.transform.rotation = viewCamera.transform.rotation;

        Dragging();
        if (placePoint != null && Input.GetKey(KeyCode.E))
        {
            movement.MoveDirection(((Vector3)placePoint - transform.position) * 5);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) Debug.Break();
    }

    void Dragging()
    {
        if (draging)
        {
            dragDistance += Input.mouseScrollDelta.y * 10 * Time.deltaTime;
            dragObject.DragMove(transform.position + viewCamera.transform.forward * dragDistance);
            if (Input.GetKey(KeyCode.R)) dragObject.Rotate(50);

            if (Input.GetMouseButtonUp(0))
            {
                draging = false;
                dragObject.Fizzing();

                TriggedProjection();
                placePoint = transform.position;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && CanDrag())
            {
                if (Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward, out RaycastHit hit, 15, interatableLayer))
                {
                    draging = true;
                    dragObject = hit.collider.GetComponent<ProjectionObject>();
                    dragDistance = hit.distance;

                    placePoint = null;
                    DisableProject();
                }
            }
        }
    }
    bool CanDrag()
    {
        return !(placePoint != null && ((Vector3)placePoint - transform.position).sqrMagnitude > 1f);
    }

    void TriggedProjection()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = projCamera.targetTexture;

        projCamera.Render();

        Texture2D image = new Texture2D(projCamera.targetTexture.width, projCamera.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, projCamera.targetTexture.width, projCamera.targetTexture.height), 0, 0);
        image.Apply();

        RenderTexture.active = currentRT;

        Matrix4x4 projMatrix = projCamera.nonJitteredProjectionMatrix * projCamera.transform.worldToLocalMatrix;
        renderProjection.SetRender(projCamera.transform.position, projCamera.transform.forward, projMatrix, image);

        projImage = image;
    }
    void DisableProject()
    {
        renderProjection.Disable();
    }

}
