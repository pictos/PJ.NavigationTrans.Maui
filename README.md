# PJ.NavigationTrans.Maui 🏳️‍⚧️

This is a library that allows you to use custom animations during Flyout navigations and Push navigations using Shell or NavigationPage.
Be aware that transition respects the behavior of the native platform, so they may look different between OSes.



* Available on NuGet: [![NuGet](https://img.shields.io/nuget/v/PJSouzaSoftware.NavigationTrans.Maui.svg?label=NuGet)](https://www.nuget.org/packages/PJSouzaSoftware.NavigationTrans.Maui/)

## Basic Usage

You can add transitions to your Shell content using XAML:

```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:PJ.NavigationTrans.Maui;assembly=PJ.NavigationTrans.Maui"
             x:Class="YourNamespace.YourPage"
             local:NavigationTrans.TransitionIn="FadeIn"
             local:NavigationTrans.TransitionOut="FadeOut"
             local:NavigationTrans.Duration="800">
    <!-- Your page content -->
</ContentPage>
```

Adding Transitions in C#

```cs
public partial class YourPage : ContentPage
{
    public YourPage()
    {
        InitializeComponent();
        
        // Set transition properties
        NavigationTrans.SetTransitionIn(this, TransitionType.RightIn);
        NavigationTrans.SetTransitionOut(this, TransitionType.LeftOut);
        NavigationTrans.SetDuration(this, 500);
    }
}
```

Available Built-in Transitions
•	Default: System default transition
•	FadeIn/FadeOut: Fade animation
•	LeftIn/LeftOut: Slide from/to left
•	RightIn/RightOut: Slide from/to right
•	TopIn/TopOut: Slide from/to top
•	BottomIn/BottomOut: Slide from/to bottom
•	Custom: User-defined custom animation

## Custom Animation

Custom Android Animations
To create custom Android animations, you need to define animation resources and reference them:

```cs
public partial class YourPage : ContentPage
{
    public YourPage()
    {
        InitializeComponent();
        
        // Use custom Android animations by resource ID
        NavigationTrans.SetAndroidTransitions(
            this,
            Resource.Animation.your_enter_animation, // Resource ID for enter animation
            Resource.Animation.your_exit_animation,  // Resource ID for exit animation
            300 // Duration in milliseconds
        );
    }
}
```

Custom iOS Animations
For iOS, you can define custom UIView animations:

```cs
public partial class YourPage : ContentPage
{
    public YourPage()
    {
        InitializeComponent();
        
#if IOS
        // Define custom iOS animations
        NavigationTrans.SetIosTransitions(
            this,
            // Animation when entering
            animationIn: view => {
                view.Alpha = 1;
                view.Transform = CGAffineTransform.MakeIdentity();
            },
            // Initial configuration when entering
            configurationOn: view => {
                view.Alpha = 0;
                view.Transform = CGAffineTransform.MakeScale(0.8f, 0.8f);
            },
            // Animation when exiting
            animationOut: view => {
                view.Alpha = 0;
                view.Transform = CGAffineTransform.MakeScale(1.2f, 1.2f);
            },
            // Initial configuration when exiting
            configurationOut: null,
            duration: 400 // Duration in milliseconds
        );
#endif
    }
}
```


## Support

This project is open source and maintained by one person, so if you need a urgent fix for a bug and don't want to submit it
you can pay for my work using [github sponsor](https://github.com/sponsors/pictos/sponsorships?sponsor=pictos&tier_id=485056&preview=false).

## Demo

https://github.com/user-attachments/assets/397324c4-c064-4e49-8fc8-574a42b9f3da

