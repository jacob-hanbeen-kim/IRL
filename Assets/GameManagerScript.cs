using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Text.RegularExpressions;

public class GameManagerScript : MonoBehaviour {

    public GameObject[] vegitables;
    public GameObject pan;
    public GameObject menu;
    public GameObject cbamenu;

    private StreamReader reader;
    private bool simulate;
    private string coord;
    private float time;
    private bool paused;

    // Use this for initialization
    void Start () {
        simulate = false;
        paused = false;
        menu.SetActive(false);
    }

    private void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (paused) {
                Time.timeScale = 1;
            } else {
                Time.timeScale = 0;
            }

            paused = !paused;
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            menu.SetActive(false);
            Time.timeScale = 1;
            SceneManager.LoadScene("Demo 4");
        }
        else if (Input.GetKeyDown(KeyCode.A)) {
            int rand = Random.Range(0, vegitables.Length);
            GameObject newVegie = vegitables[rand];
            newVegie.transform.position = new Vector3(newVegie.transform.position.x, newVegie.transform.position.y + 5, newVegie.transform.position.z);
            Instantiate(newVegie);
        }
        else if (Input.GetKeyDown(KeyCode.Q)) {
            Debug.Log("running simulation...");
            OpenFile();
            simulate = true;
        }

        if (simulate)
        {
            if (hasNext())
            {
                //Debug.Log("Speed: " + ((getCoord() - pan.transform.position) / getTime()));

                pan.transform.position = Vector3.Lerp(pan.transform.position, getCoord(), getTime());
            }
        }
    }

    void OpenFile() {
        string path = "./simulate.txt";
        reader = new StreamReader(path);
        Debug.Log("Opened reader...");
    }

    bool hasNext() {
        coord = reader.ReadLine();

        if (coord == null) {
            reader.Close();
            simulate = false;
            return false;
        }

        return true;
    }

    Vector3 getCoord() {

        string[] xyzt;
        float[] xyzt_f = new float[4];

        xyzt = Regex.Split(coord, ",");

        int i = 0;
        foreach (var e in xyzt)
        {
            var num = Regex.Match(e, @"([-+]?[0-9]*\.?[0-9]+)");
            if (num.Success) {
                xyzt_f[i] = float.Parse(num.Value);
                i += 1;
            }
        }

        if (i == 4) {
            setTime(xyzt_f[3]);
            return new Vector3(xyzt_f[0], xyzt_f[1], xyzt_f[2]);
        }

        return new Vector3(1, 1, 1);
    }

    float getTime() {
        return time;
    }

    void setTime(float t) {
        time = t;
    }
}
