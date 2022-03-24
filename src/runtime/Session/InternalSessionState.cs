using System.Management.Automation;

namespace AutomationIoC.Runtime.Session
{
    internal class InternalSessionState : ISessionState
    {
        private readonly SessionState sessionState;

        public InternalSessionState(SessionState sessionState)
        {
            this.sessionState = sessionState;
        }

        PSVariableIntrinsics ISessionState.PSVariable => sessionState.PSVariable;
    }
}
