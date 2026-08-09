using UnityEngine;



public class FollowPlayerScript : MonoBehaviour
{
  public static FollowPlayerScript Instance;
  public Transform Follow;
  public float speed;
  Vector3 Moveto;

  void OnEnable()
  {
    Instance = this;

  }

    void FixedUpdate()
    {
    if(Follow != null)
    {
      Moveto = Vector3.MoveTowards(transform.position, Follow.position, speed * Vector3.Distance(transform.position, Follow.position));
      transform.position = new Vector3(Moveto.x, Moveto.y, transform.position.z);
    }
    }
}
