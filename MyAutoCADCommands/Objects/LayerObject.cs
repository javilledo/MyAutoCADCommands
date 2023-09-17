using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Objects
{
    public class LayerObject: BaseObject
    {
        private Boolean _isFrozen = false;
        public Boolean isFrozen
        {
            get
            {
                return _isFrozen;
            }
            set
            {
                _isFrozen = value;
            }
        }
    }

    public class LayerObjectCollection : System.Collections.ObjectModel.ObservableCollection<LayerObject>
    {
        public void GetFromDrawingDatabase(Boolean IncludeReferences, Database DrawingDatabase = null)
        {
            if(DrawingDatabase == null)
            {
                DrawingDatabase = HostApplicationServices.WorkingDatabase;
            }
            Transaction trans = DrawingDatabase.TransactionManager.StartTransaction();

            try
            {
                LayerTable lyrTbl = trans.GetObject(DrawingDatabase.LayerTableId, OpenMode.ForRead) as LayerTable;
                LayerTableRecord lyrTblRec;
                LayerObject lyrObj;
                foreach(ObjectId lyrId in lyrTbl)
                {
                    lyrTblRec = trans.GetObject(lyrId, OpenMode.ForRead) as LayerTableRecord;
                    if (((lyrTblRec.Name.Contains("|") == true) && (IncludeReferences == false))) continue;

                    lyrObj = new LayerObject();
                    lyrObj.BaseId = lyrId;
                    lyrObj.isFrozen = lyrTblRec.IsFrozen;
                    lyrObj.Name = lyrTblRec.Name;
                    this.Add(lyrObj);
                }
            }
            catch
            {
                
            }
            trans.Commit();
        }
        public static string GetCurrentLayerName()
        {
            string retLayerName = "";
            Database db = HostApplicationServices.WorkingDatabase;
            Transaction trans = db.TransactionManager.StartTransaction();

            LayerTableRecord lyrTblRec = trans.GetObject(db.Clayer, OpenMode.ForRead) as LayerTableRecord;
            retLayerName = lyrTblRec.Name;

            trans.Commit();
            return retLayerName;
        }
    }
}
