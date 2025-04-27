using UnityEngine;
using System.Collections;
public class Attack : MonoBehaviour
{
    public Material laser_mat;
    public LineRenderer laser;
    public Vector3 start;
    public GameObject currentenemy;
    //public int count;
    public int damage;
    //private float timer;
    public ArrayList schedule = new ArrayList();
    private GameObject[] enemies;
    //private GameObject[] towers;
    public EnemyController currentenemy_script;
    public Scheduling parent_script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        laser = gameObject.AddComponent<LineRenderer>();
        laser.material = laser_mat;
        laser.SetPosition(0, start);
        laser.widthMultiplier = 0.2f;
        //timer = 0.5f;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
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
        //count = schedule.Count;
        /*float distance = Vector3.Distance(this.transform.position, currentenemy.transform.position);
        if (distance > 10.0f)
        {
            RemoveFromSchedule(currentenemy);
            currentenemy = null;
            //NextEnemy();
            currentenemy = (GameObject)schedule[0];
        }*/
        if (currentenemy == null)
            laser.SetPosition(1, start);
        else
        {
            laser.SetPosition(1, currentenemy.transform.position);
            currentenemy_script.health -= Time.deltaTime * damage;
            if (currentenemy_script.health < 0.0f)
            {
                parent_script.UpdateSchedules(currentenemy);
                if (schedule.Count == 0)
                {
                    laser.SetPosition(1, start);
                }
                else
                {
                    currentenemy = (GameObject)schedule[0];
                    currentenemy_script = currentenemy.GetComponent<EnemyController>();
                    laser.SetPosition(1, currentenemy.transform.position);
                }
            }
        }
    }
    /*void NextEnemy()
    {
        for(int i = 0; i < schedule.Count; i++)
            if (Vector3.Distance(schedule[i].transform.position, this.transform.position) > 10.0f)
                RemoveFromSchedule((GameObject)schedule[i]);
            else
            {
                currentenemy = (GameObject)schedule[i];
                break;
            }
    }*/
    void OnTriggerEnter(Collider other)
    {
        if (!(other.gameObject.CompareTag("Enemy")))
            return;
        schedule.Add(other.gameObject);
        if (currentenemy == null)
            currentenemy = (GameObject)schedule[0];
        currentenemy_script = currentenemy.GetComponent<EnemyController>();
    }
    void OnTriggerExit(Collider other)
    {
        if (!(other.gameObject.CompareTag("Enemy")))
            return;
        schedule.Remove(other);
        if (schedule.Count == 0)
            currentenemy = null;
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
