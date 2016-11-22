using System;
using System . Collections . Generic;
using System . ComponentModel;
using System . Linq;
using System . Text;
using System . Threading . Tasks;

namespace MahoushoujoDesktop . DataModel
{
    public class AlbumInfo : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( "Name" ) );
            }
        }

        private int _count;
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( "Count" ) );
            }
        }

        private int _id;
        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                PropertyChanged?.Invoke ( this , new PropertyChangedEventArgs ( "Id" ) );
            }
        }

        public AlbumInfo Self
        {
            get { return this; }
        }
    }
}
