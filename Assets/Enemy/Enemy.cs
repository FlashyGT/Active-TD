public class Enemy : Unit
{
    public override void OnDead()
    {
        GameManager.Instance.MoneyAmount += 10;
        base.OnDead();
    }
}
