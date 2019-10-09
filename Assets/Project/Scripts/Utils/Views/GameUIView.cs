using Toastapp.DesignPatterns;
using UnityEngine;

public class GameUIView : BaseView<GameUIViewModel>
{
    #region Singleton
    private static GameUIView mInstance;

    public static GameUIView Get
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = FindObjectOfType<GameUIView>();
            }
            return mInstance;
        }
    }
    #endregion

    private TrashManager trashManager;

    // Useful to cast ViewModel
    protected GameUIViewModel Model => this.ViewModel;

    public void SetTrashManager(TrashManager trashManager)
    {
        this.trashManager = trashManager;

        if (this.trashManager != null && this.Model != null)
        {
            this.trashManager.OnTriggerEnterAction += this.Model.OnTriggerEnterAction;
        }
    }

    public override void OnDestroy()
    {
        if (this.trashManager != null && this.Model != null)
        {
            this.trashManager.OnTriggerEnterAction -= this.Model.OnTriggerEnterAction;
        }
    }
}
