using System;

namespace entitymodel
{
    class managerp_smeta : managerp
    {
        #region Propeties
        public bindbaselist<wgr> Wgrs
        {
            get
            {
                if (this[EntityType.wgr] != null) return (this[EntityType.wgr] as bindbaselist<wgr>);
                return null;
            }
            set
            {
                if (value.Type != EntityType.wgr) { throw new Exception("Неверный тип коллекции. Присваивание не выполнено"); }
                else AddCollection(value);
            }
        }

        public bindbaselist<rab> Rabs
        {
            get
            {
                if (this[EntityType.rab] != null) return (this[EntityType.rab] as bindbaselist<rab>);
                return null;
            }
            set
            {
                if (value.Type != EntityType.rab) { throw new Exception("Неверный тип коллекции. Присваивание не выполнено"); }
                else AddCollection(value);
            }
        }


        #endregion

        #region Methods

        public override void FillLinks(entity item)
        {
            if (item == null || d == null) return;
            EntityType eType = item.Type;
            switch (eType)
            {
                case EntityType.wgr:
                    if (item is wgr vw)
                    {
                        if (Rabs != null)
                        {
                            if (vw.CRabs != null && vw.CRabs.MainBindinglist != null &&!object.ReferenceEquals(vw.CRabs.MainBindinglist, Rabs))
                            {
                                vw.CRabs.MainBindinglist.RemoveView(vw.Id.ToString());
                                vw.CRabs = null;
                            }

                            if (vw.CRabs == null)
                            {
                                if (Rabs.Count > 0)
                                    vw.CRabs =(bindviewcond<rab>) Rabs.Views[Rabs.AddView(vw.Id.ToString(), x => x.WgrId == vw.Id)];
                            }
                            else vw.CRabs.RefreshList();

                            if (vw.CRabs != null && vw.CRabs.Count > 0)
                            {
                                foreach (var p in vw.CRabs) p.Wgr = vw;
                            }
                        }
                        else vw.CRabs = null;

                    }

                    break;

                case EntityType.rab:
                    if (item is rab va)
                    {

                        if (Wgrs != null && va.WgrId > 0 && (va.Wgr == null || va.Wgr.Id != va.WgrId))
                        {
                            va.Wgr = Wgrs[va.WgrId];
                            if (va.Wgr != null) FillLinks(va.Wgr);
                        }
                    }

                    break;

            }
        }

        #endregion
            }
}