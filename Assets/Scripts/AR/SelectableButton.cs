using UnityEngine;
using UnityEngine.UI;

public class SelectableButton : MonoBehaviour
{
    public Image targetImage;
    public Sprite normalSprite;
    public Sprite selectedSprite;

    public void SetSelected(bool isSelected)
    {
        targetImage.sprite = isSelected ? selectedSprite : normalSprite;
    }
}
