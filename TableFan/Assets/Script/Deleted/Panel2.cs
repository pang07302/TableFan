using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Panel2 : MonoBehaviour
{
    [SerializeField] private TMP_InputField effect;
    static Haptic hapticEffect;
    static SightName.Sight sightEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       

        
    }

    public void submit()
    {
        Debug.Log(effect.text);

        string[] text = effect.text.Split('\n');
        string deviceId = "fan01";
        string Json="{\"deviceId\":"+"\""+deviceId+"\",";
        string haptic_effects = "\"haptic_effects\":[{";
        string sight_effects = "\"sight_effects\":[{";
        string sight = "";
        string audio = "";
        string haptic = "";
        string smell = "";
        string taste = "";
        
        for (int i =0; i<text.Length-1; i++){
            string[] t = text[i].Split('|');
            string effectType = t[0].Split(':')[1];
            Debug.Log(effectType+i);
            string container = "";
            container+="\"start\":"+t[1].Split(':')[1]+
            ", \"description\": { \"pattern\":[{ \"type\":\""+t[2].Split(':')[1]+
            "\", \"LengthMs\":"+t[3].Split(':')[1];
            switch (effectType){
                case "Sight":
                    sight+=container+", \"colour\":\""+t[5].Split(':')[1]+"\""+
                    "}], \"rate\":{\"frequency\": "+ t[4].Split(':')[1]+"}}},";  
                    break;    
                case "Audio":
                    audio+=container+"}], \"rate\":{\"frequency\": "+ t[4].Split(':')[1]+"}}},";  
                    break;
                case "Haptic":
                    haptic+=container+"}], \"rate\":{\"frequency\": "+ t[4].Split(':')[1]+"}}},";
                    break;
                case "Smell":    
                    smell+=container+", \"fragance\":\""+t[5].Split(':')[1]+"\""+
                    "}], \"rate\":{\"frequency\": "+ t[4].Split(':')[1]+"}}},";  
                    break;
                case "Taste":
                    taste+=container+", \"flavour\":\""+t[5].Split(':')[1]+"\""+
                    "}], \"rate\":{\"frequency\": "+ t[4].Split(':')[1]+"}}},"; 
                    break;

            }
        }
        string sightJson = Json+sight_effects+sight.Substring(0,sight.Length-1)+"]}";
        string hapticJson = Json+haptic_effects+haptic.Substring(0,haptic.Length-1)+"]}";
       
        Debug.Log(hapticJson);
        Debug.Log(sightJson);


        
        
        

    }
}


// effect: Haptic | start:2000 | type:Custom | LengthMs:100 | frequency:1000  
// effect: Sight | start:2000 | type:Custom | LengthMs:100 | frequency:1000 | colour:red  
