using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;


namespace GradientProgressBar
{
    public class GradientProgressBar: SKCanvasView
    {
        public static BindableProperty PercentageProperty = BindableProperty.Create(nameof(Percentage), typeof(float),
            typeof(GradientProgressBar), 0f, BindingMode.OneWay,
            validateValue: (_, value) => value != null,
            propertyChanged: OnPropertyChangedInvalidate);

        public float Percentage
        {
            get => (float)GetValue(PercentageProperty);
            set => SetValue(PercentageProperty, value);
        }

        public static BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(Percentage), typeof(float),
            typeof(GradientProgressBar), 5f, BindingMode.OneWay,
            validateValue: (_, value) => value != null && (float)value >= 0,
            propertyChanged: OnPropertyChangedInvalidate);

        public float CornerRadius
        {
            get => (float)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static BindableProperty BarBackgroundColorProperty = BindableProperty.Create(nameof(BarBackgroundColor), typeof(Color),
            typeof(GradientProgressBar), Colors.White, BindingMode.OneWay,
            validateValue: (_, value) => value != null, propertyChanged: OnPropertyChangedInvalidate);

        public Color BarBackgroundColor
        {
            get => (Color)GetValue(BarBackgroundColorProperty);
            set => SetValue(BarBackgroundColorProperty, value);
        }

        public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(float),
            typeof(GradientProgressBar), 12f, BindingMode.OneWay,
            validateValue: (_, value) => value != null && (float)value >= 0,
            propertyChanged: OnPropertyChangedInvalidate);

        public float FontSize
        {
            get => (float)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color),
            typeof(GradientProgressBar), Colors.Blue, BindingMode.OneWay,
            validateValue: (_, value) => value != null, propertyChanged: OnPropertyChangedInvalidate);
        
        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        private static void OnPropertyChangedInvalidate(BindableObject bindable, object oldvalue, object newvalue)
        {
            var control = (GradientProgressBar)bindable;

            if (oldvalue != newvalue)
                control.InvalidateSurface();
        }

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            var info = e.Info;
            var canvas = e.Surface.Canvas;

            float width = (float)Width;
            var scale = CanvasSize.Width / width;

            var percentage = Percentage;

            var cornerRadius = CornerRadius * scale;

            var textSize = FontSize * scale;

            var height = e.Info.Height;

            var str = percentage.ToString("0%");

            var percentageWidth = (int) Math.Floor(info.Width * percentage);
                
            canvas.Clear();

            var backgroundBar = new SKRoundRect(new SKRect(0, 0, info.Width, height), cornerRadius, cornerRadius);
            var progressBar = new SKRoundRect(new SKRect(0, 0, percentageWidth, height), cornerRadius, cornerRadius);
            var progressBar1 = new SKRoundRect(new SKRect(0, 0, percentageWidth, height), cornerRadius, cornerRadius);
            var progressBar2 = new SKRoundRect(new SKRect(0, 0, percentageWidth, height), cornerRadius, cornerRadius);

            var background = new SKPaint { Color = BarBackgroundColor.ToSKColor(), IsAntialias = true};

            canvas.DrawRoundRect(backgroundBar, background);

            var paint = new SKPaint() { IsAntialias = true };
            var paint1 = new SKPaint() { IsAntialias = true };
            var paint2 = new SKPaint() { IsAntialias = true };

            float xLowScale = (int)Math.Floor(info.Width * 0.3);
            float xHighScale = (int)Math.Floor(info.Width * 0.7);
            float x = percentageWidth;
            float y = info.Height;
            var rect = new SKRect(0, 0, x, y);
                
            if (percentage < 0.3)
            {
                paint.Color = Colors.Red.ToSKColor();
                canvas.DrawRoundRect(progressBar, paint);
            }
            else if (percentage >= 0.3 && percentage <= 0.7)
            {
                paint.Color = Colors.Gold.ToSKColor();
                paint1.Color = Colors.Red.ToSKColor();
                progressBar1 = new SKRoundRect(new SKRect(0, 0, xLowScale, height), cornerRadius, cornerRadius);
                canvas.DrawRoundRect(progressBar, paint);
                canvas.DrawRoundRect(progressBar1, paint1);
            }
            else
            {
                paint.Color = Colors.Green.ToSKColor();
                paint1.Color = Colors.Gold.ToSKColor();
                progressBar1 = new SKRoundRect(new SKRect(0, 0, xHighScale, height), cornerRadius, cornerRadius);
                paint2.Color = Colors.Red.ToSKColor();
                progressBar2 = new SKRoundRect(new SKRect(0, 0, xLowScale, height), cornerRadius, cornerRadius);
                canvas.DrawRoundRect(progressBar, paint);
                canvas.DrawRoundRect(progressBar1, paint1);
                canvas.DrawRoundRect(progressBar2, paint2);
            }
               
            var textPaint = new SKPaint { Color = TextColor.ToSKColor(), TextSize = textSize};
            var textBounds = new SKRect();

            textPaint.MeasureText(str, ref textBounds);

            var xText = info.Width / 2 - textBounds.MidX;
            var yText = info.Height / 2 - textBounds.MidY;

            canvas.DrawText(str, xText, yText, textPaint);
        }
    }
}
