// hacd_CLI.h

#pragma once

#include "hacdHACD.h"


bool LoadOFF(const std::string & fileName, std::vector< HACD::Vec3<HACD::Real> > & points, std::vector< HACD::Vec3<long> > & triangles, bool invert) 
{    
	FILE * fid = fopen(fileName.c_str(), "r");
	if (fid) 
    {
		const std::string strOFF("OFF");
		char temp[1024];
		fscanf(fid, "%s", temp);
		std::string stemp(temp);
		if (stemp.compare(strOFF)!=0)
		{
			printf( "Loading error: format not recognized \n");
            fclose(fid);

			return false;            
		}
		else
		{
			int nv = 0;
			int nf = 0;
			int ne = 0;
			fscanf(fid, "%i", &nv);
			fscanf(fid, "%i", &nf);
			fscanf(fid, "%i", &ne);
            points.resize(nv);
			triangles.resize(nf);
            HACD::Vec3<HACD::Real> coord;
			float x = 0;
			float y = 0;
			float z = 0;
			for (long p = 0; p < nv ; p++) 
            {
				fscanf(fid, "%f", &x);
				fscanf(fid, "%f", &y);
				fscanf(fid, "%f", &z);
				points[p].X() = x;
				points[p].Y() = y;
				points[p].Z() = z;
			}        
			int i = 0;
			int j = 0;
			int k = 0;
			int s = 0;
			for (long t = 0; t < nf ; ++t) {
				fscanf(fid, "%i", &s);
				if (s == 3)
				{
					fscanf(fid, "%i", &i);
					fscanf(fid, "%i", &j);
					fscanf(fid, "%i", &k);
					triangles[t].X() = i;
					if (invert)
					{
						triangles[t].Y() = k;
						triangles[t].Z() = j;
					}
					else
					{
						triangles[t].Y() = j;
						triangles[t].Z() = k;
					}
				}
				else			// Fix me: support only triangular meshes
				{
					for(long h = 0; h < s; ++h) fscanf(fid, "%i", &s);
				}
			}
            fclose(fid);
		}
	}
	else 
    {
		printf( "Loading error: file not found \n");
		return false;
    }
	return true;
}


using namespace System;


namespace hacd_CLI {

	public ref class Vect3D
	{
	public:
		Vect3D(double mX, double mY, double mZ)
		{
			X = mX;
			Y = mY;
			Z = mZ;
		}
		Vect3D()
		{
			X = 0;
			Y = 0;
			Z = 0;
		}
		double X;
		double Y;
		double Z;
	};

	public ref class Triangle
	{
	public:
		Triangle(int mp1, int mp2, int mp3)
		{
			p1 = mp1;
			p2 = mp2;
			p3 = mp3;
		}
		int p1;
		int p2;
		int p3;
	};


	void myCallBack(const char * msg, double progress, double concavity, size_t nVertices)
	{
		std::cout << msg;
	}


