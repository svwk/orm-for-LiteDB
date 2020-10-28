using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace entitymodel
{
    public interface IEntityBindinglist
    {
        #region Fields


        #endregion

        #region Constructors


        #endregion

        #region Indexators/Enumerators
        entity this[int  index] { get; set; }
        entity this[long Id] { get; }

        #endregion

        #region Comparer



        #endregion

        #region Events

        event AddingNewEventHandler   AddingNew;
        event ListChangedEventHandler ListChanged;

        #endregion

        #region Propeties
        bool AllowEdit { get; set; }
        bool AllowNew { get; set; }
        bool AllowRemove { get; set; }
        bool CheckModified { get; }
        int Count { get; }
        IEntityBindinglist DefaultView { get; }
        List<entity> FullList { get; }
        List<long> FullListId { get; }
        bool IsVeiw { get;  }
        bool Modified { get; }
        string Name { get; set; }
        bool RaiseListChangedEvents { get; set; }
        bool SizeChanged { get; }
        Type SystemType { get; }

        EntityType Type { get; }

        #endregion

        #region Methods
        int Add(entity item);
        void Add(IEnumerable<entity> newItems, bool withRenew );
        entity AddNew();
        bool CheckForDouble(List<long> list, bool removeDoubles = true);
        bool CheckForLosen( List<long> list, bool removeLosen = true);
        void CancelNew(int itemIndex);
        bool CheckItem(entity en);
        bool CheckItem(long entityId);
        
        bool CheckNewId( long id);
        void Clear(bool generateEvent);
        void Clear();
        bool Contains(entity item);
        void EndNew(int itemIndex);
        long FindDouble(entity newEntity);
        entity FindEntityById(long Id);
        int FindIndexById(long Id);
        void Fix();
        IEnumerator GetEnumerator();
        int IndexOf(entity item);
        int Insert(int index, entity item);
        bool Remove(entity item);
        void Remove(IEnumerable<entity> itemsToDelete);
        void Remove(IEnumerable<long> idItemsToDelete);
        void RemoveAt(int index);
        bool RemoveById(long id);
        void Reset();
        void ResetBindings();
        void ResetItem(int position);


        #endregion

        



    }
}