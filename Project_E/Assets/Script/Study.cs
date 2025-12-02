using UnityEngine;

public class Study : MonoBehaviour
{
    public string say = "";
    public char text = 'a';
    public int var01 = 1;
    public float var02 = 0.4f;
    public double var03 = 0.4;
    public bool var04 = true; 

    void Start()
    {
        Debug.Log(this.say);
        Debug.Log(this.text);

        int a = 10;

        Debug.Log(a++);
        Debug.Log(++a);

        Debug.Log(Random.Range(0, 100));

    }

    void Update()
    {
        
    }
}
