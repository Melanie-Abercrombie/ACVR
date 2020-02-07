using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using UnityEditor;

public class InventoryLoader : MonoBehaviour {

    [SerializeField]
    public GameObject[] testArtList;        // temporarly in use for preloaded, hardcoded specified artwork

    private GameObject genericPainting;     // for possible future use when reading in data
    private GameObject genericSculpture;    // for possible future use when reading in data

    public LinkedList<GameObject> artInventory;
    public GameObject current;
    public string[] resourceFiles;

    private bool destroyed;


    private static IMG2Sprite _instance;

    private void Awake()
    {
        destroyed = false;
    }
    

    public void OpenInventory(string path)
    {
        AddResourceFiles(path);
        resourceFiles = AddResourceFiles(path);
        //PopulateInventory(resourceFiles);
        artInventory = ArrayToLinkedList(PopulateInventory(resourceFiles));
        //MakeImagesPaintings(Application.dataPath  + "/Resources/test/cat.jpg");

        foreach (GameObject go in artInventory)
        {
            GameObject.Find(go.name).SetActive(false);
            //Destroy(GameObject.Find(go.name));
        }
    }

    public void OpenFile(string path)
    {
        GameObject fileObject;
        if (path == null)
        {
            current = null;
            return;
        }
        if (Path.GetExtension(path) == ".fbx")
            fileObject = MakeSculptures(path);
        else
            fileObject = MakeImagesPaintings(path);
        fileObject.name = Path.GetFileNameWithoutExtension(path);
        Vector3 temp = new Vector3(0, 0, 2);
        fileObject.transform.position = GameObject.Find("OVRPlayerController").transform.position + temp;
        current = fileObject;
    }

    public GameObject GetCurrentFile()
    {
        return current;
    }

    private LinkedList<GameObject> ReadInInventory()
    {
        // for use when we allow for dynamicly adding artwork to the game
        // this function should read in all of the desired artwork, create prefabs for them (with appropriate tags/layers) and use that to populate the artList

        //artInventory =  ArrayToLinkedList(testArtList);  // note this 1 line implementation is just for testing purposes
        LinkedList<GameObject> inventory = ArrayToLinkedList(PopulateInventory(resourceFiles));
        return inventory;
    }
    

    public LinkedList<GameObject> FetchInventory()
    {
        return artInventory;
    }
    
    
    private LinkedList<GameObject> ArrayToLinkedList(GameObject[] array)
    {
        LinkedList<GameObject> linkedList = new LinkedList<GameObject>();

        foreach (GameObject element in array)
            linkedList.AddLast(element);

        return linkedList;
    }

    private LinkedList<string> ArrayToLinkedList(string[] array)
    {
        LinkedList<string> linkedList = new LinkedList<string>();

        foreach (string element in array)
            linkedList.AddLast(element);

        return linkedList;
    }

    private string[] AddResourceFiles(string path)
    {
        //add to linked list 
        string[] filePaths = GetFileArray(@path, "*.png|*.jpg|*.jpeg", SearchOption.AllDirectories);
        foreach(string f in filePaths)
        {
            Debug.Log(f);
        }
        return filePaths;
    }

    private GameObject[] PopulateInventory(string[] filePaths)
    {
        GameObject[] art = new GameObject[filePaths.Length];
        for(int i = 0; i < art.Length; i++)
        {
            art[i] = MakeImagesPaintings(filePaths[i]);
        }
        return art;
    }


    //used to get all file types we want to use
    //in this case, we'll be calling .jpg, .png, .fbx
    public static string[] GetFileArray(string path, string searchPattern, SearchOption searchOption)
    {
        string[] searchPatterns = searchPattern.Split('|');
        List<string> files = new List<string>();
        foreach (string sp in searchPatterns) // find appropriate files by file type 
        {
            files.AddRange(System.IO.Directory.GetFiles(path, sp, searchOption));
        }
        //now add subdirs
        string[] subDirs = Directory.GetDirectories(path);
        foreach (string sub in subDirs)
        {
            files.Add(sub);
        }
        files.Sort();
        return files.ToArray();
    }



    //////////////////////////////////////////////////
    /// Manipulating the visual part

    //turn 2D images to "paintings"
    private GameObject MakeImagesPaintings(string filePath)
    {
        IMG2Sprite imgToSprite = GetComponent<IMG2Sprite>(); //this script is also attached to PocketInventoryManagger
        //make empty parent object
        //GameObject paintingItem = new GameObject(Path.GetFileNameWithoutExtension(filePath));
        

        //make canvasBase (acts as parent object)
        GameObject canvasBase = GameObject.CreatePrimitive(PrimitiveType.Cube);
        canvasBase.transform.position = new Vector3(0, 1, 0);
        canvasBase.gameObject.name = Path.GetFileNameWithoutExtension(filePath);
        canvasBase.gameObject.tag = "WallArt";
        canvasBase.gameObject.layer = LayerMask.NameToLayer("Artwork");

        //make painting
        GameObject go = new GameObject(Path.GetFileNameWithoutExtension(filePath));
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        Sprite canvas = imgToSprite.LoadNewSprite(filePath);
        renderer.sprite = canvas;
        renderer.transform.parent = canvasBase.transform;

        var bounds = canvas.bounds;
        var factory = 1 / bounds.size.y;
        var factorx = 1 / bounds.size.x;
        renderer.transform.localScale = new Vector2(factorx, factory);

        canvasBase.transform.localScale = new Vector3(canvas.texture.width / 1000.0f, canvas.texture.height / 1000.0f, 0.055f) * 1.5f;
        renderer.transform.position = new Vector3(canvasBase.transform.position.x - (canvasBase.transform.localScale.x / 2), canvasBase.transform.position.y - (0.5f * canvasBase.transform.position.y), canvasBase.transform.position.z - (canvasBase.transform.localScale.z));

        //canvasBase.AddComponent<BoxCollider>();
        canvasBase.GetComponent<MeshRenderer>().enabled = false;
        canvasBase.transform.Rotate(0.0f, 180.0f, 0.0f);
        canvasBase.AddComponent<ColliderDetector>();
        canvasBase.AddComponent<Rigidbody>();
        canvasBase.GetComponent<Rigidbody>().useGravity = false;
        canvasBase.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        
        return canvasBase;
    }

