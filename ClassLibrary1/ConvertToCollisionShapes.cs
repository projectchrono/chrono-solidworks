using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Media3D;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swcommands;
using SolidWorks.Interop.swconst;
using SolidWorks.Interop.swpublished;
using SolidWorksTools;



namespace ChronoEngine_SwAddin
{
    class ConvertToCollisionShapes
    {
        // Try to see if the SolidWorks Body2 shape is a pure sphere.
        // If so,return true. If not a sphere, return false.
        // If it is a sphere, in 'radius' and 'center' one can get 
        // the sphere data.

        public static bool SWbodyToSphere(Body2 swBody,
                                          ref double radius, 
                                          ref Point3D center)
        {
            bool issphere = false;
            if (swBody.GetFaceCount() == 1)
            {
                object[] mfaces = (object[])swBody.GetFaces();
                Face2 sphface = (Face2)mfaces[0];
                Surface msurf = (Surface)sphface.GetSurface();
                
                if (msurf.IsSphere())
                {
                    issphere= true;
                    double[] sphpar = (double[])msurf.SphereParams;
                    radius = sphpar[3];
                    center.X = sphpar[0]; center.Y = sphpar[1]; center.Z = sphpar[2];
                }
            }
            return issphere;
        }

        // Test if a SolidWorks Body2 shape can be converted to a pure sphere.
        public static bool SWbodyToSphere(Body2 swBody)
        {
            double dfoo = 0;
            Point3D vfoo = new Point3D(0, 0, 0);
            return SWbodyToSphere(swBody, ref dfoo, ref vfoo);
        }



        // Try to see if the SolidWorks Body2 shape is a pure box.
        // If so,return true. If not a box, return false.
        // If it is a box, in 'corner' and 'Dx,Dy,Dx' one can get 
        // the box main corner and the three departing edges.

        public static bool SWbodyToBox(Body2 swBody,
                                          ref Point3D corner,
                                          ref Vector3D Ex, ref Vector3D Ey, ref Vector3D Ez)
        {
            bool isbox = false;

            if (swBody.GetFaceCount() == 6)
                if (swBody.GetEdgeCount() == 12)
                    if (swBody.GetVertexCount() == 8)
                    {
                        object[] mfaces = (object[])swBody.GetFaces();
                        object[] medges = (object[])swBody.GetEdges();
                        object[] mverts = (object[])swBody.GetVertices();

                        isbox = true;

                        // rejective test 1: are all faces as planes?
                        bool allplanes = true;
                        for (int i = 0; i < 6; i++)
                        {
                            Face2 sphface = (Face2)mfaces[i];
                            Surface msurf = (Surface)sphface.GetSurface();
                            if (!msurf.IsPlane())
                            {
                                allplanes = false;
                            }
                        }
                        if (allplanes)
                        {
                            isbox = true;
                            Vector3D[] pnts = new Vector3D[8];
                            for (int ip = 0; ip < 8; ip++)
                                pnts[ip] = new Vector3D(((double[])((Vertex)mverts[ip]).GetPoint())[0],
                                                        ((double[])((Vertex)mverts[ip]).GetPoint())[1],
                                                        ((double[])((Vertex)mverts[ip]).GetPoint())[2]);
                            Vector3D pC = pnts[0];
                            int X_id = 0;
                            int Y_id = 0;
                            int Z_id = 0;
                            Vector3D pX = pnts[0];
                            Vector3D pY = pnts[0];
                            Vector3D pZ = pnts[0];
                            Vector3D Dx = (pX - pC);
                            Vector3D Dy = (pY - pC);
                            Vector3D Dz = (pZ - pC);

                            // rejective test 1: are there 3 points that define an orthogonal trihedron with pC corner?
                            bool found_corner = false;
                            for (int xi = 1; xi < 8; xi++)
                            {
                                for (int yi = xi + 1; yi < 8; yi++)
                                {
                                    for (int zi = yi + 1; zi < 8; zi++)
                                    {
                                        pX = pnts[xi];
                                        pY = pnts[yi];
                                        pZ = pnts[zi];
                                        Dx = (pX - pC);
                                        Dy = (pY - pC);
                                        Dz = (pZ - pC);
                                        if ((Math.Abs(Vector3D.DotProduct(Dx, Dy)) < Dx.Length * (1e-5)) &&
                                            (Math.Abs(Vector3D.DotProduct(Dy, Dz)) < Dx.Length * (1e-5)) &&
                                            (Math.Abs(Vector3D.DotProduct(Dz, Dx)) < Dx.Length * (1e-5)))
                                            {
                                                X_id = xi; Y_id = yi; Z_id = zi;
                                                found_corner = true;
                                                break;
                                            }
                                    }
                                    if (found_corner) break;
                                }
                                if (found_corner) break;
                            }
                            if (!found_corner)
                            {
                                isbox = false;
                            }
 
                            double box_tol = Dx.Length * (1e-5);

                            
                            //System.Windows.Forms.MessageBox.Show(" Dx " + Dx + "\n" + " Dy " + Dy + "\n"  + " Dz " + Dz); 

                            // rejective test 2: is there a point opposite to pC?
                            Vector3D pE = pC + Dx + Dy;
                            bool found_E = false;
                            int E_id = 0;
                            for (int ip = 1; ip < 8; ip++)
                            {
                                Vector3D ds = pE - pnts[ip];
                                if (ds.Length < box_tol)
                                {
                                    found_E = true;
                                    E_id = ip;
                                }
                            }
                            if (!found_E)
                            {
                                isbox = false;
                            }

                            // rejective test 3: are other four points aligned?
                            Vector3D norm = Vector3D.CrossProduct(Dx, Dy);
                            norm.Normalize();
                            Vector3D[] ptsA = new Vector3D[4];
                            bool[] aligned = new bool[4];
                            double[] dists = new double[4];
                            aligned[0] = aligned[1] = aligned[2] = aligned[3] = false;
                            ptsA[0] = pC;
                            ptsA[1] = pX;
                            ptsA[2] = pY;
                            ptsA[3] = pnts[E_id];
                            for (int iA = 0; iA < 4; iA++)
                                for (int ip = 1; ip < 8; ip++)
                                {
                                    if ((ip != E_id) && (ip != X_id) && (ip != Y_id))
                                    {
                                        Vector3D D = pnts[ip] - ptsA[iA];
                                        if ((Vector3D.CrossProduct(D, norm)).Length < box_tol)
                                        {
                                            aligned[iA] = true;
                                            dists[iA] = Vector3D.DotProduct(D, norm);
                                        }
                                    }
                                }
                            if ((aligned[0] != true) ||
                                (aligned[1] != true) ||
                                (aligned[2] != true) ||
                                (aligned[3] != true))
                            {
                                isbox = false;
                            }
                            if ((Math.Abs(dists[0] - dists[1]) > box_tol) ||
                                (Math.Abs(dists[0] - dists[2]) > box_tol) ||
                                (Math.Abs(dists[0] - dists[3]) > box_tol))
                            {
                                isbox = false;
                            }
                            // return geom.info
                            if (isbox)
                            {
                                corner = new Point3D(pC.X,pC.Y,pC.Z);
                                Ex = Dx;
                                Ey = Dy;
                                Ez = norm*dists[0];
                            }
                        }
                    }

            return isbox;
        }

