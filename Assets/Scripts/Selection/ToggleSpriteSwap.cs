using UnityEngine;
using UnityEngine.UI;

public class ToggleSpriteSwap : MonoBehaviour
{
    public Image targetImage;
    public Sprite selectedSprite;
    public Sprite unselectedSprite;

    private Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(OnToggleChanged);
        OnToggleChanged(toggle.isOn);
    }

    void OnToggleChanged(bool isOn)
    {
        targetImage.sprite = isOn ? selectedSprite : unselectedSprite;
    }

    public void ForceRefresh(bool isOn)
    {
        targetImage.sprite = isOn ? selectedSprite : unselectedSprite;
    }
}