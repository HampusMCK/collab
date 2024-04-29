using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItem : MonoBehaviour
{
    public List<GameObject> ItemsToDrop;
    public int hp;
    public GameObject foot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void dropItems()
    {
        foreach (GameObject g in ItemsToDrop)
        {
        int amountToDrop = Random.Range(1, 3);
            for (int i = 0; i < amountToDrop; i++)
            {
                GameObject s = Instantiate(g, foot.transform.position, g.transform.rotation);
                s.transform.Rotate(0, 0, Random.Range(0, 360), Space.Self);
            }
        }
        Destroy(gameObject);
    }

    public void punch(int dmg)
    {
        hp -= dmg;
        print(hp);
        if (hp <= 0)
        {
            dropItems();
        }
    }
}