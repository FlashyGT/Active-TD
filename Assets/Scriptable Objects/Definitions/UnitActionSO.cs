using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Unit Action", menuName = "Scriptable Objects/Unit Action")]
public class UnitActionSO : ScriptableObject
{
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public float SecondsToComplete { get; private set; }
    [field: SerializeField] public UnitActionType UnitActionType { get; private set; }
    [field: SerializeField] public UnitActionItem UnitActionItem { get; private set; }
}