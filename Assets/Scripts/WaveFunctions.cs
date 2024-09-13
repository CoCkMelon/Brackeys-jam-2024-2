using UnityEngine;

public static class WaveFunctions
{

    public static float RoundedSquareWave(float x)
    {
        float edgeWidth = 0.1f;
        // Normalize x to be within the range [0, 2π)
        float n = x % (2 * Mathf.PI);
        if (n < 0)
        {
            n += 2 * Mathf.PI;
        }

        // Convert to a 0-1 range
        float normalized = n / (2 * Mathf.PI);

        // Calculate the width of the rising and falling edges
        float halfEdgeWidth = edgeWidth / 2f;

        if (normalized < halfEdgeWidth)
        {
            // Rising edge
            return -1f + (normalized / halfEdgeWidth) * 2f;
        }
        else if (normalized < 0.5f - halfEdgeWidth)
        {
            // High part
            return 1f;
        }
        else if (normalized < 0.5f + halfEdgeWidth)
        {
            // Falling edge
            return 1f - ((normalized - (0.5f - halfEdgeWidth)) / edgeWidth) * 2f;
        }
        else if (normalized < 1f - halfEdgeWidth)
        {
            // Low part
            return -1f;
        }
        else
        {
            // Final rising edge
            return -1f + ((normalized - (1f - halfEdgeWidth)) / halfEdgeWidth) * 2f;
        }
    }
    public static float SquareWave(float x)
    {
        // Normalize x to be within the range [0, 2π)
        float n = x % (2 * Mathf.PI);
        if (x < 0)
        {
            n += 2 * Mathf.PI;
        }

        // Return 1 for the first half of the cycle, -1 for the second half
        if (n < Mathf.PI)
        {
            return 1.0f;
        }
        else
        {
            return -1.0f;
        }
    }
}
