////////////////////////////////////////////////////////////
/////   StateController.cs
/////   James McNeil - 2020
////////////////////////////////////////////////////////////

using System.Collections.Generic;

using static UnityEngine.Debug;

namespace PersonalFramework
{
    public class StateController
    {
        private Stack<FlowStateBase> m_stateStack = new Stack<FlowStateBase>();
        private FlowStateBase m_switchingState = null;

        public void PushState(FlowStateBase state)
        {
            Assert(m_stateStack.Count == 0 || m_stateStack.Peek() != state, "Trying to push already active state");
            
            if(m_stateStack.Count > 0)
            {
                FlowStateBase activeState = m_stateStack.Peek();
                activeState.EnteredBackground();
            }

            m_stateStack.Push(state);
            state.SetStateController(this);
        }

        public void PopState(FlowStateBase state)
        {
            Assert(m_stateStack.Count > 0 && m_stateStack.Peek() == state, "Trying to pop non active state");
            m_stateStack.Peek().EndActiveState();
        }

        public void SwitchState(FlowStateBase state)
        {
            Assert(m_stateStack.Count > 0, "Trying to switch from no state");
            Assert(m_stateStack.Peek() != state, "Trying to switch to the same state");

            PopState(m_stateStack.Peek());
            m_switchingState = state;
        }

        public void UpdateStack()
        {
            if (m_stateStack.Count > 0)
            {
                FlowStateBase state = m_stateStack.Peek();
                state.UpdateState();

                if (state.IsDismissed)
                {
                    m_stateStack.Pop();
                    SetNextState();
                }
            }
        }

        public void FixedUpdateStack()
        {
            if (m_stateStack.Count > 0)
            {
                FlowStateBase state = m_stateStack.Peek();
                state.FixedUpdateState();
            }
        }

        private void SetNextState()
        {
            if (m_switchingState != null)
            {
                PushState(m_switchingState);
                m_switchingState = null;
            }
            else if(m_stateStack.Count > 0)
            {
                m_stateStack.Peek().ReenteredForeground();
            }
        }
    }
}