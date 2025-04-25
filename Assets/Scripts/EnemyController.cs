using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 pos;
    private float timer;
    public GameObject tower;
    private bool scheduled;
    private Attack tower_script;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scheduled = false;
        tower_script = tower.GetComponent<Attack>();
        this.gameObject.transform.position = new Vector3(12.0f, 0.0f, 5.0f);
        timer = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        this.gameObject.transform.position = new Vector3(5.0f * Mathf.Sin(timer) + 12.0f, 0.0f, 5.0f);// 10.0f * Mathf.Cos(timer));

        if (Vector3.Distance(this.transform.position, tower.transform.position) <= 10.0f)
        {
            tower_script.Schedule(this.gameObject);//.Add(this.gameObject);
            scheduled = true;
        }
        else
        {
            if (scheduled)
            {
                tower_script.RemoveFromSchedule(this.gameObject);
                scheduled = false;
            }
        }
    }
}
