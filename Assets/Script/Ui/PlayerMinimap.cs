using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerMinimap : NetworkBehaviour
{
    public Camera minimapCamera;
    public RawImage minimapDisplay;

    private RenderTexture minimapRT;

    void Start()
    {
        // Работаем только для локального игрока
        if (!isLocalPlayer)
        {
            minimapCamera.gameObject.SetActive(false);
            return;
        }

        minimapRT = new RenderTexture(512, 512, 16, RenderTextureFormat.ARGB32);
        minimapCamera.targetTexture = minimapRT;
        minimapDisplay.texture = minimapRT;
    }

    void OnDestroy()
    {
        if (minimapRT != null && isLocalPlayer)
        {
            minimapRT.Release();
            Destroy(minimapRT);
        }
    }
}