        // Test if a SolidWorks Body2 shape can be converted to a pure box.
        public static bool SWbodyToBox(Body2 swBody)
        {
            Point3D vfooc = new Point3D(0, 0, 0);
            Vector3D vfoox = new Vector3D(0, 0, 0);
            Vector3D vfooy = new Vector3D(0, 0, 0);
            Vector3D vfooz = new Vector3D(0, 0, 0);
            return SWbodyToBox(swBody, ref vfooc, ref vfoox, ref vfooy, ref vfooz);
        }



        // Try to see if the SolidWorks Body2 shape is a pure cylinder.
        // If so,return true. If not a cylinder, return false.
        // If it is a cylinder, in 'P1' and 'P2' and 'radius' one can get 
        // the two ends and the radius.

        public static bool SWbodyToCylinder(Body2 swBody,
                                          ref Point3D P1, ref Point3D P2,
                                          ref double radius)
        {
            bool iscyl = false;
        
            if (swBody.GetFaceCount() == 3)
                if (swBody.GetEdgeCount() == 2)
                {
                    object[] mfaces = (object[])swBody.GetFaces();
                    object[] medges = (object[])swBody.GetEdges();
                    object[] mverts = (object[])swBody.GetVertices();

                    int iplane = 0;
                    Surface surf_cyl = null;
                    Surface surf_planeA = null;
                    Surface surf_planeB = null;
                    for (int i = 0; i < 3; i++)
                    {
                        Face2 mface = (Face2)mfaces[i];
                        Surface msurf = (Surface)mface.GetSurface();
                        if (msurf.IsCylinder())
                        {
                            surf_cyl = msurf;
                        }
                        if (msurf.IsPlane())
                        {
                            if (iplane == 0) surf_planeA = msurf;
                            if (iplane == 1) surf_planeB = msurf;
                            iplane++;
                        }
                        if ((surf_cyl != null) && (surf_planeA != null) && (surf_planeB != null))
                        {
                            iscyl = true;
                            

                            double[] cylpar = (double[])surf_cyl.CylinderParams;
                            Vector3D cyl_C = new Vector3D(cylpar[0], cylpar[1], cylpar[2]);
                            Vector3D cyl_D = new Vector3D(cylpar[3], cylpar[4], cylpar[5]);
                            double   cyl_rad = cylpar[6];
                            double cyl_tol = (1e-6);

                            double[] pApar = (double[])surf_planeA.PlaneParams;
                            Vector3D pA_N = new Vector3D(pApar[0], pApar[1], pApar[2]);
                            Vector3D pA_C = new Vector3D(pApar[3], pApar[4], pApar[5]);

                            double[] pBpar = (double[])surf_planeB.PlaneParams;
                            Vector3D pB_N = new Vector3D(pBpar[0], pBpar[1], pBpar[2]);
                            Vector3D pB_C = new Vector3D(pBpar[3], pBpar[4], pBpar[5]);

                            // Rejective test 1: cyl axis & norms ofplanes are not aligned?
                            if ((Vector3D.CrossProduct(cyl_D, pA_N)).Length > cyl_tol)
                                iscyl = false;
                            if ((Vector3D.CrossProduct(cyl_D, pB_N)).Length > cyl_tol)
                                iscyl = false;

                            // return geom.info
                            if (iscyl)
                            {
                                radius = cyl_rad;
                                P1 = (Point3D)(cyl_C + cyl_D * (Vector3D.DotProduct((pA_C - cyl_C), cyl_D)));
                                P2 = (Point3D)(cyl_C + cyl_D * (Vector3D.DotProduct((pB_C - cyl_C), cyl_D)));
                            }
                        }
                        else
                            iscyl = false;
                    }
                }

            return iscyl;
        }

