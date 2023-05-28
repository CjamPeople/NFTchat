using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LoadGallery : MonoBehaviour
{
    public RawImage img;
    private bool isImageLoaded = false;

    public bool IsImageLoaded
    {
        get { return isImageLoaded; }
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
    }

    public void OnClickImageLoad()
    {
        NativeGallery.GetImageFromGallery((file) =>
        {
            FileInfo selected = new FileInfo(file);

            if(selected.Length < 50000000) return;

            if(!string.IsNullOrEmpty(file))
            {
                StartCoroutine(LoadImage(file));
            }

        });

        
    }

    IEnumerator LoadImage(string path)
    {
        yield return null;

        byte[] fileData = File.ReadAllBytes(path);
        string filename = Path.GetFileName(path).Split('.')[0];
        string savePath = Application.persistentDataPath + "/Image";

        if(!Directory.Exists(savePath))
        {
            Directory.CreateDirectory(savePath);
        }

        File.WriteAllBytes(savePath + filename + ".png", fileData);

        var temp =  File.ReadAllBytes(savePath + filename + ".png");

        Texture2D tex = new Texture2D(0,0);
        tex.LoadImage(temp);

        img.texture = tex;
        img.SetNativeSize();
        ImageSizeSetting(img, 600, 600);

        isImageLoaded = true;
    }

    void ImageSizeSetting(RawImage img, float x, float y)
    {
        var imgX = img.rectTransform.sizeDelta.x;
        var imgY = img.rectTransform.sizeDelta.y;

        if(x/y > imgX/imgY)
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,y);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,imgX * (y/imgY));
        }
        else
        {
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal,x);
            img.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,imgY * (x/imgX));
        }
    }
}
