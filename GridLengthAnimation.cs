using System;
using System.Windows;
using System.Windows.Media.Animation;

namespace CyberSecurity_Part2._2
{
    // GridLengthAnimation enables smooth animation of ColumnDefinition.Width.
    // WPF does not include a built-in GridLength animation so this small helper
    // class fills that gap. It is used by BtnToggleTopics_Click to slide the
    // sidebar open and closed smoothly.
    public class GridLengthAnimation : AnimationTimeline
    {
        // From: the starting GridLength value of the animation.
        public static readonly DependencyProperty FromProperty =
            DependencyProperty.Register("From", typeof(GridLength), typeof(GridLengthAnimation));

        // To: the ending GridLength value of the animation.
        public static readonly DependencyProperty ToProperty =
            DependencyProperty.Register("To", typeof(GridLength), typeof(GridLengthAnimation));

        public GridLength From
        {
            get => (GridLength)GetValue(FromProperty);
            set => SetValue(FromProperty, value);
        }

        public GridLength To
        {
            get => (GridLength)GetValue(ToProperty);
            set => SetValue(ToProperty, value);
        }

        // Optional easing function to make the animation feel smooth.
        public IEasingFunction EasingFunction { get; set; }

        // TargetPropertyType tells WPF what property type this animation drives.
        public override Type TargetPropertyType => typeof(GridLength);

        // CreateInstanceCore is required by the Freezable base class.
        protected override Freezable CreateInstanceCore() => new GridLengthAnimation();

        // GetCurrentValue computes the interpolated GridLength at each animation frame.
        public override object GetCurrentValue(object defaultOriginValue,
            object defaultDestinationValue, AnimationClock animationClock)
        {
            double progress = animationClock.CurrentProgress ?? 0;
            // Apply easing if one is set so the slide feels natural.
            if (EasingFunction != null)
                progress = EasingFunction.Ease(progress);

            double from = ((GridLength)GetValue(FromProperty)).Value;
            double to = ((GridLength)GetValue(ToProperty)).Value;
            // Linear interpolation between From and To values.
            return new GridLength(from + (to - from) * progress);
        }
    }
}
