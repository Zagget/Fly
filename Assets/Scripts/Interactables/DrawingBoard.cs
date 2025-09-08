using UnityEngine;

public class DrawingBoard : MonoBehaviour
{
    [SerializeField] Texture2D texture;
    Renderer render;

    void Start()
    {
        render = GetComponent<Renderer>();
        texture = new Texture2D(512, 512, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;
        ClearTexture();
        render.material.mainTexture = texture;
    }

    public void DrawAtPosition(Vector2 position, Color drawColor, int brushSize)
    {
        int x = (int)(position.x * texture.width);
        int y = (int)(position.y * texture.height);

        for (int i = -brushSize; i < brushSize; i++)
        {
            for (int j = -brushSize; j < brushSize; j++)
            {
                if (x + i >= 0 && x + i < texture.width && y + j >= 0 && y + j < texture.height)
                    texture.SetPixel(x + i, y + j, drawColor);
            }
        }
        texture.Apply();
    }

    void ClearTexture()
    {
        Color[] clearColors = new Color[texture.width * texture.height];
        for (int i = 0; i < clearColors.Length; i++) clearColors[i] = Color.white;
        texture.SetPixels(clearColors);
        texture.Apply();
    }
}
