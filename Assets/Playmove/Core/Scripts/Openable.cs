using Playmove.Core.BasicEvents;
using UnityEngine;

namespace Playmove.Core
{
    public enum OpenableState
    {
        Opening,
        Opened,
        Closing,
        Closed
    }

    public abstract class Openable : MonoBehaviour
    {
        public PlaytableEvent<Openable> OnOpening = new();
        public PlaytableEvent<Openable> OnOpened = new();
        public PlaytableEvent<Openable> OnClosing = new();
        public PlaytableEvent<Openable> OnClosed = new();

        public OpenableState State { get; protected set; } = OpenableState.Closed;
        public bool IsOpen => State == OpenableState.Opening || State == OpenableState.Opened;
        public bool IsClose => State == OpenableState.Closing || State == OpenableState.Closed;

        public virtual void Open()
        {
            State = OpenableState.Opening;
            OnOpening.Invoke(this);
        }

        protected virtual void Opened()
        {
            State = OpenableState.Opened;
            OnOpened.Invoke(this);
        }

        public virtual void Close()
        {
            State = OpenableState.Closing;
            OnClosing.Invoke(this);
        }

        protected virtual void Closed()
        {
            State = OpenableState.Closed;
            OnClosed.Invoke(this);
        }
    }
}