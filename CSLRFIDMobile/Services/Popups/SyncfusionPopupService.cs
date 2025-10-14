using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Toolkit.Popup;

namespace CSLRFIDMobile.Services.Popups
{
    /// <summary>
    /// Implementation of IPopupService using Syncfusion .NET MAUI Popup
    /// </summary>
    public class SyncfusionPopupService : IPopupService
    {
        private SfPopup? _currentLoadingPopup;
        private readonly object _loadingLock = new object();

        public async Task ShowToastAsync(string message, string? title = null, TimeSpan? duration = null)
        {
            var toastDuration = duration ?? TimeSpan.FromSeconds(2);

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                var popup = new SfPopup
                {
                    ShowHeader = false,
                    ShowFooter = false,
                    ShowCloseButton = false,
                    AutoSizeMode = PopupAutoSizeMode.Height,
                    AnimationMode = PopupAnimationMode.SlideOnBottom,
                    StaysOpen = false,
                    WidthRequest = 300,
                    IsOpen = false,
                    PopupStyle = new PopupStyle
                    {
                        PopupBackground = new SolidColorBrush(Color.FromArgb("#323232")),
                        HasShadow = true
                    },
                };

                // Create toast content
                var stack = new VerticalStackLayout
                {
                    Spacing = 4
                };
                if (!string.IsNullOrEmpty(title))
                {
                    stack.Children.Add(new Label
                    {
                        Text = title,
                        TextColor = Colors.White,
                        FontSize = 14,
                        FontAttributes = FontAttributes.Bold,
                        HorizontalOptions = LayoutOptions.Center
                    });
                }
                stack.Children.Add(new Label
                {
                    Text = message,
                    TextColor = Colors.White,
                    FontSize = 13,
                    HorizontalOptions = LayoutOptions.Center,
                    HorizontalTextAlignment = TextAlignment.Center
                });

                var contentView = new ContentView
                {
                    Background = new SolidColorBrush(Color.FromArgb("#323232")),
                    Padding = new Thickness(16, 12),
                    Shadow = new Shadow
                    {
                        Brush = Colors.Black,
                        Opacity = 0.3f,
                        Offset = new Point(0, 4),
                        Radius = 8
                    },
                    Content = stack
                };

                popup.ContentTemplate = new DataTemplate(() => contentView);

                popup.Show();
                await Task.Delay(toastDuration);
                popup.Dismiss();
            });
        }

        public async Task ShowLoadingAsync(string message = "Loading...")
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                lock (_loadingLock)
                {
                    // Dismiss any existing loading popup
                    _currentLoadingPopup?.Dismiss();

                    _currentLoadingPopup = new SfPopup
                    {
                        ShowHeader = false,
                        ShowFooter = false,
                        ShowCloseButton = false,
                        AnimationMode = PopupAnimationMode.Fade,
                        IsOpen = false,
                        WidthRequest = 200,
                        HeightRequest = 150,
                        PopupStyle = new PopupStyle
                        {
                            PopupBackground = new SolidColorBrush(Color.FromArgb("#FFFFFF")),
                            HasShadow = true
                        },
                    };

                    // Create loading content
                    var contentView = new ContentView
                    {
                        Background = new SolidColorBrush(Colors.White),
                        Padding = new Thickness(20),
                        Shadow = new Shadow
                        {
                            Brush = Colors.Black,
                            Opacity = 0.25f,
                            Offset = new Point(0, 4),
                            Radius = 10
                        },
                        Content = new VerticalStackLayout
                        {
                            Spacing = 15,
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            Children =
                            {
                                new ActivityIndicator
                                {
                                    IsRunning = true,
                                    Color = (Color)Application.Current!.Resources["Primary"],
                                    WidthRequest = 40,
                                    HeightRequest = 40,
                                    HorizontalOptions = LayoutOptions.Center
                                },
                                new Label
                                {
                                    Text = message,
                                    TextColor = Colors.Black,
                                    FontSize = 14,
                                    HorizontalOptions = LayoutOptions.Center,
                                    HorizontalTextAlignment = TextAlignment.Center
                                }
                            }
                        }
                    };

                    _currentLoadingPopup.ContentTemplate = new DataTemplate(() => contentView);
                    _currentLoadingPopup.Show();
                }
            });
        }

        public async Task HideLoadingAsync()
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                lock (_loadingLock)
                {
                    _currentLoadingPopup?.Dismiss();
                    _currentLoadingPopup = null;
                }
            });
        }

        public async Task AlertAsync(string message, string? title = null, string okButton = "OK")
        {
            var tcs = new TaskCompletionSource<bool>();

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                var popup = new SfPopup
                {
                    HeaderTitle = title ?? string.Empty,
                    Message = message,
                    ShowHeader = !string.IsNullOrEmpty(title),
                    ShowFooter = true,
                    ShowCloseButton = false,
                    AppearanceMode = PopupButtonAppearanceMode.OneButton,
                    AcceptButtonText = okButton,
                    AnimationMode = PopupAnimationMode.Zoom,
                    IsOpen = false,
                    PopupStyle = new PopupStyle
                    {
                        PopupBackground = new SolidColorBrush(Colors.White),
                        // Header/footer colors
                        HeaderBackground = Colors.White,
                        HeaderTextColor = (Color)Application.Current!.Resources["Primary"],
                        FooterBackground = Colors.White,
                        // Button styling
                        AcceptButtonBackground = (Color)Application.Current!.Resources["Primary"],
                        AcceptButtonTextColor = Colors.White,
                        DeclineButtonBackground = (Color)Application.Current!.Resources["Primary"],
                        DeclineButtonTextColor = Colors.White,
                        HasShadow = true
                    },
                };

                popup.ContentTemplate = new DataTemplate(() =>
                {
                    return new Grid
                    {
                        Background = Colors.White, // let PopupStyle color be visible
                        Padding = new Thickness(16, 12),
                        Children =
                        {
                            new Label { Text = message, TextColor = Colors.Black }
                        }
                    };
                });

                popup.AcceptCommand = new Command(() =>
                {
                    popup.Dismiss();
                    tcs.TrySetResult(true);
                });

                popup.Closed += (s, e) => tcs.TrySetResult(true);

                popup.Show();
            });

            await tcs.Task;
        }

        public async Task<bool> ConfirmAsync(string message, string? title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            var tcs = new TaskCompletionSource<bool>();

            await MainThread.InvokeOnMainThreadAsync(() =>
            {
                var popup = new SfPopup
                {
                    HeaderTitle = title ?? string.Empty,
                    Message = message,
                    ShowHeader = !string.IsNullOrEmpty(title),
                    ShowFooter = true,
                    ShowCloseButton = false,
                    AppearanceMode = PopupButtonAppearanceMode.TwoButton,
                    AcceptButtonText = okButton,
                    DeclineButtonText = cancelButton,
                    AnimationMode = PopupAnimationMode.Zoom,
                    IsOpen = false,
                    PopupStyle = new PopupStyle
                    {
                        PopupBackground = new SolidColorBrush(Colors.White),
                        // Header/footer colors
                        HeaderBackground = Colors.White,
                        HeaderTextColor = (Color)Application.Current!.Resources["Primary"],
                        FooterBackground = Colors.White,
                        // Button styling
                        AcceptButtonBackground = (Color)Application.Current!.Resources["Primary"],
                        AcceptButtonTextColor = Colors.White,
                        DeclineButtonBackground = (Color)Application.Current!.Resources["Primary"],
                        DeclineButtonTextColor = Colors.White,
                        HasShadow = true
                    }
                };

                popup.ContentTemplate = new DataTemplate(() =>
                {
                    return new Grid
                    {
                        Background = Colors.White, // let PopupStyle color be visible
                        Padding = new Thickness(16, 12),
                        Children =
                        {
                            new Label { Text = message, TextColor = Colors.Black }
                        }
                    };
                });

                popup.AcceptCommand = new Command(() =>
                {
                    popup.Dismiss();
                    tcs.TrySetResult(true);
                });

                popup.DeclineCommand = new Command(() =>
                {
                    popup.Dismiss();
                    tcs.TrySetResult(false);
                });

                popup.Closed += (s, e) => tcs.TrySetResult(false);

                popup.Show();
            });

            return await tcs.Task;
        }
    }
}
