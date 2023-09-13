using System;

public enum EaseType { CircInOut, CircOut, Cubic, Linear }
public static class Easing {
    public static float CircInOut(float x) {
        if (x < 0.5f)
            return (float)((1 - Math.Sqrt(1 - Math.Pow(2*x, 2))) / 2);
        return (float)((Math.Sqrt(1 - Math.Pow(-2 * x + 2, 2)) + 1) / 2);
    }

    public static float CircOut(float x) {
        return (float)Math.Sqrt(1 - Math.Pow(x - 1, 2));
    }

    public static float Cubic(float x) {
        return 1 - (float)Math.Pow(1 - x, 3);
    }

    public static float EaseInBack(float x) {
        var c1 = 1.70158f;
        var c3 = c1 + 1;

        return c3 * x * x * x - c1 * x * x;
    }

    public static float EaseOutBack(float x) {
        var c1 = 1.70158f;
        var c3 = c1 + 1;

        return 1 + c3 * (float)Math.Pow(x - 1, 3) + c1 * (float)Math.Pow(x - 1, 2);
    }

    public static float Linear(float x) => x;

    public static float Ease(float value, EaseType type = EaseType.Linear) {
        switch (type) {
            case EaseType.CircInOut :
                return CircInOut(value);
            case EaseType.CircOut:
                return CircOut(value);
            case EaseType.Cubic:
                return Cubic(value);
            default:
                return Linear(value);
        }
    }


    public static float EaseOutBounce(float x) {
        float n1 = 7.5625f;
        float d1 = 2.75f;
        if (x < 2 / d1) {
            return n1 * x * x;
        } else if (x < 2.5f / d1) {
            return n1 * (x -= 2f / d1) * x + 0.75f;
        } else {
            return n1 * (x -= 2.625f / d1) * x + 0.85f;
        }
    }

    public static float EaseOutSine(float x) {
        return (float)Math.Sin((x * Math.PI) / 2f);
    }
}