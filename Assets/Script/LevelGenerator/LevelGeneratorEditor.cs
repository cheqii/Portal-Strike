#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[ CustomEditor( typeof( LevelGenerator ) ) ]

public class LevelGeneratorEditor : Editor
{
    private const string STR_GENERATE   = "GENERATE";

    private const string STR_REBUILD_UV = "REBUILD UVS";

    private const string STR_DISSOLVE   = "DISSOLVE";

    private enum CMD { NONE, GENERATE, REBUILD_UVS, DISSOLVE }

    public override void OnInspectorGUI()
    {
        LevelGenerator gen = target as LevelGenerator;

        CMD            cmd = CMD.NONE;

        if( GUI.Button( EditorGUILayout.GetControlRect( false, 32.0f ), STR_GENERATE   ) ) cmd = CMD.GENERATE;

        if( GUI.Button( EditorGUILayout.GetControlRect( false, 32.0f ), STR_REBUILD_UV ) ) cmd = CMD.REBUILD_UVS;

        if( GUI.Button( EditorGUILayout.GetControlRect( false, 32.0f ), STR_DISSOLVE   ) ) cmd = CMD.DISSOLVE;


        if( true)
        {
            switch( cmd )
            {
                case CMD.GENERATE    : if( gen != null ) gen.Generate( 128, 0.5f, 2.0f ); break;

                case CMD.REBUILD_UVS : if( gen != null ) gen.GenerateUVs();               break;
                                                                                           
                case CMD.DISSOLVE    : if( gen != null ) gen.Dissolve();                  break;

                default: break;
            }
        }

        base.OnInspectorGUI();
    }
}

#endif