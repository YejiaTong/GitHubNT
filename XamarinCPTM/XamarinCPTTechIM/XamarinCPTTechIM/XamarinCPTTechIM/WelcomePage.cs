using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinCPTTechIM
{
    public class WelcomePage : NonNavigationBarPage
    {
        public WelcomePage() : base()
        {
            Label welcomeLabel = new Label();
            welcomeLabel.FormattedText = new FormattedString();
            welcomeLabel.FormattedText.Spans.Add(new Span() { Text = "Welcome to ", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) });
            welcomeLabel.FormattedText.Spans.Add(new Span() { Text = "Invoice Manager", FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) });
            welcomeLabel.FormattedText.Spans.Add(new Span() { Text = " !", FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)) });
            welcomeLabel.FormattedText.Spans.Add(new Span() { Text = Environment.NewLine });
            welcomeLabel.FormattedText.Spans.Add(new Span() { Text = "Your day-to-day expenses management app.", FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)) });
            welcomeLabel.TextColor = Color.White;
            welcomeLabel.FontFamily = Device.OnPlatform(
                iOS: "MarkerFelt-Thin",
                Android: "Droid Sans Mono",
                WinPhone: "Comic Sans MS"
            );

            var overlay = new AbsoluteLayout();
            overlay.VerticalOptions = LayoutOptions.Center;
            var mainLayout = new StackLayout();
            AbsoluteLayout.SetLayoutFlags(mainLayout, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(mainLayout, new Rectangle(0f, 0f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            AbsoluteLayout.SetLayoutFlags(welcomeLabel, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(welcomeLabel, new Rectangle(0.5, 0.5, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            overlay.Children.Add(mainLayout);
            overlay.Children.Add(welcomeLabel);

            this.Content = overlay;
            this.BindingContext = new
            {
                Image = @"drawable/splash_logo.png"
            };
            this.BackgroundImage = @"drawable/background.png";

            var image = new Image()
            {
                Aspect = Aspect.Fill
            };
            image.SetBinding(Image.SourceProperty, "Image");

            mainLayout.Children.Add(new AspectRatioContainer
            {
                Content = image,
                AspectRatio = 1.5
            });
        }
    }
}
