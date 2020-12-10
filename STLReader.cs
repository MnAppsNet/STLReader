class STLReader
{
    //File :
    FileStream fileStream;
    BinaryReader file;

    //File contents :
    private string header; //(80 char header)
    private UInt32 triangle_number; // Number of triangles
    private float[][,] triangles;

    //Get file contents :
    public float[][,] GetHeader(){
        return header;
    }
    public float[][,] GetTriangles(){
        return triangles;
    }

    public STLReader(string filePath) //Constructor
    {
        fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        file = new BinaryReader(fileStream);
    }

    public bool ReadSTL() //If the file is read successfully returns true
    {
        if (fileStream == null || file == null)
            return false;

        header = null;
        triangle_number = 0;
        triangles = null;

        header = Encoding.UTF8.GetString(file.ReadBytes(80)); //80 8-bit chars
        triangle_number = file.ReadUInt32(); //1 32-bit unassigned integer


        triangles = new float[triangle_number][,];
        for (int i = 0; i < triangle_number; i++)
        {
            ////                            // X1 Y1 Z1  <-- Point 1 \
            triangles[i] = new float[3, 3]; // X2 Y2 Z2  <-- Point 2  = > One Triangle
            ////                            // X3 Y3 Z3  <-- Point 3 /

            //Get the 3 triangle points :
            for (int ix = 0; ix < 3; ix++) //For each point
                for (int iy = 0; iy < 3; iy++) //For each dimension
                {
                    triangles[i][ix, iy] = System.BitConverter.ToSingle(file.ReadBytes(4), 0); // 4 byte - 32 bit float number
                }
                UInt16 attr_byte_count = file.ReadUInt16(); //16-bit integer - attribute byte count, never used
        }
        DisposeFile();
        return true;
    }

    public void DisposeFile() //Dispose stream and release the file
    {
        fileStream.Close();
        file.Close();
        file = null;
        fileStream = null;
    }



}