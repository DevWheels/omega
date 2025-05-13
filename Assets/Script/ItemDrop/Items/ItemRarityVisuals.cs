using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ItemRarityVisuals : MonoBehaviour
{
    [Header("Rarity Settings")]
    public ItemRank itemRank;
    public Color rankColor = Color.white;
    public float glowIntensity = 1f;
    public ParticleSystem particleEffect;

    private Renderer itemRenderer;
    private Material[] materials;
    private Color baseEmissionColor;

    private void Awake()
    {
        itemRenderer = GetComponent<Renderer>();
        materials = itemRenderer.materials;

        InitializeVisualEffects();
    }

    private void InitializeVisualEffects()
    {

        foreach (var mat in materials)
        {
            if (mat.HasProperty("_EmissionColor"))
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", rankColor * glowIntensity);
                mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
                DynamicGI.UpdateEnvironment();
            }
        }


        if (particleEffect != null)
        {
            var mainModule = particleEffect.main;
            mainModule.startColor = new ParticleSystem.MinMaxGradient(rankColor);
            

            switch (itemRank)
            {
                case ItemRank.S:
                    particleEffect.transform.localScale = Vector2.one * 0.1f;
                    mainModule.startSizeMultiplier = 0.1f;
                    break;
                case ItemRank.A:
                    particleEffect.transform.localScale = Vector2.one * 0.1f;
                    mainModule.startSizeMultiplier = 0.1f;
                    break;
                default:
                    particleEffect.transform.localScale = Vector2.one * 0.1f;
                    mainModule.startSizeMultiplier = 0.1f;
                    break;
            }
            
            particleEffect.Play();
        }
    }

    public void SetRankVisuals(ItemRank rank, Color color, float intensity)
    {
        itemRank = rank;
        rankColor = color;
        glowIntensity = intensity;
        
        InitializeVisualEffects();
    }
}