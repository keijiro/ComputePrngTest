using UnityEngine;

[ExecuteInEditMode]
public class PrngTester : MonoBehaviour
{
    [SerializeField] ComputeShader _compute;

    RenderTexture _buffer;

    const int kSize = 512;

    void OnDestroy()
    {
        if (_buffer != null)
            if (Application.isPlaying)
                Destroy(_buffer);
            else
                DestroyImmediate(_buffer);
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_buffer == null)
        {
            _buffer = new RenderTexture(kSize, kSize, 0);
            _buffer.enableRandomWrite = true;
            _buffer.filterMode = FilterMode.Point;
            _buffer.hideFlags = HideFlags.DontSave;
            _buffer.Create();
        }

        var kernel = _compute.FindKernel("TesterKernel");
        _compute.SetTexture(kernel, "Result", _buffer);
        _compute.Dispatch(kernel, kSize / 8, kSize / 8, 1);

        Graphics.Blit(_buffer, destination);
    }
}
