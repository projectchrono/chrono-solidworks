using ChronoEngine_SwAddin;
using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ChronoEngineAddin.ChModelExporter;

namespace ChronoEngineAddin
{
    internal class ChModelExporterText
    {

        string asciitext = "";


        protected ChronoEngine_SwAddin.SWIntegration m_swIntegration;
        protected string m_saveDirShapes = "";
        protected string m_saveFilename = "";

        public ChModelExporterText(ChronoEngine_SwAddin.SWIntegration swIntegration, string save_dir_shapes, string save_filename)
        {
            m_saveDirShapes = save_dir_shapes;
            m_saveFilename = save_filename;
            m_swIntegration = swIntegration;
        }

        public void Export()
        {
            ModelDoc2 swModel;
            ConfigurationManager swConfMgr;
            Configuration swConf;
            Component2 swRootComp;

            swModel = (ModelDoc2)m_swIntegration.m_swApplication.ActiveDoc;
            swConfMgr = (ConfigurationManager)swModel.ConfigurationManager;
            swConf = (Configuration)swConfMgr.ActiveConfiguration;
            swRootComp = (Component2)swConf.GetRootComponent3(true);

            asciitext = "# Dump hierarchy from SolidWorks \n" +
                        "# Assembly: " + swModel.GetPathName() + "\n\n\n";

            // The root component (root assembly) cannot work in DumpTraverseComponent() 
            // cause SW api limit, so call feature traversal using this custom step:
            Feature swFeat = (Feature)swModel.FirstFeature();
            DumpTraverseFeatures(swFeat, 1, ref asciitext);

            // Traverse all sub components
            if (swModel.GetType() == (int)swDocumentTypes_e.swDocASSEMBLY)
            {
                DumpTraverseComponent(swRootComp, 1, ref asciitext);
            }

            string asciitext_filename = System.IO.Path.GetDirectoryName(m_saveFilename) + "\\" + System.IO.Path.GetFileNameWithoutExtension(m_saveFilename) + "_shapes/"
                + System.IO.Path.GetFileNameWithoutExtension(m_saveFilename) + ".txt";

            FileStream dumpstream = new FileStream(asciitext_filename, FileMode.Create, FileAccess.ReadWrite);
            StreamWriter dumpwriter = new StreamWriter(dumpstream);
            dumpwriter.Write(asciitext);
            dumpwriter.Flush();
            dumpstream.Close();
        }

        public void DumpTraverseFeatures(Feature swFeat, long nLevel, ref string asciitext)
        {
            Feature swSubFeat;
            string sPadStr = " ";
            long i = 0;

            for (i = 0; i <= nLevel; i++)
            {
                sPadStr = sPadStr + "  ";
            }

            while ((swFeat != null))
            {
                asciitext += sPadStr + "    -" + swFeat.Name + " [" + swFeat.GetTypeName2() + "]" + "\n";
                swSubFeat = (Feature)swFeat.GetFirstSubFeature();
                if ((swSubFeat != null))
                {
                    DumpTraverseFeatures(swSubFeat, nLevel + 1, ref asciitext);
                }
                if (nLevel == 1)
                {
                    swFeat = (Feature)swFeat.GetNextFeature();
                }
                else
                {
                    swFeat = (Feature)swFeat.GetNextSubFeature();
                }
            }
        }

        public void DumpTraverseComponent(Component2 swComp, long nLevel, ref string asciitext)
        {
            // *** SCAN THE COMPONENT FEATURES

            if (!swComp.IsRoot())
            {
                Feature swFeat;
                swFeat = (Feature)swComp.FirstFeature();
                DumpTraverseFeatures(swFeat, nLevel, ref asciitext);
            }

            // *** RECURSIVE SCAN CHILDREN COMPONENTS

            object[] vChildComp;
            Component2 swChildComp;
            string sPadStr = " ";
            long i = 0;

            for (i = 0; i <= nLevel - 1; i++)
            {
                sPadStr = sPadStr + "  ";
            }

            vChildComp = (object[])swComp.GetChildren();

            for (i = 0; i < vChildComp.Length; i++)
            {
                swChildComp = (Component2)vChildComp[i];

                asciitext += sPadStr + "+" + swChildComp.Name2 + " <" + swChildComp.ReferencedConfiguration + ">" + "\n";

                // DumpTraverseComponentFeatures(swChildComp, nLevel, ref asciitext);

                DumpTraverseComponent(swChildComp, nLevel + 1, ref asciitext);
            }
        }
    }
}
