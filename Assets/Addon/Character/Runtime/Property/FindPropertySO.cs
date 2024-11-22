using UnityEngine;

namespace Character
{
    public abstract class FindPropertySO : ScriptableObject
    {
        public abstract CharacterProperty FindProperty();
    }
}