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
    /// Tag.xaml 的交互逻辑
    /// </summary>
    public partial class Tag : UserControl
    {
        public event RoutedEventHandler TagClick;

        public string Text
        {
            get { return (string) GetValue ( TextProperty ); }
            set { SetValue ( TextProperty , value ); }
        }
        public static readonly DependencyProperty TextProperty =
            DependencyProperty . Register ( "Text" , typeof ( string ) , typeof ( Tag ) ,
                new PropertyMetadata (
                    "" ,
                    ( d , e ) =>
                    {
                        ( d as Tag ) . textTag . Text = (string) e . NewValue;
                    } )
                );
        public new Brush Background
        {
            get { return (Brush) GetValue ( BackgroundProperty ); }
            set { SetValue ( BackgroundProperty , value ); }
        }
        public static readonly new DependencyProperty BackgroundProperty =
            DependencyProperty . Register ( "Background" , typeof ( Brush ) , typeof ( Tag ) ,
                new PropertyMetadata (
                    null ,
                    ( d , e ) =>
                    {
                        ( d as Tag ) . rectMain . Fill = (Brush) e . NewValue;
                    } )
                );

        public Tag ()
        {
            InitializeComponent ();
        }

        private void buttonTag_Click ( object sender , RoutedEventArgs e )
        {
            TagClick?.Invoke ( this , new RoutedEventArgs () );
        }
    }
}
