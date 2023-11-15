using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Scripting;

#if UNITY_EDITOR

    using UnityEditor;
    using UnityEditor.SceneManagement;

#endif

//************************************************************************************
//
//************************************************************************************

namespace GAME
{
    static public class Random
    {
        static private System.Random m_rand = new System.Random();

        static public int            seed      { set { m_rand = new System.Random( value ); } }
                                     
        static public System.Random  generator { get { return m_rand; } }
    }
}

//************************************************************************************
//
//************************************************************************************

static public class BoundsExtension
{
    [ Flags ] public enum AXIS { NONE = 0X0, X = 0X1, Y = 0X2, Z = 0X4, XY = 0X3, XZ = 0X5, YZ = 0X6, ALL = 0X7 }

    static public bool OverLap( this ref Bounds bnd, ref Bounds other, int axis )
    {
        int overlap = 0X7;

        if( ( ( axis & 1 ) == 0 ) || ( bnd.min.x > other.max.x ) || ( bnd.max.x < other.min.x ) ) overlap &= ~1;

        if( ( ( axis & 2 ) == 0 ) || ( bnd.min.y > other.max.y ) || ( bnd.max.y < other.min.y ) ) overlap &= ~2;

        if( ( ( axis & 4 ) == 0 ) || ( bnd.min.z > other.max.z ) || ( bnd.max.z < other.min.z ) ) overlap &= ~4;

        return overlap == axis;
    }

    static public Bounds Inset( this ref Bounds bnd, float inset )
    {
        Vector3 sze = bnd.size;

        return new Bounds( bnd.center, new Vector3( Mathf.Max( 0.0f, sze.x - inset ), Mathf.Max( 0.0f, sze.y - inset ), Mathf.Max( 0.0f, sze.z - inset ) ) );
    }
}

//************************************************************************************
//
//************************************************************************************

static public class MeshUtility
{
    //********************************************************************************
    //
    //********************************************************************************

    static public void CombineAdd( IList< CombineInstance > dst, int sub_mesh_ind, Mesh msh, Vector3 pos, float rot, Vector3 scl )
    {
        if( dst != null )
        {
            CombineInstance cmb = new CombineInstance();

            Matrix4x4       mtx = new Matrix4x4();

            mtx.SetTRS( pos, Quaternion.AngleAxis( rot, Vector3.up ), scl );


            cmb.mesh         = msh;
                          
            cmb.transform    = mtx;

            cmb.subMeshIndex = sub_mesh_ind;

            dst.Add( cmb );
        }
    }

    //********************************************************************************
    //
    //********************************************************************************

