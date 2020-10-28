using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace entitymodel
{
    public delegate void entListChangedEventHandler(object sender, entListChangedEventArgs e);
    public class entListChangedEventArgs: ListChangedEventArgs
    {
        private entity _changedEntity=null;

        

        public entListChangedEventArgs(ListChangedType listChangedType, int newIndex) : base(listChangedType, newIndex) { }
        public entListChangedEventArgs(ListChangedType listChangedType, int newIndex, PropertyDescriptor propDesc) : base(listChangedType, newIndex, propDesc) { }
        public entListChangedEventArgs(ListChangedType listChangedType, PropertyDescriptor propDesc) : base(listChangedType, propDesc) { }
        public entListChangedEventArgs(ListChangedType listChangedType, int newIndex, int oldIndex) : base(listChangedType, newIndex, oldIndex) { }
        public entListChangedEventArgs(ListChangedType listChangedType, int newIndex, entity changedEntity) :
            base(listChangedType, newIndex)
        {
            ChangedEntity = changedEntity;
        }

        public entity ChangedEntity { get => _changedEntity; set => _changedEntity = value; }

    }
}
