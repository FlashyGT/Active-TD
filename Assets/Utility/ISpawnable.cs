using UnityEngine.Events;

public interface ISpawnable
{
    // Used for this specific objects to manage components and callbacks for external scripts
    public UnityEvent OnObjDeath { get; set; }
    public UnityEvent OnObjRespawn { get; set; }
}
