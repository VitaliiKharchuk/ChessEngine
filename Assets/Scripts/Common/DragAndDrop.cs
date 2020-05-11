using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Assets.Scripts.Common
{
    public class DragAndDrop
    {
        public delegate void PickDelegate(Vector2 from);
        public delegate void DropDelegate(Vector2 from, Vector2 to);
        public delegate void ResetDelegate();

        PickDelegate pickDelegate;
        DropDelegate dropDelegate;

        public DragAndDrop(PickDelegate pickDelegate, DropDelegate dropDelegate)
        {
            state = State.None;
            offset = Vector2.zero;
            this.dropDelegate = dropDelegate;
            this.pickDelegate = pickDelegate;
        }

        enum State
        {
            None,
            Drag
        }

        State state = State.None;
        GameObject clickedItem = null;
        Vector2 offset = Vector2.zero;
        Vector2 fromPosition;
        Vector2 toPosition;
        public void Action()
        {
            switch (state)
            {
                case State.None:
                    {
                        if (IsMouseButtonPressed())
                        {
                            Pickup();
                        }
                        break;
                    }
                case State.Drag:
                    {
                        if (IsMouseButtonPressed())
                        {
                            Drag();
                        }
                        else
                        {
                            Drop();
                        }
                        break;
                    }
            }
        }

        void Drop()
        {
            toPosition = clickedItem.transform.position;
            dropDelegate(fromPosition, toPosition);
            state = State.None;
            clickedItem = null;
        }

        void Drag()
        {
            clickedItem.transform.position = GetClickPostion() + offset;
        }

        void Pickup()
        {
            var clickPosition = GetClickPostion();
            var item = GetItemAt(clickPosition);

            if (item == null)
            {
                return;
            }

            state = State.Drag;
            clickedItem = item.gameObject;
            fromPosition = item.position;
            offset = fromPosition - clickPosition;
            pickDelegate(fromPosition);
        }

        bool IsMouseButtonPressed()
        {
            return Input.GetMouseButton(0);
        }

        Vector2 GetClickPostion()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        Transform GetItemAt(Vector2 position)
        {
            var figures = Physics2D.RaycastAll(position, position, 0.5f);

            return figures.Length == 0 ? null : figures[0].transform;
        }
    }
}
