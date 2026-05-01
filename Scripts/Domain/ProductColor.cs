using System;

namespace RedteaGreenteaTea.Domain;

public readonly record struct ProductColor(float R, float G, float B, float A)
{
    public static ProductColor FromRgb255(float r, float g, float b, float a)
    {
        return new ProductColor(r / 255f, g / 255f, b / 255f, a).Clamped();
    }

    public ProductColor WithAlpha(float alpha)
    {
        return new ProductColor(R, G, B, alpha).Clamped();
    }

    public ProductColor Clamped()
    {
        return new ProductColor(
            Math.Clamp(R, 0f, 1f),
            Math.Clamp(G, 0f, 1f),
            Math.Clamp(B, 0f, 1f),
            Math.Clamp(A, 0f, 1f));
    }
}
