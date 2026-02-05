//I really wanted that cool parallax effect, so I looked it up: https://medium.com/@Code_With_K/parallax-background-in-unity-fd8766d5a9bd

using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    private float length, startpos;
    public GameObject cam;
    public float parallaxEffect;

    void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffect));
        float dist = (cam.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        //if (temp > startpos + length) startpos += length;
        //else if (temp < startpos - length) startpos -= length;
    }
}