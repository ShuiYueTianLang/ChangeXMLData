using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;

public class XMLConfig : MonoBehaviour
{

    XmlDocument xmlDoc;
    public string saveFile;//导出数据
    private int overCount = 0;//处理完毕的文件数量
    private Dictionary<string, int> cacheStr = new Dictionary<string, int>();//缓存数据，避免重复数据
    public string rootStr = "root";
    [Header("是否导出数据")]
    public bool isExportConfig = true;//是否导出数据
    // Use this for initialization
    void Start()
    {
        string filePath = Application.dataPath;
        int indexOfLast = filePath.LastIndexOf('/');
        filePath = filePath.Substring(0, indexOfLast);
        saveFile = filePath + "/" + saveFile;
        filePath += "/config";
        Debug.Log("读取地址：" + filePath);
        Debug.Log("保存地址：" + saveFile);
        
        StartCoroutine(StartHandle(filePath));
    }

    IEnumerator StartHandle(string filePath)
    {
        ClearData(saveFile);
        yield return new WaitForSeconds(1);
        HandleConfig(filePath);
    }

    void IgnoreInfo()
    {
        XmlReaderSettings settings = new XmlReaderSettings();
        settings.IgnoreComments = true;
    }

    void HandleConfig(string p)
    {
        string[] files = Directory.GetFiles(p);
        for (int i = 0; i < files.Length; i++)
        {

            xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(files[i]);
                IgnoreInfo();
                XmlNode root = xmlDoc.SelectSingleNode(rootStr);
                if (root == null)
                {
                    Debug.LogError("此文件根节点不为root" + files[i]);
                    break;
                }
                XmlNodeList list = root.ChildNodes;
                for (int j = 0; j < list.Count; j++)
                {
                    HandleNode(list[j]);
                }
                overCount++;
                Debug.Log(files[i] + "已处理完毕;已处理完毕文件数量：" + overCount.ToString());
            }
            catch (System.Exception ex)
            {
                Debug.LogError(files[i] + "文本处理报错：" + ex.Message);
            }
            xmlDoc.Save(files[i]);
        }
        Debug.Log("处理结束分割线-----------------------------------------------------------------");
    }
    /// <summary>
    /// 处理节点属性，遍历节点所有属性值，若属性值包含汉字，则提取。
    /// </summary>
    /// <param name="node"></param>
    void HandleNode(XmlNode node)
    {
        XmlAttributeCollection attributeCollection = node.Attributes;//获得所有属性
        if (attributeCollection == null)
        {
            Debug.LogWarning("注释");//此读取xml方式，不能过滤文本中的注释。
            return;
        }
        for (int i = 0; i < attributeCollection.Count; i++)
        {
            if (HasChinese(attributeCollection[i].InnerText))//判断每个属性值是否包含汉字
            {
                if (isExportConfig)
                {
                    SaveText(attributeCollection[i].InnerText);
                }
                else
                    attributeCollection[i].InnerText = "已修改";

            }
        }
        XmlNodeList childNodes = node.ChildNodes;
        if (childNodes.Count > 0)
        {
            for (int i = 0; i < childNodes.Count; i++)
            {
                HandleNode(childNodes[i]);
            }
        }
    }

    private int saveCount = 0;

    /// <summary>
    /// 保存在字典内，以防重复数据
    /// </summary>
    /// <param name="data"></param>
    void SaveText(string data)
    {
        if (cacheStr.ContainsKey(data))
        {
            return;
        }
        cacheStr.Add(data, saveCount);
        saveCount++;
        SaveToLocal(data);
    }
    /// <summary>
    /// 保存到本地
    /// </summary>
    /// <param name="data"></param>
    void SaveToLocal(string data)
    {
        //if (!File.Exists(saveFile))
        //{
        //    File.Create(saveFile);
        //}
            
        StreamWriter fw = new StreamWriter(saveFile, true);
        fw.WriteLine(data + ":");
        fw.Close();
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

    /// <summary>
    /// 判断字符串中是否包含中文
    /// </summary>
    /// <param name="str">需要判断的字符串</param>
    /// <returns>判断结果</returns>
    public static bool HasChinese(string str)
    {
        return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
    }
}
