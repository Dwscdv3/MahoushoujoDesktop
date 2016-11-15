using System;
using System . Collections . Generic;
using System . Linq;
using System . Text;
using System . Threading . Tasks;
using System . Windows;
using System . Windows . Controls;
using System . Windows . Data;
using System . Windows . Documents;
using System . Windows . Input;
using System . Windows . Media;
using System . Windows . Media . Animation;
using System . Windows . Media . Imaging;
using System . Windows . Navigation;
using System . Windows . Shapes;

namespace MahoushoujoDesktop . UI
{
    /// <summary>
    /// WaitRing.xaml 的交互逻辑
    /// </summary>
    public partial class WaitRing : UserControl
    {
        public int FrameRate
        {
            get { return (int) GetValue ( FrameRateProperty ); }
            set { SetValue ( FrameRateProperty , value ); }
        }
        public static readonly DependencyProperty FrameRateProperty =
            DependencyProperty . Register ( "FrameRate" , typeof ( int ) , typeof ( WaitRing ) ,
                new PropertyMetadata ( 60 ) );

        public WaitRing ()
        {
            InitializeComponent ();
        }
    }
}
