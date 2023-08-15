using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float speed = 5f;

    void Update()
    {
        var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        var position = transform.position;
        transform.position = position + movement * speed * Time.deltaTime;

    }
}
