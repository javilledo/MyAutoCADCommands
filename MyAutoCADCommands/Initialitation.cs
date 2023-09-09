using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;

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

        private void AnotherConditionMethod()
        {

        }

        #endregion

        #region Initialization
        void IExtensionApplication.Initialize()
        {

        }

        void IExtensionApplication.Terminate()
        {
            
        }

        #endregion


    }
}