        // Test if a SolidWorks Body2 shape can be converted to a pure cylinder.
        public static bool SWbodyToCylinder(Body2 swBody)
        {
            double rad =0;
            Point3D pA = new Point3D(0, 0, 0);
            Point3D pB = new Point3D(0, 0, 0);
            return SWbodyToCylinder(swBody, ref pA, ref pB, ref rad);
        }




        // Try to see if the SolidWorks Body2 shape is a pure convex hull.
        // If so,return true. If not a convex hull, return false.
        // If it is a convex hull, in 'vertexes' get all the points.

        public static bool SWbodyToConvexHull(Body2 swBody,
                                          ref Point3D[] vertexes, int maxvertexes)
        {
            bool ishull = false;

            if (swBody.GetVertexCount() <= maxvertexes)
            {
                object[] mfaces = (object[])swBody.GetFaces();
                object[] medges = (object[])swBody.GetEdges();
                object[] mverts = (object[])swBody.GetVertices();

                ishull = true;

                // rejective test 1: are all faces as planes?
                bool allplanes = true;
                for (int i = 0; i < swBody.GetFaceCount(); i++)
                {
                    Face2 aface = (Face2)mfaces[i];
                    Surface msurf = (Surface)aface.GetSurface();
                    if (!msurf.IsPlane())
                    {
                        allplanes = false;
                    }
                }
                if (!allplanes) 
                    return false;

                // rejective test 2: are all edges as straight lines?
                bool allstraightedges = true;
                for (int i = 0; i < swBody.GetEdgeCount(); i++)
                {
                    Edge aedge = (Edge)medges[i];
                    Curve mcurve = (Curve)aedge.GetCurve();
                    if (!mcurve.IsLine())
                    {
                        allstraightedges = false;
                    }
                }
                if (!allstraightedges) 
                    return false;
                
                // rejective test 3: are there holes as in tori?
                // Use Euler formula  v + f - e = 2 -2*genus
                if (swBody.GetVertexCount() + swBody.GetFaceCount() - swBody.GetEdgeCount() != 2)
                    return false;

                if (ishull)
                {
                    vertexes = new Point3D[swBody.GetVertexCount()];
                    for (int ip = 0; ip < swBody.GetVertexCount(); ip++)
                    {
                        vertexes[ip] = new  Point3D(((double[])((Vertex)mverts[ip]).GetPoint())[0],
                                                    ((double[])((Vertex)mverts[ip]).GetPoint())[1],
                                                    ((double[])((Vertex)mverts[ip]).GetPoint())[2]);  
                    }
                }
            }

            return ishull;
        }

        // Test if a SolidWorks Body2 shape can be converted to a pure box.
        public static bool SWbodyToConvexHull(Body2 swBody, int maxvertexes)
        {
            Point3D[] vertexes = null;
            return SWbodyToConvexHull(swBody, ref vertexes, maxvertexes);
        }



    }
}
