using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using entitymodel;
using entitymodel.Annotations;
using LiteDB;

namespace dal
{
    class dalentity
    {
        private static string _conStr = @"g:\projects\db.db";
        private static litedb_dal _db;

        public static bool OpenDB()
        {
            if (_db == null)
            {
                _db = new litedb_dal(_conStr);
                bool res = _db.OpenDB();
                if (res) GlobalMapDb();
                return res;
            }

            return false;
        }

        public static void CloseDB()
        {
            _db.CloseDB();
        }

        public static  void SaveTable([NotNull] IBindbaselist et,bool ignoreChanges=false, bool renew = true)
        {
            if (et == null) return;
            if (_db == null) OpenDB();
            EntityType eType = et.Type;

            if (ignoreChanges || et.CheckModified)
            {
                foreach (var v in et.RemovedItems)
                {
                    _db?.Delete(et.Name, v.Id);
                }

                if (ignoreChanges)
                    for (int i = 0; i < et.Count; i++)
                    {
                        if (et[i].Changed == Status.deleted) _db?.Delete(et.Name, et[i].Id);
                        else if (et[i].Changed != Status.newdeleted) Upsert(et[i]);
                    }
                else
                {
                    foreach (var v in et.ChangedList)
                    {
                        if (v.Changed==Status.deleted) _db?.Delete(et.Name, v.Id);
                        else if (v.Changed == Status.newcreated || v.Changed == Status.edited)Upsert(v);
                    }
                }

                if (renew) LoadTable(et, true);
                else et.Reset();
            
            }
        }
        public static void SaveTable([NotNull] managerp mn, EntityType eType, bool ignoreChanges = false, bool renew = true)
        {
            if (mn==null) return;
            if (!mn.Contains(eType)) return;
            SaveTable(mn[eType], ignoreChanges,renew);
        }

        public static void SaveTable([NotNull] managerp mn, bool ignoreChanges = false, bool renew = true)
        {
            if (mn == null) return;
            foreach (var v in mn)
            {
                SaveTable(v, ignoreChanges, renew);
            }
        }

        public static  void LoadTable([NotNull] IBindbaselist et, bool renew = true)
        {
            if (et  == null) return;
            if (_db == null) OpenDB();
            EntityType eType = et.Type;

            switch (eType)
            {
                case EntityType.zak:
                    if (et is bindbaselist<zak> etz) etz.Add(_db?.LoadData<zak>(etz.Name), renew);
                    break;
                case EntityType.dog:
                    if (et is bindbaselist<dog> etg) etg.Add(_db?.LoadData<dog>(etg.Name), renew);
                    break;
                case EntityType.pls:
                    if (et is bindbaselist<pls> etl) etl.Add(_db?.LoadData<pls>(etl.Name), renew);
                    break;
                case EntityType.sor:
                    if (et is bindbaselist<sor> ets) ets.Add(_db?.LoadData<sor>(ets.Name), renew);
                    break;
                case EntityType.prj:
                    if (et is bindbaselist<prj> etj) etj.Add(_db?.LoadData<prj>(etj.Name), renew);
                    break;
                case EntityType.smt:
                    if (et is bindbaselist<smt> etm) etm.Add(_db?.LoadData<smt>(etm.Name), renew);
                    break;
                case EntityType.wgr:
                    if (et is bindbaselist<wgr> etw) etw.Add(_db?.LoadData<wgr>(etw.Name), renew);
                    break;
                case EntityType.rab:
                    if (et is bindbaselist<rab> eta) eta.Add(_db?.LoadData<rab>(eta.Name), renew);
                    break;
            }
            et.Reset();
        }

        public static void LoadTable([NotNull] managerp mn, EntityType eType, bool renew = true)
        {
            if (mn == null) return;
            if (!mn.Contains(eType)) mn.NewCollection(eType);
            LoadTable(mn[eType], renew);
        }

        public static void LoadTable([NotNull] managerp mn, bool renew = true)
        {
            if (mn == null) return;
            foreach (var v in mn)
            {
                LoadTable(v, renew);
            }
        }


