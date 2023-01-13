public class Enemy : Unit
{
    public override void OnDead()
    {
        GameManager.Instance.MoneyAmount += 50;
        base.OnDead();
    }
}
