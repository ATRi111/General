using System.Collections;
using UnityEngine;

namespace Character
{
    //�˽ű����Զ���������Ҫ�ֶ�����
    public class InputRecorderMono : MonoBehaviour
    {
        private void OnEnable()
        {
            StartCoroutine(AfterUpdate());
            StartCoroutine(AfterFixedUpdate());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private IEnumerator AfterUpdate()
        {
            for (; ; )
            {
                InputRecorder.AfterUpdate();
                yield return null;
            }
        }

        private IEnumerator AfterFixedUpdate()
        {
            for (; ; )
            {
                InputRecorder.AfterFixedUpdate();
                yield return new WaitForFixedUpdate();
            }
        }
    }
}