using UnityEngine;
using UnityEngine.UI;

public class SwingBar : MonoBehaviour
{
    public GolfBallController ballController;

    private Image bgBar;
    private Image fillBar;

    // Start is called before the first frame update
    void Start()
    {
        bgBar = gameObject.transform.Find("BG").GetComponent<Image>();
        fillBar = bgBar.transform.Find("Strength").GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SetOpacity(ballController.prepSwing ? 1f : 0.6f);
        fillBar.fillAmount = ballController.swingStrength;
    }

    private void SetOpacity(float value)
    {
        var newColor = bgBar.color;
        newColor.a = value;
        bgBar.color = newColor;
    }
}
