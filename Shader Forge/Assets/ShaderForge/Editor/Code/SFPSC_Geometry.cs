﻿using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ShaderForge {
	
	
	[System.Serializable]
	public class SFPSC_Geometry : SFPS_Category {


		public enum NormalQuality { Interpolated, Normalized };
		public string[] strNormalQuality = new string[] { "Interpolated", "Normalized" };
		public enum NormalSpace { Tangent, Object, World };
		public string[] strNormalSpace = new string[] { "Tangent", "Object", "World" };
		public enum TessellationMode { Regular, EdgeLength/*, EdgeLengthCulled*/};
		public string[] tessModeStr = new string[] { "Regular", "Edge length based"/*, "Edge length based with frustrum culling"*/};
		public enum VertexOffsetMode { Relative, Absolute }
		public string[] vertexOffsetModeStr = new string[] { "Relative", "Absolute" };

		public NormalQuality normalQuality = NormalQuality.Normalized;
		public NormalSpace normalSpace = NormalSpace.Tangent;
		public VertexOffsetMode vertexOffsetMode = VertexOffsetMode.Relative;
		public bool showPixelSnap = false;
		public bool highQualityScreenCoords = true;
		public TessellationMode tessellationMode = TessellationMode.Regular;
		

		
		

		public override string Serialize(){
			string s = "";
			s += Serialize( "hqsc", highQualityScreenCoords.ToString());
			s += Serialize( "nrmq", ( (int)normalQuality ).ToString() );
			s += Serialize( "nrsp", ( (int)normalSpace ).ToString() );
			s += Serialize( "vomd", ( (int)vertexOffsetMode ).ToString() );
			s += Serialize( "spxs",	showPixelSnap.ToString());
			s += Serialize( "tesm", ((int)tessellationMode).ToString());
			return s;
		}

		public override void Deserialize(string key, string value){

			switch( key ) {
			case "nrmq":
				normalQuality = (NormalQuality)int.Parse( value );
				break;
			case "nrsp":
				normalSpace = (NormalSpace)int.Parse( value );
				break;
			case "vomd":
				vertexOffsetMode = (VertexOffsetMode)int.Parse( value );
				break;
			case "hqsc":
				highQualityScreenCoords = bool.Parse( value );
				break;
			case "spxs":
				showPixelSnap = bool.Parse( value );
				break;
			case "tesm":
				tessellationMode = (TessellationMode)int.Parse( value );
				break;
			}

		}
	

		public override float DrawInner(ref Rect r){

			float prevYpos = r.y;
			r.y = 0;

			
			r.xMin += 20;
			r.y += 20;


			GUI.enabled = ps.catLighting.renderPath == SFPSC_Lighting.RenderPath.Forward;
			normalQuality = (NormalQuality)UndoableContentScaledToolbar( r, "Normal Quality", (int)normalQuality, strNormalQuality, "normal quality" );
			GUI.enabled = true;
			r.y += 20;

			GUI.enabled = ps.mOut.normal.IsConnectedEnabledAndAvailable();
			normalSpace = (NormalSpace)UndoableContentScaledToolbar( r, "Normal Space", (int)normalSpace, strNormalSpace, "normal space" );
			GUI.enabled = true;
			r.y += 20;

			vertexOffsetMode = (VertexOffsetMode)UndoableContentScaledToolbar( r, "Vertex offset mode", (int)vertexOffsetMode, vertexOffsetModeStr, "vertex offset mode" );
			r.y += 20;

			GUI.enabled = ps.HasTessellation();
			tessellationMode = (TessellationMode)UndoableLabeledEnumPopupNamed( r, "Tessellation Mode", tessellationMode, tessModeStr, "tessellation mode" );
			GUI.enabled = true;
			r.y += 20;

			highQualityScreenCoords = UndoableToggle( r, highQualityScreenCoords, "Per-pixel screen coordinates", "per-pixel screen coordinates", null );
			r.y += 20;

			showPixelSnap = UndoableToggle( r, showPixelSnap, "Show 2D sprite pixel snap option in material", "show pixel snap", null );
			r.y += 20;

			

			r.y += prevYpos;

			return (int)r.yMax;
		}




	}
}