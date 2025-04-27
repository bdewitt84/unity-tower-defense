using UnityEngine;
using System.Collections.Generic;
public class Scheduling : MonoBehaviour
{
    //private Attack[] tower_scripts;
    private List<Attack> tower_scripts = new List<Attack>();
    private GameObject original;
    private float x_offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        original = this.transform.GetChild(0).gameObject;
        original.SetActive(false);
        NewTower();
        NewTower();
    }
    void NewTower()
    {
        GameObject new_tower = Instantiate(original);
        Attack new_tower_script = new_tower.GetComponent<Attack>();
        tower_scripts.Add(new_tower_script);
        new_tower_script.start = new Vector3(x_offset, 2.0f, 0.0f);
        new_tower.transform.position = new Vector3(x_offset, 0.0f, 0.0f);
        
        x_offset += 10.0f;
        new_tower.SetActive(true);
    }
    public void UpdateSchedules(GameObject enemy)
    {
        for (int i = 0; i < tower_scripts.Count; i++)
            if (tower_scripts[i].schedule.Contains(enemy))
            {
                tower_scripts[i].schedule.Remove(enemy);
                if (tower_scripts[i].schedule.Count == 0)
                {
                    tower_scripts[i].laser.SetPosition(1, tower_scripts[i].start);
                    tower_scripts[i].currentenemy = null;
                    tower_scripts[i].currentenemy_script = null;
                }
                else
                {
                    tower_scripts[i].currentenemy = (GameObject)tower_scripts[i].schedule[0];
                    tower_scripts[i].currentenemy_script = tower_scripts[i].currentenemy.GetComponent<EnemyController>();
                    tower_scripts[i].laser.SetPosition(1, tower_scripts[i].currentenemy.transform.position);
                }
            }
        Destroy(enemy);
    }
}
