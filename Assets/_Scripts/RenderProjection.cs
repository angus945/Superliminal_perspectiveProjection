using UnityEngine;

[ExecuteInEditMode]
public class RenderProjection : MonoBehaviour
{
    [SerializeField] Camera _camera = null;
    [SerializeField] Material _effectMaterial = null;

    Texture2D renderImage = null;
    Vector3 cameraPosition;
    Vector3 cameraDirection;
    Matrix4x4 viewProjection;

    void OnDrawGizmos()
    {
        if (renderImage != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(cameraPosition, 0.2f);
            Gizmos.DrawRay(cameraPosition, cameraDirection);
        }
    }


    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        RaycastCornerBlit(src, dst, _effectMaterial);
    }

    void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
    {
        // Compute Frustum Corners
        float camFar = _camera.farClipPlane;
        float camFov = _camera.fieldOfView;
        float camAspect = _camera.aspect;

        float fovWHalf = camFov * 0.5f;

        Vector3 toRight = _camera.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
        Vector3 toTop = _camera.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

        Vector3 topLeft = (_camera.transform.forward - toRight + toTop);
        float camScale = topLeft.magnitude * camFar;

        topLeft.Normalize();
        topLeft *= camScale;

        Vector3 topRight = (_camera.transform.forward + toRight + toTop);
        topRight.Normalize();
        topRight *= camScale;

        Vector3 bottomRight = (_camera.transform.forward + toRight - toTop);
        bottomRight.Normalize();
        bottomRight *= camScale;

        Vector3 bottomLeft = (_camera.transform.forward - toRight - toTop);
        bottomLeft.Normalize();
        bottomLeft *= camScale;

        // Custom Blit, encoding Frustum Corners as additional Texture Coordinates
        RenderTexture.active = dest;

        mat.SetTexture("_MainTex", source);

        mat.SetTexture("_ProjTex", renderImage);
        mat.SetVector("_ProjOrigin", cameraPosition);
        mat.SetVector("_ProjDir", cameraDirection);
        mat.SetMatrix("_ViewProjectionMat", viewProjection);

        GL.PushMatrix();
        GL.LoadOrtho();

        mat.SetPass(0);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.MultiTexCoord(1, bottomLeft);
        GL.Vertex3(0.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.MultiTexCoord(1, bottomRight);
        GL.Vertex3(1.0f, 0.0f, 0.0f);

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.MultiTexCoord(1, topRight);
        GL.Vertex3(1.0f, 1.0f, 0.0f);

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.MultiTexCoord(1, topLeft);
        GL.Vertex3(0.0f, 1.0f, 0.0f);

        GL.End();
        GL.PopMatrix();
    }

    public void SetRender(Vector3 cameraPosition, Vector3 cameraDirection, Matrix4x4 viewProjection, Texture2D renderImage)
    {
        enabled = true;

        this.cameraPosition = cameraPosition;
        this.cameraDirection = cameraDirection;
        this.viewProjection = viewProjection;
        this.renderImage = renderImage;
    }
    public void Disable()
    {
        enabled = false;
    }
}
