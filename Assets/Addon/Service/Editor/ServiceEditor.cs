using EditorExtend;
using UnityEditor;

namespace Services
{
    [CustomEditor(typeof(Service), true)]
    public class ServiceEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty isGlobal;

        private Service service;

        protected override void OnEnable()
        {
            base.OnEnable();
            service = target as Service;
        }

        protected override void MyOnInspectorGUI()
        {
            isGlobal.BoolField("全局服务");
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("作用域", service.isGlobal ? "全局" : service.gameObject.scene.name);
            EditorGUI.EndDisabledGroup();
        }
    }
}
