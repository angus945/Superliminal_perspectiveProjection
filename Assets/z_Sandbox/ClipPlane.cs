//using UnityEngine;

//public class ClipPlane : MonoBehaviour
//{
//    [SerializeField] Camera camera = null;
//    [SerializeField] Transform target = null;

//    [SerializeField] float focalLength = 2;

//    float near { get => camera.nearClipPlane; }
//    float far { get => camera.farClipPlane; }

    
//    void OnDrawGizmos()
//    {
//        //return;
//        if (target == null) return;

//        Gizmos.DrawWireSphere(target.position, 0.1f);
//        Gizmos.DrawLine(target.position, transform.position);

//        Vector3 worldTarget = target.position;

//        Matrix4x4 worldToCamera = camera.cameraToWorldMatrix;
//        worldToCamera = worldToCamera.inverse;
//        Vector3 cameraTarget = worldToCamera.MultiplyPoint(worldTarget);
//        Gizmos.DrawWireSphere(cameraTarget, 0.1f);

//        Matrix4x4 projMatrix = new Matrix4x4();
//        projMatrix.SetRow(0, new Vector4(near, 0, 0, 0));
//        projMatrix.SetRow(1, new Vector4(0, near, 0, 0));
//        projMatrix.SetRow(2, new Vector4(0, 0, 0, 0));
//        projMatrix.SetRow(3, new Vector4(0, 0, 1, 0));

//        Vector3 targetPorj = projMatrix.MultiplyPoint(cameraTarget);
//        Gizmos.color = Color.yellow;
//        Gizmos.DrawWireSphere(targetPorj, 0.1f);
//        Gizmos.DrawLine(Vector3.zero, targetPorj);

//        Vector2 screen = camera.WorldToScreenPoint(targetPorj);
//        //Debug.Log(screen);

//    }
    

//}
