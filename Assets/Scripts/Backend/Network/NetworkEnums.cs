using UnityEngine;
using System.Collections;

public sealed class NetworkEnums {
    private readonly string name;
    private readonly int value;

#if UNITY_EDITOR
    public static readonly NetworkEnums LOGIN = new NetworkEnums(0, "http://localhost:8080/login");
    public static readonly NetworkEnums GETSOLAR = new NetworkEnums(1, "http://localhost:8080/getsolarsystem");
    public static readonly NetworkEnums REGISTER = new NetworkEnums(2, "http://localhost:8080/register");
    public static readonly NetworkEnums HIGHSCORE = new NetworkEnums(3, "http://localhost:8080/gethighscore");
    public static readonly NetworkEnums SENDSTARTDATA = new NetworkEnums(4, "http://localhost:8080/senddata");
    public static readonly NetworkEnums SENDDATA = new NetworkEnums(4, "http://localhost:8080/senddata");
#else
    public static readonly NetworkEnums LOGIN = new NetworkEnums(0, "https://takemetothemoontest.appspot.com/login");
    public static readonly NetworkEnums GETSOLAR = new NetworkEnums(1, "https://takemetothemoontest.appspot.com/getsolarsystem");
    public static readonly NetworkEnums REGISTER = new NetworkEnums(2, "https://takemetothemoontest.appspot.com/register");
    public static readonly NetworkEnums HIGHSCORE = new NetworkEnums(3, "https://takemetothemoontest.appspot.com/gethighscore");
    public static readonly NetworkEnums SENDSTARTDATA = new NetworkEnums(4, "https://takemetothemoontest.appspot.com/senddata");
    public static readonly NetworkEnums SENDDATA = new NetworkEnums(4, "https://takemetothemoontest.appspot.com/senddata");
#endif

    private NetworkEnums(int value,string name)
    {
        this.name = name;
        this.value = value;
    }

    public override string ToString()
    {
 	    return name;
    }

}