using System.Collections;
using UnityEngine.UI;

public interface IUIElement
{
    Image FillImg { get; }

    float CurrentValue { get; }

    float TotalWeight { get; }

    int Index { get; }

    bool Filled { get; }

    bool Completed { get; }
    
    bool Depleted { get; }

    IEnumerator Fill(int value, bool complete = false);
}
