using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileObject : MonoBehaviour
{
	// for example for object 3x1 (3 - longitude, 1 - width) [0,0,1,0,2,0] (0,0 will be added by default)
	public int[] additionalCoords;

	public int[] _objectCoords;
	public int[] _activatorsCoords;

	public bool _coordsNeeded;
	public int type;

	public float GetLocationChanger(){
		/// CHANGE TO BY SIZE COMPARATOR
        if (type == ObjectTypes.building_area){
			return 1.0f;
        }
		return 0.5f;
	}

	public int GetTileType(){
		/// CHANGE TO BY SIZE COMPARATOR
        
		return type;
	}


	public void SetObjectCoords(int[] coords){
		if (_coordsNeeded){
			_objectCoords = coords;
			}
	}


	public int[] GetAdditionalCoords(){
		return additionalCoords;
	}

	public int[] GetActivatorsCoords(){
		return _activatorsCoords;
	}

	public void Init(){
		
	}
}
