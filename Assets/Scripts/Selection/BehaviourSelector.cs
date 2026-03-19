using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BehaviourSelector : MonoBehaviour
{
    [Header("Behaviour Buttons")]
    public Button spinButton;
    public Button moveButton;
    public Button bounceButton;
    public Button scaleButton;

    [Header("Navigation")]
    public Button backButton;

    void Start()
    {
        if (string.IsNullOrEmpty(ShapeModuleCache.data.shapeName))
        {
            Debug.LogWarning("No shape in cache. Returning to ShapeSelectionScene.");
            SceneManager.LoadScene("ShapeSelectionScene");
            return;
        }

        spinButton.onClick.AddListener(()   => SelectBehaviour("Spin"));
        moveButton.onClick.AddListener(()   => SelectBehaviour("Move"));
        bounceButton.onClick.AddListener(() => SelectBehaviour("Bounce"));
        scaleButton.onClick.AddListener(()  => SelectBehaviour("Scale"));

        backButton.onClick.AddListener(GoBack);
    }

    void SelectBehaviour(string behaviourName)
    {
        ShapeModuleCache.data.behaviourName = behaviourName;
        SceneManager.LoadScene("ARPlayScene");
    }

    void GoBack()
    {
        ShapeModuleCache.ResetBehaviour();
        SceneManager.LoadScene("ShapeSelectionScene");
    }
}
