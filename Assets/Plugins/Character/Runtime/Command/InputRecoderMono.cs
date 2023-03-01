using System.Collections;
using UnityEngine;

namespace Character
{
    //�˽ű����Զ���������Ҫ�ֶ�����
    public class InputRecoderMono : MonoBehaviour
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
                InputRecoder.AfterUpdate();
                yield return null;
            }
        }

        private IEnumerator AfterFixedUpdate()
        {
            for(; ; )
            {
                InputRecoder.AfterFixedUpdate();
                yield return new WaitForFixedUpdate();
            }
        }
    }
}