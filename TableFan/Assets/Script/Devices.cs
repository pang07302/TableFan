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
                name = "LED-light";
                category = "Sight";
                status = action;
                break;
            case 2:
                name = "VR-Headset";
                category = "Sight";
                status = action;
                break;
            case 3:
                name = "headphone";
                category = "Audio";
                status = action;
                break;
            case 4:
                name = "speaker";
                category = "Audio";
                status = action;
                break;
            case 5:
                name = "exoskeletons";
                category = "Haptic";
                status = action;
                break;
            case 6:
                name = "glove";
                category = "Haptic";
                status = action;
                break;
            case 7:
                name = "joysticks";
                category = "Haptic";
                status = action;
                break;
            case 8:
                name = "fan";
                category = "Haptic";
                status = action;
                break;
            case 9:
                name = "olfactometers";
                category = "Smell";
                status = action;
                break;
            case 10:
                name = "electronic vaporizers";
                category = "Audio";
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
