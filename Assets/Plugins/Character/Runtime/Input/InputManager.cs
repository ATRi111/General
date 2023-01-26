using System.Collections.Generic;
using UnityEngine.Events;

namespace Character
{
    public class InputManager<T> where T : InputEvent, new()
    {
        private readonly List<InputTuple<T>> tuples = new List<InputTuple<T>>();
        private readonly Dictionary<string, int> searchDict = new Dictionary<string, int>();

        public void Update()
        {
            for (int i = 0; i < tuples.Count; i++)
            {
                tuples[i].input.GetInput();
                tuples[i].Invoke(EInvokeMode.Update);
            }
        }

        public void FiexdUpdate()
        {
            for (int i = 0; i < tuples.Count; i++)
            {
                tuples[i].Invoke(EInvokeMode.FixedUpdate);
            }
        }

        public void Add(string axesName, UnityAction<T> action, EInvokeMode mode = EInvokeMode.Update)
        {
            if (!searchDict.ContainsKey(axesName))
            {
                tuples.Add(new InputTuple<T>(axesName));
                searchDict.Add(axesName, tuples.Count - 1);
            }
            tuples[searchDict[axesName]].Add(action, mode);
        }

        public void Remove(string axesName, UnityAction<T> action, EInvokeMode mode = EInvokeMode.Update)
        {
            if (searchDict.ContainsKey(axesName))
            {
                tuples[searchDict[axesName]].Remove(action, mode);
            }
        }

        public T GetInputEvent(string axesName)
        {
            if (searchDict.ContainsKey(axesName))
                return tuples[searchDict[axesName]].input;
            return null;
        }
    }
}