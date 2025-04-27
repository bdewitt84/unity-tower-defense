using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //public Vector3 pos;
    //private float timer;
    //public GameObject tower;
    //private bool scheduled;
    //private Attack tower_script;
    public float health;
    public Vector3 starting_offset;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //scheduled = false;
        //tower_script = tower.GetComponent<Attack>();
        this.gameObject.transform.position = starting_offset;
        //timer = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        this.gameObject.transform.position += new Vector3(Time.deltaTime * speed, 0.0f, 0.0f);// 10.0f * Mathf.Cos(timer));
    }
}
