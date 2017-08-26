using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class TestXml : MonoBehaviour
{
    public TextAsset tex;
    public Vector3[] map02V;
    public Vector3[] map03V;
    public Vector3[] map04V;
    public Vector3[] map05V;
    public Vector3[] moudleV;
    //public Vector3 moduleV5;
    // Use this for initialization
    void Start()
    {
        ChangeXmlData();
    }

    void ChangeXmlData()
    {
        string data = tex.text;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(data);
        XmlNodeList list = xmlDoc.GetElementsByTagName("subBigLevel");
        for (int i = 0; i < list.Count; i++)
        {
            Debug.Log(list[i].Attributes["imageName"].InnerText + " " + list[i].Attributes["imageName"].InnerXml);
            if (list[i].Attributes["imageName"].InnerXml == "Image_Map01")
                Debug.Log("ddd");
            if (list[i].Attributes["imageName"].InnerXml == "Image_Map02")
            {
                #region
                //XmlNode subSmall = list[i].ChildNodes[0];
                //try
                //{
                //    Debug.Log("position" + subSmall.Attributes["position"].InnerXml);
                //    if (subSmall.Attributes["position"] != null)
                //    {
                //        subSmall.Attributes["position"].InnerXml = "11";
                //    }
                //}
                //catch
                //{ }
                #endregion
                ChangeData(list[i], map02V, 0);
            }
            else
            {
                #region
                if (list[i].Attributes["imageName"].InnerXml == "Image_Map03")
                {
                    ChangeData(list[i], map03V, 1);
                }
                else
                {
                    if (list[i].Attributes["imageName"].InnerXml == "Image_Map04")
                    {
                        ChangeData(list[i], map04V, 2);
                    }
                    else
                    {
                        if (list[i].Attributes["imageName"].InnerXml == "Image_Map05")
                        {
                            ChangeData(list[i], map05V, 3);
                        }
                    }
                }
                #endregion
            }
        }
        xmlDoc.Save("SubLevelPrototype.xml");
        Debug.Log("over");
    }

    void ChangeData(XmlNode nodes, Vector3[] vs, int index)
    {
        XmlNodeList subs = nodes.ChildNodes;
        for (int j = 0; j < subs.Count; j++)
        {
            if(j<vs.Length)
            {
                if (subs[j].Attributes["position"] != null)
                {
                    subs[j].Attributes["position"].InnerXml = vs[j].x.ToString() + "," + vs[j].y.ToString() + "," + vs[j].z.ToString();
                }
            }
            if(subs[j].Attributes["modulePos"]!=null)
            {
                subs[j].Attributes["modulePos"].InnerXml = moudleV[index].x.ToString() + "," + moudleV[index].y.ToString() + "," + moudleV[index].z.ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
