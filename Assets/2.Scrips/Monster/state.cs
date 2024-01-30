public abstract class State
{
    protected Monster monster;    
    public State(Monster monster)
    {
        this.monster = monster;        
    }
    protected Boss boss;
    public State(Boss boss)
    {
        this.boss = boss; 
    }
   
    public abstract void OnstateEnter();
    public abstract void OnstateStay();
    public abstract void OnstateExit();
}
