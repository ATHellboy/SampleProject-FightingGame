using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class StateMachine
    {
        public void Start(IStateMachineContext context)
        {
            ChangeState(null, context.StartState, context);
        }

        public void Update(float deltaTime, IStateMachineContext context)
        {
            if (!context.IsActive)
                return;

            if (context.CurrentState != null) context.CurrentState.Update(deltaTime, this, context);
        }

        public void FixedUpdate(float deltaTime, IStateMachineContext context)
        {
            if (!context.IsActive)
                return;

            if (context.CurrentState != null) context.CurrentState.FixedUpdate(deltaTime, this, context);
        }

        public void LateUpdate(float deltaTime, IStateMachineContext context)
        {
            if (!context.IsActive)
                return;

            if (context.CurrentState != null) context.CurrentState.LateUpdate(deltaTime, this, context);
        }

        private void ChangeState(IState newState, IStateMachineContext context)
        {
            if (context.DebugStateMachine)
            {
                Debug.Log(context.GO.name + " " + context.GetType().Name + " " + newState.GetType().Name);
            }

            if (context.CurrentState != null) context.CurrentState.Exit(context);
            context.CurrentState = newState;
            if (context.CurrentState != null) context.CurrentState.Enter(context);
        }

        public void ChangeState(object sender, IState newState, IStateMachineContext context)
        {
            if (!context.IsActive && sender != null)
                return;

            if (sender == context.CurrentState)
            {
                ChangeState(newState, context);
            }
        }

        public void Reset(IStateMachineContext context)
        {
            ChangeState(context.StartState, context);
        }
    }
}