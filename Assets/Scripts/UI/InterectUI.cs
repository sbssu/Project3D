using UnityEngine;
using UnityEngine.UI;


public class InterectUI : Singleton<InterectUI>
{
    [SerializeField] GameObject panel;
    [SerializeField] Text hotkeyText;
    [SerializeField] Text interectText;

    private void Start()
    {
        SwitchUI(false);
    }
    public void SwitchUI(bool isShow)
    {
        panel.SetActive(isShow);
    }
    public void Setup(string hotkey, string str)
    {
        hotkeyText.text = hotkey;
        interectText.text = str;

        RectTransform panelRect = panel.GetComponent<RectTransform>();
        float margin = interectText.rectTransform.anchoredPosition.x;
        float textSize = interectText.preferredWidth + 1;

        // textSize�� ContentSizeFitter ������Ʈ�� ���� �ʺ� �����ϰ� �ִ�.
        panelRect.sizeDelta = new Vector2(margin + textSize, panelRect.sizeDelta.y);

        
    }
}
