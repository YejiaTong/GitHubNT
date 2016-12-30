using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Support.V4.View;
using Java.Lang;

namespace AndroidTM
{
    internal class SamplePagerAdaper : PagerAdapter
    {
        List<string> items = new List<string>();

        public SamplePagerAdaper() : base()
        {
            items.Add("Home");
            items.Add("Add Expenses");
            items.Add("About");
            items.Add("Contact");
            items.Add("Leave Your Message");
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override bool IsViewFromObject(View view, Java.Lang.Object obj)
        {
            return view == obj;
        }

        public override Java.Lang.Object InstantiateItem(ViewGroup container, int position)
        {
            View view = LayoutInflater.From(container.Context).Inflate(Resource.Layout.PagerItems, container, false);
            container.AddView(view);

            TextView textTitle = view.FindViewById<TextView>(Resource.Id.item_title);
            int pos = position + 1;
            textTitle.Text = pos.ToString();

            return view;
        }

        public override void DestroyItem(ViewGroup container, int position, Java.Lang.Object obj)
        {
            container.RemoveView((View)obj);
        }

        public string GetHeaderTitle(int position)
        {
            return items[position];
        }
    }
}