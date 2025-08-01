using System.Collections;

namespace Five.Tutorial
{
    public abstract class YieldableTutorialBase : TutorialBase
    {
        private bool isDone;

        protected abstract IEnumerator OnInitializeRoutine();
        protected abstract IEnumerator OnRunRoutine();
        protected abstract IEnumerator OnDisposeRoutine();

        public IEnumerator InitializeRoutine()
        {
            yield return OnInitializeRoutine();
        }

        public IEnumerator RunThenDisposeRoutine()
        {
            if (isDone)
            {
                yield return OnDisposeRoutine();
                yield break;
            }

            if (GetState() == State.Ready)
            {
                yield return OnRunRoutine();

                while (!isDone)
                {
                    yield return null;
                }

                yield return OnDisposeRoutine();
            }
        }

        public IEnumerator InitializeRunThenDisposeRoutine()
        {
            yield return InitializeRoutine();

            if (isDone)
            {
                yield return OnDisposeRoutine();
                yield break;
            }

            if (GetState() == State.Ready)
            {
                yield return OnRunRoutine();

                while (!isDone)
                {
                    yield return null;
                }

                yield return OnDisposeRoutine();
            }
        }

        protected void EndRun()
        {
            isDone = true;
        }
    }
}