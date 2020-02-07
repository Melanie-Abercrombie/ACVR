using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MaterialCreator : MonoBehaviour {

    private string path, currentFile;
    private DirectoryInfo dir;
    private DirectoryInfo[] subdirs;
    private FileInfo[] filesInSubdir;
    private Texture m_MainTexture, m_Normal, m_Metal;


    void Awake()
    {
        path = Application.dataPath + "/Resources/Artwork/";
        dir = new DirectoryInfo(@path);
        Debug.Log(dir.Name);
        subdirs = dir.GetDirectories();
    }


    // Use this for initialization
    void Start () {
        foreach (DirectoryInfo sb in subdirs)
        {
            filesInSubdir = sb.GetFiles();
            foreach (FileInfo f in filesInSubdir)
            {
                if (f.Name.Contains(".fbx") && !(f.Name.Contains(".meta")))
                {
                    Debug.Log(f.Name);
                    currentFile = f.Name;

                }
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