    // read .fbx files in their appropriate folders and create texture for objects
    public GameObject MakeSculptures(string path)
    {
        int foundStart = path.IndexOf("Resources/");
        string pathToUse = path.Substring(foundStart+10);
        pathToUse = pathToUse.Substring(0, pathToUse.Length - 4);
        Object prefab = Resources.Load(pathToUse);
        GameObject sculpture = (GameObject)Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
        //sculpture = sculpture.transform.GetChild(0).gameObject;

        //add unity details
        sculpture.name = Path.GetFileNameWithoutExtension(path);
        sculpture.tag = "FloorArt";
        sculpture.layer = LayerMask.NameToLayer("Artwork");
        sculpture.AddComponent<MeshCollider>();
        sculpture.AddComponent<ColliderDetector>();
        sculpture.AddComponent<Rigidbody>();
        sculpture.GetComponent<Rigidbody>().useGravity = false;
        sculpture.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        //assign material
        pathToUse = pathToUse.Remove(pathToUse.IndexOf(sculpture.name)) + "Materials/";
        Material sculptureChildMat = sculpture.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material;
        CreateMaterial(ref sculptureChildMat, sculpture.name, pathToUse);
        //sculpture.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = CreateMaterial(sculpture, pathToUse);
        Debug.Log(sculptureChildMat.mainTexture);
        return sculpture;
    }

    //public void CreateMaterial(GameObject sculpture, string path)
    //{
    //    //Texture m_MainTexture, m_Normal, m_Metal;
    //    mat = new Material(Shader.Find("Standard"));
    //    mat.name = objName;
    
    //    //find the textures within the Materials folder
    //    string pathtest = Application.dataPath + "/Resources/Artwork/test1/Materials/";
    //    DirectoryInfo texPath = new DirectoryInfo(@pathtest);
    //    //DirectoryInfo texPath = new DirectoryInfo(@path);
    //    FileInfo[] matsInDir = texPath.GetFiles();

    //    mat.EnableKeyword("_NORMALMAP");
    //    mat.EnableKeyword("_PARALLAXMAP");
    //    mat.EnableKeyword("_METALLICGLOSSMAP");
    //    //Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
    //    //byte[] fileData;
    //    foreach (FileInfo f in matsInDir)
    //    {
    //        string current = f.Name.ToLower();
    //        //string current = pathtest.Substring(pathtest.IndexOf("Resources/")) + f.Name;
    //        if (current.Contains(objName.ToLower()) && !current.Contains(".meta"))
    //        {
    //            Texture tex = Resources.Load<Texture>("Textures/" + current);
    //            //fileData = File.ReadAllBytes(@current);
    //            //tex.LoadImage(fileData);
    //            if (current.Contains("albedo"))
    //            {
    //                Debug.Log("Main: " + mat.GetTexture("_MainTex"));
    //                mat.SetTexture("_MainTex", tex);

    //                Debug.Log("Main: " + mat.GetTexture("_MainTex"));
    //            }
    //            if (current.Contains("metallic"))
    //                mat.SetTexture("_MetallicGlossMap", tex);
    //            if (current.Contains("normal"))
    //                mat.SetTexture("_BumpMap", tex);
    //        }
    //    }
    //}

    public void CreateMaterial(ref Material mat, string objName, string stringPath)
    {
        TextAsset imageAsset; //https://docs.unity3d.com/Manual/class-TextAsset.html
        Texture2D tex = new Texture2D(2, 2);

        string pathtest = Application.dataPath + "/Resources/Artwork/test1/Materials/";
        DirectoryInfo texPath = new DirectoryInfo(@pathtest);

        mat = new Material(Shader.Find("Standard"));
        mat.name = objName;
        FileInfo[] matsInDir = texPath.GetFiles();

        mat.EnableKeyword("_NORMALMAP");
        mat.EnableKeyword("_PARALLAXMAP");
        mat.EnableKeyword("_METALLICGLOSSMAP");

        foreach (FileInfo f in matsInDir)
        {
            string current = f.Name.ToLower();
            if (current.Contains(objName.ToLower()) && !current.Contains(".meta"))
            {
                imageAsset = Resources.Load<TextAsset>(pathtest + objName + ".bytes") as TextAsset;
                tex.LoadImage(imageAsset.bytes);

                if (current.Contains("albedo"))
                    mat.SetTexture("_MainTex", tex);
                if (current.Contains("metallic"))
                    mat.SetTexture("_MetallicGlossMap", tex);
                if (current.Contains("normal"))
                    mat.SetTexture("_BumpMap", tex);
            }
        }

    }
   

}
