using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ExprotLableValue : MonoBehaviour
{

    [ContextMenu("Export")]
    void Export()
    {
        Transform[] gos = GameObject.FindObjectsOfType<Transform>();
        for (int i = 0; i < gos.Length; i++)
        {
            Find(gos[i]);
        }
    }

    void Find(Transform trans)
    {
        if (trans.GetComponent<UILabel>() != null && trans.GetComponent<LableModifer>() == null)
        {
            LableModifer lm = trans.gameObject.AddComponent<LableModifer>();
            lm.SetConfigID(index);
            index++;
        }
        Transform[] childs = trans.GetComponentsInChildren<Transform>(true);
        for (int i = 1; i < childs.Length; i++)
        {
            Find(childs[i]);
        }
    }
    private int index = 0;
    [ContextMenu("Clear")]
    void Clear()
    {
        index = 0;
        GameObject[] allTransforms = (GameObject[])Resources.FindObjectsOfTypeAll(typeof(GameObject));
        Debug.Log(allTransforms.Length);
        for (int i = 0; i < allTransforms.Length; i++)
        {
            if (allTransforms[i].GetComponent<UILabel>() != null && allTransforms[i].GetComponent<UILabel>().text.Trim() != "")
            {
                LableModifer lm;
                if (allTransforms[i].GetComponent<LableModifer>() == null)
                    lm = allTransforms[i].gameObject.AddComponent<LableModifer>();
                else
                    lm = allTransforms[i].GetComponent<LableModifer>();
                lm.SetConfigID(index);
                index++;
            }
        }
        Debug.Log("Over-----------------------");
    }

    /// <summary>
    /// 保存到本地
    /// </summary>
    /// <param name="data"></param>
    void SaveToLocal(string data)
    {
        string saveFile = Application.dataPath;
        int pos = saveFile.LastIndexOf("/");
        saveFile = saveFile.Substring(0, pos) + "/lableConfig.txt";
        if (!File.Exists(saveFile))
            File.Create(saveFile);
        StreamWriter fw = new StreamWriter(saveFile, true);
        fw.WriteLine(data);
        fw.Close();
    }
    [ContextMenu("ClearFile")]
    void ClearLocalData()
    {
        string saveFile = Application.dataPath;
        int pos = saveFile.LastIndexOf("/");
        saveFile = saveFile.Substring(0, pos) + "/lableConfig.txt";
        ClearData(saveFile);
    }
    /// <summary>
    /// 清空文件
    /// </summary>
    /// <param name="path"></param>
    void ClearData(string path)
    {
        if (!File.Exists(path))
            return;
        FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
        stream.Seek(0, SeekOrigin.Begin);
        stream.SetLength(0);
        stream.Close();
    }
}
