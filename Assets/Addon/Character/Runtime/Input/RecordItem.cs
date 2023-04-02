using UnityEngine;

namespace Character
{
    internal class RecordItem
    {
        internal string axisName;

        //从上次Update结束后到现在，是否发生
        internal bool up_update;
        internal bool down_update;

        //从上次FixedUpdate结束后到现在，是否至少发生一次
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