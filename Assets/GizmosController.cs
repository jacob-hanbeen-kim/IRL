using UnityEngine;
using System.IO;

public class GizmosController : MonoBehaviour
{
    public GameObject menu;

    private Vector3 curPos;
    private Vector3 lastPos;
    private float maxPosX;

    private float maxPosY;
    private float minPosY;
    private float constZ;

    private StreamWriter writer;
    private StreamReader reader;
    private float time;

    public GameObject[] veggies;
    private int[] steps;
    private int curStep;
    private int stepIndex;
    private Vector3 prevPos;

    private float[] maxVeggieHeight;

    private float maxHeightReached;
    private float maxTime;
    private Vector3 maxPos;
    private string maxString;

    private bool demoEnd;

    private void Start()
    {
        maxPosX = transform.position.x; //12f

        maxPosY = 7.5f;
        minPosY = 0.0f;
        constZ = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        steps = new int[] { 1, 4, 7, 4, 1 };
        curStep = 1;
        stepIndex = 0;
        maxHeightReached = 0.0f;
        prevPos = new Vector3(0, 0, 0);
        demoEnd = false;

        maxVeggieHeight = new float[veggies.Length];
    }

    private void Update()
    {
        //transform.position = Vector3.Lerp(transform.position, new Vector3(9.9f, 1.9f, -0.8f), Time.deltaTime);
        if (demoEnd) {

            int terminate = 0;
            foreach (var veggie in veggies) {
                if (veggie.GetComponent<Rigidbody>().IsSleeping()) {
                    terminate++;
                }
            }

            if (terminate == 3)
            {
                Debug.Log("DONE");
                // distance from block to pan
                string write = "";

                // number of veggie in the pan
                int count = 0;
                foreach (var height in maxVeggieHeight) {
                    write += height + ", ";
                }

                foreach (var veggie in veggies)
                {
                    float distance = Mathf.Abs(1 + transform.position.x - veggie.transform.position.x);
                    //write += distance + ", ";
                    if (distance < 4)
                    {
                        count += 1;
                    }
                }
                write += count;

                writer.WriteLine(write);
                writer.Close();

                demoEnd = false;
                menu.SetActive(true);
                Time.timeScale = 0;
            }
        }
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

        // max veggie height
        int i = 0;
        foreach (var veggie in veggies)
        {
            float relativeY = veggie.transform.position.y - newPos.y;
            if (relativeY > maxVeggieHeight[i]) {
                maxVeggieHeight[i] = relativeY;
            }
            i++;
        }

        // find the maximum height pan reaches
        if (newPos.y > maxHeightReached) {
            maxHeightReached = newPos.y;

            maxString = "3, ";

            // iterate through veggies to record the position
            foreach (var veggie in veggies)
            {
                string v = (1 + newPos.x - veggie.transform.position.x) + ", " + (1 + newPos.y - veggie.transform.position.y);
                maxString += v + ", ";
            }

            // add the time it took to reach curstep from prevstep
            //write = write.Substring(1, write.Length - 2) + ", " + (Time.timeSinceLevelLoad - time);
            maxString += (Time.timeSinceLevelLoad - time);

            // calculate directional vector
            float angle = Mathf.Atan2(newPos.y - prevPos.y, newPos.x - prevPos.x) * 180 / Mathf.PI;
            int bin = Mathf.FloorToInt(angle / 45);


            maxString += ", " + bin;

            maxTime = Time.timeSinceLevelLoad;
            maxPos = newPos;
        }

        // if the pan crosses the step point, write data
        if (stepIndex < steps.Length && ((stepIndex < 3 && newPos.y > curStep) || (stepIndex > 2 && newPos.y < curStep))) {
           
            if (stepIndex == 3) {
                Debug.Log(maxHeightReached);
                writer.WriteLine(maxString);
                time = maxTime;
                prevPos = maxPos;
            }

            Debug.Log(newPos.y);

            // write the step
            int stepIdx = stepIndex;
            if (stepIndex > 2) {
                stepIdx += 1;
            }
            string write = stepIdx + ", ";

            // iterate through veggies to record the position
            foreach (var veggie in veggies)
            {
                string v = (1 + newPos.x - veggie.transform.position.x) + ", " + (1 + newPos.y - veggie.transform.position.y);
                write += v + ", ";
            }

            // add the time it took to reach curstep from prevstep
            //write = write.Substring(1, write.Length - 2) + ", " + (Time.timeSinceLevelLoad - time);
            write += (Time.timeSinceLevelLoad - time);

            // calculate directional vector
            float angle = Mathf.Atan2(newPos.y - prevPos.y, newPos.x - prevPos.x) * 180 / Mathf.PI;
            int bin = Mathf.FloorToInt(angle / 45);


            write += ", " + bin;
            writer.WriteLine(write);

            prevPos = newPos;
            time = Time.timeSinceLevelLoad;
            stepIndex += 1;

            if (stepIndex < steps.Length)
                curStep = steps[stepIndex];
        }

        /*
        if (curPos.x > maxPosX) {
            curPos.x = maxPosX;
        } else if (curPos.x < minPosX) {
            curPos.x = minPosX;
        }*/

        if (newPos.y > maxPosY)
        {
            newPos.y = maxPosY;
        }
        else if (newPos.y < minPosY)
        {
            newPos.y = minPosY;
        }

        transform.position = newPos;
        lastPos = curPos;
    }

    private void OnMouseUp()
    {
        Debug.Log("IN");
        demoEnd = true;
    }
}