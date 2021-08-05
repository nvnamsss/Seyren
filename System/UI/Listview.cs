using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Listview : MonoBehaviour
{

    /*What we do here?
     * Create an object to storage all necessary things of listview
     * Header is an object
     * Row is an object
     *  Each column in header is an object thus we can interact with header column 
     *  header column will have self margin, these margin will be changed automatically
     *  picture: 
     *      -Header
     *          HeaderColumn1
     *          HeaderColumn2
     *          HeaderColumn3
     *      -Rows
     *          Row1
     *              RowColumn1
     *              RowColumn2
     *              RowColumn3
     *          Row2
     *              RowColumn1
     *              RowColumn2
     *              RowColumn3
     *  So a Header is also a row but it will have some unique features to match with a Header instead of a Row
     *  then we will have a diagram following these rules:
     *  ListView -> Header
     *           -> Rows -> Row
     *  Header and Row are also have event
     *  Row will be updated when Header is updated
     */
    /// <summary>
    /// Represents data of a tile in view
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public struct ViewData
    {
        public object value;
        public GameObject gameObject;
        public ViewData(object value, GameObject gameObject)
        {
            this.value = value;
            this.gameObject = gameObject;
        }
    }

    public struct HeaderData
    {
        public string header;
        public GameObject gameObject;
        public HeaderData(string header, GameObject gameObject)
        {
            this.header = header;
            this.gameObject = gameObject;
        }
    }
    public struct RowData
    {
        public List<ViewData> datas;
        public GameObject gameObject;

        public RowData(GameObject gameObject)
        {
            this.datas = new List<ViewData>();
            this.gameObject = gameObject;
        }

        public void AddData(ViewData data)
        {
            datas.Add(data);
        }

        public void RemoveData(ViewData data)
        {
            datas.Remove(data);
        }
    }
    /// <summary>
    /// Width of listview
    /// </summary>
    public int Width;
    /// <summary>
    /// Height of listview
    /// </summary>
    public int Height;
    /// <summary>
    /// List headers, include titles
    /// </summary>
    public List<HeaderData> Headers;
    /// <summary>
    /// List rows, include datas
    /// </summary>
    public List<RowData> Rows;
    
    /// <summary>
    /// Add new header, do not allow duplicate
    /// </summary>
    /// <param name="name"></param>
    public void AddHeader(string name)
    {
        for (int loop = 0; loop < 0; loop++)
        {
            if (Headers[loop].header == name)
            {
                return;
            }
        }

        GameObject go = new GameObject();
        Text text = go.AddComponent<Text>();
        text.text = name;
        HeaderData header = new HeaderData(name, go);
        Headers.Add(header);
        for (int loop = 0; loop < Rows.Count; loop++)
        {
            AddView(Rows[loop], string.Empty);
        }
    }

    public void RemoveHeader(string name)
    {

    }


    public void AddRow(object o)
    {
        //using reflection to get showing data
        for (int loop = 0; loop < Headers.Count; loop++)
        {

        }
    }

    public void RemoveRow(object o)
    {

    }

    public void RemoveRowAt(int index)
    {

    }


    private void AddView(RowData row, object value)
    {
        GameObject go = new GameObject();
        ViewData data = new ViewData(value, go);

        row.AddData(data);
    }
}
