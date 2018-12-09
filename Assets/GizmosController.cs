using UnityEngine;
using System.IO;

public class GizmosController : MonoBehaviour
{

    private Vector3 curPos;
    private Vector3 lastPos;
    private float maxPosX;
    private float minPosX;
    private float maxPosY;
    private float minPosY;
    private float constZ;

    private StreamWriter writer;
    private StreamReader reader;
    private float time;

    private void Start()
    {
        maxPosX = transform.position.x; //12f
        minPosX = 9.5f;
        maxPosY = 3.5f;
        minPosY = 1.8f;
        constZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    }

    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, new Vector3(9.9f, 1.9f, -0.8f), Time.deltaTime);
    }

    private void OnMouseDown()
    {
        Debug.Log("Start writing...");
        time = Time.timeSinceLevelLoad;
        string path = "./data.txt";
        var stream = new FileStream(path, FileMode.Truncate);
        writer = new StreamWriter(stream);

        lastPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, constZ));
    }

    void OnMouseDrag()
    {
        curPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, constZ));


        Vector3 delta = curPos - lastPos;
        Vector3 newPos = transform.position + delta;

        //Debug.Log(curPos);
        string write = newPos + "";
        write = write.Substring(1,write.Length - 2) + ", " + (Time.timeSinceLevelLoad - time);
        //Debug.Log(write);
        writer.WriteLine(write);

        /*
        if (curPos.x > maxPosX) {
            curPos.x = maxPosX;
        } else if (curPos.x < minPosX) {
            curPos.x = minPosX;
        }*/

        /*
        if (curPos.y > maxPosY)
        {
            curPos.y = maxPosY;
        }
        else if (curPos.y < minPosY)
        {
            curPos.y = minPosY;
        }*/
        transform.position = newPos;
        lastPos = curPos;
    }

    private void OnMouseUp()
    {
        writer.Close();
    }
}