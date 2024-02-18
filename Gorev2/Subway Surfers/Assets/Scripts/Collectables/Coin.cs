public class Coin : Spawnable<Coin>, ICollectable
{
    public void OnCollect()
    {
        _pool.Release(this);
        PlayerEvents.CallCoinCollected(1);
    }
}
