using System;
using System.Collections.Generic;

[Serializable]
public class BuildingData
{
    public List<BuildingInfo> buildings = new List<BuildingInfo>();

    public BuildingData(BuildingPlacement B)
    {
        foreach (var building in B.placedBuildings)
        {
            buildings.Add(new BuildingInfo(
                building.name.Replace("(Clone)", "").Trim(),
                building.transform.position,
                building.transform.rotation
            ));
        }
    }
}


[Serializable]
public class BuildingInfo
{
    public string prefabName;
    public float positionX;
    public float positionY;
    public float positionZ;
    public float rotationX;
    public float rotationY;
    public float rotationZ;
    public float rotationW;

    public BuildingInfo(string prefabName, UnityEngine.Vector3 position, UnityEngine.Quaternion rotation)
    {
        this.prefabName = prefabName;
        this.positionX = position.x;
        this.positionY = position.y;
        this.positionZ = position.z;
        this.rotationX = rotation.x;
        this.rotationY = rotation.y;
        this.rotationZ = rotation.z;
        this.rotationW = rotation.w;
    }

    public UnityEngine.Vector3 GetPosition()
    {
        return new UnityEngine.Vector3(positionX, positionY, positionZ);
    }

    public UnityEngine.Quaternion GetRotation()
    {
        return new UnityEngine.Quaternion(rotationX, rotationY, rotationZ, rotationW);
    }
}
