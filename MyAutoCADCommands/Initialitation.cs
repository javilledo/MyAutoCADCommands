using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.ApplicationServices;
using System.Reflection;

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
