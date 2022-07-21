
public class Devices
{
    public string name;
    public string category;
    public string status;

    public Devices(int id, string action)
    {

        switch (id)
        {
            case 1:
                name = "fan";
                category = "haptic";
                status = action;
                break;
            case 2:
                name = "VR-Headset";
                category = "sight";
                status = action;
                break;
            case 3:
                name = "glove";
                category = "haptic";
                status = action;
                break;
            case 4:
                name = "headphone";
                category = "audio";
                status = action;
                break;
            default:
                name = "e-";
                category = "taste";
                status = action;
                break;

        }
    }

    public Devices(string name, string category, string action)
    {
        this.name = name;
        this.category = category;
        this.status = action;
    }

}
