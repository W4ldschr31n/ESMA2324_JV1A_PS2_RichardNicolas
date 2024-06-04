using UnityEngine;

public class SpriteSpin : MonoBehaviour
{
    public float spinSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, -spinSpeed*Time.deltaTime));
    }
}