        public static void Upsert(entity item, string name = "")
        {
            if (_db == null) OpenDB();
            EntityType eType = item.Type;
            if (name=="") name = eType.ToString();
            switch (eType)
            {
                case EntityType.zak:
                    if (item is zak iz) _db?.Upset<zak>(name,iz);
                    break;
                case EntityType.dog:
                    if (item is dog id) _db?.Upset<dog>(name, id); break;
                case EntityType.pls:
                    if (item is pls il) _db?.Upset<pls>(name, il); break;
                case EntityType.sor:
                    if (item is sor ir) _db?.Upset<sor>(name, ir); break;
                case EntityType.prj:
                    if (item is prj ij) _db?.Upset<prj>(name, ij); break;
                case EntityType.smt:
                    if (item is smt im) _db?.Upset<smt>(name, im); break;
                case EntityType.wgr:
                    if (item is wgr iw) _db?.Upset(name, iw); break;
                case EntityType.rab:
                    if (item is rab ia) _db?.Upset(name, ia); break;

            }
        }

        public static void Update(entity item, string name = "")
        {
            if (_db == null) OpenDB();
            EntityType eType = item.Type;
            if (name == "") name = eType.ToString();
            switch (eType)
            {
                case EntityType.zak:
                    if (item is zak iz) _db?.Update<zak>(name, iz);
                    break;
                case EntityType.dog:
                    if (item is dog id) _db?.Update<dog>(name, id); break;
                case EntityType.pls:
                    if (item is pls il) _db?.Update<pls>(name, il); break;
                case EntityType.sor:
                    if (item is sor ir) _db?.Update<sor>(name, ir); break;
                case EntityType.prj:
                    if (item is prj ij) _db?.Update<prj>(name, ij); break;
                case EntityType.smt:
                    if (item is smt im) _db?.Update<smt>(name, im); break;
                case EntityType.wgr:
                    if (item is wgr iw) _db?.Update(name, iw); break;
                case EntityType.rab:
                    if (item is rab ia) _db?.Update(name, ia); break;

            }

        }

        public static void Insert(entity item, string name = "")
        {
            if (_db == null) OpenDB();
            EntityType eType = item.Type;
            if (name == "") name = eType.ToString();
            switch (eType)
            {
                case EntityType.zak:
                    if (item is zak iz) _db?.Insert<zak>(name, iz);
                    break;
                case EntityType.dog:
                    if (item is dog id) _db?.Insert<dog>(name, id); break;
                case EntityType.pls:
                    if (item is pls il) _db?.Insert<pls>(name, il); break;
                case EntityType.sor:
                    if (item is sor ir) _db?.Insert<sor>(name, ir); break;
                case EntityType.prj:
                    if (item is prj ij) _db?.Insert<prj>(name, ij); break;
                case EntityType.smt:
                    if (item is smt im) _db?.Insert<smt>(name, im); break;
                case EntityType.wgr:
                    if (item is wgr iw) _db?.Insert(name, iw); break;
                case EntityType.rab:
                    if (item is rab ia) _db?.Insert(name, ia); break;

            }

        }
        public bool FindById(string name, long id, ref entity item)
        {
            if (_db == null) OpenDB();
            if (_db == null) return false;
 
            EntityType eType = item.Type;
            switch (eType)
            {
                case EntityType.zak:
                    if (item is zak iz) return _db.FindById<zak>(name,id,ref iz);break;
                case EntityType.dog:
                    if (item is dog ig) return _db.FindById<dog>(name,id, ref ig); break;
                case EntityType.pls:
                    if (item is pls il) return _db.FindById<pls>(name,id, ref il); break;
                case EntityType.sor:
                    if (item is sor ir) return _db.FindById<sor>(name,id, ref ir); break;
                case EntityType.prj:
                    if (item is prj ij) return _db.FindById<prj>(name,id, ref ij); break;
                case EntityType.smt:
                    if (item is smt im) return _db.FindById<smt>(name, id, ref im); break;
                case EntityType.wgr:
                    if (item is wgr iw) return _db.FindById<wgr>(name, id, ref iw); break;
                case EntityType.rab:
                    if (item is rab ia) return _db.FindById(name, id, ref ia); break;



            }

            return false;
        }