	public ref class hacd_CLI_wrapper
	{
	public:
		hacd_CLI_wrapper()  
		{
			heapManager = HACD::createHeapManager(65536*(1000));
			myHACD = HACD::CreateHACD(heapManager);
			points = new std::vector< HACD::Vec3<HACD::Real> >;
			triangles = new std::vector< HACD::Vec3<long> >;
		}

		~hacd_CLI_wrapper() { this->!hacd_CLI_wrapper(); }

		!hacd_CLI_wrapper() 
		{ 
			HACD::DestroyHACD(myHACD);
			HACD::releaseHeapManager(heapManager);
			delete points;
			delete triangles;
		}

			/// Preferences-setting functions:

		void SetVolumeWeight(double mval)
		{
			myHACD->SetVolumeWeight(mval);
		}

		void SetConnectDist(double mval)
		{
			myHACD->SetConnectDist(mval);
		}

		void SetMinNClusters(int mval)
		{
			myHACD->SetNClusters(mval);
		}

		void SetMaxNVerticesPerCH(int mval)
		{
			myHACD->SetNVerticesPerCH(mval);
		}

		void SetConcavity(double mval)
		{
			myHACD->SetConcavity(mval);
		}

		void SetSmallClusterThreshold(double mval)
		{
			myHACD->SetSmallClusterThreshold(mval);
		}

		void SetNTargetTrianglesDecimatedMesh(int mval)
		{
			myHACD->SetNTargetTrianglesDecimatedMesh(mval);
		}

		void SetAddExtraDistPoints(bool mval)
		{
			myHACD->SetAddExtraDistPoints(mval);
		}

		void SetAddFacesPoints(bool mval)
		{
			myHACD->SetAddFacesPoints(mval);
		}


			/// Pass the mesh data as an array of vertexes 
			/// and an array of triangles (indices of vertexes)
		void SetMesh(array<Vect3D^> ^managedpoints, array<Triangle^> ^managedtriangles)
		{
			// marshal array in:
			points->clear();
			for (int i = 0; i< managedpoints->GetLength(0); i++)
			{
				HACD::Vec3<HACD::Real> mpt(managedpoints[i]->X, managedpoints[i]->Y, managedpoints[i]->Z);
				points->push_back(mpt);
			}
			myHACD->SetPoints(&((*points)[0])); // (&points[0]));
			myHACD->SetNPoints(managedpoints->GetLength(0));

			// marshal array in:
			triangles->clear();
			for (int i = 0; i< managedtriangles->GetLength(0); i++)
			{
				HACD::Vec3<long> mtri(managedtriangles[i]->p1, managedtriangles[i]->p2, managedtriangles[i]->p3);
				triangles->push_back(mtri);
			}
			myHACD->SetTriangles(&((*triangles)[0]));
			myHACD->SetNTriangles(managedtriangles->GetLength(0));
		}


			/// Compute the convex decomposition!!!
			/// Call this function after having set the preferences and 
			/// after having used SetMesh(), and fetch results via GetNClusters() and GetClusterConvexHull()
		int Compute(bool fullCH, bool exportDistPoints) 
		{
			
			myHACD->Compute(fullCH, exportDistPoints);
			return myHACD->GetNClusters();
			
		}

		void Save(System::String^ mpath)
		{
			IntPtr p = System::Runtime::InteropServices::Marshal::StringToHGlobalAnsi(mpath);
			char *pNewCharStr = static_cast<char*>(p.ToPointer());
 
			myHACD->Save(pNewCharStr, false);

			System::Runtime::InteropServices::Marshal::FreeHGlobal(p);
		}

		void SetCompacityWeight(double mcomp)
		{
			myHACD->SetCompacityWeight(mcomp);
		}

			/// Get the number of computed convex hulls
		int GetNClusters()
		{
			return myHACD->GetNClusters();
		}

			/// Get the vertexes (and triangulation, if needed)
			/// of the Nth resulting convex hull.
		int GetClusterConvexHull(int ncluster, array<Vect3D^>^% managedpoints, array<Triangle^>^% managedtriangles)
		{
			size_t nPoints = myHACD->GetNPointsCH(ncluster);
			size_t nTriangles = myHACD->GetNTrianglesCH(ncluster);
			HACD::Vec3<HACD::Real> * pointsCH = new HACD::Vec3<HACD::Real>[nPoints];
			HACD::Vec3<long> * trianglesCH = new HACD::Vec3<long>[nTriangles];
			myHACD->GetCH(ncluster, pointsCH, trianglesCH);
			//marshal array out:
			Array::Resize(managedpoints,nPoints);
			for (int i=0;i<nPoints;i++)
				managedpoints[i] = gcnew Vect3D(pointsCH[i].X(), pointsCH[i].Y(), pointsCH[i].Z());
			//marshal array out:
			Array::Resize(managedtriangles,nTriangles);
			for (int i=0;i<nTriangles;i++)
				managedtriangles[i] = gcnew Triangle(trianglesCH[i].X(), trianglesCH[i].Y(), trianglesCH[i].Z());
			
			return ncluster;
		}

	private:
		HACD::HACD* myHACD;
		HACD::HeapManager * heapManager;	
		
		std::vector< HACD::Vec3<HACD::Real> >*	points;
		std::vector< HACD::Vec3<long> >*		triangles;

	};



}
