using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    [SerializeField] private TMP_Text _pointsText;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _speed;
    [SerializeField] private float _jump;
    [SerializeField] private KickWeapon _leftKick;
    [SerializeField] private KickWeapon _rightKick;

    private bool _isGrounded;
    private bool _facingLeft;
    private int _points;

    private void Start()
    {
        var legend = Instantiate(_pointsText, _pointsText.transform.parent);
        legend.name = "FoodScoreLegend";
        legend.text = "Bagel / Wine: 1    Sushi / Chicken: 2    Steak: 3";
        legend.fontSize = 20;
        legend.alignment = TextAlignmentOptions.Center;
        legend.raycastTarget = false;
        legend.margin = Vector4.zero;
        var rect = legend.rectTransform;
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0f);
        rect.pivot = new Vector2(0.5f, 0f);
        rect.anchoredPosition = new Vector2(0f, 65f);
        rect.sizeDelta = new Vector2(850f, 45f);
    }

    // ------------------------------------------------------------------------
    private void Update()
    {
        // Detect input to move player left/right
        _rigidbody.linearVelocity = new Vector2(
            Input.GetAxis("Horizontal") * _speed,
            _rigidbody.linearVelocity.y
        );

        // Detect input to jump
        if (Input.GetAxis("Vertical") > 0 && _isGrounded)
        {
            _rigidbody.linearVelocity = new Vector2(
                _rigidbody.linearVelocity.x,
                _jump
            );
            _isGrounded = false;
        }

        // Change character facing 
        if (Input.GetAxis("Horizontal") < 0 && !_facingLeft)
        {
            _spriteRenderer.flipX = true;
            _facingLeft = true;
        }
        else if (Input.GetAxis("Horizontal") > 0 && _facingLeft)
        {
            _spriteRenderer.flipX = false;
            _facingLeft = false;
        }

        // Kick if the player presses space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Kick();
        }

        // Set animation state params
        _animator.SetBool("walking", _rigidbody.linearVelocity.x != 0.0f);
        _animator.SetBool("inAir", _rigidbody.linearVelocity.y != 0.0f);
    }

    // ------------------------------------------------------------------------
    public void SetGameOver ()
    {
        _rigidbody.linearVelocity = Vector2.zero;
        _leftKick.gameObject.SetActive(false);
        _rightKick.gameObject.SetActive(false);
        _animator.SetBool("walking", false);
        this.enabled = false;
    }

    // ------------------------------------------------------------------------
    // Activates the colliders that kick objects
    private void Kick()
    {
        if (_facingLeft)
        {
            _leftKick.ActivateKick();
        }
        else
        {
            _rightKick.ActivateKick();
        }

        _animator.SetTrigger("kick");
    }

    // STEP 10 -----------------------------------------------------------------
    // This method is called when the player kicks a Kickable item.
    public void IncreasePoints(int amount = 1)
    {
        _points += Mathf.Clamp(amount, 1, 3);
        _pointsText.text = "Points: " + _points;
        // Each food supplies its configured point value.

        
    }
    // STEP 10 -----------------------------------------------------------------

    // ------------------------------------------------------------------------
    // Don't let the player jump again until they've touched the ground
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isGrounded = true;
    }
}
