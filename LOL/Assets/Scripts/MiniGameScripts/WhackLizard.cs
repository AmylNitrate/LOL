using UnityEngine;
using System.Collections;

public class WhackLizard : MonoBehaviour {

    public bool isTail;
    public GameObject myHole;

    public float speed;

    [SerializeField]
    bool moveBack;

    Vector3 originalPos;

    public void Init(GameObject obj)
    {
        myHole = obj;
        originalPos = transform.position;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (!moveBack && transform.position.z > originalPos.z - 1f)
        {
            Vector3 temp = transform.position;
            temp.z -= speed * Time.deltaTime;
            transform.position = temp;
        }
        else
        {
            moveBack = true;
        }
        if (moveBack && transform.position.z < originalPos.z)
        {
            Vector3 temp = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime);
            transform.position = temp;
        }
        else if (moveBack)
        {
            Finish();
        }
    }

    void Finish()
    {
        myHole.GetComponent<WhackHole>().isOccupied = false;
        Destroy(this.gameObject);
    }

    void OnMouseDown()
    {
        Data.control.energy--;
        //Give points
        if (!isTail)
        {
            Data.control.points += 5;
        }
        else
        {
            Data.control.points -= 5;
        }
        Finish();
    }
}
