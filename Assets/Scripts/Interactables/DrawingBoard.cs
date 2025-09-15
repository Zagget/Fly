using UnityEngine;

public class DrawingBoard : MonoBehaviour
{
    [SerializeField] Texture2D texture;
    Renderer render;
    [SerializeField] Color boardColor;
    public Color BoardColor { get { return boardColor; } set { boardColor = value; } }
    Color[] pixels;

    void Start()
    {
        render = GetComponent<Renderer>();
        pixels = new Color[texture.width * texture.height];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = BoardColor;
        }
        ChangeTextureFormat(pixels, TextureFormat.RGBA32);

        texture.filterMode = FilterMode.Point;
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
        for (int i = 0; i < clearColors.Length; i++) clearColors[i] = BoardColor;
        texture.SetPixels(clearColors);
        texture.Apply();
    }

    void ChangeTextureFormat(Color[] colors, TextureFormat newFormat)
    {
        texture = new Texture2D(texture.width, texture.height, newFormat, false);
        texture.SetPixels(colors);
        texture.Apply();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = boardColor;
        Gizmos.DrawSphere(transform.position, transform.localScale.x * 0.25f);
    }
}
