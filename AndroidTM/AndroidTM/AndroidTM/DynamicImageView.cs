using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

namespace AndroidTM
{
    public class DynamicImageView : ImageView
    {
        public DynamicImageView(Context context, IAttributeSet attributeSet) : base(context, attributeSet) { }

        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            int width = MeasureSpec.GetSize(widthMeasureSpec);
            int height = width * Drawable.IntrinsicHeight / Drawable.IntrinsicWidth;
            SetMeasuredDimension(width, height);
        }
    }
}