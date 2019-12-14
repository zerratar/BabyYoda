using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image imgBg;
    [SerializeField] private Image imgFill;

    public void SetValue(float value)
    {
        var width = imgBg.rectTransform.rect.width;
        var height = imgFill.rectTransform.sizeDelta.y;
        imgFill.rectTransform.sizeDelta = new Vector2(width * value, height);
    }
}
