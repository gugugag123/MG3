using UnityEngine;

public class Kickable : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField, Range(1, 3)] private int _pointValue = 1;
    [SerializeField] private float _inertia = 1.0f;
    [SerializeField] private Rigidbody2D _rigidbody;

    // ------------------------------------------------------------------------
    // This method is called by Unity whenever the cat hits something
    //      whose collider's "Is Trigger" option is checked.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("LeftKick"))
        {
            _rigidbody.AddForce(new Vector2(-1, 1) * _inertia, ForceMode2D.Impulse);
            _player.IncreasePoints(_pointValue);
        }
        else if(other.gameObject.tag.Equals("RightKick"))
        {
            _rigidbody.AddForce(new Vector2(1, 1) * _inertia, ForceMode2D.Impulse);
            _player.IncreasePoints(_pointValue);
        }
    }
}