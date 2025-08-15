using UnityEngine;
using UnityEngine.UI;

public class FrontImage : MonoBehaviour
{
    public Image scratchImage;       // UI Image (긁히는 표면)
    public Texture2D brushTexture;   // 원형 브러시 PNG (흰색 부분: 칠해질 영역)

    Texture2D scratchTex;

    void Start()
    {
        // 원본 Sprite를 Texture2D로 복사 (수정 가능한 상태로)
        Texture2D original = scratchImage.sprite.texture;
        scratchTex = new Texture2D(original.width, original.height, TextureFormat.RGBA32, false);
        scratchTex.SetPixels(original.GetPixels());
        scratchTex.Apply();

        // 수정한 Texture2D를 Sprite로 교체
        scratchImage.sprite = Sprite.Create(
            scratchTex,
            new Rect(0, 0, scratchTex.width, scratchTex.height),
            new Vector2(0.5f, 0.5f)
        );
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                scratchImage.rectTransform,
                Input.mousePosition,
                null,
                out localPos
            );

            // localPos를 0~1 UV로 변환
            Rect rect = scratchImage.rectTransform.rect;
            float u = (localPos.x - rect.x) / rect.width;
            float v = (localPos.y - rect.y) / rect.height;

            // UV → 픽셀 좌표
            int px = Mathf.RoundToInt(u * scratchTex.width);
            int py = Mathf.RoundToInt(v * scratchTex.height);

            Erase(px, py);
        }
    }

    void Erase(int cx, int cy)
    {
        int bw = brushTexture.width;
        int bh = brushTexture.height;

        for (int x = 0; x < bw; x++)
        {
            for (int y = 0; y < bh; y++)
            {
                Color bc = brushTexture.GetPixel(x, y);
                if (bc.a > 0f)
                {
                    int px = cx + x - bw / 2;
                    int py = cy + y - bh / 2;
                    if (px >= 0 && px < scratchTex.width && py >= 0 && py < scratchTex.height)
                    {
                        scratchTex.SetPixel(px, py, new Color(0, 0, 0, 0)); // 투명
                    }
                }
            }
        }
        scratchTex.Apply();
    }
}
