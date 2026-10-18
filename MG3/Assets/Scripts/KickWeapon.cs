using UnityEngine;
public class KickWeapon : MonoBehaviour
{
    [SerializeField] private float _lifetimeSeconds;
    [SerializeField] private Timer _lifeTimer;
    public void ActivateKick()
    {
        gameObject.SetActive(true);
        _lifeTimer.BeginTimer(_lifetimeSeconds);
    }
    private void Update()
    {
        if (_lifeTimer.IsFinished()) { gameObject.SetActive(false); }
    }
}
