namespace CalculatorBrain;

public class CalculatorBase

{
    public decimal CurrentState { get; protected set; }

    public void SetState(decimal state)
    {
        CurrentState = state;
    }
}