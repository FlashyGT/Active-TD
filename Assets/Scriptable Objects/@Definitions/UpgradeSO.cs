using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Scriptable Objects/Upgrade")]
public class UpgradeSO : ScriptableObject
{
    [field: SerializeField] public UpgradeType UpgradeType { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public string Description { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
}
