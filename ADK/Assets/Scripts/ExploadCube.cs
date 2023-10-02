using UnityEngine;
public class ExploadCube : MonoBehaviour
{
    private bool _collisionSet;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Cube" && !_collisionSet)
        { 
            for (int i = collision.transform.childCount - 1; i >= 0; i--)
            {
                Transform Child = collision.transform.GetChild(i);
                Child.gameObject.AddComponent<Rigidbody>();
                Child.GetComponent<Rigidbody>().AddExplosionForce(70f, Vector3.up, 5f);
                Child.SetParent(null);
            }

            Destroy(collision.gameObject);
            _collisionSet = true;
        }
    }
}