        public static void GlobalMapDb()
        {
            MapDb(BsonMapper.Global);
        }
        public static void MapDb(BsonMapper mapper)
        {

            mapper.Entity<zak>()
                  .Id(x => x.Id)
                  .Field(x => x.Name, "n")
                  .Field(x => x.Rekvizity, "r")
                  .Field(x => x.TimeStamp, "t")
                  .Ignore(x => x.CDogs)
                  .Ignore(x => x.Type)
                  .Ignore(x => x.Changed)
                  .Ignore(x => x.Value)
                  .Ignore(x => x.CreationTime);

            mapper.Entity<prj>()
                   .Id(x => x.Id)
                   .Field(x => x.Name, "n")
                   .Field(x => x.Oboznach, "o")
                   .Field(x => x.Folder, "f")
                   .Field(x => x.SorId, "s")
                   .Field(x => x.TimeStamp, "t")
                    .Ignore(x => x.Sor)
                    .Ignore(x => x.Type)
                    .Ignore(x => x.Changed)
                  .Ignore(x => x.Value)
                  .Ignore(x => x.CSmts)
                    .Ignore(x => x.CreationTime);


            mapper.Entity<sor>()
                  .Id(x => x.Id)

                  .Field(x => x.Name, "n")
                  .Field(x => x.GP, "g")
                  .Field(x => x.Adress, "a")
                  .Field(x => x.TimeStamp, "t")
                  .Field(x => x.PlsId, "p")
                  .Ignore(x => x.CPrjs)
                  .Ignore(x => x.Pls)
                  .Ignore(x => x.Type)
                  .Ignore(x => x.Changed)
                  .Ignore(x => x.Value)
                  .Ignore(x => x.CreationTime);


            mapper.Entity<pls>()
                  .Id(x => x.Id)
                  .Field(x => x.Name, "n")
                  .Field(x => x.Adress, "a")
                  .Field(x => x.TimeStamp, "t")
                  .Field(x => x.DogId, "d")
                  .Ignore(x => x.Type)
                  .Ignore(x => x.Dog)
                  .Ignore(x => x.Changed)
                  .Ignore(x => x.Value)
                  .Ignore(x => x.CreationTime)
                  .Ignore(x => x.CSors);


            mapper.Entity<dog>()
                  .Id(x => x.Id)
                  .Field(x => x.DataZakl, "d")
                  .Field(x => x.Summa, "s")
                  .Field(x => x.Srok, "r")
                  .Field(x => x.ObjectName, "o")
                  .Field(x => x.ObjectAdress, "a")
                  .Field(x => x.TimeStamp, "t")
                  .DbRef(x => x.Content, "c")
                  .Field(x => x.ZakId, "z")
                  .Ignore(x => x.CPlss)
                  .Ignore(x => x.Zak)
                  .Ignore(x => x.Type)
                  .Ignore(x => x.Changed)
                  .Ignore(x => x.Value)
                  .Ignore(x => x.CreationTime);


            mapper.Entity<dogc>()
                  .Id(x => x.Id)

                  .Field(x => x.Predmet, "p")
                  .Field(x => x.Document, "d")
                  .Field(x => x.TimeStamp, "t")
                  .Ignore(x => x.Changed)
                  .Ignore(x => x.Value)
                  .Ignore(x => x.CreationTime)
                  .Ignore(x => x.Type);

            mapper.Entity<smt>()
                  .Id(x => x.Id)
                  .Field(x => x.SmetaName,      "n")
                  .Field(x => x.ProjectVersiya,    "v")
                  .Field(x => x.SmetaLink, "l")
                  .Field(x => x.SmetaNumber, "u")
                  .Field(x => x.SmetaOboznach, "o")
                  .Field(x => x.TimeStamp, "t")
                  .Field(x => x.PrjId,     "p")
                  .Field(x => x.Projectlists, "s")
                  .Ignore(x => x.Type)
                  .Ignore(x => x.CWgrs)
                  .Ignore(x => x.Project)
                  .Ignore(x => x.Changed)
                  .Ignore(x => x.Value)
                  .Ignore(x => x.CreationTime);

            mapper.Entity<wgr>()
                  .Id(x => x.Id)
                  .Field(x => x.Name, "n")
                  .Field(x => x.Position, "p")
                  .Field(x => x.TimeStamp, "t")
                  .Field(x => x.SmtId,"s")
                  .Ignore(x => x.Type)
                  .Ignore(x => x.Smt)
                  .Ignore(x => x.CRabs)
                  .Ignore(x => x.Changed)
                  .Ignore(x => x.Value)
                  .Ignore(x => x.CreationTime);

            mapper.Entity<rab>()
                  .Id(x => x.Id)
                  .Field(x => x.Name,"n")
                  .Field(x => x.Position, "p")
                  .Field(x => x.Originalname,"o")
                  .Field(x => x.Count,"c")
                  .Field(x => x.Cost,  "s")
                  .Field(x => x.Costtotal, "l")
                  .Field(x => x.Costtotalnds, "i")
                  .Field(x => x.TimeStamp,"t")
                  .Field(x => x.WgrId, "w")
                  .Ignore(x => x.Type)
                  .Ignore(x => x.Wgr)
                  .Ignore(x => x.Changed)
                  .Ignore(x => x.Value)
                  .Ignore(x => x.CreationTime);

        }



    }
}
