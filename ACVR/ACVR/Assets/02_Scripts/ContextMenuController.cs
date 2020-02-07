using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ContextMenuController : MonoBehaviour
{

    [SerializeField]
    bool hasFrameOption;

    private int index;
    private TextMeshProUGUI[] texts;
    private readonly static Color32 SELECTED_COLOR = new Color32(255, 255, 255, 255);
    private readonly static Color32 UNSELECTED_COLOR = new Color32(128, 128, 128, 128);

    private Camera cameraToLookAt;

    // Use this for initialization
    void Start()
    { 
        cameraToLookAt = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        //gameObject.transform.rotation = cameraToLookAt.transform.rotation;

        var newScale = transform.localScale;
        newScale.x *= -1f;
        gameObject.transform.localScale = newScale;

        if (hasFrameOption)
        {
            texts = new TextMeshProUGUI[3];
            texts[2] = transform.Find("Header/OptionFrame/Text").gameObject.GetComponent<TextMeshProUGUI>();
            SetColor(texts[2], UNSELECTED_COLOR);
        }
        else
        {
            texts = new TextMeshProUGUI[2];
        }
        texts[0] = transform.Find("Header/OptionGrab/Text").gameObject.GetComponent<TextMeshProUGUI>();
        texts[1] = transform.Find("Header/OptionDelete/Text").gameObject.GetComponent<TextMeshProUGUI>();

        index = 0;
        SetColor(texts[0], SELECTED_COLOR);
        SetColor(texts[1], UNSELECTED_COLOR);
    }

    private void Update()
    {
        //rotates menu to look at camera
        var newRotation = cameraToLookAt.transform.rotation;
        newRotation.z = 0f;
        newRotation.x = 0f;
        gameObject.transform.rotation = newRotation;
        //transform.LookAt(cameraToLookAt.transform);
    }

    public void AdvanceUp()
    {
        SetColor(texts[index], UNSELECTED_COLOR);
        index = ((index - 1 +texts.Length) % texts.Length);
        SetColor(texts[index], SELECTED_COLOR);
    }

    public void AdvanceDown()
    {
        SetColor(texts[index], UNSELECTED_COLOR);
        index = ((index + 1) % texts.Length);
        SetColor(texts[index], SELECTED_COLOR);
    }

    public int SetIndex(int ind)
    {
        ind = Mathf.Clamp(ind, 0, texts.Length);
        index = ind;
        return index;
    }

    private void SetColor(TextMeshProUGUI text, Color32 color)
    {
        text.color = color;
    }

    /// <summary>
    /// gives this context menu's current selection: 
    /// 0 = Grab, 1 = Delete, 2 = Frame
    /// </summary>
    public int GetSelection()
    {
        return index;
    }
}
