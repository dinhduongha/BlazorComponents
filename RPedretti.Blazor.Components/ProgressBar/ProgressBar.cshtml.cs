using Microsoft.AspNetCore.Components;

namespace RPedretti.Blazor.Components.ProgressBar
{
    public class ProgressBarBase : ComponentBase
    {
        #region Properties

        [Parameter] protected bool Active { get; set; }

        #endregion Properties
    }
}
