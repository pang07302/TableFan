
    


public class Devices
{
    public string name;
    public string category;
    public string status;

    public Devices GenerateDevice(int id, string action)
    {

        switch (id)
        {
            case 1:
                return 
                new Devices{
                    name = "fan",
                    category = "haptic",
                    status = action
                };
            case 2:
                return 
                new Devices{
                    name= "VR-Headset",
                    category="sight",
                    status = action

                };
            case 3:
                return 
                new Devices{
                    name= "glove",
                    category="haptic",
                    status = action
                };
            case 4:
                return
                new Devices{
                    name= "headphone",
                    category= "audio",
                    status = action

                };
            default:
                return
                new Devices{
                    name= "e-",
                    category= "taste",
                    status = action
                };    

        }

    }

}
