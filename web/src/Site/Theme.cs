namespace Site;

public class Theme
{
    public readonly Media Media = new Media();
    public readonly string FontFamily = "Roboto";
    public readonly Palette Palette = new Palette();
}

public class Palette
{
    public readonly string Primary = "#418cff";
    public readonly string Secondary = "#ff4186";
    public readonly string Paper = "#ffffff";
    public readonly string Gray8 = "#888";
}

public class Media
{
    public string Xs = "(max-width: 575px)";
    public string Sm = "(min-width: 576px) and (max-width: 767px)";
    public string Md = "(min-width: 768px) and (max-width: 991px)";
    public string Lg = "(min-width: 992px) and (max-width: 1199px)";
    public string Xl = "(min-width: 1200px) and (max-width: 1599px)";
    public string Xxl = "(min-width: 1600px)";
}