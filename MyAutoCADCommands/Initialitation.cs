using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using System.Reflection;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace MyAutoCADCommands
{
    public class Initialitation: IExtensionApplication
    {
        #region Commands
        [CommandMethod("MyFirstCommand")] public void cmdMyFirst()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            //ed.WriteMessage("\nHello World!");

            if(IsSavedFile() == true)
            {
                ed.WriteMessage("\nFile is an existing file");
            }
            else
            {
                ed.WriteMessage("\nFile is a new drawing file");
            }
        }
        
        [CommandMethod("LIGetVersion")] public void cmdAcadVersion()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            switch(Application.DocumentManager.MdiActiveDocument.Database.LastSavedAsVersion)
            {
                case (Autodesk.AutoCAD.DatabaseServices.DwgVersion.AC1032):
                    ed.WriteMessage("\nAutoCAD 2018");
                    break;

                case (Autodesk.AutoCAD.DatabaseServices.DwgVersion.AC1027):
                    ed.WriteMessage("\nAutoCAD 2013");
                    break;

                case (Autodesk.AutoCAD.DatabaseServices.DwgVersion.AC1024):
                    ed.WriteMessage("\nAutoCAD 2010");
                    break;

                case (Autodesk.AutoCAD.DatabaseServices.DwgVersion.AC1021):
                    ed.WriteMessage("\nAutoCAD 2007");
                    break;

                default:
                    ed.WriteMessage("\nPrior to AutoCAD 2007");
                    break;

            }
        }

        [CommandMethod("LIGetLoginName")] public void cmdLogin()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            string logName = System.Convert.ToString(Application.GetSystemVariable("LoginName"));
            ed.WriteMessage("\n" +  logName);

            int firstL = logName.IndexOf("l");
            ed.WriteMessage("\n" + logName.Substring(0, firstL) + " " + logName.Substring(firstL));

        }

        [CommandMethod("LILoops")] public void cmdLoops()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            int[] arr = new int[5];
            arr[0] = 10;
            arr[1] = 9;
            arr[2] = 8;
            arr[3] = 7;
            arr[4] = 6;

            foreach(int iObj in arr)
            {
                ed.WriteMessage("\n" + iObj);
            }
            ed.WriteMessage("\nEnd of first loop\n");

            for (int i = arr.Length -  1; i >= 0; i--)
            {
                try
                {
                    if(i == 3)
                    {
                        throw new Autodesk.AutoCAD.Runtime.Exception(ErrorStatus.AnonymousEntry, "7 is not an acceptable number");
                    }
                    else
                    {
                        ed.WriteMessage("\n" + arr[i]);
                    }
                }
                catch (Autodesk.AutoCAD.Runtime.Exception ex)
                {
                    Application.ShowAlertDialog("Error in LILoops > Backwards Array Loop \n" + ex.Message);
                }
                //ed.WriteMessage("\n" + arr[i]);
            }
            ed.WriteMessage("\nEnd of second loop\n");

            List<int> iList = new List<int>();
            iList.Add(1);
            iList.Add(2);
            iList.Add(3);
            iList.Add(4);
            iList.Add(5);

            foreach(int iObj in iList)
            {
                ed.WriteMessage("\n" + iObj);
            }

        }

        [CommandMethod("LIDocumentCount")] public void cmdDocCount()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("\nThere are " + Application.DocumentManager.Count + " drawings opened.");
        }

        [CommandMethod("LIDocumentProperties")] public void cmdDocProp()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            acDoc.Editor.WriteMessage("\n" + acDoc.Name);
        }

        [CommandMethod("LINewDocument")] public void cmdNewDoc()
        {
            DocumentCollection docCol = Application.DocumentManager;
            Document newDoc = docCol.Add("C:/Users/javil/AppData/Local/Autodesk/C3D 2024/esp/Template/_Autodesk Civil 3D (Metric) NCS.dwt");
            docCol.MdiActiveDocument = newDoc;
        }

        [CommandMethod("LIDBProps")] public void cmdDBProps()
        {
            Database db = Application.DocumentManager.MdiActiveDocument.Database;
            db.Ltscale = 48;  
        }

        [CommandMethod("LIObjectID")] public void cmdDBObjID()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            //Database dbAcad = Application.DocumentManager.MdiActiveDocument.Database;
            ObjectId lyrTblId = db.LayerTableId;
            Boolean testIsErased = lyrTblId.IsErased;
        }

        [CommandMethod("LITransactions")] public void cmmdDBTrans()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId lyrTblId = db.LayerTableId;

            Transaction trans = db.TransactionManager.StartTransaction();
            LayerTable lyrTbl = trans.GetObject(lyrTblId, OpenMode.ForRead) as LayerTable;
            
            Int16 cntr = 0;
            foreach(ObjectId lyrId in lyrTbl) {
                cntr += 1;
            }
            trans.Commit();

            ed.WriteMessage("\nThere are " + cntr + " layers in the drawing.");
        }

        [CommandMethod("LICreateLayer")] public void cmdCreateLayer()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            ObjectId lyrTblId = db.LayerTableId;
            Transaction trans = db.TransactionManager.StartTransaction();

            try
            {
                LayerTable lyrTbl = trans.GetObject(lyrTblId, OpenMode.ForWrite) as LayerTable;
                string lyrName = "MyNewLayer";
                if (IsLayerExists(trans, lyrTbl, lyrName) == false)
                {
                    LayerTableRecord lyrTblRec = new LayerTableRecord();
                    lyrTblRec.Name = lyrName;
                    lyrTbl.Add(lyrTblRec);
                    trans.AddNewlyCreatedDBObject(lyrTblRec, true);
                } else
                {
                    ed.WriteMessage("\nLayer " + lyrName + " already exists in the drawing");
                }

            }
            catch(Autodesk.AutoCAD.Runtime.Exception ex)
            {
                Application.ShowAlertDialog("Error in creating layer\n" + ex.Message);
            }
        }

        [CommandMethod("LICreateLine")] public void cmdCreateLine()
         {
            Database db = HostApplicationServices.WorkingDatabase;
            Transaction trans = db.TransactionManager.StartTransaction();

            try
            {

                ObjectId blkId = db.CurrentSpaceId;
                BlockTableRecord curSpaceBlk = trans.GetObject(blkId, OpenMode.ForWrite) as BlockTableRecord;
                Point3d pnt1 = new Point3d(0, 0, 0);
                Point3d pnt2 = new Point3d(10, 10, 0);
                Line lineObj = new Line(pnt1, pnt2);
                curSpaceBlk.AppendEntity(lineObj);
                trans.AddNewlyCreatedDBObject(lineObj, true);
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)
            {
                Application.ShowAlertDialog("Error in creating line\n" + ex.Message);
            }

            trans.Commit();
        }

        [CommandMethod("LIGetPoint")] public void cmdEdGetPoint()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;

            PromptPointOptions prPtOpts = new PromptPointOptions("\nPinck a point: ");
            prPtOpts.AllowArbitraryInput = false;
            prPtOpts.AllowNone = true;

            PromptPointResult prPtRes1 = ed.GetPoint(prPtOpts);
            if (prPtRes1.Status == PromptStatus.OK) return;

            prPtOpts.BasePoint = prPtRes1.Value;
            prPtOpts.UseBasePoint = true;

            PromptPointResult prPtRes2 = ed.GetPoint(prPtOpts);
            if (prPtRes2.Status != PromptStatus.OK) return;

            ed.WriteMessage("\nPoint 1 = " + prPtRes1.Value.ToString() + "\nPoint 2 = " + prPtRes2.Value.ToString());

        }

        [CommandMethod("LIGetDistance")] public void cmdEdGetDistance()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;

            PromptDistanceOptions prDistOpts = new PromptDistanceOptions("\nSpecify a distance: ");
            prDistOpts.AllowArbitraryInput = false;
            prDistOpts.AllowNegative = false;
            prDistOpts.AllowNone = true;
            prDistOpts.AllowZero = false;
            prDistOpts.DefaultValue = 1;
            prDistOpts.Only2d = true;
            prDistOpts.UseDefaultValue = true;

            PromptDoubleResult prDistRes = ed.GetDistance(prDistOpts);
            if(prDistRes.Status != PromptStatus.OK) return;

            ed.WriteMessage("\nDistance is " + Math.Round(prDistRes.Value,db.Luprec, MidpointRounding.AwayFromZero));

        }

        #endregion

        #region SupportFunctions
        private Boolean IsSavedFile()
        {
            int dResult = System.Convert.ToInt16(Application.GetSystemVariable("DWGTITLED"));

            if (dResult != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private Boolean IsLayerExists(Transaction CurrentTransaction, LayerTable Layers, String LayerName)
        {
            LayerTableRecord lyrObj;
            foreach(ObjectId lyrId in Layers)
            {
                lyrObj = CurrentTransaction.GetObject(lyrId, OpenMode.ForRead) as LayerTableRecord;
                if(lyrObj.Name == LayerName) return true;
            }
            return false;
        }

        private void AnotherConditionMethod()
        {

        }

        #endregion

        #region Initialization
        void IExtensionApplication.Initialize()
        {
            Application.MainWindow.Text = "My Application";

            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            AssemblyName appName = Assembly.GetExecutingAssembly().GetName();

            object[] attrs = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
            ed.WriteMessage(((AssemblyTitleAttribute)attrs[0]).Title + " " + appName.Version.Major.ToString() + " is loaded...");
        }

        void IExtensionApplication.Terminate()
        {
            
        }

        #endregion

    }
}
