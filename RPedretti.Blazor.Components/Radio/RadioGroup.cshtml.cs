﻿using Microsoft.AspNetCore.Components;
using System;

namespace RPedretti.Blazor.Components.Radio
{
    public class RadioGroupBase : BaseComponent
    {
        #region Properties

        [Parameter] protected RadioButton[] Buttons { get; set; }
        [Parameter] protected bool CanDeselect { get; set; }
        [Parameter] protected RadioOrientation Orientation { get; set; } = RadioOrientation.VERTICAL;
        [Parameter] protected RadioButton Selected { get; set; }
        [Parameter] protected Action<RadioButton> SelectedChanged { get; set; }

        #endregion Properties

        #region Methods

        protected void SelectButton(RadioButton button)
        {
            if (Selected == button && CanDeselect)
            {
                Selected = null;
            }
            else if (Selected != button)
            {
                Selected = button;
                SelectedChanged?.Invoke(button);
            }

            StateHasChanged();
        }

        #endregion Methods
    }
}
