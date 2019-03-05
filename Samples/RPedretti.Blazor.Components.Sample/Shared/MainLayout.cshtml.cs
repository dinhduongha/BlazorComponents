using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Layouts;
using RPedretti.Blazor.Components.Sample.Managers;
using System.Collections.Generic;

namespace RPedretti.Blazor.Components.Sample.Shared
{
    public class MainLayoutBase : LayoutComponentBase
    {
        #region Properties

        [Inject] private NotificationManager NotificationManager { get; set; }

        #endregion Properties

        #region Methods

        private void ShowNotification(object sender, NotificationManager.NotificationEventArgs e)
        {
            Messages.Add(e.Message);
            StateHasChanged();
        }

        #endregion Methods

        #region Fields

        protected List<string> Messages = new List<string>();

        #endregion Fields

        protected override void OnInit()
        {
            NotificationManager.ShowNotification += ShowNotification;
        }
    }
}