    static public Vector3[] CalcNormals( IList< Vector3 > verts, IList< int > tris )
    {
        Vector3[] normals = new Vector3[ verts.Count ];

        int nb_verts = ( verts != null ) ? verts.Count : 0;

        int nb_ind   = ( tris  != null ) ? tris.Count  : 0;

        if( nb_verts <= 0 ) return normals;

        if( nb_ind   <= 0 ) return normals;


        int     v0, v1, v2;

        Vector3 e0, e1, n;

        for( int t = 0; t < nb_ind; t += 3 )
        {
            v0 = tris[ t + 0 ]; v1 = tris[ t + 1 ]; v2 = tris[ t + 2 ];

            if( v0 >= nb_verts ) continue;

            if( v1 >= nb_verts ) continue;

            if( v2 >= nb_verts ) continue;

            e0   = verts[ v1 ] - verts[ v0 ];
                         
            e1   = verts[ v2 ] - verts[ v0 ];

            n    = Vector3.Cross( e0, e1 ).normalized;

            normals[ v0 ] = n;

            normals[ v1 ] = n;

            normals[ v2 ] = n;
        }

        return normals;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static public void WeldVertices( Mesh mesh, float dist )
    {
        if( mesh == null ) return;

        int        sub_meshes = mesh.subMeshCount;

        float        sqr_dist = dist * dist;

        List< Vector3 > verts = new List< Vector3 >( mesh.vertices  );

        List< int     > tris  = new List< int     >( mesh.triangles );

        for( int v = 0; v < verts.Count; )
        {
            bool    nxt  = true;

            Vector3 vert = verts[ v ];

            for( int o = v + 1; o < verts.Count; )
            {
                Vector3 other = verts[ o ];

                if( ( vert - other ).sqrMagnitude <= sqr_dist )
                {
                    for( int t = 0; t < tris.Count; ++t )
                    { 
                        if     ( tris[ t ] >  o ) tris[ t ] = tris[ t ] - 1; 

                        else if( tris[ t ] == o ) tris[ t ] = v; 
                    }

                    for( int sub = 0; sub < sub_meshes; ++sub )
                    {
                        UnityEngine.Rendering.SubMeshDescriptor desc = mesh.GetSubMesh( sub );

                        if     ( desc.baseVertex >  o ) { desc.baseVertex = desc.baseVertex - 1;         mesh.SetSubMesh( sub, desc, ( UnityEngine.Rendering.MeshUpdateFlags )0XF ); }

                        else if( desc.baseVertex == o ) { desc.baseVertex = desc.baseVertex - ( o - v ); mesh.SetSubMesh( sub, desc, ( UnityEngine.Rendering.MeshUpdateFlags )0XF ); }
                    }

                    verts.RemoveAt( o );
                    
                    if( o == v + 1 ) nxt = false;
                }
                else
                {
                    ++o;
                }
            }

            if( nxt ) ++v;
        }

        mesh.triangles = tris.ToArray ();

        mesh.vertices  = verts.ToArray();
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private bool TriContainsVertice( int[] tris, int t, int v )
    {
        if( v == tris[ t     ] ) return true;
                    
        if( v == tris[ t + 1 ] ) return true;
                    
        if( v == tris[ t + 2 ] ) return true;

        return false;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private bool UV_FindNextEdgeLoopVertex( Vector3[] verts, int[] tris, Vector2[] uvs, int v_bgn, ref int v_prv, ref int v_cur )
    {
        Vector3 pos_cur = verts[ v_cur ];

        for( int t = 0, count = tris.Length; t < count; t += 3 )
        {
            if( TriContainsVertice( tris, t, v_cur ) )
            {
                for( int i = 0; i < 3; ++i )
                {
                    int     v_nxt   = tris [ t + i ];

                    Vector3 pos_nxt = verts[ v_nxt ];

                    if( pos_cur.y == pos_nxt.y )
                    {
                        if( ( uvs[ v_nxt ].x == float.MinValue ) || ( ( v_nxt == v_bgn ) && ( v_nxt != v_prv ) ) )
                        {
                            v_prv = v_cur; v_cur = v_nxt; return true;
                        }
                    }
                }
            }
        }

        return false;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static public void GenerateUVs( Mesh mesh, float world_unit_to_uv )
    {
        if( mesh  == null ) return;

        int[]     tris  = mesh.triangles;
                        
        Vector3[] verts = mesh.vertices;
                        
        Vector2[] UVs   = new Vector2[ verts.Length ];

        Vector2   NAS   = new Vector2( float.MinValue, float.MinValue );

        for( int uv = 0, count = UVs.Length; uv < count; ++uv ) UVs[ uv ] = NAS;


        int     v0, v1, v2;

        Vector3 V0, V1, V2;

        for( int t = 0, count = tris.Length; t < count; t += 3 )
        {
            v0 = tris [ t  ]; v1 = tris [ t + 1 ]; v2 = tris [ t + 2 ];

            V0 = verts[ v0 ]; 
            
            V1 = verts[ v1 ]; 
                           
            V2 = verts[ v2 ];
            
            if( UVs[ v0 ].y == float.MinValue ) UVs[ v0 ].y = V0.y * world_unit_to_uv; 

            if( UVs[ v1 ].y == float.MinValue ) UVs[ v1 ].y = V1.y * world_unit_to_uv; 

            if( UVs[ v2 ].y == float.MinValue ) UVs[ v2 ].y = V2.y * world_unit_to_uv;
        }
        

        while( true )
        {
            int edge_loop = -1;

            for( int t = 0, count = tris.Length; t < count; ++t )
            {
                v0 = tris[ t ];

                if( verts[ v0 ].y > 0.5f ) continue;

                if( UVs[ v0 ].x == float.MinValue )
                {
                    UVs[ v0 ].x = verts[ v0 ].x * world_unit_to_uv;

                    edge_loop = v0;

                    break;
                }
            }

            if( edge_loop <  0 ) break;
        
            if( edge_loop >= 0 )
            {
                int  v_bgn         = edge_loop;
                                   
                int  v_cur         = edge_loop;
                                   
                int  v_prv         = edge_loop;
                                   
                int  watch_dog     = 0;
                                   
                bool loop_complete = false;

                while( UV_FindNextEdgeLoopVertex( verts, tris, UVs, v_bgn, ref v_prv, ref v_cur ) && ( ++watch_dog < 16 ) )
                {
                    V0 = verts[ v_prv ];

                    V1 = verts[ v_cur ];

                    if( v_cur != v_bgn )
                    {
                        if( V1.x != V0.x ) UVs[ v_cur ].x = UVs[ v_prv ].x + ( ( V1 - V0 ).magnitude * Mathf.Sign( ( V1 - V0 ).x ) * world_unit_to_uv );

                        else               UVs[ v_cur ].x = UVs[ v_prv ].x + ( ( V1 - V0 ).magnitude * Mathf.Sign( ( V1 - V0 ).z ) * world_unit_to_uv );
                    }

                    for( int v = 0, count = verts.Length; v < count; ++v )
                    {
                        Vector3 oth_pos = verts[ v ];

                        if( ( V1.x == oth_pos.x ) && ( V1.z == oth_pos.z ) && ( V1.y != oth_pos.y ) ) 
                        {
                            UVs[ v ].x = UVs[ v_cur ].x;

                            break;
                        }
                    }

                    if( v_cur == v_bgn ) { loop_complete = true; break; }
                }

                if( loop_complete == false ) Debug.LogWarning( "Incomplete edge loop" );

                Debug.Assert( watch_dog < 16, "Infinite edge loop iteration prevented at iteration " + watch_dog );
            }
        }

        mesh.uv = UVs;
    }
}

//************************************************************************************
//
//************************************************************************************

public class LevelGenerator : MonoBehaviour
{
    //********************************************************************************
    //
    //********************************************************************************

    private const string DEF_ROOM_NAME  = "Room";

    private const string DEF_FLOOR_NAME = "Floor";

    //********************************************************************************
    //
    //********************************************************************************

    private struct Range< T > where T : IComparable 
    { 
        public readonly T min, max; 
        
        public Range( T param_min, T param_max ) { min = param_min; max = param_max; } 
        
        public bool encompass( T val ) { return ( val.CompareTo( min ) >= 0 ) && ( val.CompareTo( max ) <= 0 ); } 
    }

    static private readonly Range< float > DOORS_WIDTH     = new Range< float >( 2.0f, 4.0f );

    static private readonly Range< float > WALLS_THICKNESS = new Range< float >( 1.0f, 2.0f );

    static private readonly Range< int   > ROOMS           = new Range< int   >( 1, 20 );

    private const float                    ROOMS_HEIGHT    = 4.0f;

    //********************************************************************************
    //
    //********************************************************************************

    static private Shader   DEF_SHADER   = null;
                                         
    static private Material DEF_MATERIAL = null;

    static private void LoadDefaultMaterial()
    {
        if( DEF_SHADER   == null ) DEF_SHADER   = Shader.Find ( "Standard" );
                                        
        if( DEF_MATERIAL == null ) DEF_MATERIAL = new Material( DEF_SHADER );
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private Mesh BLOCK = null;

    static private void BuildBlockPreset( ref Mesh mesh )
    {
        if( mesh != null ) return;
        
        mesh = new Mesh();

        Vector3[] verts = new Vector3[ 8  ];

        int    [] tris  = new int    [ 30 ];

        verts[ 0 ] = new Vector3( -0.5f, 1.0f,  0.0f ); 
        verts[ 1 ] = new Vector3(  0.5f, 1.0f,  0.0f );
        verts[ 2 ] = new Vector3(  0.5f, 1.0f, -1.0f );
        verts[ 3 ] = new Vector3( -0.5f, 1.0f, -1.0f );
        verts[ 4 ] = new Vector3( -0.5f, 0.0f,  0.0f ); 
        verts[ 5 ] = new Vector3(  0.5f, 0.0f,  0.0f );
        verts[ 6 ] = new Vector3(  0.5f, 0.0f, -1.0f );
        verts[ 7 ] = new Vector3( -0.5f, 0.0f, -1.0f );

        // BULK

        tris [ 0  ] = 0; tris [ 1  ] = 1; tris [ 2  ] = 2;
        tris [ 3  ] = 2; tris [ 4  ] = 3; tris [ 5  ] = 0;
        tris [ 6  ] = 3; tris [ 7  ] = 2; tris [ 8  ] = 6;
        tris [ 9  ] = 6; tris [ 10 ] = 7; tris [ 11 ] = 3;
        tris [ 12 ] = 1; tris [ 13 ] = 0; tris [ 14 ] = 4;
        tris [ 15 ] = 4; tris [ 16 ] = 5; tris [ 17 ] = 1;

        // L CAP

        tris [ 18 ] = 0; tris [ 19 ] = 3; tris [ 20 ] = 7;
        tris [ 21 ] = 7; tris [ 22 ] = 4; tris [ 23 ] = 0;

        // R CAP

        tris [ 24 ] = 2; tris [ 25 ] = 1; tris [ 26 ] = 5;
        tris [ 27 ] = 5; tris [ 28 ] = 6; tris [ 29 ] = 2;

        mesh.vertices  = verts;

        mesh.triangles = tris;

        mesh.normals   = MeshUtility.CalcNormals( verts, tris );
    }

    //********************************************************************************
    //
    //********************************************************************************

    [ Flags ] public enum PINCH { NONE = 0X0, L= 0X1, R = 0X2, LR = 0X3 }

    static private Mesh AdjustBlock( Mesh mesh, Vector3 sze, PINCH mask )
    {
        Mesh block = new Mesh();
        
        if( mesh != null )
        {
            Vector3[] verts   = mesh.vertices;

            int[]     tris    = mesh.triangles;

            Vector3[] normals = mesh.normals;

            Vector3 pinch_ofs = Vector3.right * sze.z;

            float   extent_x  = sze.x * 0.5f;


            verts[ 0 ].y =  sze.y; verts[ 1 ].y =  sze.y; verts[ 2 ].y =  sze.y; verts[ 3 ].y =  sze.y;

            verts[ 2 ].z = -sze.z; verts[ 3 ].z = -sze.z; verts[ 7 ].z = -sze.z; verts[ 6 ].z = -sze.z;


            verts[ 0 ].x = -extent_x; verts[ 3 ].x = -extent_x; verts[ 4 ].x = -extent_x; verts[ 7 ].x = -extent_x;

            verts[ 1 ].x =  extent_x; verts[ 2 ].x =  extent_x; verts[ 5 ].x =  extent_x; verts[ 6 ].x =  extent_x;


            if( ( mask & PINCH.R ) != 0 ) 
            { 
                verts[ 2 ] -= pinch_ofs; verts[ 6 ] -= pinch_ofs; 

                Array.Resize( ref tris, tris.Length - 6 );
            }

            if( ( mask & PINCH.L ) != 0 ) 
            { 
                verts[ 3 ] += pinch_ofs; verts[ 7 ] += pinch_ofs; 

                if( ( mask & PINCH.R ) == 0 ) 
                {
                    for( int t = 18; t <= 23; ++t ) tris[ t ] = tris[ t + 6 ];
                }

                Array.Resize( ref tris, tris.Length - 6 );
            }

            block.vertices  = verts;

            block.triangles = tris;

            block.normals   = normals;
        }

        return block;
    }

    //********************************************************************************
    //
    //********************************************************************************

    static private Mesh CreateFloor( Bounds bnd )
    {
        Mesh mesh = new Mesh();

        Vector3[] verts = new Vector3[ 4  ];

        int    [] tris  = new int    [ 6 ];

        verts[ 0 ] = new Vector3( -bnd.extents.x, 0.0f,  bnd.extents.z ); 
        verts[ 1 ] = new Vector3(  bnd.extents.x, 0.0f,  bnd.extents.z );
        verts[ 2 ] = new Vector3(  bnd.extents.x, 0.0f, -bnd.extents.z );
        verts[ 3 ] = new Vector3( -bnd.extents.x, 0.0f, -bnd.extents.z );

        // BULK

        tris [ 0  ] = 0; tris [ 1  ] = 1; tris [ 2  ] = 2;
        tris [ 3  ] = 2; tris [ 4  ] = 3; tris [ 5  ] = 0;

        mesh.vertices  = verts;

        mesh.triangles = tris;

        mesh.normals   = MeshUtility.CalcNormals( verts, tris );

        return mesh;
    }

    //********************************************************************************
    //
    //********************************************************************************

    private int   m_max_rooms       = ROOMS.min;

    private float m_walls_thickness = WALLS_THICKNESS.min;

    private float m_doors_width     = DOORS_WIDTH.min;

    //********************************************************************************
    //
    //********************************************************************************

    [ Serializable ] private class RoomBluePrint
    {
        [ SerializeField ] public Bounds                         bnd;

        [ SerializeField ] public readonly List< DoorBluePrint > doors = new List< DoorBluePrint >( 4 );

        public RoomBluePrint( Bounds param_bnd ) { bnd = param_bnd; }
    }

    [ Serializable ]private class DoorBluePrint 
    {
        [ SerializeField ] public Vector3 pos;

        [ SerializeField ] public Vector3 dir;
    }

    [ SerializeField ] private bool                  m_weld_vertices    = true;

    [ SerializeField ] private float                 m_world_unit_to_uv = 0.25f;

    [ SerializeField ] private Material              m_wall_material    = null;

    [ SerializeField ] private Material              m_floor_material   = null;

    [ NonSerialized  ] private List< RoomBluePrint > m_rooms_shelf      = new List< RoomBluePrint >( ROOMS.max );
                                                                        
    [ SerializeField ] private List< RoomBluePrint > m_rooms            = new List< RoomBluePrint >( ROOMS.max );
                                                                        
    [ SerializeField ] private List< DoorBluePrint > m_doors            = new List< DoorBluePrint >( ROOMS.max << 2 );
                                                                        
    [ SerializeField ] private Transform             m_level            = null;

    //********************************************************************************
    //
    //********************************************************************************

    private void CreateShelf()
    {
        m_rooms_shelf.Clear();

        Vector3 y_ofs = Vector3.up * ( ROOMS_HEIGHT * 0.5f );

        while( m_rooms_shelf.Count < m_max_rooms )
        {
            int   size  = ( Mathf.CeilToInt( m_walls_thickness ) << 1 ) + 1 + Mathf.CeilToInt( m_doors_width );

            float width = GAME.Random.generator.Next( size, size << 3 );

            float depth = GAME.Random.generator.Next( size, size << 3 );

            m_rooms_shelf.Add( new RoomBluePrint( new Bounds( y_ofs, new Vector3( width, ROOMS_HEIGHT, depth ) ) ) );
        }
    }

    //********************************************************************************
    //
    //********************************************************************************

    private bool RoomOverlap( RoomBluePrint room, RoomBluePrint ignore = null )
    {
        for( int r = 0, count = m_rooms.Count; r < count; ++r )
        {
            RoomBluePrint other = m_rooms[ r ];

            if( ( ignore != null ) && ( ignore == other ) ) continue;

            if( room.bnd.OverLap( ref other.bnd, ( int )BoundsExtension.AXIS.XZ ) ) return true;
        }

        return false;
    }

    //********************************************************************************
    //
    //********************************************************************************

    private void PlaceRoom( RoomBluePrint room )
    {
        if( m_rooms.Count <= 0 ) { m_rooms.Add( room ); return; }


        RoomBluePrint anchor = null;

        Vector3       ofs    = Vector3.zero;

        int           start  = GAME.Random.generator.Next( ( m_rooms.Count >> 1 ), ( m_rooms.Count >> 1 ) + m_rooms.Count );

        for( int r = 0, count = m_rooms.Count; r < count; ++r )
        {
            RoomBluePrint other = m_rooms[ ( r + start ) % m_rooms.Count ];

            Vector3       sep   = other.bnd.extents + room.bnd.extents;

            ofs = new Vector3(   0.0f, 0.0f,  sep.z ); room.bnd.center = other.bnd.center + ofs; if( RoomOverlap( room, other ) == false ) { anchor = other; break; }
                                                                
            ofs = new Vector3(   0.0f, 0.0f, -sep.z ); room.bnd.center = other.bnd.center + ofs; if( RoomOverlap( room, other ) == false ) { anchor = other; break; }

            ofs = new Vector3(  sep.x, 0.0f,   0.0f ); room.bnd.center = other.bnd.center + ofs; if( RoomOverlap( room, other ) == false ) { anchor = other; break; }
                                                                              
            ofs = new Vector3( -sep.x, 0.0f,   0.0f ); room.bnd.center = other.bnd.center + ofs; if( RoomOverlap( room, other ) == false ) { anchor = other; break; }
        }

        m_rooms.Add( room );

        if( anchor != null ) 
        {
            DoorBluePrint door = new DoorBluePrint();

            door.dir = ( anchor.bnd.center - room.bnd.center ).normalized;

            if( ofs.x != 0.0f ) door.pos = anchor.bnd.center + new Vector3( anchor.bnd.extents.x * Mathf.Sign( ofs.x ), 0.0f, 0.0f );

            else                door.pos = anchor.bnd.center + new Vector3( 0.0f, 0.0f, anchor.bnd.extents.z * Mathf.Sign( ofs.z ) );

            m_doors.Add     ( door );

            room.doors.Add  ( door );

            anchor.doors.Add( door );
        }
    }

    //********************************************************************************
    //
    //********************************************************************************

    private void PlaceRooms()
    {
        m_rooms.Clear();

        m_doors.Clear();

        while( m_rooms_shelf.Count > 0 )
        {
            PlaceRoom( m_rooms_shelf[ m_rooms_shelf.Count - 1 ] );

            m_rooms_shelf.RemoveAt  ( m_rooms_shelf.Count - 1 );
        }
    }

    //********************************************************************************
    //
    //********************************************************************************

    private void CreateBlueprints()
    {
        CreateShelf();

        PlaceRooms ();
    }

    //********************************************************************************
    //
    //********************************************************************************

    private void BuildRoomGeometry( RoomBluePrint room, Mesh mesh )
    {
        List< CombineInstance > meshes = new List< CombineInstance >();

        bool T_DOOR = false, B_DOOR = false, L_DOOR = false, R_DOOR = false;

        for( int d = 0, count = room.doors.Count; d < count; ++d )
        {
            DoorBluePrint door = room.doors[ d ];

            if( door.dir.z != 0.0f ) { if( door.pos.z > room.bnd.center.z ) T_DOOR = true; else B_DOOR = true; }

            else                     { if( door.pos.x > room.bnd.center.x ) R_DOOR = true; else L_DOOR = true; }
        }

        float   door_half_w = m_doors_width * 0.5f;

        Vector3 door_ofs_x  = Vector3.right   * ( ( room.bnd.extents.x + door_half_w ) * 0.5f );

        Vector3 door_ofs_z  = Vector3.forward * ( ( room.bnd.extents.z + door_half_w ) * 0.5f );

        Vector3 x_ofs       = Vector3.right   * room.bnd.extents.x;
                            
        Vector3 z_ofs       = Vector3.forward * room.bnd.extents.z;

        if( T_DOOR == false ) MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.size.x,                  ROOMS_HEIGHT, m_walls_thickness ), PINCH.LR ),  z_ofs,                0.0f, Vector3.one );
                                                                                                                                                                                         
        else                { MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.extents.x - door_half_w, ROOMS_HEIGHT, m_walls_thickness ), PINCH.L  ),  z_ofs - door_ofs_x,   0.0f, Vector3.one );
                                                                                                                                                                                            
                              MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.extents.x - door_half_w, ROOMS_HEIGHT, m_walls_thickness ), PINCH.R  ),  z_ofs + door_ofs_x,   0.0f, Vector3.one ); }
                                                                                                                                                                                  
        if( R_DOOR == false ) MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.size.z,                  ROOMS_HEIGHT, m_walls_thickness ), PINCH.LR ),  x_ofs,               90.0f, Vector3.one );
                                                                                                                                                                                           
        else                { MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.extents.z - door_half_w, ROOMS_HEIGHT, m_walls_thickness ), PINCH.L  ),  x_ofs + door_ofs_z,  90.0f, Vector3.one );
                                                                                                                                                                                           
                              MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.extents.z - door_half_w, ROOMS_HEIGHT, m_walls_thickness ), PINCH.R  ),  x_ofs - door_ofs_z,  90.0f, Vector3.one ); }
                                                                                                                                                                                  
        if( B_DOOR == false ) MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.size.x,                  ROOMS_HEIGHT, m_walls_thickness ), PINCH.LR ), -z_ofs,              180.0f, Vector3.one );
                                                                                                                                                                                  
        else                { MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.extents.x - door_half_w, ROOMS_HEIGHT, m_walls_thickness ), PINCH.R  ), -z_ofs - door_ofs_x, 180.0f, Vector3.one );
                                                                                                                                                                                  
                              MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.extents.x - door_half_w, ROOMS_HEIGHT, m_walls_thickness ), PINCH.L  ), -z_ofs + door_ofs_x, 180.0f, Vector3.one ); }
                                                                                                                                                                                  
        if( L_DOOR == false ) MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.size.z,                  ROOMS_HEIGHT, m_walls_thickness ), PINCH.LR ), -x_ofs,              -90.0f, Vector3.one );
                                                                                                                                                                                          
        else                { MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.extents.z - door_half_w, ROOMS_HEIGHT, m_walls_thickness ), PINCH.L  ), -x_ofs - door_ofs_z, -90.0f, Vector3.one );
                                                                                                                                                                                                  
                              MeshUtility.CombineAdd( meshes, 0, AdjustBlock( BLOCK, new Vector3( room.bnd.extents.z - door_half_w, ROOMS_HEIGHT, m_walls_thickness ), PINCH.R  ), -x_ofs + door_ofs_z, -90.0f, Vector3.one ); }

        mesh.CombineMeshes( meshes.ToArray(), true, true );



        if( m_weld_vertices ) MeshUtility.WeldVertices( mesh, 0.01f );

        mesh.RecalculateTangents( UnityEngine.Rendering.MeshUpdateFlags.DontRecalculateBounds );

        mesh.RecalculateNormals ( UnityEngine.Rendering.MeshUpdateFlags.DontRecalculateBounds );

        MeshUtility.GenerateUVs ( mesh, m_world_unit_to_uv );

        mesh.RecalculateBounds  ();
    }

    //********************************************************************************
    //
    //********************************************************************************

    private Mesh BuildFloorGeometry( RoomBluePrint room )
    {
        Mesh mesh = CreateFloor( room.bnd );

        mesh.RecalculateTangents( UnityEngine.Rendering.MeshUpdateFlags.DontRecalculateBounds );

        mesh.RecalculateNormals ( UnityEngine.Rendering.MeshUpdateFlags.DontRecalculateBounds );

        Vector3[] verts = mesh.vertices;

        Vector2[] UVs   = new Vector2[ 4 ];

            UVs[ 0 ] = Vector2.up                     * ( verts[ 0 ] - verts[ 3 ] ).magnitude * m_world_unit_to_uv;

            UVs[ 1 ] = UVs[ 0 ] + Vector2.right      *  ( verts[ 1 ] - verts[ 0 ] ).magnitude * m_world_unit_to_uv;

            UVs[ 2 ] = Vector2.right                  * ( verts[ 2 ] - verts[ 3 ] ).magnitude * m_world_unit_to_uv;

            UVs[ 3 ] = Vector2.zero;

        mesh.uv = UVs;

        mesh.RecalculateBounds  ();

        return mesh;
    }

    //********************************************************************************
    //
    //********************************************************************************

    private void BuildRoomMesh( RoomBluePrint room )
    {
        if( m_level == null )  return;

        BuildBlockPreset( ref BLOCK );
         
        string mesh_name_room  = DEF_ROOM_NAME  + m_level.childCount.ToString();

        string mesh_name_floor = DEF_FLOOR_NAME + m_level.childCount.ToString();


        GameObject room_obj = new GameObject( mesh_name_room, typeof( MeshFilter ), typeof( MeshRenderer ), typeof( MeshCollider ) );

        room_obj.transform.SetParent( m_level, false );

        room_obj.transform.localRotation = Quaternion.identity;
                                      
        room_obj.transform.localPosition = room.bnd.center - ( Vector3.up * room.bnd.extents.y );

        room_obj.transform.localScale    = Vector3.one;


        MeshFilter   mesh_filter   = room_obj.GetComponent< MeshFilter   >();

        MeshRenderer mesh_renderer = room_obj.GetComponent< MeshRenderer >();

        MeshCollider mesh_collider = room_obj.GetComponent< MeshCollider >();

        if( mesh_renderer != null ) 
        {
            mesh_renderer.sharedMaterials = new Material[] { ( m_wall_material != null ) ? m_wall_material : DEF_MATERIAL };
        }

        if( mesh_filter   != null )
        {
            Mesh mesh = new Mesh();

            mesh.name = mesh_name_room;

            BuildRoomGeometry( room, mesh );

            mesh_filter.sharedMesh = mesh;
        }

        if( mesh_collider != null ) mesh_collider.sharedMesh = mesh_filter.sharedMesh;




        GameObject floor_obj = new GameObject( mesh_name_floor, typeof( MeshFilter ), typeof( MeshRenderer ), typeof( MeshCollider ) );

        floor_obj.transform.SetParent( room_obj.transform, false );

        floor_obj.transform.localRotation = Quaternion.identity;
                                      
        floor_obj.transform.localPosition = Vector3.zero;

        floor_obj.transform.localScale    = Vector3.one;


        mesh_filter   = floor_obj.GetComponent< MeshFilter   >();

        mesh_renderer = floor_obj.GetComponent< MeshRenderer >();

        mesh_collider = floor_obj.GetComponent< MeshCollider >();

        if( mesh_renderer != null ) 
        {
            mesh_renderer.sharedMaterials = new Material[] { ( m_floor_material != null ) ? m_floor_material : DEF_MATERIAL };
        }

        if( mesh_filter   != null )
        {
            Mesh mesh = BuildFloorGeometry( room );

            mesh.name = mesh_name_floor;

            mesh_filter.sharedMesh = mesh;
        }

        if( mesh_collider != null ) mesh_collider.sharedMesh = mesh_filter.sharedMesh;
    }

    //********************************************************************************
    //
    //********************************************************************************

    private void BuildLevel()
    {
        if( m_level == null ) return;

        for( int r = 0, count = m_rooms.Count; r < count; ++r )
        {
            RoomBluePrint room = m_rooms[ r ];

            BuildRoomMesh( room );
        }
    }

    //********************************************************************************
    //
    //********************************************************************************

    public void Generate( int max_rooms, float walls_thickness, float doors_width )
    {
        LoadDefaultMaterial();

        Dissolve();

        m_max_rooms       = Mathf.Clamp( max_rooms,       ROOMS.min,           ROOMS.max           );

        m_walls_thickness = Mathf.Clamp( walls_thickness, WALLS_THICKNESS.min, WALLS_THICKNESS.max );

        m_doors_width     = Mathf.Clamp( doors_width,     DOORS_WIDTH.min,     DOORS_WIDTH.max     );

        CreateBlueprints();

        BuildLevel();

        MarkSceneDirty();
    }

    //********************************************************************************
    //
    //********************************************************************************

    public void GenerateUVs()
    {
        if( m_level != null )
        {
            for( int chld = 0, count = m_level.childCount; chld < count; ++chld )
            {
                Transform  child  = m_level.GetChild( chld );

                MeshFilter filter = child.GetComponent< MeshFilter >();

                Mesh       mesh   = ( filter != null ) ? filter.sharedMesh : null;

                if( mesh != null )
                {
                    MeshUtility.GenerateUVs( mesh, m_world_unit_to_uv );
                }
            }
        }
    }

    //********************************************************************************
    //
    //********************************************************************************

    public void Dissolve()
    {
        m_rooms_shelf.Clear();

        m_rooms.Clear();

        m_doors.Clear();

        if( m_level != null )
        {
            while( m_level.childCount > 0 ) 
            {
                Transform child = m_level.GetChild( m_level.childCount - 1 );

                child.parent    = null;

                DestroyImmediate( child.gameObject );
            }
        }
    }

    //********************************************************************************
    //
    //********************************************************************************

    [ Preserve ] private void MarkSceneDirty()
    {
        #if UNITY_EDITOR

        //EditorSceneManager.MarkSceneDirty( EditorSceneManager.GetActiveScene() );

        #endif
    }

    //********************************************************************************
    //
    //********************************************************************************

//#if UNITY_EDITOR

//    private void OnDrawGizmos()
//    {
//        return;
        
//        Color restore = Gizmos.color;

//        for( int r = 0, count = m_rooms.Count; r < count; ++r )
//        {
//            RoomBluePrint room   = m_rooms[ r ];

//            ref Bounds    bounds = ref room.bnd;

//            Gizmos.color = Color.cyan; Gizmos.DrawWireCube( bounds.center, bounds.size );

//            Gizmos.color = Color.red;  Gizmos.DrawCube    ( bounds.center, Vector3.one );
//        }

//        Gizmos.color  = Color.green;

//        for( int d = 0; d < m_doors.Count; ++d )
//        {
//            DoorBluePrint door = m_doors[ d ];

//            Gizmos.DrawCube( door.pos, new Vector3( Mathf.Abs( door.dir.z ) * m_doors_width, ROOMS_HEIGHT, Mathf.Abs( door.dir.x ) * m_doors_width ) );
//        }

//        Gizmos.color = restore;
//    }
//#endif
}
