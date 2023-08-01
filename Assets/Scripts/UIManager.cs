using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.Networking;

//public enum DataType
//{
//    None,
//    Title,
//    Overview,
//    Image
//}
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI overview;
    [SerializeField] private Image image;
    //Dictionary<DataType, GameObject> objects = new Dictionary<DataType, GameObject>();
    private Data currentData = new Data();

    private List<Texture> images = new List<Texture>();

    private void Start()
    {
        SetDictionary();
    }

    private void SetDictionary()
    {
        //objects.Add(DataType.Title, title.gameObject);
        //objects.Add(DataType.Overview, overview.gameObject);
        //objects.Add(DataType.Image, image.gameObject);
    }

    //public GameObject GetObject(DataType type) => objects[type];
    
    public void SetValue(Data value, List<Texture> textures)
    {
        if(currentData != value)
        {
            currentData = value;
        }

        images = textures;
        ApplyValue();
    }

    public void ApplyValue()
    {
        image.material.mainTexture = images.FirstOrDefault();
    }

    
}
