using Five;
using System.Collections;
using UnityEngine;

public abstract class Tutorial : MonoBehaviour
{
    [SerializeField] protected string id;
    [SerializeField] protected int levelNumberToRun;

    protected State currentState = State.NonActive;

    public virtual bool ShouldRunTutorial => IsTutorialLevel && GetState() != State.Completed && GetState() != State.Skipped;

    protected abstract bool IsTutorialLevel { get; }

    protected abstract void OnSecurePrefsInitialized();

    public void SetState(State newTutorialState)
    {
        if (string.IsNullOrEmpty(id))
        {
            ThrowEmptyIdException();
            return;
        }

        SecurePrefs.SetInt(id, (int)newTutorialState);
        currentState = newTutorialState;
    }

    public State GetState()
    {
        if (string.IsNullOrEmpty(id))
        {
            ThrowEmptyIdException();
            return State.NonActive;
        }

        if (!SecurePrefs.HasKey(id))
        {
            SetState(State.NonActive);
        }
        else
        {
            var intValue = SecurePrefs.GetInt(id);
            currentState = (State)intValue;
        }

        return currentState;
    }

    private IEnumerator Start()
    {
        yield return WaitFor.SecurePrefsInitialized();

        if (string.IsNullOrEmpty(id))
        {
            ThrowEmptyIdException();
            yield break;
        }

        OnSecurePrefsInitialized();
    }

    private void ThrowEmptyIdException()
    {
        var e = new System.Exception("tutorial: id cannot be empty");
        Debug.LogException(e);
        Debug.Break();
    }

    public enum State
    {
        NonActive,
        Ready,
        Completed,
        Skipped
    }
}