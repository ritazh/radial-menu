﻿namespace RadialMenuControl.UserControl
{
    using RadialMenuControl.UserControl;
    using System;
    using System.Diagnostics;
    using Windows.UI;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Media;

    public sealed partial class PieSlice : UserControl
    {
        // Inner Arc Colors
        public static readonly DependencyProperty InnerNormalColorProperty =
            DependencyProperty.Register("InnerNormalColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty InnerHoverColorProperty =
            DependencyProperty.Register("InnerHoverColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty InnerTappedColorProperty =
            DependencyProperty.Register("InnerTappedColor", typeof(Color), typeof(PieSlice), null);

        public Color InnerHoverColor
        {
            get { return (Color)GetValue(InnerHoverColorProperty); }
            set { SetValue(InnerHoverColorProperty, value); }
        }

        public Color InnerNormalColor
        {
            get { return (Color)GetValue(InnerNormalColorProperty); }
            set { SetValue(InnerNormalColorProperty, value); }
        }

        public Color InnerTappedColor
        {
            get { return (Color)GetValue(InnerTappedColorProperty); }
            set { SetValue(InnerTappedColorProperty, value); }
        }

        // Outer Arc Colors
        public static readonly DependencyProperty OuterNormalColorProperty =
            DependencyProperty.Register("OuterNormalColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty OuterHoverColorProperty =
            DependencyProperty.Register("OuterHoverColor", typeof(Color), typeof(PieSlice), null);

        public static readonly DependencyProperty OuterTappedColorProperty =
            DependencyProperty.Register("OuterTappedColor", typeof(Color), typeof(PieSlice), null);

        public Color OuterHoverColor
        {
            get { return (Color)GetValue(OuterHoverColorProperty); }
            set { SetValue(OuterHoverColorProperty, value); }
        }

        public Color OuterNormalColor
        {
            get { return (Color)GetValue(OuterNormalColorProperty); }
            set { SetValue(OuterNormalColorProperty, value); }
        }

        public Color OuterTappedColor
        {
            get { return (Color)GetValue(OuterTappedColorProperty); }
            set { SetValue(OuterTappedColorProperty, value); }
        }

        // Angles & Radius
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(PieSlice), null);

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(PieSlice), null);

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(PieSlice), null);

        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value); }
        }

        // Label
        public static readonly DependencyProperty HideLabelProperty =
            DependencyProperty.Register("HideLabel", typeof(bool), typeof(PieSlice), null);

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(PieSlice), null);

        public static readonly DependencyProperty LabelSizeProperty =
            DependencyProperty.Register("LabelSize", typeof(int), typeof(PieSlice), null);

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(string), typeof(PieSlice), null);

        public static readonly DependencyProperty IconSizeProperty =
            DependencyProperty.Register("IconSize", typeof(int), typeof(PieSlice), null);

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public int LabelSize
        {
            get { return (int)GetValue(LabelSizeProperty); }
            set { SetValue(LabelSizeProperty, value); }
        }

        public bool HideLabel
        {
            get { return (bool)GetValue(HideLabelProperty); }
            set { SetValue(HideLabelProperty, value); }
        }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public int IconSize
        {
            get { return (int)GetValue(IconSizeProperty); }
            set { SetValue(IconSizeProperty, value); }
        }

        // Pass in the original RadialMenuButton
        public Components.RadialMenuButton _radialMenuButton;
        public static readonly DependencyProperty _radialMenuButtonProperty =
            DependencyProperty.Register("_radialMenuButton", typeof(Components.RadialMenuButton), typeof(PieSlice), null);

        // Change Menu
        public delegate void ChangeMenuRequestHandler(object sender, RadialMenu submenu);
        public event ChangeMenuRequestHandler ChangeMenuRequestEvent;

        private void OnChangeMenuRequest(object s, RadialMenu sm)
        {
            if (ChangeMenuRequestEvent != null)
            {
                ChangeMenuRequestEvent(s, sm);
            }
        }

        public PieSlice()
        {
            this.InitializeComponent();
            this.DataContext = this;

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // TODO: Check if we have a submenu, "grey out" if we don't
            outerPieSlicePath.Radius = this.Radius;
            outerPieSlicePath.StartAngle = this.StartAngle;
            outerPieSlicePath.Angle = this.Angle;
            outerPieSlicePath.Fill = new SolidColorBrush((Color)this.OuterNormalColor);

            innerPieSlicePath.Radius = this.Radius - 20;
            innerPieSlicePath.StartAngle = this.StartAngle;
            innerPieSlicePath.Angle = this.Angle;
            innerPieSlicePath.Fill = new SolidColorBrush((Color)this.InnerNormalColor);

            // Calculating a point in the "direction" of our button
            // TODO: This calculation is, erm, "weird". It probably doesn't work for different menu sizes. It should be improved.
            double middleRadian = (Math.PI / 180) * (this.StartAngle + (this.Angle / 2) - 15);
            iconTranslate.X = 85 * Math.Cos(middleRadian);
            iconTranslate.Y = -85 * Math.Sin(middleRadian);
        }

        private void outerPieSlicePath_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "OuterPressed", true);

            // Check for Submenu
            if (_radialMenuButton.Submenu != null)
            {
                OnChangeMenuRequest(_radialMenuButton, _radialMenuButton.Submenu);
            }

            _radialMenuButton.OnOuterArcPressed(e);
        }

        private void outerPieSlicePath_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // TODO: Check if we're actually still hovering
            VisualStateManager.GoToState(this, "OuterHover", true);
            _radialMenuButton.OnOuterArcReleased(e);
        }

        private void outerPieSlicePath_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            // TODO: Check if we have a submenu, otherwise don't highlight
            VisualStateManager.GoToState(this, "OuterHover", true);
        }

        private void outerPieSlicePath_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "OuterNormal", true);
        }

        private void innerPieSlicePath_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "InnerHover", true);
        }

        private void innerPieSlicePath_PointerExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "InnerNormal", true);
        }

        private void innerPieSlicePath_PointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "InnerPressed", true);
            _radialMenuButton.OnInnerArcPressed(e);
        }

        private void innerPieSlicePath_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "InnerHover", true);
            _radialMenuButton.OnOuterArcReleased(e);
        }
    }
}
