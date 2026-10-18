using UnityEngine;
public class Timer : MonoBehaviour
{
    [SerializeField] private float _timeLeft;
    public float defaultFloat;
    public int defaultInt;
    public bool defaultBool;
    public string defaultString;
    public void BeginTimer(float seconds) { _timeLeft = Mathf.Max(0f, seconds); }
    private void Update() { _timeLeft = Mathf.Max(0f, _timeLeft - Time.deltaTime); }
    public float GetTimeLeft() { return _timeLeft; }
    public bool IsFinished() { return _timeLeft <= 0f; }
}
