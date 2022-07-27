using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ToggleSwitch : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private bool _isPlay = false;
    public bool isPlay { get { return _isPlay; } }
    [SerializeField] private RectTransform toggleIndicator;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Color playColor;
    [SerializeField] private Color createColor;
    private float playPos;
    private float createPos;
    [SerializeField] private float tweenTime = 0.25f;
    public delegate void ValueChanged(bool value);
    public event ValueChanged valueChanged;


    void Start()
    {
        playPos = toggleIndicator.anchoredPosition.x;
        playPos = backgroundImage.rectTransform.rect.width - toggleIndicator.rect.width;
    }
    private void OnEnable()
    {
        Toggle(isPlay);
    }
    private void Toggle(bool value)
    {
        if (value != isPlay)
        {
            _isPlay = value;
            ToggleColor(isPlay);
            MoveIndicator(isPlay);

            if (valueChanged != null)
                valueChanged(isPlay);
        }
    }
    private void ToggleColor(bool value)
    {
        if (value)
            backgroundImage.DOColor(playColor, tweenTime);
        else
            backgroundImage.DOColor(createColor, tweenTime);
    }
    private void MoveIndicator(bool value)
    {
        if (value)
            toggleIndicator.DOAnchorPosX(playPos, tweenTime);
        else
            toggleIndicator.DOAnchorPosX(createPos, tweenTime);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Toggle(!_isPlay);
    }
}
