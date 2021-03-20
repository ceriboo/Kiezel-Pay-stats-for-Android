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

namespace kiezelpaywidget
{
    public class Datakiezel
    {
        public string date { get; set; }
        public int purchases { get; set; }
        public double amount { get; set; }
        public int rank { get; set; }
    }

    public class mydata
    {
        public Datakiezel datakiezel { get; set; }
    }

}