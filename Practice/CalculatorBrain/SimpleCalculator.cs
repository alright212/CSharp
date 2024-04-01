namespace CalculatorBrain;

public class SimpleCalculator : CalculatorBase, ISimpleCalculation
{
    public void Add(decimal x)
    {
        CurrentState = CurrentState + x;
    }

    public void Minus(decimal x)
    {
        CurrentState = CurrentState - x;
    }

    public void Multiply(decimal x)
    {
        CurrentState = CurrentState * x;
    }

    public void Divide(decimal x)
    {
        CurrentState = CurrentState / x;
    }
}