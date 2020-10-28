using System;
using System.Collections.Generic;
using System.ComponentModel;
using entitymodel.Annotations;

namespace entitymodel
{
    public class bindviewlist<T> : bindview<T>, IBindviewList where T : entity, new()
    {
        #region Fields
            private List<long> _sourceList = null;

        #endregion

        #region Constructors
        public bindviewlist()
        {
            _isVeiw = true;
            _source = SourceType.List;
            _sourceList=new List<long>();
        }


        public bindviewlist([NotNull] bindbaselist<T> baseList) :base()
        {
            _isVeiw                      =  true;
            _source = SourceType.List;
            _mainBindinglist             =  baseList;
            _mainBindinglist.ListChanged += _mainBindinglist_ListChanged;
            _sourceList = new List<long>();
        }

        public bindviewlist([NotNull] bindbaselist<T> baseList, [NotNull] List<T> list) : base()
        {
            _isVeiw                      =  true;
            _source = SourceType.List;
            _mainBindinglist             =  baseList;
            _mainBindinglist.ListChanged += _mainBindinglist_ListChanged;
            _sourceList = new List<long>();
            AddLink(list,false);
        }

        public bindviewlist([NotNull] bindbaselist<T> baseList, [NotNull] List<long> sourceList)
        {
            _isVeiw                      =  true;
            _source                      =  SourceType.List;
            _mainBindinglist             =  baseList;
            _mainBindinglist.ListChanged += _mainBindinglist_ListChanged;
            SourceList                  =  sourceList;

        }


        #endregion

        #region Indexators/Enumerators

        public override T this[int index] => ItemByIndex(index);

        public override T this[long Id] => FindEntityById(Id);

        #endregion

        #region Comparer



        #endregion

        #region Events


        #endregion

        #region Propeties
        public List<long> SourceList
        {
            get => _sourceList;
            set
            {
                if (value==null || _sourceList==value) return;
                GenerateSourceList(value);
                RefreshList();
            }
        }

        #endregion

        #region Methods

        #region Checks проверки ссылок

        public override bool CheckItem([NotNull] T en)
        {
            return _mainBindinglist.CheckItem(en) && _sourceList.Contains(en.Id);
        }

        public override bool CheckItem([NotNull] entity en)
        {
            return _mainBindinglist.CheckItem(en) && _sourceList.Contains(en.Id);
        }


        public override bool CheckNewId([NotNull] long id)
        {
           return !_mainBindinglist.CheckNewId(id) && !_sourceList.Contains(id) ;
        }



        #endregion

        #region Resetting


        public override void ClearView(bool generateEvent)
        {
            _sourceList?.Clear();
            base.ClearView(generateEvent);
        }

        public override void RefreshList()
        {
            int c = Count;
            if (_items == null) _items = new List<T>();
            
            if (_mainBindinglist == null || _mainBindinglist.Count == 0|| _sourceList==null|| _sourceList.Count==0)
            {
                if (_items.Count > 0) ClearItems();
                if (c != Count)
                {
                    _m = true;
                    FireListChanged(ListChangedType.Reset, -1);
                }
                return;
            }
           
            List<T> list = _mainBindinglist?.CreateList(_sourceList);
            if (_sourceList == null || _sourceList.Count == 0|| list == null || list.Count == 0)
            {
                if (_items.Count > 0) ClearItems();
                if (c != Count)
                {
                    _m = true;
                    FireListChanged(ListChangedType.Reset, -1);
                }
                return;
            }

            
            c = list.Count;
            //List<T> list2 = list;
            for (int j = 0; j < _items.Count; j++)
            {
                if (_items[j].Changed == Status.deleted || _items[j].Changed == Status.newdeleted || !_sourceList.Contains(_items[j].Id))
                {
                    RemoveLink(j); j -= 1;
                    continue;
                }

                bool b = true;
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

            if (list.Count>0 || Count != c) AddLink(list);
        }

        public void GenerateSourceList([NotNull] List<long> list, bool withCheck = true)
        {
            List<long> list1 = list;

            if (withCheck){
                CheckForDouble(list1);

            }
            list1.Sort();
            _sourceList = list1;
        }

        public void GenerateSourceList([NotNull] List<T> list,bool withCheck=true)
        {
            if (_mainBindinglist == null || _mainBindinglist.Count == 0) return;
            List<T> list1 = list;
            if (withCheck)
            {
                CheckForDouble(list1);
 
            }
            if (_sourceList == null) _sourceList = new List<long>();
            else _sourceList.Clear();
            foreach (var v in list1)
            {
                _sourceList.Add(v.Id);
            }
            _sourceList.Sort();
        }

        public void GenerateSourceList()
        {
            GenerateSourceList(_items,false);
        }

        #endregion

        #region Add

        #region добавление ссылок

        public override void AddLink([NotNull] List<T> list, bool renew = false)
        {
            base.AddLink(list,renew);
            GenerateSourceList();
        }

        public override void AddLink([NotNull] List<long> list, bool renew = false)
        {
            base.AddLink(list, renew);
            GenerateSourceList();
        }

        public override int AddLink([NotNull] T item, int index = -1)
        {
            int i = base.AddLink(item, index);
            if ( i>= 0)
            {
                if (_sourceList == null) _sourceList = new List<long>();
                _sourceList?.Add(item.Id);
                _sourceList?.Sort();
            }
            return i;
        }

        public override int AddLink(long itemId)
        {
            int i = base.AddLink(itemId);
            if (i >= 0)
            {
                if (_sourceList == null) _sourceList = new List<long>();
                _sourceList?.Add(itemId);
                _sourceList?.Sort();
            }
            return i;
        }
        

        #endregion

        #region Добавление новых элементов в основную коллекцию и ссылок на них в эту

        protected internal override int InsertItem(int index, [NotNull] T item)
        {
            if (_mainBindinglist == null || item == null) return -1;
            if (_sourceList == null) _sourceList = new List<long>();

            int i= base.InsertItem(index, item);
            if (i < 0) return -1;
            if (!_sourceList.Contains(item.Id) && _items[i].Id == item.Id)
            {
                _sourceList?.Add(item.Id);
                _sourceList?.Sort();
            }
            return i;
        }

        #endregion

        #endregion
        
        #region Удаление самих элементов из основной коллекции и ссылок на них
        protected override void RemoveItem(int index)
        {
            if (_items == null || _items.Count == 0 || index < 0 || index >= _items.Count) return;// throw new ArgumentOutOfRangeException(); //return;
            _sourceList?.Remove(_items[index].Id);
            base.RemoveItem( index);
        }

        #endregion

        #region Удаление ссылок на элементы

        public override void RemoveLink(int index)
        {
            if (_items == null || _items.Count == 0 || index < 0 || index >= _items.Count) return;// throw new ArgumentOutOfRangeException(); //return;
            _sourceList?.Remove(_items[index].Id);
            base.RemoveLink(index);
        }

  
        #endregion

        #endregion

    }
}