using System.Collections;

namespace Five.Tutorial
{
    public abstract class YieldableTutorialBase : TutorialBase
    {
        private bool isDone;

        protected abstract IEnumerator InitializeRoutine();
        protected abstract IEnumerator RunRoutine();
        public abstract IEnumerator DisposeRoutine();

        public virtual void EndRun()
        {
            isDone = true;
        }

        public IEnumerator ExecuteRoutine()
        {
            yield return InitializeRoutine();

            if (GetState() == State.Ready)
            {
                yield return RunRoutine();

                while (!isDone)
                {
                    yield return null;
                }

                yield return DisposeRoutine();
            }
        }
    }
}