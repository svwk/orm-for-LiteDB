using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using entitymodel.Annotations;

namespace entitymodel
{
    [DebuggerDisplay("Count={d.Count}")]
    public class managerp:IEnumerable<IBindbaselist> 
    {

        #region Fields
        protected Dictionary<EntityType, IBindbaselist> d;
        
        #endregion

        #region Constructors
        public managerp()
        {
            d=new Dictionary<EntityType, IBindbaselist>();
        }

        ~managerp()
        {
            if (d!=null) Clear();

        }

        #endregion

        #region Indexators/Enumerators
     
        public IBindbaselist this[EntityType eType]
        {
            get
            {
                if (d == null || d.Count == 0) return null;
                if (d.ContainsKey(eType)) return d[eType];
                else return null;
            }
            set
            {
                if (value == null) { throw new Exception("Нельзя добавить неинициализированную коллекцию"); }
                if (d == null) { d = new Dictionary<EntityType, IBindbaselist>(); }
                if (eType != value.Type) throw new Exception("Неверный тип добавляемой коллекции");
                AddCollection(value);
            }
        }
        
        public IEnumerator<IBindbaselist> GetEnumerator()
        {
            return d.Values.GetEnumerator();

        }
        IEnumerator IEnumerable.GetEnumerator()
        {

            return d.Values.GetEnumerator();

        }

        #endregion

        #region Events
        [NonSerialized] protected ListChangedEventHandler _onListChanged;
        protected bool _BindinglistChange = true;
        #region Обработка событий изменения списков
        protected void HookListChanged(IEntityBindinglist list)
        { 
            if (_onListChanged == null) _onListChanged = new ListChangedEventHandler(_Bindinglist_ListChanged);
            list.ListChanged += _onListChanged;
        }

        protected void UnhookListChanged(IEntityBindinglist list)
        {
           
            if (list == null || _onListChanged == null) return;
            if (_onListChanged.GetInvocationList().Length > 0)
                list.ListChanged -= _onListChanged;
        }


        protected virtual void _Bindinglist_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (!_BindinglistChange) return;
            if (sender == null ) return;

            entity o = null;
            if (e is entListChangedEventArgs en) o = en.ChangedEntity;

            IEntityBindinglist list=null;
            if (sender is IEntityBindinglist se)
            {
                list = d[se.Type];
            }

            if (list == null) return;
            if (o == null && e.ListChangedType != ListChangedType.ItemDeleted) { o = list[e.NewIndex]; }
            
            switch (e.ListChangedType)
            {
                case ListChangedType.Reset:
                    FillLinks(list.Type);   break;
                case ListChangedType.ItemChanged:
                    if (o != null && list.FindIndexById(o.Id) >= 0)
                    {
                        if (e.PropertyDescriptor.Name.Contains("Id"))
                        FillLinks(o);
                    }
                    break;
                case ListChangedType.ItemAdded:
                    if (o != null && list.FindIndexById(o.Id) >= 0 )
                    {
                        FillLinks(o); 
                    }
                    break;
                case ListChangedType.ItemDeleted:
                    break;
                case ListChangedType.ItemMoved:                 break;
                
                case ListChangedType.PropertyDescriptorAdded:   break;
                case ListChangedType.PropertyDescriptorDeleted: break;
                case ListChangedType.PropertyDescriptorChanged: break;
                default:                                        throw new ArgumentOutOfRangeException();
            }
        }


        #endregion



        #endregion


        #region Propeties
        public int Count => d?.Count() ?? -1;

        public bool Contains(EntityType eType) => d.ContainsKey(eType);

        #endregion

        #region Methods

        #region Clear


        public void Reset()
        {
            if (d==null) return;
            foreach (var v in d)
            {
                v.Value.Reset();
            }
        }
        public void Clear(bool generateEvent=false)
        {
            ClearCollections(generateEvent);
           d?.Clear();
        }

