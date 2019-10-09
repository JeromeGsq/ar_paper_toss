using Toastapp.DesignPatterns;
using UnityEngine;
using UnityWeld.Binding;

[Binding]
public class GameUIViewModel : BaseViewModel
{
    private int points;

    [Binding]
    public int Points
    {
        get => this.points;
        set
        {
            this.Set(ref this.points, value, nameof(this.Points));
            this.RaisePropertyChanged(nameof(this.Score));
        }
    }

    [Binding]
    public string Score => $"Score: {this.Points}";

    private void OnEnable()
    {
    }

    public void OnTriggerEnterAction(Collider collider)
    {
        this.Points++;
    }
}
