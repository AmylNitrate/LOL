using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Data : MonoBehaviour {


    public static Data control;

    public int energy;
    public int upgradePoints;
    public int points;

    public int bluffPoints;
    public int assessPoints;

	// Use this for initialization
	void Awake ()
    {
        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if(control != this)
        {
            Destroy(gameObject);
        }

	}

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");

        PlayerData _data = new PlayerData();
        _data.energy = energy;
        _data.upgradePoints = upgradePoints;
        _data.points = points;

        _data.bluffPoints = bluffPoints;
        _data.assessPoints = assessPoints;

        bf.Serialize(file, _data);
        file.Close();
    }

    public void Load()
    {
        if(File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData _data = (PlayerData)bf.Deserialize(file);
            file.Close();

            energy = _data.energy;
            upgradePoints = _data.upgradePoints;
            points = _data.points;

            bluffPoints = _data.bluffPoints;
            assessPoints = _data.assessPoints;
        }
    }

}

[Serializable]
class PlayerData
{
    public int energy;
    public int upgradePoints;
    public int points;

    public int bluffPoints;
    public int assessPoints;
}