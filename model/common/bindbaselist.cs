using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Diagnostics;
using entitymodel.Annotations;

namespace entitymodel
{
    /// <summary>Предоставляет коллекцию сущностей, которая поддерживает привязку данных.</summary>
    /// <typeparam name="T">Тип элементов в списке, наследников класса entity.</typeparam>
    
    [DebuggerDisplay("Count = {this.Count}")]
    [Serializable]
    public class bindbaselist<T> : bindlist<T>, IBindbaselist where T : entity, new()
    {
        //protected new IList<KeyValuePair<long,T>> items;

        #region Fields

        //списки
        private List<entity> _removedItems;
        private Dictionary<string,bindview<T>> _views;

       
        #endregion


        #region Constructors

        /// <summary>Инициализирует новый экземпляр класса <see cref="T:System.ComponentModel.BindingList`1" />, используя значения по умолчанию.</summary>
        public bindbaselist()
        {
            _isVeiw = false;
                _removedItems = new List<entity>();
            _views = new Dictionary<string, bindview<T>> {["default"] = new bindviewdefault<T>(this)};
        }

        /// <summary>Инициализирует новый экземпляр <see cref="T:System.ComponentModel.BindingList`1" /> класса с указанным списком.</summary>
        /// <param name="list">
        /// <see cref="T:System.Collections.Generic.IList`1" /> Элементов, которые должны содержаться в <see cref="T:System.ComponentModel.BindingList`1" />.</param>
        public bindbaselist(IList<T> list):base(list)
        {
            _isVeiw       = false;
            _removedItems = new List<entity>();
            _views = new Dictionary<string, bindview<T>> {["default"] = new bindviewdefault<T>(this)};
        }

        //~bindbaselist()
        //{
        //    Clear();
        //    //_removedItems = null; _views = null;
        //}
        #endregion

        #region Events

        #endregion

        #region Indexators/Enumerators

        #endregion


        #region Propeties

        public List<entity> RemovedItems => _removedItems;
        //private IEntityBindinglist MainBindinglist { get; set; }

        public List<entity> ChangedList
        {
            get
            {
                List<entity> es = new List<entity>();
                foreach (T item in _items)
                {
                    if ((item != null) && item.Changed != Status.not_changed && item.Changed != Status.newdeleted )
                    {
                        es.Add(item);
                    }
                }
                return es;
            }
        }
        public Dictionary<string, bindview<T>> Views => _views;

        public bindview<T> DefaultViewT => _views["default"];
        public override IEntityBindinglist DefaultView => _views["default"];
        #endregion

        #region Methods

        #region Checks

        private bool InRemoved(T en)
        {
            if (_removedItems == null || _removedItems.Count == 0) return false;
            foreach (var v in _removedItems)
            {
                if (v.Id == en.Id) return true;
            }
            return false;
        }
        public override bool CheckModified => base.CheckModified || _removedItems.Count > 0;

        #endregion

        #region Resetting

        public override void Reset()
        {
            base.Reset();
            if (_views==null || _views.Count==0) return;
            foreach (var v in _views.Values)
            {
                v.Reset();
            }
        }

        public override void Clear(bool generateEvent )
        {
            _removedItems?.Clear();
            if (_views != null)
            {
                foreach (var v in _views.Values)
                {
                    v.Clear(generateEvent);
                }
            }
            //_views?.Clear(); 
            base.Clear(generateEvent);
        }
        
        #endregion

         #region Remove
        
        protected override void RemoveItem(int index)
        {
            if (_items == null || _items.Count == 0 || index < 0 || index >= _items.Count) return;// throw new ArgumentOutOfRangeException(); //return;
            T a = _items[index];
            if (_removedItems == null) _removedItems = new List<entity>();
            base.RemoveItem(index);
            if (a.Changed == Status.deleted) // если элнмент не вновь добавленный
            {
                if (!InRemoved(a)) _removedItems.Add(a); //добавляем в список удленных
            }
        }


        #endregion

        #region Views Methods


        public string AddView(string name,List<long> idList)
        {
            string n = name;
            if (_views.ContainsKey(n))
            {
                if (_views[n] is bindviewlist<T> a)
                {
                    a.SourceList = idList;
                    return n;
                }
                else
                {
                    _views[n].ClearView(false);
                }
            }

            bindviewlist<T> e = new bindviewlist<T>(this, idList) {Name = n};


            _views[n]=e;
            return n;
        }

        public string AddView(string name,[NotNull] Predicate<T> condition)
        {
            
            string n = name;
            if (_views.ContainsKey(n))
            {
                if (_views[n] is bindviewcond<T> a)
                {
                    a.Condition = condition;
                    return n;
                }
                else
                {
                    _views[n].ClearView(false);
                }
            }

            bindviewcond<T> e = new bindviewcond<T>(this, condition) {Name = n};

            _views[n] = e;
            return n;

        }

        public string AddView(string name)
        {
            string n = name;
            if (_views.ContainsKey(n))
            {
                _views[n].ClearView(false);
            }

 
            bindview<T> e = new bindview<T>(this) {Name = n};
            _views[n] = e;
            return n;

 
        }
        

        public void RemoveView(string name)
        {
            if (_views.ContainsKey(name)) _views.Remove(name);

        }

        #endregion

        #endregion


    }
}
