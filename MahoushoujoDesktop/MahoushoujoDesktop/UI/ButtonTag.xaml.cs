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
using System . Windows . Media . Imaging;
using System . Windows . Navigation;
using System . Windows . Shapes;

namespace MahoushoujoDesktop . UI
{
    /// <summary>
    /// ButtonTag.xaml 的交互逻辑
    /// </summary>
    public partial class ButtonTag : UserControl
    {
        public event RoutedEventHandler TagClick;



        public string Text
        {
            get { return (string) GetValue ( TextProperty ); }
            set { SetValue ( TextProperty , value ); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty . Register ( "Text" , typeof ( string ) , typeof ( ButtonTag ) ,
                new PropertyMetadata (
                    "" ,
                    ( d , e ) =>
                      {
                          ( d as ButtonTag ) . textTag . Text = (string) e . NewValue;
                      } )
                );
        public new Brush Background
        {
            get { return (Brush) GetValue ( BackgroundProperty ); }
            set { SetValue ( BackgroundProperty , value ); }
        }
        public static readonly new DependencyProperty BackgroundProperty =
            DependencyProperty . Register ( "Background" , typeof ( Brush ) , typeof ( ButtonTag ) ,
                new PropertyMetadata (
                    null ,
                    ( d , e ) =>
                    {
                        ( d as ButtonTag ) . rectMain . Fill = (Brush) e . NewValue;
                    } )
                );



        public ButtonTag ()
        {
            InitializeComponent ();
        }

        private void buttonTag_Click ( object sender , RoutedEventArgs e )
        {
            
        }

        private void buttonDelete_Click ( object sender , RoutedEventArgs e )
        {

        }
    }
}
