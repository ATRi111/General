using System.Collections;
using UnityEngine;

namespace Character
{
    //此脚本会自动创建，不要手动创建
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