public abstract class BaseState{
    public EnemyTM enemyTM;
    public StateMachine stateMachine;

    public abstract void Enter();
    public abstract void Perform();
    public abstract void Exit();

}