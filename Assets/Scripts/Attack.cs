using UnityEngine;
using System.Collections;
public class Attack : MonoBehaviour
{
    public Material laser_mat;
    private LineRenderer laser;
    public Vector3 start;
    private GameObject currentenemy;
    //private float timer;
    public ArrayList schedule = new ArrayList();//<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laser = gameObject.AddComponent<LineRenderer>();
        laser.material = laser_mat;
        laser.SetPosition(0, start);
        laser.widthMultiplier = 0.2f;
        //timer = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        //if(timer >= 0.5f)
        //{
        //    timer -= 0.5f;
        //    Occasional();
        //}
        if (schedule.Count == 0)
            laser.SetPosition(1, start);
        else
        {
            //currentenemy = schedule[0];
            laser.SetPosition(1, currentenemy.transform.position);
        }
    }
    public void Schedule(GameObject enemy)
    {
        schedule.Add(enemy);
    }
    public void RemoveFromSchedule(GameObject enemy)
    {
        schedule.Remove(enemy);
        if (schedule.Count == 0)
            laser.SetPosition(1, start);
        else
        {
            //currentenemy = schedule[0];
            laser.SetPosition(1, enemy.transform.position);
        }
    }
    /*void Occasional()
    {
        enemy.transform.position = new Vector3(Random.Range(-20.0f, 20.0f), 0.0f, Random.Range(-20.0f, 20.0f));
        if (Vector3.Distance(enemy.transform.position, start) >= 14.1421356237f) //sqrt of 200, or 10^2 + 10^2
            laser.SetPosition(1, start);
        else
            laser.SetPosition(1, enemy.transform.position);
    }*/
}
