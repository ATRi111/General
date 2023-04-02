using UnityEngine;

namespace Character
{
    internal class RecordItem
    {
        internal string axisName;

        //���ϴ�Update���������ڣ��Ƿ���
        internal bool up_update;
        internal bool down_update;

        //���ϴ�FixedUpdate���������ڣ��Ƿ����ٷ���һ��
        internal bool up_fixedUpdate;
        internal bool down_fixedUpdate;

        public RecordItem(string axisName)
        {
            this.axisName = axisName;
        }

        internal void AfterUpdate()
        {
            up_update = Input.GetButtonUp(axisName);
            down_update = Input.GetButtonDown(axisName);
            if (up_update)
                up_fixedUpdate = true;
            if (up_fixedUpdate)
                down_fixedUpdate = true;
        }

        internal void AfterFixedUpdate()
        {
            up_fixedUpdate = false;
            down_fixedUpdate = false;
        }
    }
}