using System.Collections.Generic;

namespace entitymodel
{
    public interface IBindbaselist: IEntityBindinglist 
    {
        List<entity> ChangedList { get; }
        List<entity> RemovedItems { get; }
        string AddView(string name);
        string AddView(string name, List<long> idList);
        void RemoveView(string name);
    }
}