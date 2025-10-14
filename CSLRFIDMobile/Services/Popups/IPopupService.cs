using System;
using System.Threading.Tasks;

namespace CSLRFIDMobile.Services.Popups
{
    /// <summary>
    /// Interface for popup and dialog services, compliant with MVVM pattern.
    /// Replaces Controls.UserDialogs.Maui dependency.
    /// </summary>
    public interface IPopupService
    {
        /// <summary>
        /// Shows a toast message (non-blocking notification)
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="title">Optional title</param>
        /// <param name="duration">Optional duration (defaults to 3 seconds if not specified)</param>
        Task ShowToastAsync(string message, string? title = null, TimeSpan? duration = null);

        /// <summary>
        /// Shows a loading indicator with a message
        /// </summary>
        /// <param name="message">Loading message to display</param>
        Task ShowLoadingAsync(string message = "Loading...");

        /// <summary>
        /// Hides the currently displayed loading indicator
        /// </summary>
        Task HideLoadingAsync();

        /// <summary>
        /// Shows an alert dialog with a single OK button
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="title">Optional title</param>
        /// <param name="okButton">Text for OK button (default: "OK")</param>
        Task AlertAsync(string message, string? title = null, string okButton = "OK");

        /// <summary>
        /// Shows a confirmation dialog with OK and Cancel buttons
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="title">Optional title</param>
        /// <param name="okButton">Text for OK button (default: "OK")</param>
        /// <param name="cancelButton">Text for Cancel button (default: "Cancel")</param>
        /// <returns>True if OK was selected, False if Cancel was selected</returns>
        Task<bool> ConfirmAsync(string message, string? title = null, string okButton = "OK", string cancelButton = "Cancel");
    }
}
