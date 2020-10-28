using System;
using System.Collections.Generic;
using System.ComponentModel;
using entitymodel.Annotations;

namespace entitymodel
{
    public class bindviewdefault<T> : bindview<T> where T : entity, new()
    {
        public bindviewdefault()
        {
            _isVeiw = true;
            _source = SourceType.Default;
            _isCascading = true;
        }

        public bindviewdefault([NotNull] bindbaselist<T> baseList) :base()
        {
            _isVeiw                      =  true;
            _source = SourceType.Default;
            _mainBindinglist             =  baseList;
            _mainBindinglist.ListChanged += _mainBindinglist_ListChanged;
            _isCascading = true;
            RefreshList();
        }


        #region Events

        protected override void _mainBindinglist_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (!_mainBindinglistChange) return;
            if (_mainBindinglist == null || sender == null || !Object.ReferenceEquals(sender, _mainBindinglist)) return;
            //bindlist<T> tmpList2 = sender as bindlist<T>;
            //entListChangedEventArgs t        = e as entListChangedEventArgs;
            T o                                    = null;
            if (e is entListChangedEventArgs en) o = en.ChangedEntity as T;
            //if (t != null) { o = t.ChangedEntity as T; }

            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:     RefreshList(); break;
                case ListChangedType.ItemAdded:
                    if (o == null)
                    {
                        if (e.NewIndex >= 0 && e.NewIndex<_mainBindinglist.Count) o = _mainBindinglist[e.NewIndex];
                    }
                    if (o != null) AddLink(o);
                    break;
                case ListChangedType.ItemDeleted:
                    if (o != null && Contains(o) && (o.Changed == Status.deleted || o.Changed == Status.newdeleted)) RemoveLink(o.Id);
                    break;
                case ListChangedType.ItemMoved:                 break;
                case ListChangedType.ItemChanged:               break;
                case ListChangedType.PropertyDescriptorAdded:   break;
                case ListChangedType.PropertyDescriptorDeleted: break;
                case ListChangedType.PropertyDescriptorChanged: break;
            }
        }


        #endregion

        #region Methods

        #region Resetting
        public override void RefreshList()
        {
            int c                      = Count;
            if (_items == null) _items = new List<T>();

            if (_mainBindinglist == null || _mainBindinglist.Count == 0)
            {
                if (_items.Count > 0) ClearItems();
                if (c != Count)
                {
                    _m = true;
                    FireListChanged(ListChangedType.Reset, -1);
                }
                return;
            }

            List<T> list = _mainBindinglist?.FullListT;

            if (list == null || list.Count == 0)
            {
                if (_items.Count > 0) ClearItems();
                if (c != Count)
                {
                    _m = true;
                    FireListChanged(ListChangedType.Reset, -1);
                }
                return;
            }

            bool b;
            c = list.Count;
            for (int j = 0; j < _items.Count; j++)
            {
                if (_items[j].Changed == Status.deleted || _items[j].Changed == Status.newdeleted)
                {
                    RemoveLink(j); j -= 1;
                    continue;
                }
                b = true;
                foreach (var v in list)
                {
                    if (v.Id == _items[j].Id)
                    {
                        b = false;
                        list.Remove(v);
                        break;
                    }
                }

                if (b)
                {
                    RemoveLink(j);
                    j -= 1;
                }

                if (list.Count == 0 && _items.Count == 0) return;
            }
            if (list.Count > 0 || Count != c) AddLink(list);
        }
        #endregion




        #endregion
    }
}