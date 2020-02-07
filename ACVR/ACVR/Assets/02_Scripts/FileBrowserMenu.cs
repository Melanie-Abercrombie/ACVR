using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System.IO;
using System.Linq;

public class FileBrowserMenu : MonoBehaviour {

    public GameObject preview;

   
    private Vector3 location;
    private DirectoryInfo dir;
    private string path;
    private string[] validExtensions = { ".jpg", ".jpeg", ".png", ".fbx" };

    private int index;
    private int stage = 0;
    private Text[] texts;
    private readonly static Color32 SELECTED_COLOR = new Color32(255, 255, 255, 255);
    private readonly static Color32 UNSELECTED_COLOR = new Color32(128, 128, 128, 128);


    private void Awake()
    {
        location = new Vector3(-2, 31, 0);
        index = 0;

        //populate menu with dir names
        path = Application.dataPath + "/Resources/Artwork/";
        dir = new DirectoryInfo(@path);
        CreateMenuList(dir, 0);
    }
    
    void Start () {
        SetVisibility(false);
	}

    //Creates the list of text seen on the menu
    //type = 0 -> create for folders; type = 1 -> create for files
    private void CreateMenuList(DirectoryInfo dir, int type)
    {
        location = new Vector3(-2, 31, 0);
        string[] DirList;

        GameObject parent = GameObject.Find("Preview");
        foreach (Transform child in parent.transform)
        {
            Destroy(child.gameObject);
        }

        if (type == 0)
        {
            DirectoryInfo[] info = dir.GetDirectories();
            texts = new Text[info.Length];

            DirList = new string[info.Length];

            for (int i = 0; i < info.Length; i++)
            {
                string name = info[i].Name;
                DirList[i] = name;
                UpdateFileBrowser(name, location, i);
                location.y -= 9f;
                SetColor(texts[i], UNSELECTED_COLOR);
            }

        }
         else if(type == 1)
        {
            FileInfo[] info = dir.GetFiles();

            DirList = new string[info.Length];

            int count = 0;
            for (int i = 0; i < info.Length; i++)
            {
                if (validExtensions.Contains(Path.GetExtension(info[i].Name)))
                {
                    count++;
                }
            }

            
            texts = new Text[count];
            count = 0;
            
            for (int i = 0; i < info.Length; i++)
            {
                //only add files with the extensions we want
                if (validExtensions.Contains(Path.GetExtension(info[i].Name)))
                {
                    string name = Path.GetFileName(info[i].Name);
                    //string name = Path.GetFileNameWithoutExtension(info[i].Name);
                    DirList[i] = name;
                    UpdateFileBrowser(name, location, count);
                    location.y -= 9f;
                    SetColor(texts[count], UNSELECTED_COLOR);
                    count++;
                }
                else if (info[i].GetType() == typeof(DirectoryInfo))
                {
                    string name = info[i].Name;
                    Debug.Log(name);
                }

            }
        }

        SetColor(texts[0], SELECTED_COLOR);
    }
   

    public void SetVisibility(bool input)
    {
        gameObject.GetComponent<Canvas>().enabled = input;
        if (gameObject.GetComponent<Canvas>().enabled == false)
        {
            path = Application.dataPath + "/Resources/Artwork/";
            dir = new DirectoryInfo(@path);
            CreateMenuList(dir, 0);
            index = 0;
            stage = 0;
        }
    }

    public void AdvanceUp()
    {
        SetColor(texts[index], UNSELECTED_COLOR);
        index = ((index - 1 + texts.Length) % texts.Length);
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

    private void SetColor(Text text, Color32 color)
    {
        text.color = color;
    }
    
    public int GetSelection()
    {
        return stage;
    }

    public string GetFileName()
    {
        return texts[index].name;
    }

    //stage = 0 -> directory list; stage = 1 -> file list; stage = 2 -> hold object
    public void NextStage()
    {
        stage += 1;
        string folderName = texts[index].name + "/";
        path += folderName;
        DirectoryInfo fileDir = new DirectoryInfo(@path);
        index = 0;

        //populate menu with file names
        if (stage == 1)
        {
            CreateMenuList(fileDir, 1);
        }
        else if (stage == 2)
        {

        }
        
    }

    //Adds text to the menu
    private void UpdateFileBrowser(string name, Vector3 pos, int i)
    {
        GameObject go = new GameObject(name);
        go.transform.parent = gameObject.transform.Find("Preview");
        go.transform.localPosition = pos;
        go.transform.localRotation = Quaternion.Euler(0, 0, 0);
        Text newText = go.AddComponent<Text>();
        newText.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        newText.fontSize = 7;
        newText.text = name;
        newText.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        newText.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 10f);
        newText.alignment = TextAnchor.MiddleLeft;

        texts[i] = newText;
    }

}
