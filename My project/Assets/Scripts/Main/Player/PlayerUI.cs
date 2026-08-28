using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Image upImage;
    public Image downImage;
    public Image leftImage;
    public Image rightImage;

    private Color normalColor = Color.white;
    private Color pressedColor = new Color(1f, 1f, 1f, 0.5f);

    void Update()
    {
        upImage.color =
            Input.GetKey(KeyCode.W) ? pressedColor : normalColor;

        downImage.color =
            Input.GetKey(KeyCode.S) ? pressedColor : normalColor;

        leftImage.color =
            Input.GetKey(KeyCode.A) ? pressedColor : normalColor;

        rightImage.color =
            Input.GetKey(KeyCode.D) ? pressedColor : normalColor;
    }
}