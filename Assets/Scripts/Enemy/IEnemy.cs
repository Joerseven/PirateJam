interface IEnemy
{
    // When we start to make the enemies more complex we can add more functions to this interface.
    public void Move();
    public void Attack();
    public float GetAttackRange();
}