        public void ClearCollections(bool generateEvent = false)
        {
            if (d != null) foreach (var v in d) v.Value.Clear(generateEvent);
        }

 

        #endregion
         
        #region ModifyEntities

        public IEntityBindinglist NewCollection(EntityType newType)
        {
            if (d == null) { d = new Dictionary<EntityType, IBindbaselist>(); }
            if (d.ContainsKey(newType)) return d[newType]; //d[newType].Clear();
            try
            {
            d[newType] =(IBindbaselist) conv.CreateNew(newType,BindListTypes.BindBaseList);
               // d[newType].Name = newType.ToString();
            HookListChanged(d[newType]);
            return d[newType];
            }
            catch (Exception e)
            {

                return null;
            }
        }
        
        public void NewCollection(IEnumerable<EntityType> newTypes)
        {
            if (newTypes == null) { throw new Exception("Для добавления коллекции надо указать тип"); }
            foreach (EntityType et in newTypes)
            {
                NewCollection(et);
            }
        }

        public void AddCollection([NotNull] IBindbaselist entityCollection, bool toReplace=true)
        {
            if (entityCollection == null) { throw new Exception("Нельзя добавить неинициализированную коллекцию"); }
            if (d == null) { d = new Dictionary<EntityType, IBindbaselist>(); }
            //entityCollection.RelationChanged += Relation_Changed;
            if (d.ContainsKey(entityCollection.Type))
            {
                if (toReplace)
                {
                    UnhookListChanged(d[entityCollection.Type]);
                    d[entityCollection.Type].Clear();
                }
                else
                {
                    d[entityCollection.Type].Add(entityCollection.FullList ,false);
                    return;//merge
                }
            }
            d[entityCollection.Type] =entityCollection;
            HookListChanged(d[entityCollection.Type]);
           
        }

        public void AddCollection<T>([NotNull] List<T> entityCollection, bool toReplace = true) where T : entity, new()
        {
            if (entityCollection == null || entityCollection.Count==0) { throw new Exception("Нельзя добавить неинициализированную коллекцию"); }
            if (d                == null) { d = new Dictionary<EntityType, IBindbaselist>(); }

            EntityType eType = entityCollection[0].Type;
            if (!d.ContainsKey(eType)) NewCollection(eType);
            if (d.ContainsKey(eType))
            {
                if (toReplace)
                {
                    d[eType].Add(entityCollection, true);
                }
                else
                {
                    d[eType].Add(entityCollection, false); return; //merge
                }
            }

        }

        public void AddCollection([NotNull] IEnumerable<IBindbaselist> entityCollections, bool toReplace = true)
        {
            if (entityCollections == null) { throw new Exception("Нельзя добавить неинициализированную коллекцию"); }
            foreach (IBindbaselist entityCollection in entityCollections)
            {
                AddCollection(entityCollection,toReplace);
            }
        }

        public virtual void FillLinks(EntityType eType)
        {
            if (d == null || !d.ContainsKey(eType)) return;
            foreach (var v in d[eType])
            {
                if (v is entity ve)
                    FillLinks(ve);
            }
        }


        public virtual void FillLinks(entity item ) { }

        public void FillLinks()
        {
            if (d == null) return;
            foreach (var p in d)
            {
                FillLinks(p.Key);
            }
        }


        public void RemoveCollection(EntityType typeToDelete)
        {
            if (d == null) { return; }//throw new Exception("Нельзя удалить из неинициализированной коллекции"); }

            if (d.ContainsKey(typeToDelete))
            {
                UnhookListChanged(d[typeToDelete]);
                d[typeToDelete].Clear();
                d.Remove(typeToDelete);
                //ResetNames();
            }
        }

        public void RemoveCollection([NotNull] IEnumerable<EntityType> typesToDelete)
        {
            if (typesToDelete == null) { throw new Exception("Нельзя удалить неинициализированную сущность"); }
            foreach (EntityType i in typesToDelete)
            {
                RemoveCollection(i);
            }
        }
        #endregion

      

        #endregion





    }
}
