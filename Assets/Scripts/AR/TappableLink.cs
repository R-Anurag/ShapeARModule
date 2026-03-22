using UnityEngine;

public class TappableLink : MonoBehaviour
{
    public string url;
    public void Open() => Application.OpenURL(url);
}
