using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SweetCandy.Basic;
using SweetCandy.Settings;
using UnityEngine;

namespace SweetCandy.Managers
{
    
    public class SaveManager : MonoSingleton<SaveManager>
    {
        
        public UserSettings CreateSave()
        {
            UserSettings us = new UserSettings();
            us.MUSIC_VOLUME = Global.MUSIC_VOLUME;
            us.SOUND_VOLUME = Global.SOUND_VOLUME;
            us.SOUND_SWITCH = Global.SOUND_SWITCH;
            us.MUSIC_SWITCH = Global.MUSIC_SWITCH;
            us.resolution = Global.resolution;
            us.windowMode = Global.windowMode;
            us.isWindowMode = Global.isWindowMode;
            //us.isPassedGuide = Global.isPassedGuide;
            return us;
        }
        public void Save(){
            UserSettings us=CreateSave();
            BinaryFormatter bf=new BinaryFormatter();
            FileStream fs=File.Create(Application.persistentDataPath+"/Meow.set");
            bf.Serialize(fs,us);
            fs.Close();
        }
        public void Load(){
            if(File.Exists(Application.persistentDataPath+"/Meow.set"))
            {
                BinaryFormatter bf=new BinaryFormatter();
                FileStream fs=File.Open(Application.persistentDataPath+"/Meow.set",FileMode.Open);
                UserSettings usn=bf.Deserialize(fs) as UserSettings;
                fs.Close();
                Global.MUSIC_VOLUME=usn.MUSIC_VOLUME;
                Global.SOUND_VOLUME=usn.SOUND_VOLUME;
                Global.SOUND_SWITCH=usn.SOUND_SWITCH;
                Global.MUSIC_SWITCH=usn.MUSIC_SWITCH;
                Global.resolution=usn.resolution;
                Global.windowMode=usn.windowMode;
                Global.isWindowMode=usn.isWindowMode;
                //Global.isPassedGuide = usn.isPassedGuide;
            }else{
                Debug.Log("Data Not Found");
            }
        }

        public void Delete()
        {
            if (File.Exists(Application.persistentDataPath + "/Meow.set"))
            {
                File.Delete(Application.persistentDataPath + "/Meow.set");
                Debug.Log("delete data.set");
            }
            else
            {
                Debug.Log("Data Not Found");
            }
        }

    }
}