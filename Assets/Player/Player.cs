public class Player : Unit
{
    public override void OnDead()
    {
        base.OnDead();
        GameManager.Instance.GameLost();
    }
}